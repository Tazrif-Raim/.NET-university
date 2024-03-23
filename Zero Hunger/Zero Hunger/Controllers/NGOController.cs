using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.Models.EF;

namespace Zero_Hunger.Controllers
{
    public class NGOController : Controller
    {
        public ActionResult Dashboard()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Username = Session["Username"].ToString();
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            return View();
        }

        public ActionResult EditProfile()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Username = Session["Username"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult ChangeUsername(FormCollection Form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var username = Form["Username"];
            var db = new ZeroHungerDbContext();
            var Username = Session["UserName"].ToString();
            var data = db.Users.FirstOrDefault(u => u.UserName == Username);
            var data2 = db.Users.FirstOrDefault(u => u.UserName == username);
            if (data2 != null)
            {
                TempData["Error"] = "The Username already exists";
                return RedirectToAction("EditProfile");
            }

            if (data != null && username != "")
            {
                data.UserName = username;
                Session["Username"] = username;
                db.SaveChanges();
            }

            return RedirectToAction("EditProfile");
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection Form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var currentPassword = Form["CurrentPassword"];
            var newPassword = Form["NewPassword"];
            var confirmPassword = Form["ConfirmPassword"];

            var db = new ZeroHungerDbContext();
            var Username = Session["UserName"].ToString();
            var user = db.Users.FirstOrDefault(u => u.UserName == Username);
            if (user != null)
            {
                if (user.Password != currentPassword)
                {
                    TempData["Error"] = "Current Password is incorrect";
                    return RedirectToAction("EditProfile");
                }
                if (newPassword != confirmPassword)
                {
                    TempData["Error"] = "New Password cannot be empty";
                    return RedirectToAction("EditProfile");
                }
                if (confirmPassword != newPassword)
                {
                    TempData["Error"] = "New Password does not match the confirm password";
                    return RedirectToAction("EditProfile");
                }
                user.Password = newPassword;
                db.SaveChanges();
            }
            return RedirectToAction("EditProfile");
        }

        public ActionResult ShowAllEmployee()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var employees = db.Employees.ToList();
            ViewBag.employees = employees;
            return View();
        }

        public ActionResult HireEmployee()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];  
            } 
            return View();
        }

        [HttpPost]
        public ActionResult HireEmployee(FormCollection form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var name = form["name"];
            if(name == "")
            {
                TempData["Error"] = "Name cannot be empty";
                return RedirectToAction("HireEmployee");
            }
            var db = new ZeroHungerDbContext();
            var data = new Employee();
            data.Id = Guid.NewGuid();
            data.Name = name;
            db.Employees.Add(data);
            db.SaveChanges();
            TempData["Success"] = "Employee was Added";
            return RedirectToAction("HireEmployee");
        }

        public ActionResult FoodCollection(string filter)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var foodList = db.Foods.ToList();
            foreach(var donation in foodList)
            {
                if (donation.StatusName == "Pending" || donation.StatusName == "Accepted")
                {
                    if (donation.ExpireTime <= DateTime.Now)
                    {
                        donation.CompleteTime = DateTime.Now;
                        donation.StatusName = "Expired";
                        db.SaveChanges();
                    }
                }
            }
            var employees = db.Employees;
            if(filter == "All")
            {
                ViewBag.foodlist = db.Foods.ToList();
            }
            else
            {
                ViewBag.foodlist = db.Foods.Where(s => s.StatusName == filter);
            }
            ViewBag.employees = employees.ToList();
            ViewBag.Filter = filter;
            return View();
        }
        [HttpPost]
        public ActionResult assignEmployee(FormCollection form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var foodId = Guid.Parse(form["Id"]);
            var employeeId = form["employee"];
            var filter = form["filter"];


            var db = new ZeroHungerDbContext();
            var food = db.Foods.Find(foodId);
            if(food == null)
            {
                return RedirectToAction("FoodCollection", new { filter = filter });
            }
            if(food.ExpireTime<=DateTime.Now)
            {
                food.CompleteTime = DateTime.Now;
                food.StatusName = "Expired";
                db.SaveChanges();
                return RedirectToAction("FoodCollection", new { filter = filter });
            }
            if(food.StatusName != "Pending")
            {
                return RedirectToAction("FoodCollection", new { filter = filter });
            }
            food.StatusName = "Accepted";
            food.AssignedTo = Guid.Parse(employeeId);
            db.SaveChanges();
            return RedirectToAction("FoodCollection", new { filter = filter });
        }

        public ActionResult Filter()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "NGO")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var status = db.Statuses.ToList();
            ViewBag.statuses = status;
            return View();
        }
    }
}