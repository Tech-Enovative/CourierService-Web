using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierService_Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }


        private void UpdateLayout()
        {

            var riderCount = _context.Riders.Count();
            ViewBag.RiderCount = riderCount;

            var merchantCount = _context.Merchants.Count();
            ViewBag.MerchantCount = merchantCount;

            //hub count
            var hubCount = _context.Hubs.Count();
            ViewBag.HubCount = hubCount;

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
            var todayCancelled = _context.Parcels
                .Where(p => p.CancelDate >= todayStart && p.CancelDate < tomorrowStart)
                .Count();
            ViewBag.TodayCancelled = todayCancelled;

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

          



        }
        //isAdminLogged in or not
        public bool IsAdminLoggedIn()
        {
            if (Request.Cookies["AdminId"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult Index()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            UpdateLayout();
            return View();
        }

        //complains
        public IActionResult Complain()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var complains = _context.Complain.Include(u => u.Merchant).ToList();
            if (complains == null)
            {
                return NotFound();
            }
            return View(complains);
        }

        public IActionResult AddAdmin()
        {

            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public IActionResult Merchant()
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchants = _context.Merchants.ToList();
            if (merchants == null)
            {
                return NotFound();
            }
            return View(merchants);
        }


        public IActionResult Rider()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var riders = _context.Riders.ToList();
            if (riders == null)
            {
                return NotFound();
            }
            return View(riders);

        }

        public IActionResult AddRider()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddRider(Rider rider, IFormFile? file)
        {

            if (rider == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                //check email exists or not
                var email = _context.Riders.FirstOrDefault(a => a.Email == rider.Email);
                if (email != null)
                {
                    TempData["error"] = "Email Already Exists";
                    return View(rider);
                }
                //handle image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string RiderPath = Path.Combine(wwwRootPath, @"Images\Rider");
                    using (var FileSteam = new FileStream(Path.Combine(RiderPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(FileSteam);
                    }
                    //upload to aws s3
                    var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                    var fileTransferUtility = new TransferUtility(s3Client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        FilePath = RiderPath + "\\" + fileName,
                        BucketName = "courierbuckets3",
                        Key = "Rider/" + fileName,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    fileTransferUtility.Upload(uploadRequest);
                    //after upload delete from local storage
                    System.IO.File.Delete(RiderPath + "\\" + fileName);
                    rider.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Rider/" + fileName;

                    //rider.ImageUrl = @"\Images\Rider\" + fileName;
                }
                else
                {
                    rider.ImageUrl = "";
                }

                _context.Riders.Add(rider);
                _context.SaveChanges();
                TempData["success"] = "Rider Added Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(rider);
            }

        }


        public IActionResult AddMerchant()
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


        [HttpPost]
        public IActionResult AddMerchant(Merchant merchant, IFormFile? file)
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
                    TempData["error"] = "Merchant Email Already Exists";
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

                    //upload to aws s3
                    var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                    var fileTransferUtility = new TransferUtility(s3Client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        FilePath = MerchantPath + "\\" + fileName,
                        BucketName = "courierbuckets3",
                        Key = "Merchant/" + fileName,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    fileTransferUtility.Upload(uploadRequest);
                    //after upload delete from local storage
                    System.IO.File.Delete(MerchantPath + "\\" + fileName);
                    merchant.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Merchant/" + fileName;


                    //merchant.ImageUrl = @"\Images\Merchant\" + fileName;
                }
                else
                {
                    merchant.ImageUrl = "";
                }

                _context.Merchants.Add(merchant);
                _context.SaveChanges();
                TempData["success"] = "Merchant Added Successfully";
                return RedirectToAction("Merchant");
            }
            else
            {
                return View(merchant);
            }
        }

        public IActionResult DeleteRider(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var rider = _context.Riders.Find(id);
            if (rider == null)
            {
                return NotFound();
            }

            // Find all parcels associated with the rider
            var parcels = _context.Parcels.Where(p => p.RiderId == id).ToList();

            // Set RiderId to null for all associated parcels
            foreach (var parcel in parcels)
            {
                parcel.RiderId = null;
            }

            // Save changes to update the parcels
            _context.SaveChanges();

            //delete from aws s3
            if (rider.ImageUrl != null)
            {
                string[] split = rider.ImageUrl.Split("/");
                string key = "Rider/" + split[split.Length - 1];
                var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                var fileTransferUtility = new TransferUtility(s3Client);
                fileTransferUtility.S3Client.DeleteObjectAsync("courierbuckets3", key);
            }

            // Remove the rider
            _context.Riders.Remove(rider);
            _context.SaveChanges();

            TempData["error"] = "Rider Deleted Successfully";
            return RedirectToAction("Rider");
        }


        //edit rider
        public IActionResult EditRider(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var rider = _context.Riders.Find(id);
            if (rider == null)
            {
                return NotFound();
            }
            return View(rider);
        }

        [HttpPost]
        public IActionResult EditRider(Rider rider, IFormFile? file)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (rider == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //handle image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string riderPath = Path.Combine(wwwRootPath, "Images", "Rider");

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(rider.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, rider.ImageUrl.TrimStart('~', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        //delete from aws s3
                        string[] split = rider.ImageUrl.Split("/");
                        string key = "Rider/" + split[split.Length - 1];
                        var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                        var fileTransferUtility = new TransferUtility(s3Client);
                        fileTransferUtility.S3Client.DeleteObjectAsync("courierbuckets3", key);



                    }

                    // Save new image
                    using (var fileStream = new FileStream(Path.Combine(riderPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    //upload to aws s3
                    var news3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                    var uploadfileTransferUtility = new TransferUtility(news3Client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        FilePath = riderPath + "\\" + fileName,
                        BucketName = "courierbuckets3",
                        Key = "Rider/" + fileName,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    uploadfileTransferUtility.Upload(uploadRequest);
                    //after upload delete from local storage
                    System.IO.File.Delete(riderPath + "\\" + fileName);
                    rider.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Rider/" + fileName;



                }

                // Update other properties
                _context.Riders.Update(rider);
                _context.SaveChanges();
                TempData["success"] = "Rider Updated Successfully";
                return RedirectToAction("Rider");
            }
            else
            {
                return View(rider);
            }
        }



        //Merchant



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
                return RedirectToAction("Home");
            }
            else
            {
                return View(merchant);
            }
        }


        //edit merchant
        public IActionResult EditMerchant(string? id)
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }
            var merchant = _context.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return View(merchant);
        }

        [HttpPost]
        public IActionResult EditMerchant(Merchant merchant, IFormFile? file)
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (merchant == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //handle image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //handle if prevoius image exist
                    if (merchant.ImageUrl != null)
                    {

                        string imagePath = Path.Combine(wwwRootPath, merchant.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        //delete from aws s3
                        string[] split = merchant.ImageUrl.Split("/");
                        string key = "Merchant/" + split[split.Length - 1];
                        var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                        var fileTransferUtility = new TransferUtility(s3Client);
                        fileTransferUtility.S3Client.DeleteObjectAsync("courierbuckets3", key);


                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string MerchantPath = Path.Combine(wwwRootPath, @"Images\Merchant");
                    using (var FileSteam = new FileStream(Path.Combine(MerchantPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(FileSteam);
                    }

                    //upload to aws s3
                    var news3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                    var updatedfileTransferUtility = new TransferUtility(news3Client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        FilePath = MerchantPath + "\\" + fileName,
                        BucketName = "courierbuckets3",
                        Key = "Merchant/" + fileName,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    updatedfileTransferUtility.Upload(uploadRequest);
                    //after upload delete from local storage
                    System.IO.File.Delete(MerchantPath + "\\" + fileName);
                    merchant.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Merchant/" + fileName;
                }


                _context.Merchants.Update(merchant);
                _context.SaveChanges();
                TempData["success"] = "Merchant Updated Successfully";
                return RedirectToAction("Merchant");
            }
            else
            {
                return View(merchant);
            }
        }


        public IActionResult DeleteMerchant(string? id)
        {

            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            // Find all parcels associated with the rider
            var parcels = _context.Parcels.Where(p => p.MerchantId == id).ToList();

            // Set RiderId to null for all associated parcels
            foreach (var parcel in parcels)
            {
                parcel.MerchantId = null;
            }

            // Save changes to update the parcels
            _context.SaveChanges();

            var merchant = _context.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }

            //delete from aws s3
            if (merchant.ImageUrl != null)
            {
                string[] split = merchant.ImageUrl.Split("/");
                string key = "Merchant/" + split[split.Length - 1];
                var s3Client = new AmazonS3Client("AKIAU6GDYMTHTIZML6UG", "9Mjr5N26gAtUX6aOyGBNy688zMgP9Dt46ndJOIh/", RegionEndpoint.USEast1);
                var fileTransferUtility = new TransferUtility(s3Client);
                fileTransferUtility.S3Client.DeleteObjectAsync("courierbuckets3", key);
            }


            _context.Merchants.Remove(merchant);
            _context.SaveChanges();
            TempData["error"] = "Merchant Deleted Successfully";
            return RedirectToAction("Merchant");
        }

        public IActionResult Parcel()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var parcels = _context.Parcels.Include(u => u.Merchant).Include(u => u.Rider).ToList();
            if (parcels == null)
            {
                return NotFound();
            }
            return View(parcels);
        }

        public IActionResult DeleteParcel(string? Id)
        {
            var parcel = _context.Parcels.Find(Id);
            if (parcel == null)
            {
                return NotFound();
            }
            _context.Parcels.Remove(parcel);
            _context.SaveChanges();
            TempData["error"] = "Parcel Deleted Successfully";
            return RedirectToAction("Parcel");
        }


        public IActionResult ChangePassword()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ResetPassword resetPassword)
        {
            if (resetPassword == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                var admin = _context.Admins.Find(resetPassword.Id);
                if (admin == null)
                {
                    return NotFound();
                }
                if (admin.Password == resetPassword.OldPassword)
                {
                    admin.Password = resetPassword.NewPassword;
                    _context.SaveChanges();
                    TempData["success"] = "Password Changed Successfully";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = "Old Password is Incorrect";
                    return View(resetPassword);
                }
            }
            else
            {
                return View(resetPassword);
            }

        }

       

        public IActionResult ApplicationUser()
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            var users = _context.Admins.ToList();
            if (users == null)
            {
                return NotFound();
            }
            return View(users);

        }
        
        
        //delete admin
        public IActionResult DeleteAdmin(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var admin = _context.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }
            _context.Admins.Remove(admin);
            _context.SaveChanges();
            TempData["error"] = "Admin Deleted Successfully";
            return RedirectToAction("ApplicationUser");
        }


        public IActionResult Queries()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var contacts = _context.Contacts.ToList();
            if (contacts == null)
            {
                return NotFound();
            }
            return View(contacts);
        }

        //delete query
        public IActionResult DeleteQuery(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            TempData["error"] = "Query Deleted Successfully";
            return RedirectToAction("Queries");
        }

        //hub

        public IActionResult Hub()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubs = _context.Hubs.ToList();
            if (hubs == null)
            {
                return NotFound();
            }
            return View(hubs);
        }


        //create hub
        public IActionResult CreateHub()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateHub(Hub hub)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (hub == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //check name already exist
                var name = _context.Hubs.FirstOrDefault(a => a.Name == hub.Name);
                if (name != null)
                {
                    TempData["error"] = "Hub Name Already Exists";
                    return View(hub);
                }
                //check email already exist
                var email = _context.Hubs.FirstOrDefault(a => a.Email == hub.Email);
                if (email != null)
                {
                    TempData["error"] = "Email Already Exists";
                    return View(hub);
                }
                hub.AdminId = Request.Cookies["AdminId"];
                hub.CreatedBy = Request.Cookies["AdminEmail"];
                _context.Hubs.Add(hub);
                _context.SaveChanges();
                TempData["success"] = "Hub Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(hub);
            }
        }

        //delete hub
        public IActionResult DeleteHub(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var hub = _context.Hubs.Find(id);
            if (hub == null)
            {
                return NotFound();
            }
            _context.Hubs.Remove(hub);
            _context.SaveChanges();
            TempData["error"] = "Hub Deleted Successfully";
            return RedirectToAction("Hub");
        }

        //edit hub
        public IActionResult EditHub(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var hub = _context.Hubs.Find(id);
            if (hub == null)
            {
                return NotFound();
            }
            return View(hub);
        }

        [HttpPost]
        public IActionResult EditHub(Hub hub)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (hub == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Hubs.Update(hub);
                _context.SaveChanges();
                TempData["success"] = "Hub Updated Successfully";
                return RedirectToAction("Hub");
            }
            else
            {
                return View(hub);
            }
        }

        

    }
}
