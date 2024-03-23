using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.Models.DTO;
using Zero_Hunger.Models.EF;

namespace Zero_Hunger.Controllers
{
    public class BenefactorController : Controller
    {
        public ActionResult Dashboard()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Username = Session["Username"].ToString();
            return View();
        }

        public ActionResult EditProfile()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var username = Session["UserName"].ToString();
            var Id = db.Users.FirstOrDefault(u => u.UserName == username).Id;
            var data = db.Benefactors.Find(Id);
            ViewBag.Name = data.Name;
            ViewBag.Username = Session["Username"];
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(FormCollection Form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var name = Form["Name"];
            var db = new ZeroHungerDbContext();
            var username = Session["UserName"].ToString();
            var Id = db.Users.FirstOrDefault(u => u.UserName == username).Id;
            var data = db.Benefactors.Find(Id);
            if (data!= null && name!="")
            {
                data.Name = name;
                db.SaveChanges();
            }

            return RedirectToAction("EditProfile");
        }

        [HttpPost]
        public ActionResult ChangeUsername(FormCollection Form)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var username = Form["Username"];
            var db = new ZeroHungerDbContext();
            var Username = Session["UserName"].ToString();
            var data = db.Users.FirstOrDefault(u => u.UserName == Username);
            var data2 = db.Users.FirstOrDefault(u => u.UserName == username);
            if(data2 != null)
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
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
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
                if(user.Password != currentPassword)
                {
                    TempData["Error"] = "Current Password is incorrect";
                    return RedirectToAction("EditProfile");
                }
                if(newPassword != confirmPassword)
                {
                    TempData["Error"] = "New Password cannot be empty";
                    return RedirectToAction("EditProfile");
                }
                if(confirmPassword != newPassword)
                {
                    TempData["Error"] = "New Password does not match the confirm password";
                    return RedirectToAction("EditProfile");
                }
                user.Password = newPassword;
                db.SaveChanges();
            }
            return RedirectToAction("EditProfile");
        }

        public ActionResult CollectionRequest()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollectionRequest(CollectionRequestDTO collectionRequest)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                Food food = new Food();
                food.Id = Guid.NewGuid();
                food.Name = collectionRequest.Name;
                var currentDateTime = DateTime.Now;
                food.UploadDate = currentDateTime;
                food.ExpireTime = currentDateTime.AddHours(collectionRequest.ExpireTime);
                food.Amount = collectionRequest.Amount.ToString();
                food.StatusName = "Pending";
                var db = new ZeroHungerDbContext();
                var username = Session["UserName"].ToString();
                var Id = db.Users.FirstOrDefault(u => u.UserName == username).Id;
                food.BenefactorId = Id;
                db.Foods.Add(food);
                db.SaveChanges();
                TempData["Message"] = "The meal collection request has been uplaoded";
                return RedirectToAction("CollectionRequest");
            }
            return View();
        }

        public ActionResult DonationHistory(string filter)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var username = Session["UserName"].ToString();
            var Id = db.Users.FirstOrDefault(u => u.UserName == username).Id;
            var donations = db.Foods.ToList();
            foreach(var donation in donations)
            {
                if (donation.StatusName == "Pending" || donation.StatusName =="Accepted")
                {
                    if(donation.ExpireTime <= DateTime.Now)
                    {
                        donation.CompleteTime = DateTime.Now;
                        donation.StatusName = "Expired";
                        db.SaveChanges();
                    }
                }
            }
            if (filter == "All")
            {
                ViewBag.Donations = db.Foods.Where(i => i.BenefactorId == Id);
            }
            else
            {
                ViewBag.Donations = db.Foods.Where(i => i.BenefactorId == Id && i.StatusName == filter);
            }
            ViewBag.filter = filter;
            return View();
        }

        public ActionResult CollectedMeal(Guid id, string filter)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var food = db.Foods.Find(id);

            if (food != null)
            {
                
                if (food.StatusName == "Accepted")
                {
                    if (food.ExpireTime <= DateTime.Now)
                    {
                        food.CompleteTime = DateTime.Now;
                        food.StatusName = "Expired";
                        db.SaveChanges();
                        return RedirectToAction("DonationHistory", new { filter = filter });
                    }
                    food.StatusName = "Collected";
                    food.CompleteTime = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("DonationHistory", new { filter = filter });
                }
            }
            return RedirectToAction("DonationHistory", new { filter = filter });

        }

        public ActionResult CancelledMeal(Guid id, string filter)
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
            {
                return RedirectToAction("Login", "Auth");
            }
            var db = new ZeroHungerDbContext();
            var food = db.Foods.Find(id);

            if(food != null)
            {
                
                
                if(food.StatusName == "Pending" || food.StatusName == "Accepted")
                {
                    if (food.ExpireTime <= DateTime.Now)
                    {
                        food.CompleteTime = DateTime.Now;
                        food.StatusName = "Expired";
                        db.SaveChanges();
                        return RedirectToAction("DonationHistory", new { filter = filter});
                    }
                    food.StatusName = "Cancelled";
                    food.CompleteTime = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("DonationHistory", new {filter=filter});
                }
            }
            
            return RedirectToAction("DonationHistory", new { filter = filter });
        }

        public ActionResult Filter()
        {
            if (Session["Username"] == null || Session["Access"].ToString() != "Benefactor")
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