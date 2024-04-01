using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourierService_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private void UpdateLayout()
        {

            var riderCount = _context.Riders.Count();
            ViewBag.RiderCount = riderCount;

            var merchantCount = _context.Merchants.Count();
            ViewBag.MerchantCount = merchantCount;

            //pickup count
            var pickupCount = _context.Parcels.Where(p => p.Status == "Pickup Request").Count();
            ViewBag.PickupCount = pickupCount;

            //dispatch count
            var dispatchCount = _context.Parcels.Where(p => p.Status == "Dispatched").Count();
            ViewBag.DispatchCount = dispatchCount;

            //Transit Count
            var transitCount = _context.Parcels.Where(p => p.Status == "Transit").Count();
            ViewBag.TransitCount = transitCount;

            //delivered count
            var deliveredCount = _context.Parcels.Where(p => p.Status == "Delivered").Count();
            ViewBag.DeliveredCount = deliveredCount;

            //cancelled count
            var cancelledCount = _context.Parcels.Where(p => p.Status == "Cancelled").Count();
            ViewBag.CancelledCount = cancelledCount;

            //return count
            var returnCount = _context.Parcels.Where(p => p.Status == "Returned").Count();
            ViewBag.ReturnCount = returnCount;

            //total parcel
            var totalParcel = _context.Parcels.Count();
            ViewBag.TotalParcel = totalParcel;

            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);

            var todayPickupRequest = _context.Parcels
                .Where(p => p.PickupRequestDate >= todayStart && p.PickupRequestDate < tomorrowStart)
                .Count();

            ViewBag.TodayPickupRequest = todayPickupRequest;

            //Today Dispatched
            var todayDispatched = _context.Parcels
            .Where(p => p.DispatchDate >= todayStart && p.DispatchDate < tomorrowStart)
            .Count();
            ViewBag.TodayDispatched = todayDispatched;

            // Today Delivered
            var todayDelivered = _context.Parcels
                .Where(p => p.DeliveryDate >= todayStart && p.DeliveryDate < tomorrowStart)
                .Count();
            ViewBag.TodayDelivered = todayDelivered;

            // Today Cancelled
            //var todayCancelled = _context.Parcels
            //    .Where(p => p.CancelDate >= todayStart && p.CancelDate < tomorrowStart)
            //    .Count();
            //ViewBag.TodayCancelled = todayCancelled;

            // Today Returned
            var todayReturned = _context.Parcels
                .Where(p => p.ReturnDate >= todayStart && p.ReturnDate < tomorrowStart)
                .Count();
            ViewBag.TodayReturned = todayReturned;

            //all parcel list for today
            var todayParcelList = _context.Parcels
                .Where(p => p.PickupRequestDate >= todayStart && p.PickupRequestDate < tomorrowStart).Include(u => u.Merchant).Include(u => u.Rider)
                .ToList();
            ViewBag.TodayParcelList = todayParcelList;

            //parcel status based on parcel id



        }

        public IActionResult Index()
        {
            UpdateLayout();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                TempData["success"] = "Message Sent Successfully";
                return RedirectToAction("Index");
            }
            return View(contact);
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
            //var hub = _context.Hubs.FirstOrDefault(a => a.Email == email && a.Password == password);
            //if (hub != null)
            //{
            //    TempData["success"] = "Login Successful";
            //    if (IsRememberME == "on")
            //    {
            //        options.Expires = DateTime.Now.AddDays(7);
            //    }
            //    else
            //    {
            //        options.Expires = DateTime.Now.AddDays(1);
            //    }
            //    Response.Cookies.Append("HubId", hub.Id, options);
            //    Response.Cookies.Append("HubEmail", hub.Email, options);
            //    return RedirectToAction("Index", "Hub");
            //}
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
            return RedirectToAction("Index", "Home");

        }

        //Register Merchant
        public IActionResult RegisterMerchant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterMerchant(Merchant merchant, IFormFile? file)
        {
            if (merchant == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //check if merchant already exists
                var merchantExists = _context.Merchants.Where(u => u.Email == merchant.Email).FirstOrDefault();
                if (merchantExists != null)
                {
                    TempData["error"] = "Email Already Exists";
                    return View(merchant);
                }
                if (merchant.Password != merchant.ConfirmPassword)
                {
                    TempData["error"] = "Password and Confirm Password does not match";
                    return View(merchant);
                }

                //handle image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string MerchantPath = Path.Combine(wwwRootPath, @"Images\Merchant");
                    using (var FileSteam = new FileStream(Path.Combine(MerchantPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(FileSteam);
                    }
                    merchant.ImageUrl = @"\Images\Merchant\" + fileName;
                }
                else
                {
                    merchant.ImageUrl = "";
                }

                _context.Merchants.Add(merchant);
                _context.SaveChanges();
                TempData["success"] = "Welcome OnBoard";
                return RedirectToAction("Index");
            }
            else
            {
                return View(merchant);
            }
        }


        [HttpPost]
        public IActionResult ParcelStatus(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                TempData["error"] = "Parcel Not Found";
                return RedirectToAction("Index");
            }
            else
            {
                var status = parcel.Status;
                ViewBag.Status = status;
                TempData["warning"] = $"Parcel Status: {status} ";
            }
            return RedirectToAction("Index");
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
