using CodeKicker.BBCode;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using redinc_reboot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace redinc_reboot.Controllers
{
    public class ProblemController : Controller
    {
        public ActionResult Edit(int id = 0)
        {
            ProblemViewModel model = new ProblemViewModel();
            if (id == 0)
            {
                model.Problem = new ProblemData();
            }
            else
            {
                model.Problem = GlobalStaticVars.StaticCore.GetProblemById(id);
                model.Sets = GlobalStaticVars.StaticCore.GetSetsForProblem(id);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProblemViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Problem.Id == 0)
                    model.Problem.Id = GlobalStaticVars.StaticCore.AddProblem(model.Problem);
                else
                    GlobalStaticVars.StaticCore.ModifyProblem(model.Problem);
                GlobalStaticVars.StaticCore.UpdateProblemSets(model.Problem.Id, model.Sets);

                //This is necessary in case bad prereqs (ex. duplicates) are removed by the backend
                model.Sets = GlobalStaticVars.StaticCore.GetSetsForProblem(model.Problem.Id);
            }

            return View(model);
        }

        public ActionResult Solve(int id)
        {
            List<ProblemData> problems = GlobalStaticVars.StaticCore.GetProblemsForSet(id);
            ProblemData prob = problems[new Random().Next(problems.Count)];
            ViewBag.Description = BBCode.ToHtml(prob.Description);
            ViewBag.SubmitAction = "Solve";
            return View();
        }

        [HttpPost]
        public JsonResult Solve(string code)
        {
            string output = GlobalStaticVars.StaticCore.ExecutePythonCode(code);
            return Json(new { success = true, output = output });
        }
        
        [HttpPost]
        public ActionResult TestPage(ProblemData prob)
        {
            ViewBag.Description = BBCode.ToHtml(prob.Description);
            ViewBag.SubmitAction = "Test";
            return View("Solve");
        }

        [HttpPost]
        public ActionResult Test(string code)
        {
            //TODO This could verify the code without recording the problem as solved
            string output = GlobalStaticVars.StaticCore.ExecutePythonCode(code);
            return Json(new { success = true, output = output + " (test)" });
        }

        public ActionResult AddProblemSet(ProblemSetData model)
        {
            return PartialView("EditorTemplates/ProblemSetRow", model);
        }
    }
}
