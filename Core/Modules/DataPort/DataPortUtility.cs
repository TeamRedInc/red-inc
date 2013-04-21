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
                        if (!exportData.Problems.Contains(problem))
                        {
                            exportData.Problems.Add(problem);
                        }
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
                var problemIDMap = new Dictionary<int, int>();
                var problemSetIDMap = new Dictionary<int, int>();

                // Retrieve our class
                var cls = ModelFactory.ClassModel.GetById(classID);

                // Now, add all the problems and update our map
                foreach (var problem in importData.Problems)
                {
                    var oldID = problem.Id;
                    var newID = ModelFactory.ProblemModel.Add(problem);
                    problemIDMap[oldID] = newID;
                }

                // Now, update all problem set classes
                foreach (var pset in importData.ProblemSets.Keys)
                {
                    pset.Class = cls;
                    foreach (var prereq in importData.ProblemSets[pset])
                    {
                        prereq.Class = cls;
                    }
                }

                // Now, add all the problem sets (only keys!) and update our map
                foreach (var pset in importData.ProblemSets.Keys)
                {
                    var oldID = pset.Id;
                    var newID = ModelFactory.ProblemSetModel.Add(pset);
                    problemSetIDMap[oldID] = newID;
                }

                // Now, update all problem set IDs
                foreach (var pset in importData.ProblemSets.Keys)
                {
                    foreach (var prereq in importData.ProblemSets[pset])
                    {
                        prereq.Id = problemSetIDMap[prereq.Id];
                    }
                    pset.Id = problemSetIDMap[pset.Id];
                }

                // Finally, add the prereqs
                foreach (var pset in importData.ProblemSets)
                {
                    ModelFactory.ProblemSetModel.UpdatePrereqs(pset.Key, pset.Value);
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
