using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourierService_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {

            if (Request.Cookies["AdminId"] != null)
            {
                TempData["success"] = "You are logged in as Admin";
                return RedirectToAction("Index", "Admin");
            }
            else if (Request.Cookies["RiderId"] != null)
            {
                TempData["success"] = "You are  logged in as Rider";
                return RedirectToAction("Index", "Rider");
            }
            else if (Request.Cookies["MerchantId"] != null)
            {
                TempData["success"] = "You are logged in as Merchant";
                return RedirectToAction("Index", "Merchant");
            }
            else if (Request.Cookies["HubId"] !=null)
            {
                TempData["success"] = "You are logged in as Hub";
                return RedirectToAction("Index", "Hub");
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public IActionResult Login(string email, string password, string IsRememberME)
        {

            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
            var rider = _context.Riders.FirstOrDefault(a => a.Email == email && a.Password == password);
            var merchant = _context.Merchants.FirstOrDefault(a => a.Email == email && a.Password == password);
            CookieOptions options = new CookieOptions();

            if (admin != null)
            {
                TempData["success"] = "Login Successful";
                if (IsRememberME == "on")
                {
                    options.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    options.Expires = DateTime.Now.AddDays(1);
                }
                Response.Cookies.Append("AdminId", admin.Id, options);
                Response.Cookies.Append("AdminEmail", admin.Email, options);
                return RedirectToAction("Index", "Admin");
            }




            if (rider != null)
            {
                TempData["success"] = "Login Successful";




                if (IsRememberME == "on")
                {
                    options.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    options.Expires = DateTime.Now.AddDays(1);
                }



                //store rider id and email in cookie
                Response.Cookies.Append("RiderId", rider.Id, options);
                Response.Cookies.Append("RiderEmail", rider.Email, options);



                return RedirectToAction("Index", "Rider");
            }
            else if (merchant != null)
            {
                TempData["success"] = "Login Successful";


                if (IsRememberME == "on")
                {
                    options.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    options.Expires = DateTime.Now.AddDays(1);
                }

                Response.Cookies.Append("MerchantId", merchant.Id, options);
                Response.Cookies.Append("MerchantEmail", merchant.Email, options);
                return RedirectToAction("Index", "Merchant");
            }

            //for hub login
            var hub = _context.Hubs.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (hub != null)
            {
                TempData["success"] = "Login Successful";
                if (IsRememberME == "on")
                {
                    options.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    options.Expires = DateTime.Now.AddDays(1);
                }
                Response.Cookies.Append("HubId", hub.Id, options);
                Response.Cookies.Append("HubEmail", hub.Email, options);
                return RedirectToAction("Index", "Hub");
            }
            else
            {
                TempData["error"] = "Invalid Email or Password";
                return View();
            }



        }

        public IActionResult Logout()
        {

            Response.Cookies.Delete("AdminId");
            Response.Cookies.Delete("AdminEmail");
            Response.Cookies.Delete("RiderId");
            Response.Cookies.Delete("RiderEmail");
            Response.Cookies.Delete("MerchantId");
            Response.Cookies.Delete("MerchantEmail");
            Response.Cookies.Delete("HubId");
            Response.Cookies.Delete("HubEmail");
            TempData["success"] = "Logout Successfully";
            return RedirectToAction("Login", "Home");

        }

        //forget password
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(string email, string password, string cpassword)
        {

            var admin = _context.Admins.FirstOrDefault(a => a.Email == email);
            var rider = _context.Riders.FirstOrDefault(a => a.Email == email);
            var merchant = _context.Merchants.FirstOrDefault(a => a.Email == email);
            if (admin != null)
            {
                //check password and confirm password
                if (password != cpassword)
                {
                    TempData["error"] = "Password and Confirm Password does not match";
                    return RedirectToAction("ForgetPassword", "Home");
                }
                admin.Password = password;
                _context.SaveChanges();
                TempData["success"] = "Password Updated Successfully";
                return RedirectToAction("Login", "Home");
            }
            else if (rider != null)
            {
                //check password and confirm password
                if (password != cpassword)
                {
                    TempData["error"] = "Password and Confirm Password does not match";
                    return RedirectToAction("ForgetPassword", "Home");
                }
                rider.Password = password;
                _context.SaveChanges();
                TempData["success"] = "Password Updated Successfully";
                return RedirectToAction("Login", "Home");
            }
            else if (merchant != null)
            {
                //check password and confirm password
                if (password != cpassword)
                {
                    TempData["error"] = "Password and Confirm Password does not match";
                    return RedirectToAction("ForgetPassword", "Home");
                }
                merchant.Password = password;
                _context.SaveChanges();
                TempData["success"] = "Password Updated Successfully";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["error"] = "Invalid Email";
                return RedirectToAction("ForgetPassword", "Home");
            }

        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
