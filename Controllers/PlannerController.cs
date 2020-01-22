using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Wedding_Planner.Controllers
{
    [Route("Planner")]
    public class PlannerController : Controller
    {
        private HomeContext dbContext;
     
        public PlannerController(HomeContext context)
        {
            dbContext = context;
        }

        public IActionResult Privacy(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

//pages

        [HttpGet("")]
        public IActionResult Dashboard(){
            if(HttpContext.Session.GetInt32("User_Id")==null || HttpContext.Session.GetInt32("User_Id")==0){
                ModelState.AddModelError("Email", "You must log in.");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.User = GetUser();

            Past_Wedding_Check();
            
            List<Wedding> weddings = dbContext.Weddings
                                    .Include(e=>e.RSVPs)
                                    .ThenInclude(e=>e.User)
                                    .ToList();
            
            return View("Dashboard", weddings);
        }

        [HttpGet("Wedding/Add")]
        public IActionResult Wedding_Add_Page(){
            ViewBag.User = GetUser();
            return View("Wedding_Add_Page");
        }

        [HttpGet("Wedding/View/{id}")]
        public IActionResult Wedding_View_Page(int id){

            Wedding wedding = dbContext.Weddings
                                .Include(e=>e.RSVPs)
                                .ThenInclude(e=>e.User)
                                .FirstOrDefault(e=>e.WeddingId == id);

            ViewBag.User = GetUser();
            return View("Wedding_View_Page", wedding);

        }


//actions

        [HttpPost("Create_Wedding")]
        public IActionResult Create_Wedding(Wedding wedding){
            if (ModelState.IsValid){
                User user = GetUser();
                wedding.UserId = user.UserId;
                dbContext.Weddings.Add(wedding);
                dbContext.SaveChanges();
                return Redirect("/Planner");
            }
            else {
                ViewBag.User = GetUser();
                return View("Wedding_Add_Page");
            }
        }

        [HttpGet("Delete_Wedding/{id}")]
        public IActionResult Delete_Wedding(int id){

            Wedding wedding = dbContext.Weddings.FirstOrDefault(e=>e.WeddingId == id);

            dbContext.Weddings.Remove(wedding);
            dbContext.SaveChanges();

            return Redirect("/Planner");
        }

        [HttpGet("Add_RSVP/{wed_id}")]
        public IActionResult Add_RSVP(int wed_id){
            User user = GetUser();

            RSVP rsvp = new RSVP();
            rsvp.WeddingId = wed_id;
            rsvp.UserId = user.UserId;

            dbContext.RSVPs.Add(rsvp);
            dbContext.SaveChanges();

            return Redirect("/Planner");
        }

        [HttpGet("Remove_RSVP/{wed_id}")]
        public IActionResult Remove_RSVP(int wed_id){
            User user = GetUser();

            RSVP rsvp = dbContext.RSVPs
                            .Where(e=>e.WeddingId == wed_id)
                            .Where(e=>e.UserId == user.UserId)
                            .FirstOrDefault();

            dbContext.RSVPs.Remove(rsvp);
            dbContext.SaveChanges();

            return Redirect("/Planner");
        }

        



        //Functions
        public User GetUser(){
            return dbContext.Users.FirstOrDefault(e=>e.UserId == HttpContext.Session.GetInt32("User_Id"));
        }

        public void Past_Wedding_Check(){

            List<Wedding> past_weddings = dbContext.Weddings
                                .Where(e=>e.Date <= DateTime.Now)
                                .ToList();
            if(past_weddings.Count != 0){
                foreach(var w in past_weddings){
                    dbContext.Weddings.Remove(w);
                    dbContext.SaveChanges();
                }
            }
        }




        






        
    }
}
