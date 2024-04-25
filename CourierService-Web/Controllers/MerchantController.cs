using Amazon.S3.Transfer;
using Amazon.S3;
using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon;
using System.Text;
using System.Formats.Asn1;
using CsvHelper;
using System.Linq;
using Hangfire;
using OfficeOpenXml;

namespace CourierService_Web.Controllers
{
    public class MerchantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MerchantController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        private void UpdateLayout()
        {

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            if (string.IsNullOrEmpty(merchantId))
            {
                return;
            }
            var merchant = _context.Merchants.Find(merchantId);
            if (merchant == null)
            {
                return;
            }
            ViewBag.TotalPickupRequest = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Pending");
            ViewBag.TotalDispatched = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Dispatched");
            ViewBag.TotalTransit = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "In Transit");
            ViewBag.TotalDelivered = _context.Parcels.Count(x => x.MerchantId == merchantId && x.DeliveryId !=null);
            ViewBag.TotalExchanged = _context.Parcels.Count(x => x.MerchantId == merchantId && x.ExchangeId != null);
            ViewBag.TotalCancelled = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Cancelled");
            ViewBag.TotalReturned = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Returned");
            ViewBag.TotalParcel = _context.Parcels.Count(x => x.MerchantId == merchantId);

            //parcel list for the merchant
            ViewBag.ParcelList = _context.Parcels.Where(x => x.MerchantId == merchantId).ToList();

