using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Exam2.Controllers
{
    public class HomeController : Controller
    {
        private MyContext context;
        public HomeController(MyContext mc)
        {
            context = mc;
        }

       [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("RegCheck")]
        public IActionResult RegCheck(User user)
        {
            User CheckUser = context.Users.FirstOrDefault(u => u.Email == user.Email);
            if(CheckUser !=null)
            {
                ModelState.AddModelError("Email", "Email already exist!");
                return View("Index");
            }
            else if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                context.Add(user);
                context.SaveChanges();
                HttpContext.Session.SetInt32("UID", user.UserId);
                return Redirect("Dashboard");
            }
            return View("Index");
        }

        [HttpPost("LogCheck")]
        public IActionResult LogCheck(LUser l)
        {
            if(ModelState.IsValid)
            {
                var userInDB = context.Users.FirstOrDefault(u => u.Email == l.LEmail);
                if(userInDB == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password!");
                    return View("Index");
                }
                
                var hasher = new PasswordHasher<LUser>();
                var result = hasher.VerifyHashedPassword(l, userInDB.Password, l.LPassword);
                if(result == 0)
                {
                   ModelState.AddModelError("LEmail", "Invalid Email/Password!");
                   return View("Index");
                }
                HttpContext.Session.SetInt32("UID", userInDB.UserId);
                return Redirect("Dashboard");
            } 
            return View("Index");
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("Logout");
            }
            
            ViewBag.User = context.Users.Include(ja=> ja.JoiningActivity).Include(ju => ju.CreatetedActivities).FirstOrDefault(user => user.UserId == (int)Sess);
            ViewBag.AllActivities = context.Activities.Include(a => a.Coordinator).Include(a => a.JoiningUser).OrderBy(a => a.Date).ToList();

            foreach(var d in ViewBag.AllActivities)
            {
                int ActivityId = d.ActivityId;
                if (d.Date < DateTime.Now)
                {
                    var Delete = d;
                    context.Remove(Delete);
                    context.SaveChanges();
                }
            }

            return View("Dashboard");
        }

        [HttpGet("NewActivityPage")]
        public IActionResult NewActivityPage()
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("Logout");
            }
            ViewBag.User = context.Users
            .FirstOrDefault(user => user.UserId == (int)Sess);
            return View("NewActivityPage");
        }
        
        [HttpPost("AddNewActivity")]
        public IActionResult AddNewActivity(ActivityModel a)
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("Logout");
            }
            if(a.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", "Your Activity cant be in the past!!!!!!");
                    return View("NewActivityPage");
                }
            if(ModelState.IsValid)
            {
                a.UserId = (int)Sess;
                context.Add(a);
                context.SaveChanges();
                return Redirect("Dashboard");
            }
            return View("AddNewActivity");
        }
         [HttpGet("Join/{UserId}/{ActivityId}")]
        public IActionResult Join(int UserId, int ActivityId)
        {
            UserActivity Join = new UserActivity();
            Join.UserId = UserId;
            Join.ActivityId = ActivityId;
            
            context.Add(Join);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("Leave/{UserId}/{ActivityId}")]
        public IActionResult Leave(int UserId, int ActivityId)
        {
            var Leave = context.UserActivities.Where(a => a.UserId == UserId).FirstOrDefault(a => a.ActivityId == ActivityId);
            context.Remove(Leave);
            context.SaveChanges();
            return RedirectToAction("Dashboard");

        }


        [HttpGet("Delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            var Delete = context.Activities.FirstOrDefault(a => a.ActivityId == ActivityId);
            context.Remove(Delete);
            context.SaveChanges();
            return RedirectToAction("Dashboard");

        }
        
        [HttpGet("About/{ActivityId}")]
        public IActionResult About(int ActivityId)
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("Logout");
            }
            User currentUser = context.Users.Include(ja => ja.JoiningActivity).ThenInclude(u => u.User).FirstOrDefault(user => user.UserId == (int)Sess);

            ActivityModel currentActivity = context.Activities.Include(a => a.JoiningUser).ThenInclude(a => a.JoiningActivity).FirstOrDefault(a => a.ActivityId == ActivityId);

            User Coordinator =  context.Users.FirstOrDefault(a => a.UserId == currentActivity.UserId);

            ViewBag.User = currentUser;
            ViewBag.ActivitieWithGuests = currentActivity;
            ViewBag.Coordinator = Coordinator;
            return View("About");

        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        
    }
}
