using core.Modules.Class;
using core.Modules.Problem;
using core.Modules.ProblemSet;
using core.Modules.User;
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
        [Authorize]
        public ActionResult Edit(int classId, int problemSetId = 0)
        {
            try
            {
                if ((int) Session["ClassId"] != classId || (UserType) Session["UserType"] != UserType.Instructor)
                {
                    return RedirectToAction("Unauthorized", "Error");
                }
                ProblemSetViewModel model = new ProblemSetViewModel();
                if (problemSetId == 0) //Create a new set
                {
                    model.Set = new ProblemSetData();
                    model.Set.Class = new ClassData((int) Session["ClassId"]);
                }
                else if (problemSetId == -1) //View the unassigned set
                {
                    ProblemSetData unassigned = new ProblemSetData(-1);
                    unassigned.Name = "Unassigned Problems";
                    unassigned.Class = new ClassData((int) Session["ClassId"]);
                    model.Set = unassigned;
                    model.Problems = GlobalStaticVars.StaticCore.GetProblemsForSet(model.Set);
                }
                else //Edit a set normally
                {
                    model.Set = GlobalStaticVars.StaticCore.GetSetById(problemSetId);
                    model.Prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(problemSetId);
                    model.Problems = GlobalStaticVars.StaticCore.GetProblemsForSet(model.Set);
                }
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ProblemSetViewModel model)
        {
            try
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
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        [Authorize]
        public ActionResult AddPrereq(ProblemSetData model)
        {
            try
            {
                return PartialView("EditorTemplates/PrereqRow", model);
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        public JsonResult Search(string term)
        {
            try
            {
                List<ProblemSetData> sets = GlobalStaticVars.StaticCore.SearchSetsInClass((int) Session["ClassId"], term);
                return Json(sets.Select(s => new {Id = s.Id, label = s.Name, Name = s.Name}),
                            JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                RedirectToAction("ServerError", "Error");
                return new JsonResult();
            }
        }

        public JsonResult GetAllSetsInClass()
        {
            try
            {
                List<ProblemSetData> sets = GlobalStaticVars.StaticCore.GetSetsForClass((int)Session["ClassId"]);
                return Json(sets.Select(s => new { Id = s.Id, label = s.Name, Name = s.Name }),
                            JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                RedirectToAction("ServerError", "Error");
                return new JsonResult();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                GlobalStaticVars.StaticCore.DeleteSet(id);

                return RedirectToAction("Home", "Class", new {id = Session["ClassId"]});
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}
