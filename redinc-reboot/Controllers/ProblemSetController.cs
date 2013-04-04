using core.Modules.Class;
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
    public class ProblemSetController : Controller
    {
        public ActionResult Edit(int id = 0)
        {
            ProblemSetViewModel model = new ProblemSetViewModel();
            if (id == 0) //Create a new set
            {
                model.Set = new ProblemSetData();
                model.Set.Class = new ClassData((int)Session["ClassId"]);
            }
            else if (id == -1) //View the unassigned set
            {
                ProblemSetData unassigned = new ProblemSetData(-1);
                unassigned.Name = "Unassigned Problems";
                unassigned.Class = new ClassData((int)Session["ClassId"]);
                model.Set = unassigned;
                model.Problems = GlobalStaticVars.StaticCore.GetProblemsForSet(model.Set);
            }
            else //Edit a set normally
            {
                model.Set = GlobalStaticVars.StaticCore.GetSetById(id);
                model.Prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(id);
                model.Problems = GlobalStaticVars.StaticCore.GetProblemsForSet(model.Set);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProblemSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Set.Id == 0)
                    model.Set.Id = GlobalStaticVars.StaticCore.AddSet(model.Set);
                else
                    GlobalStaticVars.StaticCore.ModifySet(model.Set);
                GlobalStaticVars.StaticCore.UpdateSetPrereqs(model.Set.Id, model.Prereqs);

                //This is necessary in case bad prereqs (ex. duplicates) are removed by the backend
                model.Prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(model.Set.Id);
            }

            return View(model);
        }

        public ActionResult AddPrereq(ProblemSetData model)
        {
            return PartialView("EditorTemplates/PrereqRow", model);
        }

        public JsonResult Search(string term)
        {
            List<ProblemSetData> sets = GlobalStaticVars.StaticCore.SearchSetsInClass((int)Session["ClassId"], term);
            return Json(sets.Select(s => new { Id = s.Id, label = s.Name, Name = s.Name }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            GlobalStaticVars.StaticCore.DeleteSet(id);

            return RedirectToAction("Home", "Class", new { id = Session["ClassId"] });
        }
    }
}
