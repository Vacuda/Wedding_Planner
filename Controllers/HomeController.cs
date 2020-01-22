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
    public class HomeController : Controller
    {
        private HomeContext dbContext;
     
        public HomeController(HomeContext context)
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

        [HttpGet("")]
        public IActionResult Index(){
            if(HttpContext.Session.GetInt32("User_Id")==null){
                HttpContext.Session.SetInt32("User_Id", 0);
            }
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(User new_user){
            if(ModelState.IsValid){

                //check if email already in use
                if(dbContext.Users.Any(e=>e.Email == new_user.Email)){
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                dbContext.Users.Add(new_user);

                //store password hash
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                new_user.Password = Hasher.HashPassword(new_user, new_user.Password);

                dbContext.SaveChanges();

                //change session
                User user = dbContext.Users.Last();
                HttpContext.Session.SetInt32("User_Id", user.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(Login login){
            if(ModelState.IsValid){

                //find user, if they are in database
                var user_in_db = dbContext.Users.FirstOrDefault(e=>e.Email == login.Login_Email);

                //no email found
                if(user_in_db == null){
                    ModelState.AddModelError("Login_Email", "Invalid Email Or Password");
                    return View("Index");
                }

                //check password
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(login, user_in_db.Password, login.Login_Password);

                //password false
                if(result == 0){
                    ModelState.AddModelError("Login_Email", "Invalid Email Or Password");
                    return View("Index");
                }

                //everything checked out
                else{
                    HttpContext.Session.SetInt32("User_Id", user_in_db.UserId);
                    return RedirectToAction("Success");
                }
            }

            // validations didn't work
            else{
                return View("Index");
            }
        }

        [HttpGet("Success")]
        public IActionResult Success(){

            //if not logged in
            if(HttpContext.Session.GetInt32("User_Id") == null || HttpContext.Session.GetInt32("User_Id") == 0){
                ModelState.AddModelError("Login_Email", "You must be signed in!");
                return View("Index");
            }
            //move to Planner controller
            return Redirect("/Planner");
        }

        [HttpGet("Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return View("Index");
        }















        
    }
}
