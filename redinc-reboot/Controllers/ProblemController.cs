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
            ProblemData prob = GlobalStaticVars.StaticCore.GetProblemById(id);
            ICollection<ProblemSetData> sets = GlobalStaticVars.StaticCore.GetSetsForProblem(id);
            ProblemViewModel model = new ProblemViewModel();
            model.Problem = prob;
            model.Sets = sets;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProblemViewModel model)
        {
            if (ModelState.IsValid)
            {
                GlobalStaticVars.StaticCore.ModifyProblem(model.Problem);
                GlobalStaticVars.StaticCore.UpdateProblemSets(model.Problem.Id, model.Sets);

                //This is necessary in case bad prereqs (ex. duplicates) are removed by the backend
                model.Sets = GlobalStaticVars.StaticCore.GetSetsForProblem(model.Problem.Id);
            }

            return View(model);
        }

        public ActionResult AddProblemSet(ProblemSetData model)
        {
            return PartialView("EditorTemplates/ProblemSetRow", model);
        }
    }
}
