using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.Models.DTO;
using Zero_Hunger.Models.EF;

namespace Zero_Hunger.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                if (Session["Access"].ToString() == "Benefactor")
                {
                    return RedirectToAction("Dashboard", "Benefactor");
                }
                else
                {
                    return RedirectToAction("Dashboard", "NGO");
                }
                
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerDbContext();
                var user = db.Users.FirstOrDefault(u => u.UserName == login.Username);

                if (user!= null && user.Password == login.Password)
                {
                    Session["Username"] = login.Username;
                    Session["Access"] = user.AccessName;
                    if (user.AccessName == "Benefactor")
                    {
                        return RedirectToAction("Dashboard", "Benefactor");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "NGO");
                    }
                }
                ViewBag.Error = "Invalid Username or Password";
                return View();
            }
            
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult BenefactorRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BenefactorRegistration(UserDTO user)
        {
            if(ModelState.IsValid)
            {
                var db = new ZeroHungerDbContext();
                var data = db.Users.FirstOrDefault(u => u.UserName == user.UserName);

                if (data == null)
                {
                    User userEntity = new User();
                    userEntity.Id = Guid.NewGuid();
                    userEntity.UserName = user.UserName;
                    userEntity.Password = user.Password;
                    userEntity.AccessName = "Benefactor";
                    db.Users.Add(userEntity);
                    db.SaveChanges();

                    Benefactor BenefactorEntity = new Benefactor();
                    BenefactorEntity.Id = userEntity.Id;
                    BenefactorEntity.Name = user.Name;
                    db.Benefactors.Add(BenefactorEntity);
                    db.SaveChanges();

                    return RedirectToAction("Login", "Auth");
                }
                ViewBag.Error = "The Username Already Exists";
                return View();
            }
            ViewBag.Error = "Something Went Wrong";
            return View();
        }
        public ActionResult createAdmin()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            if (TempData["Success"]!=null)
            {
                ViewBag.Success = TempData["Success"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createAdmin(adminUserDTO adminUser)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerDbContext();
                var data = db.Users.FirstOrDefault(u => u.UserName == adminUser.UserName);

                if (data != null)
                {
                    TempData["Error"] = "The Username Already exists";
                    return RedirectToAction("CreateAdmin");
                }

                var newAdminUser = new User();
                newAdminUser.Id = Guid.NewGuid();
                newAdminUser.UserName = adminUser.UserName;
                newAdminUser.Password = adminUser.Password;
                newAdminUser.AccessName = "NGO";

                db.Users.Add(newAdminUser);
                db.SaveChanges();

                TempData["Success"] = "New Admin User has been Created";
                return RedirectToAction("createAdmin");
            }
            return RedirectToAction("createAdmin");
        }
    }
}