using Amazon.S3.Transfer;
using Amazon.S3;
using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon;

namespace CourierService_Web.Controllers
{
    public class RiderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RiderController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        private void UpdateLayout()
        {
            var riderId = HttpContext.Request.Cookies["RiderId"];
            if (string.IsNullOrEmpty(riderId))
            {
                return;
            }
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return;
            }

            ViewBag.TotalDispatched = _context.Parcels.Count(x => x.RiderId == riderId && x.Status == "Dispatched");
            ViewBag.TotalDelivered = _context.Parcels.Count(x => x.RiderId == riderId && x.Status == "Delivered");
            ViewBag.TotalExchanged = _context.Parcels.Count(x => x.RiderId == riderId && x.ExchangeId !=null);
            ViewBag.TotalReturned = _context.Parcels.Count(x => x.RiderId == riderId && x.ReturnId !=null);
            ViewBag.TotalParcel = _context.Parcels.Count(x => x.RiderId == riderId);

            // Today Dispatched Parcel
            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);
            ViewBag.TodayDispatched = _context.Parcels
                .Count(x => x.RiderId == riderId &&
                            x.DispatchDate >= todayStart &&
                            x.DispatchDate < tomorrowStart);

            // Today Delivered Parcel
            ViewBag.TodayDelivered = _context.Parcels
                .Count(x => x.RiderId == riderId &&
                            x.DeliveryDate >= todayStart &&
                            x.DeliveryDate < tomorrowStart);


            //parcel list for the rider
            ViewBag.ParcelList = _context.Parcels.Where(x => x.RiderId == riderId).ToList();

            //all parcel list for today by rider
            ViewBag.AllParcelList = _context.Parcels.Where(x => x.RiderId == riderId && x.DispatchDate >= todayStart && x.DispatchDate < tomorrowStart).Include(u => u.Merchant).Include(h=>h.Hub).ToList();


            //ViewBag.AllParcelList = _context.Parcels.Where(z => z.RiderId == riderId && y=> y.DispatchDate >= todayStart && x.DispatchDate < tomorrowStart).Include(u => u.Merchant).ToList();

            //all parcel list count for today by rider
            ViewBag.AllParcelListCount = _context.Parcels.Count(x => x.DispatchDate >= todayStart && x.DispatchDate < tomorrowStart && x.RiderId == riderId);

            //amount collected by rider today
            ViewBag.AmountCollected = _context.riderPayments.Where(x => x.RiderId == riderId && x.PaymentDate >= todayStart && x.PaymentDate < tomorrowStart).Sum(x => x.Amount);

            //hub received amount today
            ViewBag.HubReceivedAmount = _context.riderPayments.Where(x => x.RiderId == riderId && x.HubReceivedDate >= todayStart && x.HubReceivedDate < tomorrowStart).Sum(x => x.HubReceivedAmount);

            //hub due amount today
            //ViewBag.HubDueAmount = _context.riderPayments.Where(x => x.RiderId == riderId && x.HubDueDate >= todayStart && x.HubDueDate < tomorrowStart).Sum(x => x.HubDue);

            //calculate hub due amount accroding to HubReceivedAmount
            ViewBag.HubDue = ViewBag.AmountCollected - ViewBag.HubReceivedAmount;

            

        }


        private bool IsRiderLoggedIn()
        {

            var riderId = HttpContext.Request.Cookies["RiderId"];
            if (string.IsNullOrEmpty(riderId))
            {
                return false;
            }


            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return false;
            }

            return true;
        }




        public IActionResult Index()
        {

            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }
            UpdateLayout();
            return View();
        }

        //profile
        public IActionResult Profile()
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }
            var riderId = HttpContext.Request.Cookies["RiderId"];
            var rider = _context.Riders.Find(riderId);
            return View(rider);
        }

        //update profile
        [HttpPost]
        public IActionResult UpdateProfile(Rider rider, IFormFile? file)
        {
            if (!IsRiderLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            if (string.IsNullOrEmpty(riderId))
            {

                return RedirectToAction("Login", "Home");
            }

            var riderToUpdate = _context.Riders.Find(riderId);
            if (riderToUpdate == null)
            {

                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string riderPath = Path.Combine(wwwRootPath, "Images", "Rider");

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(riderToUpdate.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, riderToUpdate.ImageUrl.TrimStart('~', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        //delete from aws s3
                        string[] split = riderToUpdate.ImageUrl.Split("/");
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
                    riderToUpdate.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Rider/" + fileName;
                }

                // Update other rider information
                riderToUpdate.Name = rider.Name;
                riderToUpdate.NID = rider.NID;
                riderToUpdate.District = rider.District;
                riderToUpdate.Area = rider.Area;
                riderToUpdate.Email = rider.Email;
                riderToUpdate.ContactNumber = rider.ContactNumber;
                riderToUpdate.FullAddress = rider.FullAddress;

                _context.Riders.Update(riderToUpdate);
                _context.SaveChanges();
                TempData["success"] = "Profile Updated Successfully";
                return RedirectToAction("Profile");
            }


            return View("Profile", rider);
        }

        public IActionResult AllParcel(DateTime? startDate, DateTime? endDate)
        {
            if (!IsRiderLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Where(x => x.RiderId == riderId)
                .Include(u => u.Rider)
                .Include(m => m.Merchant)
                .Include(h => h.Hub);

           
            if (startDate.HasValue && endDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date >= startDate.Value.Date && x.PickupRequestDate.Value.Date <= endDate.Value.Date);
            }
            // If only start date is provided, filter from start date to today
            else if (startDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date >= startDate.Value.Date && x.PickupRequestDate.Value.Date <= DateTime.Today);
            }
            // If only end date is provided, filter from the beginning to end date
            else if (endDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date <= endDate.Value.Date);
            }
            // If no date range is provided, default to today
            else
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date == DateTime.Today);
            }

            var parcels = parcelsQuery.ToList();

            return View(parcels);
        }


        //status change to PickedUp
        public IActionResult PickedUp(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Picked Up";
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            //rider.State = "Busy";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }
        public IActionResult ParcelOntheWay(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel On The Way";
           
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }

        //status change to Delivered
        public IActionResult Delivered(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.DeliveryParcel = new DeliveredParcel
            {
                ParcelId = parcel.Id,
                RiderId = riderId,
                DeliveryDate = DateTime.Now.Date,
                HubId = parcel.HubId,
                MerchantId = parcel.MerchantId
            };
            parcel.Status = "Delivered";
            parcel.DeliveryDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }


        //status change to Parcel In Hub
        public IActionResult ParcelInHub(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel In Hub";
            parcel.DeliveryDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }
        //Cancel Parcel
        public IActionResult Cancel(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Cancelled";
            parcel.CancelDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }
        //return parcel
        public IActionResult Return(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);

            parcel.ReturnParcel = new ReturnParcel
            {
                ParcelId = parcel.Id,
                RiderId = riderId,
                ReturnDate = DateTime.Now,
                HubId = parcel.HubId,
                MerchantId = parcel.MerchantId
            };



            parcel.Status = "Returned";
            parcel.ReturnDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }
        //ReturnParcelToMerchant
        public IActionResult ReturnParcelToMerchant(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel Returned To Merchant";
            //parcel.ReturnDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }

        //ExchangeParcelToMerchant
        public IActionResult ExchangeParcelToMerchant(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Exchanged Parcel Returned Merchant";
            //parcel.ReturnDate = DateTime.Now.Date;
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }

        //parcel status change to Exchange
        public IActionResult Exchange(string id)
        {
            if (!IsRiderLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            var riderId = HttpContext.Request.Cookies["RiderId"];
            //find rider by riderId
            var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);

            parcel.ExchangeParcel = new ExchangeParcel
            {
                ParcelId = parcel.Id,
                RiderId = riderId,
                ExchangeDate = DateTime.Now,
                HubId = parcel.HubId,
                MerchantId = parcel.MerchantId
            };
            parcel.Status = "Exchanged";
            rider.State = "Available";
            _context.Parcels.Update(parcel);
            _context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");
        }

        public IActionResult ChangePassword()
        {
            if (!IsRiderLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public IActionResult Payment(string? id)
        {
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            //update payment status to paid
            parcel.PaymentStatus = "Payment Received By Rider";
            _context.Parcels.Update(parcel);

            var riderPayment = new RiderPayment
            {
                Amount = parcel.TotalPrice,
                ParcelId = parcel.Id,
                RiderId = parcel.RiderId
            };
            _context.riderPayments.Add(riderPayment);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");

        }

        //paymentDeliveryCharge
        public IActionResult PaymentDeliveryCharge(string? id)
        {
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            //update payment status to paid
            parcel.PaymentStatus = "Delivery Charge Received By Rider";
            var riderPayment = new RiderPayment
            {
                Amount = parcel.DeliveryCharge,
                ParcelId = parcel.Id,
                RiderId = parcel.RiderId
            };
            _context.riderPayments.Add(riderPayment);
            _context.SaveChanges();
            return RedirectToAction("AllParcel");

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
                var rider = _context.Riders.Find(resetPassword.Id);
                if (rider == null)
                {
                    return NotFound();
                }
                if (rider.Password == resetPassword.OldPassword && resetPassword.NewPassword == resetPassword.ConfirmPassword)
                {
                    rider.Password = resetPassword.NewPassword;
                    _context.SaveChanges();
                    TempData["success"] = "Password Changed Successfully";
                    return RedirectToAction("Index");
                }

                else if (rider.Password != resetPassword.OldPassword)
                {
                    TempData["error"] = "Old Password is Incorrect";
                    return View(resetPassword);
                }
                else if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
                {
                    TempData["error"] = "New Password and Confirm Password does not match";
                    return View(resetPassword);
                }
                else
                {
                    return View(resetPassword);
                }
            }
            else
            {
                return View(resetPassword);
            }



        }

        [HttpPost]
        public IActionResult SendPermissionRequest(string parcelId, int newPrice)
        {
            // Get the parcel and rider information
            var parcel = _context.Parcels.Find(parcelId);
            var riderId = HttpContext.Request.Cookies["RiderId"];

            // Check if the rider is authorized to edit the price
            if (parcel.RiderId != riderId)
            {
                return BadRequest("Unauthorized");
            }

            // Send permission request to the merchant
            var rider = _context.Riders.Find(riderId);
            var merchant = _context.Merchants.Find(parcel.MerchantId);
            var notification = new RequestPermission
            {
                Title = "Permission Request",
                Message = $"{rider.Name} has requested permission to change the price of {parcel.ProductName} to {newPrice} TK",
                SenderId = riderId,
                ReceiverId = merchant.Id,
                Date = DateTime.Now,
                ParcelId = parcelId,
                RequestedPrice = newPrice
                
            };
            _context.NotificationsPermission.Add(notification);
            _context.SaveChanges();
            TempData["success"] = "Permission request sent to merchant";

            return RedirectToAction("AllParcel"); // Redirect to the parcel list page
        }
    }
}