            // Today Pickup Request
            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);
            ViewBag.TodayPickupRequest = _context.Parcels
                .Count(x => x.MerchantId == merchantId &&
                            x.PickupRequestDate >= todayStart &&
                            x.PickupRequestDate < tomorrowStart);

            // Today Dispatched Parcel
            ViewBag.TodayDispatched = _context.Parcels
                .Count(x => x.MerchantId == merchantId &&
                            x.DispatchDate >= todayStart &&
                            x.DispatchDate < tomorrowStart);

            // Today Delivered Parcel
            ViewBag.TodayDelivered = _context.Parcels
                .Count(x => x.MerchantId == merchantId &&
                            x.DeliveryParcel.DeliveryDate >= todayStart &&
                            x.DeliveryParcel.DeliveryDate < tomorrowStart);

            // Today Cancelled Parcel
            ViewBag.TodayCancelled = _context.Parcels
                .Count(x => x.MerchantId == merchantId &&
                            x.Status == "Cancelled" &&
                            x.DeliveryDate >= todayStart &&
                            x.DeliveryDate < tomorrowStart);

            // Today Returned Parcel
            ViewBag.TodayReturned = _context.Parcels
                .Count(x => x.MerchantId == merchantId &&
                            x.Status == "Returned" &&
                            x.DeliveryDate >= todayStart &&
                            x.DeliveryDate < tomorrowStart);

            //today on transit parcel
            ViewBag.TodayTransit = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Transit");

            //parcel in hub
            ViewBag.ParcelInHub = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "At The Hub Received");

            //parcel on the way
            ViewBag.ParcelOnTheWay = _context.Parcels.Count(x => x.MerchantId == merchantId && x.Status == "Parcel On The Way");


            //all parcel list for today
            ViewBag.TodayParcelList = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.PickupRequestDate >= todayStart && x.PickupRequestDate < tomorrowStart).Include(x => x.Rider).Include(h=>h.Hub).ToList();

            //count of all parcel for today
            ViewBag.TodayTotalParcel = _context.Parcels
                .Count(x => x.MerchantId == merchantId && x.PickupRequestDate >= todayStart && x.PickupRequestDate < tomorrowStart);

            //return parcel count
            ViewBag.ReturnParcelCount = _context.Parcels.Count(x => x.MerchantId == merchantId && x.ReturnId !=null);
            //if paymentStatus paid then merchant can see the amount only for today according to delivered parcel date
            

            var todayDeliveryCharge = _context.Parcels
    .Where(x => x.MerchantId == merchantId &&
                (x.PaymentStatus == "Paid To Merchant" || x.PaymentStatus == "Paid Delivery Charge To Merchant") &&
                (x.DeliveryParcel.DeliveryDate >= todayStart && x.DeliveryParcel.DeliveryDate < tomorrowStart) ||
                (x.ExchangeParcel.ExchangeDate >= todayStart && x.ExchangeParcel.ExchangeDate < tomorrowStart) ||
                (x.ReturnParcel.ReturnDate >= todayStart && x.ReturnParcel.ReturnDate < tomorrowStart)
                
                )
    .Sum(x => x.DeliveryCharge);

            ViewBag.TodayDeliveryCharge = todayDeliveryCharge;


            var todayExchangeReturnCharge = _context.Parcels
    .Where(x => x.MerchantId == merchantId &&
                (x.PaymentStatus == "Paid Delivery Charge To Merchant") &&
                (x.ExchangeParcel.ExchangeDate >= todayStart && x.ExchangeParcel.ExchangeDate < tomorrowStart) ||
                (x.ReturnParcel.ReturnDate >= todayStart && x.ReturnParcel.ReturnDate < tomorrowStart)

                )
    .Sum(x => x.DeliveryCharge);

           




            //ViewBag.TodayDeliveryCharge = _context.Parcels.Where(x => x.MerchantId == merchantId && x.PaymentStatus == "Paid To Merchant" || x.PaymentStatus == "Paid Delivery Charge To Merchant" && x.DeliveryParcel.DeliveryDate >= todayStart && x.DeliveryParcel.DeliveryDate < tomorrowStart).Sum(x => x.DeliveryCharge);


            //calculate total product price for today if payment status is paid & also with quantity
            var totalProductPrice = _context.Parcels.Where(x => x.MerchantId == merchantId && x.PaymentStatus == "Paid To Merchant" && x.DeliveryParcel.DeliveryDate >= todayStart && x.DeliveryParcel.DeliveryDate < tomorrowStart).Sum(x => x.ProductPrice * x.ProductQuantity);
            ViewBag.TodayProductPrice = totalProductPrice;

            //ViewBag.TodayProductPrice = _context.Parcels.Where(x => x.MerchantId == merchantId && x.PaymentStatus == "Paid" && x.DeliveryParcel.DeliveryDate >= todayStart && x.DeliveryParcel.DeliveryDate < tomorrowStart).Sum(x => x.ProductPrice);
            ViewBag.TodayPayment = totalProductPrice + todayDeliveryCharge;

            //notification
            ViewBag.Notifications = _context.NotificationsPermission.Where(x => x.ReceiverId == merchantId).OrderByDescending(x => x.Date).Include(p=>p.Parcel).Include(x=>x.Parcel.Rider).ToList();

            //amount collected by hub according to merchant id
            ViewBag.TotalAmountCollected = _context.HubPayments.Where(x => x.Hub.Merchants.Any(x => x.Id == merchantId)).Sum(x => x.TotalAmount);

            //total price sum calculation from parcel according to merchant id today
            ViewBag.TotalPriceSum = _context.Parcels.Where(x => x.MerchantId == merchantId && x.PickupRequestDate >=todayStart && x.PickupRequestDate < tomorrowStart ).Sum(x => x.TotalPrice);
            //amount paid by merchant today
            ViewBag.AmountPaid = _context.MerchantPayments.Where(x => x.MerchantId == merchantId && x.DateTime >= todayStart && x.DateTime < tomorrowStart).Sum(x => x.AmountPaid);
            //calculate due amount
            ViewBag.DueAmount = ViewBag.TotalPriceSum - ViewBag.AmountPaid;

        }
        public bool IsMerchantLoggedIn()
        {
            if (Request != null && Request.Cookies["MerchantId"] != null)
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
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            UpdateLayout();
            return View();
        }

        public IActionResult Profile()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            var merchant = _context.Merchants.Find(merchantId);
            return View(merchant);

        }

        //Return Parcel List
        public IActionResult ReturnParcelList(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> returnParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.ReturnId != null)
                .Include(x => x.ReturnParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if(!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            returnParcelsQuery = returnParcelsQuery.Where(x => x.ReturnParcel.ReturnDate.Date == selectedDate.Value.Date);
            var returnParcels = returnParcelsQuery.ToList();

            ViewBag.SelectedDate = selectedDate.Value.Date;

            return View(returnParcels);

            
            
        }

        public IActionResult DownloadReturnParcelsCsv(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> returnParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.ReturnId != null)
                .Include(x => x.ReturnParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }

            returnParcelsQuery = returnParcelsQuery.Where(x => x.ReturnParcel.ReturnDate.Date == selectedDate.Value.Date);
            var returnParcels = returnParcelsQuery.ToList();

            // Generate CSV content
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Hub,Rider,Pickup Location,Pickup Request Date,Return Date,Receiver Name,Receiver Address,Receiver Contact,Product Name,Product Weight,Product Price,Product Quantity,Delivery Type,Delivery Charge,Total Price,Status,Payment Status");
            foreach (var parcel in returnParcels)
            {
                csvContent.AppendLine($"{parcel.ReturnId}, {parcel.Hub?.Name ?? "Not Assigned"}, {parcel.Rider?.Name ?? "Not Assigned"}, {parcel.PickupLocation}, {parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}, {parcel.ReturnParcel.ReturnDate.ToString("M/d/yyyy, h:mm tt")}, {parcel.ReceiverName}, {parcel.ReceiverAddress}, {parcel.ReceiverContactNumber}, {parcel.ProductName}, {parcel.ProductWeight}, {parcel.ProductPrice}, {parcel.ProductQuantity}, {parcel.DeliveryType}, {parcel.DeliveryCharge}, {parcel.TotalPrice}, {parcel.Status}, {parcel.PaymentStatus}");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"Return_Parcels_{selectedDate.Value.ToString("yyyy-MM-dd")}.csv");
        }

        //Exchange Parcel List
        public IActionResult ExchangeParcelList(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> exchangeParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.ExchangeId != null)
                .Include(x => x.ExchangeParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);
            if(!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            exchangeParcelsQuery = exchangeParcelsQuery.Where(x => x.ExchangeParcel.ExchangeDate.Date == selectedDate.Value.Date);
            var exchangeParcels = exchangeParcelsQuery.ToList();
            ViewBag.SelectedDate = selectedDate.Value.Date;
            return View(exchangeParcels);

         
        }

        public IActionResult DownloadExchangeCsv(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> exchangeParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.ExchangeId != null)
                .Include(x => x.ExchangeParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }

            exchangeParcelsQuery = exchangeParcelsQuery.Where(x => x.ExchangeParcel.ExchangeDate.Date == selectedDate.Value.Date);

            var exchangeParcels = exchangeParcelsQuery.ToList();

            
            StringBuilder csvContent = new StringBuilder();

            
            csvContent.Append("\uFEFF");

            
            csvContent.AppendLine("ID, Hub, Rider, Pickup Location, Pickup Request Date, Exchange Date, Receiver Name, Receiver Address, Receiver Contact, Product Name, Product Weight, Product Price, Product Quantity, Delivery Type, Delivery Charge, Total Price, Status, Payment Status");

            
            foreach (var parcel in exchangeParcels)
            {
                csvContent.AppendLine($"{parcel.Id}, {parcel.Hub?.Name ?? "Not Assigned"}, {parcel.Rider?.Name ?? "Not Assigned"}, {parcel.PickupLocation}, {parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}, {parcel.ExchangeParcel.ExchangeDate.ToString("M/d/yyyy, h:mm tt")}, {parcel.ReceiverName}, {parcel.ReceiverAddress}, {parcel.ReceiverContactNumber}, {parcel.ProductName}, {parcel.ProductWeight}, {parcel.ProductPrice}, {parcel.ProductQuantity}, {parcel.DeliveryType}, {parcel.DeliveryCharge}, {parcel.TotalPrice}, {parcel.Status}, {parcel.PaymentStatus}");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"ExchangeParcels_{selectedDate.Value.ToString("yyyy-MM-dd")}.csv");
        }


        //delivered parcel list
        public IActionResult DeliveredParcelList(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> deliveredParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.DeliveryId != null)
                .Include(x => x.DeliveryParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            
                deliveredParcelsQuery = deliveredParcelsQuery.Where(x => x.DeliveryParcel.DeliveryDate.Date == selectedDate.Value.Date);
                var deliveredParcels = deliveredParcelsQuery.ToList();

                ViewBag.SelectedDate = selectedDate.Value.Date;
                return View(deliveredParcels);

           

            
        }

        public IActionResult DownloadDeliveredParcelsCsv(DateTime? selectedDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> deliveredParcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.DeliveryId != null)
                .Include(x => x.DeliveryParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }

            deliveredParcelsQuery = deliveredParcelsQuery.Where(x => x.DeliveryParcel.DeliveryDate.Date == selectedDate.Value.Date);
            var deliveredParcels = deliveredParcelsQuery.ToList();

            // Generate CSV content
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Hub,Rider,Pickup Location,Pickup Request Date,Delivery Date,Receiver Name,Receiver Address,Receiver Contact,Product Name,Product Weight,Product Price,Product Quantity,Delivery Type,Delivery Charge,Total Price,Status");
            foreach (var parcel in deliveredParcels)
            {
                // Ensure proper formatting of text fields by enclosing them in double quotes
                csvContent.AppendLine($"\"{parcel.Id}\",\"{parcel.Hub?.Name ?? "Not Assigned"}\",\"{parcel.Rider?.Name ?? "Not Assigned"}\",\"{parcel.PickupLocation}\",\"{parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}\",\"{parcel.DeliveryParcel.DeliveryDate.ToString("M/d/yyyy, h:mm tt")}\",\"{parcel.ReceiverName}\",\"{parcel.ReceiverAddress}\",\"{parcel.ReceiverContactNumber}\",\"{parcel.ProductName}\",{parcel.ProductWeight},{parcel.ProductPrice},{parcel.ProductQuantity},\"{parcel.DeliveryType}\",{parcel.DeliveryCharge},{parcel.TotalPrice},\"{parcel.Status}\"");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"Delivered_Parcels_{selectedDate.Value.ToString("yyyy-MM-dd")}.csv");
        }





        //update profile
        [HttpPost]
        public IActionResult UpdateProfile(Merchant merchant, IFormFile? file)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            if (string.IsNullOrEmpty(merchantId))
            {

                return RedirectToAction("Login", "Home");
            }

            var merchantToUpdate = _context.Merchants.Find(merchantId);
            if (merchantToUpdate == null)
            {

                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string riderPath = Path.Combine(wwwRootPath, "Images", "Merchant");

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(merchantToUpdate.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, merchantToUpdate.ImageUrl.TrimStart('~', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        //delete from aws s3
                        string[] split = merchantToUpdate.ImageUrl.Split("/");
                        string key = "Merchant/" + split[split.Length - 1];
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
                    var updatedfileTransferUtility = new TransferUtility(news3Client);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        FilePath = riderPath + "\\" + fileName,
                        BucketName = "courierbuckets3",
                        Key = "Merchant/" + fileName,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    updatedfileTransferUtility.Upload(uploadRequest);
                    //after upload delete from local storage
                    System.IO.File.Delete(riderPath + "\\" + fileName);
                    merchantToUpdate.ImageUrl = "https://courierbuckets3.s3.amazonaws.com/Merchant/" + fileName;


                }

                // Update other rider information
                merchantToUpdate.Name = merchant.Name;
                merchantToUpdate.CompanyName = merchant.CompanyName;


                merchantToUpdate.Email = merchant.Email;
                merchantToUpdate.ContactNumber = merchant.ContactNumber;
                merchantToUpdate.FullAddress = merchant.FullAddress;

                _context.Merchants.Update(merchantToUpdate);
                _context.SaveChanges();
                return RedirectToAction("Profile");
            }

            // If ModelState is not valid, return to the profile page with validation errors
            return View("Profile", merchant);
        }

        //PArcel Statu Changed to Return receive by merchant
        public IActionResult ReturnReceivedByMerchant(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return RedirectToAction("Parcels");
            }

            parcel.Status = "Return Received By Merchant";
            parcel.ReturnReceivedByMerchantAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

           

            return RedirectToAction("Parcels");
        }



        public IActionResult AddNewParcel()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            //parcel with hub information
            ViewBag.HubList = _context.Hubs.ToList();
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
           //merchant area
           var merchant = _context.Merchants.Find(merchantId);
            ViewBag.MerchantArea = merchant.Area;
            //merhcant full address
            ViewBag.MerchantFullAddress = merchant.FullAddress;

            //count total percel and delivery count according to ReceiverContactNumber
            var parcelCount = _context.Parcels.Count(x => x.ReceiverContactNumber == merchant.ContactNumber);
            ViewBag.ParcelCount = parcelCount;
            var deliveryCount = _context.Parcels.Count(x => x.ReceiverContactNumber == merchant.ContactNumber && x.DeliveryId != null);
            ViewBag.DeliveryCount = deliveryCount;

            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Area = _context.Areas.ToList();

            ViewBag.InsideDhakaDeliveryCharge = merchant.InsideDhakaDeliveryCharge;
            ViewBag.SubDhakaDeliveryCharge = merchant.SubDhakaDeliveryCharge;
            ViewBag.OutsideDhakaDeliveryCharge = merchant.OutsideDhakaDeliveryCharge;
            ViewBag.InDhakaEmergencyDeliveryCharge = merchant.InDhakaEmergencyDeliveryCharge;
            ViewBag.P2PDeliveryCharge = merchant.P2PDeliveryCharge;
            ViewBag.MaxiumWeight = merchant.MaxiumWeight;
            ViewBag.ExtraWeightCharge = merchant.ExtraWeightCharge;

            //find stores of the merchant
            ViewBag.Stores = _context.Stores.Where(x => x.MerchantId == merchantId).ToList();


            return View();

            
        }

        [HttpGet]
        public IActionResult GetZonesByDistrict(string districtId)
        {
            var zones = _context.Zone.Where(z => z.DistrictId == districtId).ToList();
            return Json(zones);
        }

        [HttpGet]
        public IActionResult GetAreasByZone(string zoneId)
        {
            var areas = _context.Areas.Where(a => a.ZoneId == zoneId).ToList();
            return Json(areas);
        }

        //GetHubsByDistrict
        [HttpGet]
        public IActionResult GetHubsByDistrict(string districtId)
        {
            var hubs = _context.Hubs.Where(h => h.DistrictId == districtId).ToList();
            return Json(hubs);
        }

        [HttpGet]
        public IActionResult GetParcelDeliveryCounts(string receiverContactNumber)
        {
            var parcelCount = _context.Parcels.Count(x => x.ReceiverContactNumber == receiverContactNumber);
            var deliveryCount = _context.Parcels.Count(x => x.ReceiverContactNumber == receiverContactNumber && x.DeliveryId != null);

            //calulate delivery success rate based on return count
            var returnCount = _context.Parcels.Count(x => x.ReceiverContactNumber == receiverContactNumber && x.ReturnId != null);
            var successRate = 0;
            if (parcelCount > 0)
            {
                successRate = ((parcelCount - returnCount) * 100) / parcelCount; // Exclude returned parcels from success rate calculation
            }

            receiverContactNumber = receiverContactNumber.Substring(0, 3) + "*****" + receiverContactNumber.Substring(receiverContactNumber.Length - 3);
            return Json(new { parcelCount, deliveryCount, successRate, returnCount, receiverContactNumber });

            

           
        }
        [HttpPost]
        public IActionResult AddParcel(Parcel parcel)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Get the merchant's ID
            var merchantId = HttpContext.Request.Cookies["MerchantId"];

            // Find the merchant
            var merchant = _context.Merchants.Find(merchantId);

            //find hub according to store id
            var store = _context.Stores.Find(parcel.StoreId);
            var hub = _context.Hubs.Find(store.HubId);
            //find hub id and set
            parcel.HubId = hub.Id;
            //find area id and set
            var area = _context.Areas.Find(parcel.AreaId);
            parcel.AreaId = area.Id;

            //find zone id and set
            var zone = _context.Zone.Find(parcel.ZoneId);
            parcel.ZoneId = zone.Id;

            //find district id and set
            var district = _context.District.Find(parcel.DistrictId);
            parcel.DistrictId = district.Id;

            parcel.PickupRequestDate = DateTime.Now;


            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddNewParcel");    
            }

            // Add the parcel to the context and save changes
            _context.Parcels.Add(parcel);
            _context.SaveChanges();

            TempData["success"] = "Parcel Added Successfully";
            return RedirectToAction("Index");
        }

        //Edit Parcel
        public IActionResult EditParcel(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return RedirectToAction("Parcels");
            }

            //parcel with hub information
            ViewBag.HubList = _context.Hubs.ToList();
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            //merchant area
            var merchant = _context.Merchants.Find(merchantId);
            ViewBag.MerchantArea = merchant.Area;
            //merhcant full address
            ViewBag.MerchantFullAddress = merchant.FullAddress;

            //count total percel and delivery count according to ReceiverContactNumber
            var parcelCount = _context.Parcels.Count(x => x.ReceiverContactNumber == merchant.ContactNumber);
            ViewBag.ParcelCount = parcelCount;
            var deliveryCount = _context.Parcels.Count(x => x.ReceiverContactNumber == merchant.ContactNumber && x.DeliveryId != null);
            ViewBag.DeliveryCount = deliveryCount;

            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Area = _context.Areas.ToList();

            //find stores of the merchant
            ViewBag.Stores = _context.Stores.Where(x => x.MerchantId == merchantId).ToList();

            return View(parcel);
        }

        [HttpPost]
        public IActionResult UpdateParcel(Parcel parcel)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Get the merchant's ID
            var merchantId = HttpContext.Request.Cookies["MerchantId"];

            // Find the merchant
            var merchant = _context.Merchants.Find(merchantId);

            parcel.MerchantId = merchant.Id;

            //find hub according to store id
            var store = _context.Stores.Find(parcel.StoreId);

            //find hub id and set
            parcel.HubId = store.HubId;
            //find area id and set
            var area = _context.Areas.Find(parcel.AreaId);
            parcel.AreaId = area.Id;

            //find zone id and set
            var zone = _context.Zone.Find(parcel.ZoneId);
            parcel.ZoneId = zone.Id;

            //find district id and set
            var district = _context.District.Find(parcel.DistrictId);
            parcel.DistrictId = district.Id;

            parcel.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                return RedirectToAction("EditParcel", new { id = parcel.Id });
            }

            // Update the parcel in the context and save changes
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Parcel Updated Successfully";
            return RedirectToAction("Parcels");
        }


        public IActionResult DownloadTemplate()
        {
            // CSV content with headers and sample data
            var csvContent = "Store,ReceiverName,ReceiverAddress,ReceiverContactNumber,District,Zone,Area,ProductName,ProductWeight,ProductPrice,ProductQuantity\n";
            csvContent += "NewStore,John Doe,123 Main St,1234567890,Dhaka,Mirpur,Mirpur,Example Product,2,100,1\n";
            // Create a byte array from the CSV content
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            // Return the CSV file as a downloadable file
            return File(bytes, "text/csv", "parcel_template.csv");
        }

        //public IActionResult DownloadTemplate()
        //{
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Parcels");

        //        // Set headers
        //        worksheet.Cells["A1"].Value = "Store";
        //        worksheet.Cells["B1"].Value = "ReceiverName";
        //        worksheet.Cells["C1"].Value = "ReceiverAddress";
        //        worksheet.Cells["D1"].Value = "ReceiverContactNumber";
        //        worksheet.Cells["E1"].Value = "District"; // Dropdown list will be set for this column
        //        worksheet.Cells["F1"].Value = "Zone"; // Dropdown list will be set for this column
        //        worksheet.Cells["G1"].Value = "Area"; // Dropdown list will be set for this column
        //        worksheet.Cells["H1"].Value = "ProductName";
        //        worksheet.Cells["I1"].Value = "ProductWeight";
        //        worksheet.Cells["J1"].Value = "ProductPrice";
        //        worksheet.Cells["K1"].Value = "ProductQuantity";

        //        // Fetch district and zone data from the database
        //        var districts = _context.District.ToList();
        //        var zones = _context.Zone.ToList();

        //        // Define data validation ranges
        //        var districtRange = worksheet.DataValidations.AddListValidation("E2:E1048576"); // Apply to entire District column
        //        var zoneRange = worksheet.DataValidations.AddListValidation("F2:F1048576"); // Apply to entire Zone column

        //        // Populate district dropdown
        //        foreach (var district in districts)
        //        {
        //            districtRange.Formula.Values.Add(district.Name);
        //        }

        //        // Define event handler for when a district is selected
        //        districtRange.Formula.ExcelFormula = "IF(E2=\"\", \"\", INDIRECT(TEXTJOIN(\"\", TRUE, \"'\", E2, \"'!$A$2:$A$\", COUNTIF(E2:E1048576, E2))))";

        //        // Populate zone dropdown
        //        foreach (var zone in zones)
        //        {
        //            zoneRange.Formula.Values.Add(zone.Name);
        //        }

        //        // Define event handler for when a zone is selected
        //        zoneRange.Formula.ExcelFormula = "IF(F2=\"\", \"\", INDIRECT(TEXTJOIN(\"\", TRUE, \"'\", F2, \"'!$A$2:$A$\", COUNTIF(F2:F1048576, F2))))";

        //        // Sample data
        //        worksheet.Cells["A2"].Value = "NewStore";
        //        worksheet.Cells["B2"].Value = "John Doe";
        //        worksheet.Cells["C2"].Value = "123 Main St";
        //        worksheet.Cells["D2"].Value = "1234567890";
        //        worksheet.Cells["H2"].Value = "Example Product";
        //        worksheet.Cells["I2"].Value = 2;
        //        worksheet.Cells["J2"].Value = 100;
        //        worksheet.Cells["K2"].Value = 1;

        //        // Convert the Excel package to a byte array
        //        var bytes = package.GetAsByteArray();

        //        // Return the Excel file as a downloadable file
        //        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "parcel_template.xlsx");
        //    }
        //}











        //AddBulkParcels
        public IActionResult AddBulkParcels()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // Method to calculate delivery charge based on product weight and delivery type
        // Method to calculate delivery charge based on product weight and delivery type
        private decimal CalculateDeliveryCharge(decimal productWeight, string deliveryType)
        {
            decimal deliveryCharge = 0;

            // Define delivery charges based on delivery types
            decimal insideDhakaChargePerKg = 15m;
            decimal subDhakaChargePerKg = 20m;
            decimal outsideDhakaChargePerKg = 25m;
            decimal inDhakaEmergencyChargePerKg = 20m;
            decimal p2pChargePerKg = 30m;

            // Calculate delivery charge based on delivery type and product weight
            if (deliveryType == "InsideDhaka")
            {
                deliveryCharge = 70m; // Base charge for Inside Dhaka
                if (productWeight > 1)
                {
                    deliveryCharge += (productWeight - 1) * insideDhakaChargePerKg;
                }
            }
            else if (deliveryType == "SubDhaka")
            {
                deliveryCharge = 90m; // Base charge for Sub Dhaka
                if (productWeight > 1)
                {
                    deliveryCharge += (productWeight - 1) * subDhakaChargePerKg;
                }
            }
            else if (deliveryType == "OutsideDhaka")
            {
                deliveryCharge = 120m; // Base charge for Outside Dhaka
                if (productWeight > 1)
                {
                    deliveryCharge += (productWeight - 1) * outsideDhakaChargePerKg;
                }
            }
            else if (deliveryType == "InDhakaEmergency")
            {
                deliveryCharge = 100m; // Base charge for In Dhaka Emergency
                if (productWeight > 1)
                {
                    deliveryCharge += (productWeight - 1) * inDhakaEmergencyChargePerKg;
                }
            }
            else if (deliveryType == "P2P")
            {
                deliveryCharge = 200m; // Base charge for P2P
                if (productWeight > 1)
                {
                    deliveryCharge += (productWeight - 1) * p2pChargePerKg;
                }
            }

            return deliveryCharge;
        }


        // Method to calculate COD charge based on product price
        private decimal CalculateCOD(decimal productPrice)
        {
            // Calculate COD charge as a percentage of the product price
            // Example calculation: 10% of the product price
            decimal codCharge = productPrice * 0.01m;
            return codCharge;
        }

        // Method to calculate total price including product price, delivery charge, and COD charge
        private decimal CalculateTotalPrice(decimal productPrice, decimal codCharge, decimal deliveryCharge)
        {
            // Calculate total price by adding product price, delivery charge, and COD charge
            decimal totalPrice = productPrice + deliveryCharge + codCharge;
            return totalPrice;
        }

        //pickup failed if merchant did'nt give parcel in 2 min from pickup request date
        public IActionResult PickupFailed()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            var parcels = _context.Parcels
                .Where(x => x.MerchantId == merchantId && x.Status == "Assigned A Rider For Pickup")
                .ToList();

            // Calculate the cutoff time (1 minute from pickup request date)
            var cutoffTime = DateTime.Now.AddMinutes(-1);

            foreach (var parcel in parcels)
            {
                if (parcel.PickupRequestDate < cutoffTime)
                {
                    parcel.Status = "Pickup Failed";
                    _context.Parcels.Update(parcel);
                }
            }

            _context.SaveChanges();
            TempData["success"] = "Pickup Failed Parcels Updated Successfully";
            return RedirectToAction("Index");
        }

        public IActionResult ScheduleBackgroundJob()
        {
            // Schedule the PickupFailed method to run every 3 minutes
            RecurringJob.AddOrUpdate("CheckForFailedPickups", () => PickupFailed(), Cron.MinuteInterval(1));

           return RedirectToAction("Parcels");
        }

        //trigger schedule background job
        public IActionResult TriggerBackgroundJob()
        {
            BackgroundJob.Enqueue(() => ScheduleBackgroundJob());
            return RedirectToAction("Parcels");
        }

        //cancel pickup 
        public IActionResult CancelPickupRequest(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return RedirectToAction("Parcels");
            }

            parcel.Status = " Pickup Cancelled";
            parcel.PickupCancelledAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Cancelled Successfully";
            return RedirectToAction("Parcels");
        }

        //pickup on hold
        public IActionResult PickupOnHold()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            //if merchant did'nt give me parcel today & time limited crossed from today, first check status those parcels which is pickup
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            var parcels = _context.Parcels.Where(x => x.MerchantId == merchantId && x.Status == "Assigned A Rider For Pickup").ToList();
            foreach (var parcel in parcels)
            {
                if (parcel.PickupRequestDate.Value.AddDays(1) < DateTime.Now)
                {
                    parcel.Status = "Pickup On Hold";
                    _context.Parcels.Update(parcel);
                }
            }
            _context.SaveChanges();

            TempData["success"] = "Pickup On Hold Successfully";
            return RedirectToAction("Parcels");
        }




        //public IActionResult PickupFailed()
        //{
        //    if (!IsMerchantLoggedIn())
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    var merchantId = HttpContext.Request.Cookies["MerchantId"];
        //    var parcels = _context.Parcels.Where(x => x.MerchantId == merchantId && x.Status == "Pickup Requested").ToList();
        //    foreach (var parcel in parcels)
        //    {
        //        if (parcel.PickupRequestDate.Value.AddDays(3) < DateTime.Now)
        //        {
        //            parcel.Status = "Pickup Failed";
        //            _context.Parcels.Update(parcel);
        //        }
        //    }
        //    _context.SaveChanges();
        //    TempData["success"] = "Pickup Failed Parcels Updated Successfully";
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        ModelState.AddModelError("File", "Please select a file");
        return View();
    }

    using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
    {
        List<ParcelImportViewModel> parcels = new List<ParcelImportViewModel>();
        bool isFirstLine = true; // Flag to skip the first line

        while (!reader.EndOfStream)
        {
            string line = await reader.ReadLineAsync();

            if (isFirstLine)
            {
                isFirstLine = false;
                continue; // Skip the first line
            }

            // Split the line by comma to get the values
            string[] values = line.Split(',');

            parcels.Add(new ParcelImportViewModel
            {
                Store = values[0],
                ReceiverName = values[1],
                ReceiverAddress = values[2],
                ReceiverContactNumber = values[3],
                District = values[4],
                Zone = values[5],
                Area = values[6],
                ProductName = values[7],
                ProductWeight = decimal.Parse(values[8]),
                ProductPrice = int.Parse(values[9]),
                ProductQuantity = string.IsNullOrEmpty(values[10]) ? null : (int?)int.Parse(values[10])
            });
        }

        // Save parcels to the database asynchronously
        foreach (var parcel in parcels)
        {
            var deliveryType = "";
            // Calculate delivery charge (assuming a default delivery type of "InsideDhaka")
            var deliveryCharge = CalculateDeliveryCharge(parcel.ProductWeight, "InsideDhaka");

                    //check store name is valid or not
                    if (_context.Stores.FirstOrDefault(x => x.Name == parcel.Store) == null)
                    {
                        TempData["Error"] = "Store name is not valid";
                        return RedirectToAction("AddBulkParcels");
                    }
                    //check district, zone and area name is valid or not
                    if (_context.District.FirstOrDefault(x => x.Name == parcel.District) == null)
                    {
                        TempData["Error"] = "District name is not valid";
                        return RedirectToAction("AddBulkParcels");
                    }
                    //if zone and area name is not found in the database
                    if (_context.Zone.FirstOrDefault(x => x.Name == parcel.Zone) == null || _context.Areas.FirstOrDefault(x => x.Name == parcel.Area) == null)
                    {
                        TempData["Error"] = "Zone or Area name is not valid";
                        return RedirectToAction("AddBulkParcels");
                    }
            if (parcel.District != "Dhaka")
            {
                deliveryCharge = CalculateDeliveryCharge(parcel.ProductWeight, "OutsideDhaka");
                deliveryType = "OutsideDhaka";
            }

            // Calculate COD charge
            var codCharge = CalculateCOD(parcel.ProductPrice);

            // Calculate total price
            var totalPrice = CalculateTotalPrice(parcel.ProductPrice, codCharge, deliveryCharge);

            // Find hub according to store id
            var store = await _context.Stores.FirstOrDefaultAsync(x => x.Name == parcel.Store);
            var hubId = store.HubId;

            // Add parcel to the database asynchronously
            _context.Parcels.Add(new Parcel
            {
                MerchantId = HttpContext.Request.Cookies["MerchantId"],
                StoreId = (await _context.Stores.FirstOrDefaultAsync(s => s.Name == parcel.Store))?.Id,
                ReceiverName = parcel.ReceiverName,
                ReceiverAddress = parcel.ReceiverAddress,
                ReceiverContactNumber = parcel.ReceiverContactNumber,
                DistrictId = (await _context.District.FirstOrDefaultAsync(d => d.Name == parcel.District))?.Id,
                ZoneId = (await _context.Zone.FirstOrDefaultAsync(z => z.Name == parcel.Zone))?.Id,
                AreaId = (await _context.Areas.FirstOrDefaultAsync(a => a.Name == parcel.Area))?.Id,
                ProductName = parcel.ProductName,
                ProductWeight = parcel.ProductWeight,
                ProductPrice = parcel.ProductPrice,
                ProductQuantity = parcel.ProductQuantity,
                DeliveryType = deliveryType, // Default to "InsideDhaka"
                DeliveryCharge = (int)deliveryCharge,
                COD = (int)codCharge,
                TotalPrice = (int)totalPrice,
                PickupRequestDate = DateTime.Now,
                HubId = hubId
            });
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = "Parcels imported successfully";
    }

    return RedirectToAction("Parcels");
}









        [HttpPost]
        public async Task<IActionResult> ImportParcels(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                return View("AddBulkParcels");
            }

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var parcels = csv.GetRecords<Parcel>().ToList();

                foreach (var parcel in parcels)
                {
                    // Additional validation if needed
                    if (!ModelState.IsValid)
                    {
                        // Handle validation errors
                        // For example: return View with errors
                        return View("AddBulkParcels", parcel);
                    }

                    // Assign additional properties if needed
                    parcel.Id = "P-" + Guid.NewGuid().ToString().Substring(0, 4);
                    parcel.Status = "Pickup Request";
                    parcel.PickupRequestDate = DateTime.Now;


                    // Add parcel to database
                    _context.Parcels.Add(parcel);
                }

                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Parcels added successfully.";
            return RedirectToAction("Index", "Merchant");
        }

        public IActionResult Parcels(DateTime? startDate, DateTime? endDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId)
                .Include(u => u.Rider)
                .Include(h => h.Hub);

            // If both start date and end date are provided, filter by date range
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

            // Pass selected date range to the ViewBag
            ViewBag.StartDate = startDate ?? DateTime.Today;
            ViewBag.EndDate = endDate ?? DateTime.Today;
            ScheduleBackgroundJob();
            return View(parcels);
        }


        public IActionResult DownloadCsv(DateTime? startDate, DateTime? endDate)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Get the list of parcels based on the selected date range
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Where(x => x.MerchantId == merchantId)
                .Include(u => u.Rider)
                .Include(h => h.Hub);

            // Filter parcels by date range
            if (startDate.HasValue && endDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date >= startDate.Value.Date && x.PickupRequestDate.Value.Date <= endDate.Value.Date);
            }
            else if (startDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date >= startDate.Value.Date);
            }
            else if (endDate.HasValue)
            {
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date <= endDate.Value.Date);
            }
            else
            {
                // Default to today if no date range is provided
                parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date == DateTime.Today);
            }

            var parcels = parcelsQuery.ToList();

            // Generate CSV content
            StringBuilder csvContent = new StringBuilder();

            // Column headers
            csvContent.AppendLine("ID,Hub,Rider,Pickup Location,Pickup Request Date,Receiver Name,Receiver Address,Receiver Contact,Product Name,Product Weight,Product Price,Product Quantity,Delivery Type,Delivery Charge,Total Price,Status,Payment Status");

            // Data rows
            foreach (var parcel in parcels)
            {
                // Ensure proper formatting of text fields by enclosing them in double quotes
                csvContent.AppendLine($"\"{parcel.Id}\",\"{parcel.Hub?.Name ?? "Not Assigned"}\",\"{parcel.Rider?.Name ?? "Not Assigned"}\",\"{parcel.PickupLocation}\",\"{parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}\",\"{parcel.ReceiverName}\",\"{parcel.ReceiverAddress}\",\"{parcel.ReceiverContactNumber}\",\"{parcel.ProductName}\",{parcel.ProductWeight},{parcel.ProductPrice},{parcel.ProductQuantity},\"{parcel.DeliveryType}\",{parcel.DeliveryCharge},{parcel.TotalPrice},\"{parcel.Status}\",\"{parcel.PaymentStatus}\"");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"Parcels_{startDate?.ToString("yyyy-MM-dd")}_{endDate?.ToString("yyyy-MM-dd")}.csv");
        }

        





        //change parcel status to ReturnParcelReceived
        public IActionResult ReturnParcelReceived(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return Parcel Received";
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return Parcel Received";
            return RedirectToAction("Parcels");
        }
        //ExchangeParcelReceived
        public IActionResult ExchangeParcelReceived(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Exchange Parcel Received";
            _context.SaveChanges();
            TempData["success"] = "Exchange Parcel Received";
            return RedirectToAction("Parcels");
        }

        public IActionResult ChangePassword()
        {
            if (!IsMerchantLoggedIn())
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
            //adminid from cookie

            if (ModelState.IsValid)
            {
                var merchant = _context.Merchants.Find(resetPassword.Id);
                if (merchant == null)
                {
                    return NotFound();
                }
                if (merchant.Password == resetPassword.OldPassword && resetPassword.NewPassword == resetPassword.ConfirmPassword)
                {
                    merchant.Password = resetPassword.NewPassword;
                    _context.SaveChanges();
                    TempData["success"] = "Password Changed Successfully";
                    return RedirectToAction("Index");
                }

                else if (merchant.Password != resetPassword.OldPassword)
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

        //complain
        public IActionResult Complain()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            var complains = _context.Complain.Where(x => x.MerchantId == merchantId).ToList();
            return View(complains);
        }

        public IActionResult AddComplain()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        [HttpPost]
        public IActionResult AddComplain(Complain complain)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(complain);
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            complain.MerchantId = merchantId;
            complain.Date = DateTime.Now;
            _context.Complain.Add(complain);
            _context.SaveChanges();
            TempData["success"] = "Complain Added Successfully";
            return RedirectToAction("Complain");
        }


        [HttpPost]
        public IActionResult ApprovePermission(string parcelId,int newPrice)
        {
            
            var parcel = _context.Parcels.Find(parcelId);
            var merchantId = HttpContext.Request.Cookies["MerchantId"];

            
            if (parcel.MerchantId != merchantId)
            {
                return BadRequest("Unauthorized");
            }

            
            parcel.ProductPrice = newPrice;
            

            
            parcel.TotalPrice = (int)(parcel.ProductPrice * parcel.ProductQuantity + parcel.DeliveryCharge);
           
            var notification = _context.NotificationsPermission.Where(p=>p.ParcelId == parcelId).FirstOrDefault();
            notification.Status = 1;

            _context.SaveChanges();

            TempData["success"] = "Permission Approved Successfully";

            
            return RedirectToAction("Index");
        }

        //Store list
        public IActionResult StoreList()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            var stores = _context.Stores.Where(x => x.MerchantId == merchantId).Include(d=>d.District).Include(d => d.Area).Include(d => d.Zone).Include(d => d.Hub).ToList();
            return View(stores);
        }
        
        //create store
        public IActionResult CreateStore()
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            //district list,zone list,area list
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Areas = _context.Areas.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateStore(Store store)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(store);
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            store.MerchantId = merchantId;
            // Find the hub based on the district, zone, and area associated with the store
            var hub = _context.Hubs.FirstOrDefault(h => h.Areas.Any(a => a.Id == store.AreaId));
            if (hub == null)
            {
                TempData["error"] = "No hub found for the specified district, zone, and area. Please contact support.";
                return RedirectToAction("StoreList");
            }

            // Assign the found hub ID to the store
            store.HubId = hub.Id;


            _context.Stores.Add(store);
            _context.SaveChanges();
            TempData["success"] = "Store Added Successfully";
            return RedirectToAction("StoreList");
        }

        //edit store
        public IActionResult EditStore(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Areas = _context.Areas.ToList();
            return View(store);
        }

        [HttpPost]
        public IActionResult EditStore(Store store)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(store);
            }
            var merchantId = HttpContext.Request.Cookies["MerchantId"];
            store.MerchantId = merchantId;

            //var hub = _context.Hubs.FirstOrDefault(h => h.Areas.Any(a => a.Id == store.AreaId));
            //if (hub == null)
            //{
            //    TempData["error"] = "No hub found for the specified district, zone, and area. Please contact support.";
            //    return RedirectToAction("StoreList");
            //}

            //store.HubId = hub.Id;

            _context.Stores.Update(store);
            _context.SaveChanges();
            TempData["success"] = "Store Updated Successfully";
            return RedirectToAction("StoreList");
        }

        public IActionResult DeleteStore(string id)
        {
            if (!IsMerchantLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }

            // Retrieve related parcels
            var relatedParcels = _context.Parcels.Where(p => p.StoreId == id).ToList();

            // Delete related parcels first
            _context.Parcels.RemoveRange(relatedParcels);
            _context.SaveChanges();

            // Now delete the store
            _context.Stores.Remove(store);
            _context.SaveChanges();

            TempData["success"] = "Store Deleted Successfully";
            return RedirectToAction("StoreList");
        }


    }
}
