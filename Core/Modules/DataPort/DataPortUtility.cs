using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;

namespace core.Modules.DataPort
{
    public class DataPortUtility
    {
        public string ExportFromClass(int classID)
        {
            try
            {
                // First, construct our ClassExportObject to be exported
                var exportData = new PortableClassData();

                // Then, get the problem sets and prereqs for the class
                var psets = ModelFactory.ProblemSetModel.GetForClass(new ClassData(classID));
                foreach (var pset in psets)
                {
                    var prereqList = ModelFactory.ProblemSetModel.GetPrereqs(pset);
                    exportData.ProblemSets[pset] = prereqList;
                }

                // Then, get each problem for each problem set and add them to the export list (without repetition!)
                foreach (var pset in exportData.ProblemSets.Keys)
                {
                    var problems = ModelFactory.ProblemModel.GetForSet(pset);
                    foreach (var problem in problems)
                    {
                        if (!exportData.Problems.Keys.Contains(problem))
                        {
                            var list = new List<ProblemSetData>();
                            list.Add(pset);
                            exportData.Problems.Add(problem, list);
                        }
                        else
                        {
                            exportData.Problems[problem].Add(pset);
                        }
                    }
                }

                // Retrieve unassigned problems
                var unassignedPset = new ProblemSetData(-1);
                unassignedPset.Class = ModelFactory.ClassModel.GetById(classID);
                var unassignedProblems = ModelFactory.ProblemModel.GetForSet(unassignedPset);
                foreach (var problem in unassignedProblems)
                {
                    if (!exportData.Problems.Keys.Contains(problem))
                    {
                        var list = new List<ProblemSetData>();
                        list.Add(unassignedPset);
                        exportData.Problems.Add(problem, list);
                    }
                    else
                    {
                        exportData.Problems[problem].Add(unassignedPset);
                    }
                }

                // Serialize and export
                var x = new DataContractSerializer(exportData.GetType());
                using (var stream = new MemoryStream())
                {
                    x.WriteObject(stream, exportData);
                    stream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // Shit's jank
                return "There was an error in the export process.";
            }
        }

        public bool ImportToClass(int classID, string data)
        {
            try
            {
                // First, deserialize our data string
                var importData = new PortableClassData();
                var x = new DataContractSerializer(importData.GetType());
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(data)))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    importData = (PortableClassData) x.ReadObject(stream);
                }

                // Then, construct our two ID maps
                var problemMap = new Dictionary<ProblemData, ProblemData>();
                var problemSetMap = new Dictionary<ProblemSetData, ProblemSetData>();

                // Retrieve our class
                var newClass = ModelFactory.ClassModel.GetById(classID);

                // Now, add all our problems
                foreach (var problem in importData.Problems.Keys)
                {
                    var oldClass = problem.Class;
                    problem.Class = newClass;
                    var newProblemId = ModelFactory.ProblemModel.Add(problem);
                    problem.Class = oldClass;
                    problemMap[problem] = ModelFactory.ProblemModel.GetById(newProblemId);
                }

                // Next, add all of our problem sets
                foreach (var problemSet in importData.ProblemSets.Keys)
                {
                    var oldClass = problemSet.Class;
                    problemSet.Class = newClass;
                    var newProblemSetId = ModelFactory.ProblemSetModel.Add(problemSet);
                    problemSet.Class = oldClass;
                    problemSetMap[problemSet] = ModelFactory.ProblemSetModel.GetById(newProblemSetId);
                }

                // Now hook up all our problem set prerequisites
                foreach (var problemSet in importData.ProblemSets.Keys)
                {
                    var newProblemSet = problemSetMap[problemSet];
                    var newPrereqs = new List<ProblemSetData>();
                    foreach (var prereq in importData.ProblemSets[problemSet])
                    {
                        problemSetMap[prereq].PrereqCount = prereq.PrereqCount;
                        newPrereqs.Add(problemSetMap[prereq]);
                    }
                    ModelFactory.ProblemSetModel.UpdatePrereqs(newProblemSet, newPrereqs);
                }

                // Now update our problem set assignments
                foreach (var problem in importData.Problems.Keys)
                {
                    var newProblem = problemMap[problem];
                    var newProblemSets = new List<ProblemSetData>();
                    foreach (var problemSet in importData.Problems[problem])
                    {
                        if (problemSet.Id == -1)
                        {
                            newProblemSets.Add(problemSet);
                        }
                        else
                        {
                            newProblemSets.Add(problemSetMap[problemSet]);
                        }
                    }
                    ModelFactory.ProblemModel.UpdateSets(newProblem, newProblemSets);
                }

                // Return true if we got to this point
                return true;
            }
            catch (Exception e)
            {
                // Shit's jank
                return false;
            }
        }
    }
}
