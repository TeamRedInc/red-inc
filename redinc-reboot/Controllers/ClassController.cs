using core.Modules.Class;
using core.Modules.ProblemSet;
using core.Modules.User;
using redinc_reboot.Filters;
using redinc_reboot.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace redinc_reboot.Controllers
{
    [InitializeSimpleMembership]
    public class ClassController : Controller
    {
        [HttpPost]
        public ActionResult Edit(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                GlobalStaticVars.StaticCore.ModifyClass(model.Class);
            }

            return View("InstructorClassHome", model);
        }

        public ActionResult JoinClass(int id)
        {
            GlobalStaticVars.StaticCore.AddStudent(WebSecurity.CurrentUserId, id);
            return RedirectToAction("Home");
        }

        public ActionResult Home(int id)
        {
            ClassData cls = GlobalStaticVars.StaticCore.GetClassById(id);

            if (WebSecurity.CurrentUserId == cls.Instructor.Id)
            {
                Session["ClassId"] = id;
                Session["UserType"] = UserType.Instructor;

                ClassViewModel model = new ClassViewModel();
                model.Class = cls;
                model.Sets = GlobalStaticVars.StaticCore.GetSetsForClass(id);

                //Add in the unassigned set that captures any problems not assigned to any sets
                ProblemSetData unassigned = new ProblemSetData(-1);
                unassigned.Name = "Unassigned Problems";
                unassigned.Class = new ClassData(id);
                model.Sets.Add(unassigned);

                return View("InstructorClassHome", model);
            }
            else if (GlobalStaticVars.StaticCore.IsStudent(WebSecurity.CurrentUserId, id))
            {
                Session["ClassId"] = id;
                Session["UserType"] = UserType.Student;
                Tuple<List<ProblemSetData>, List<ProblemSetData>, List<ProblemSetData>> sets =
                    GlobalStaticVars.StaticCore.GetSetsForStudent(WebSecurity.CurrentUserId, id);
                ViewBag.UnlockedSets = sets.Item1;
                ViewBag.LockedSets = sets.Item2;
                ViewBag.SolvedSets = sets.Item3;
                return View("StudentClassHome", cls);
            }

            Session.Clear();
            return RedirectToAction("Home");
        }

        public ActionResult NewClass()
        {
            return PartialView("NewClassDialog", new ClassData());
        }

        [HttpPost]
        public ActionResult NewClass(ClassData newClass)
        {
            newClass.Instructor = new UserData(WebSecurity.CurrentUserId);
            GlobalStaticVars.StaticCore.AddClass(newClass);
            return RedirectToAction("Home", "Home");
        }
    }
}
