using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Xceed.Words.NET;
using ZXing;
using ZXing.Common;
using SkiaSharp;
using System.IO.Compression;
using Xceed.Document.NET;
using Newtonsoft.Json;

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

            

            //total parcel
            var totalParcel = _context.Parcels.Count();
            ViewBag.TotalParcel = totalParcel;

            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);

            var todayPickupRequest = _context.Parcels
                .Where(p => p.PickupRequestDate >= todayStart && p.PickupRequestDate < tomorrowStart)
                .Count();

            ViewBag.TodayPickupRequest = todayPickupRequest;

           
            // Today Delivered
            var todayDelivered = _context.Parcels
                .Where(p => p.DeliveryParcel.DeliveryDate >= todayStart && p.DeliveryParcel.DeliveryDate < tomorrowStart)
                .Count();
            ViewBag.TodayDelivered = todayDelivered;

            //parcel assigned for pickup
            var assignedForPickup = _context.Parcels
                .Where(p => p.Status == "Assigned A Rider For Pickup")
                .Count();
            ViewBag.AssignedForPickup = assignedForPickup;

            //parcel in hub
            var inHub = _context.Parcels
                .Where(p => p.Status == "Parcel In Hub")
                .Count();
            ViewBag.InHub = inHub;

            //parcel On The Way
            var onTheWay = _context.Parcels
                .Where(p => p.Status == "Parcel On The Way")
                .Count();
            ViewBag.OnTheWay = onTheWay;

            //Assigned For Delivery
            var assignedForDelivery = _context.Parcels
                .Where(p => p.Status == "Assigned A Rider For Delivery")
                .Count();
            ViewBag.AssignedForDelivery = assignedForDelivery;

            //total delivery
            var totalDelivery = _context.Parcels
                .Where(p => p.DeliveryId !=null)
                .Count();
            ViewBag.TotalDelivery = totalDelivery;

            //total exchange
            var totalExchange = _context.Parcels
                .Where(p => p.ExchangeId != null)
                .Count();
            ViewBag.TotalExchange = totalExchange;

            //total return
            var totalReturn = _context.Parcels
                .Where(p => p.ReturnId != null)
                .Count();
            ViewBag.TotalReturn = totalReturn;


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

            //total from MerchantPayment table for today
            var todayMerchantPayment = _context.MerchantPayments
                .Where(p => p.DateTime >= todayStart && p.DateTime < tomorrowStart)
                .Sum(p => p.AmountPaid);
            ViewBag.TodayMerchantPayment = todayMerchantPayment;

            //total delivery charge for today which is delivered, exchange and return
            var todayDeliveryCharge = _context.Parcels
                .Where(p => p.DeliveryParcel.DeliveryDate >= todayStart && p.DeliveryParcel.DeliveryDate < tomorrowStart)
                .Sum(p => p.DeliveryCharge);
            ViewBag.TodayDeliveryCharge = todayDeliveryCharge;

            //total delivery charge for today which is delivered, exchange and return
            var todayExchangeCharge = _context.Parcels
                .Where(p => p.ExchangeParcel.ExchangeDate >= todayStart && p.ExchangeParcel.ExchangeDate < tomorrowStart)
                .Sum(p => p.DeliveryCharge);
            ViewBag.TodayExchangeCharge = todayExchangeCharge;

            //total delivery charge for today which is delivered, exchange and return
            var todayReturnCharge = _context.Parcels
                .Where(p => p.ReturnParcel.ReturnDate >= todayStart && p.ReturnParcel.ReturnDate < tomorrowStart)
                .Sum(p => p.DeliveryCharge);
            ViewBag.TodayReturnCharge = todayReturnCharge;

            var totalDeliveryCharge = todayDeliveryCharge + todayExchangeCharge + todayReturnCharge;
            ViewBag.TotalDeliveryCharge = totalDeliveryCharge;


            //calculate total COD for today
            var totalCOD = _context.Parcels
                .Where(p => p.DeliveryParcel.DeliveryDate >= todayStart && p.DeliveryParcel.DeliveryDate < tomorrowStart)
                .Sum(p => p.COD);
            ViewBag.TotalCOD = totalCOD;

            var amountPayable = ViewBag.TodayMerchantPayment - ViewBag.TotalDeliveryCharge - ViewBag.TotalCOD;
            ViewBag.AmountPayable = amountPayable;

            var profit = ViewBag.TodayMerchantPayment - ViewBag.AmountPayable;
            ViewBag.Profit = profit;







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

        //accept pickup request
        public IActionResult AcceptPickupRequest(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Pickup Requested";
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Pickup Request Accepted";
            return RedirectToAction("Parcel");
        }

       

        //Return Parcel List
        public IActionResult ReturnParcelList(DateTime? selectedDate)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            
            IQueryable<Parcel> returnParcelsQuery = _context.Parcels
                .Where(x =>x.ReturnId != null)
                .Include(x => x.ReturnParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            returnParcelsQuery = returnParcelsQuery.Where(x => x.ReturnParcel.ReturnDate.Date == selectedDate.Value.Date);
            var returnParcels = returnParcelsQuery.ToList();

            return View(returnParcels);
        }

        //Exchange Parcel List
        public IActionResult ExchangeParcelList(DateTime? selectedDate)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            IQueryable<Parcel> exchangeParcelsQuery = _context.Parcels
                .Where(x => x.ExchangeId != null)
                .Include(x => x.ExchangeParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);
            if(!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            exchangeParcelsQuery = exchangeParcelsQuery.Where(x => x.ExchangeParcel.ExchangeDate.Date == selectedDate.Value.Date);
            var exchangeParcels = exchangeParcelsQuery.ToList();

            return View(exchangeParcels);
        }

        //Delivered Parcel List
        public IActionResult DeliveredParcelList(DateTime? selectedDate)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            IQueryable<Parcel> parcelsQuery = _context.Parcels.Where(p=>p.DeliveryId !=null)
                .Include(m => m.Merchant)
                .Include(u => u.Rider)
                .Include(d=>d.DeliveryParcel)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }

            parcelsQuery = parcelsQuery.Where(x => x.DeliveryDate.Value.Date == selectedDate.Value.Date);

            var parcels = parcelsQuery.ToList();

            


            return View(parcelsQuery);
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

        //parcel status changed to At the Hub Received
        public IActionResult ParcelAtTheHubReceived(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "At The Hub Received";
            parcel.ParcelAtTheHubReceivedAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to At The Hub Received";
            return RedirectToAction("Parcel");
        }

        //OnTheWayToSortingHub
        public IActionResult OnTheWayToSortingHub(string id)
        {
            if(!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "On The Way To Sorting Hub";
            parcel.OnTheWayToSortingHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to On The Way To Sorting Hub";
            return RedirectToAction("Parcel");

        }

        //AtTheSortingHub
        public IActionResult AtTheSortingHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "At The Sorting Hub";
            parcel.AtTheSortingHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            return RedirectToAction("Parcel");

            //// Generate DOCX file
            //var docxContent = $"Parcel ID: {parcel.Id}\nStatus: At The Sorting Hub";
            //var docxBytes = Encoding.UTF8.GetBytes(docxContent);
            //var fileName = $"Parcel_{parcel.Id}_Status.docx";

            //return File(docxBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }


        [HttpPost]
        public ActionResult GenerateBarcode()
        {
            // Generate barcode using ZXing.Net
            BarcodeWriter<SKBitmap> writer = new BarcodeWriter<SKBitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 150,
                    Width = 300,
                    Margin = 10 // Set margin if needed
                }
            };

            SKBitmap barcodeBitmap = writer.Write("123456789");

            // Convert SKBitmap to PNG byte array
            byte[] imageBytes;
            using (SKImage image = SKImage.FromBitmap(barcodeBitmap))
            using (SKData encoded = image.Encode(SKEncodedImageFormat.Png, 100))
            using (MemoryStream ms = new MemoryStream())
            {
                encoded.SaveTo(ms);
                imageBytes = ms.ToArray();
            }

            // Create DOCX file using DocX library (assuming you have DocX library installed)
            using (DocX doc = DocX.Create("BarcodeDetails.docx"))
            {
                // Add content to the document
                Paragraph paragraph = doc.InsertParagraph();
                paragraph.AppendLine("Barcode Details:");
                paragraph.AppendLine("Parcel ID: 123456789"); // Update with actual parcel ID

                // Save the document
                doc.Save();
            }

            // Create a ZIP file containing the barcode image and DOCX file
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    // Add barcode image to the ZIP file
                    ZipArchiveEntry barcodeEntry = zip.CreateEntry("barcode.png");
                    using (Stream barcodeStream = barcodeEntry.Open())
                    {
                        barcodeStream.Write(imageBytes, 0, imageBytes.Length);
                    }

                    // Add DOCX file to the ZIP file
                    ZipArchiveEntry docxEntry = zip.CreateEntry("BarcodeDetails.docx");
                    using (Stream docxStream = docxEntry.Open())
                    {
                        byte[] docxBytes = System.IO.File.ReadAllBytes("BarcodeDetails.docx");
                        docxStream.Write(docxBytes, 0, docxBytes.Length);
                    }
                }

                byte[] zipBytes = zipStream.ToArray();
                return File(zipBytes, "application/zip", "BarcodeDetails.zip");
            }

            }
            public IActionResult DownloadDocx(string id)
        {
            // Create .docx file logic using a library like DocX or NPOI

            // For example, using DocX library:
            using DocX doc = DocX.Create("ParcelDetails.docx");
            // Add content to the document
            doc.InsertParagraph("Parcel Details:");
            doc.InsertParagraph($"Parcel ID: {id}");
            // Add more content as needed

            // Save the document to a specific location
            doc.Save();

            // Return the file for download
            byte[] fileBytes = System.IO.File.ReadAllBytes("ParcelDetails.docx");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ParcelDetails.docx");
        }

        //On The Way To Last Mile Hub
        public IActionResult OnTheWayToLastMileHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "On The Way To Last Mile Hub";
            parcel.OnTheWayToLastMileHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to On The Way To Last Mile Hub";
            return RedirectToAction("Parcel");

        }

        //Received At Last Mile Hub
        public IActionResult ReceivedAtLastMileHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Received At Last Mile Hub";
            parcel.ReceivedAtLastMileHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Received At Last Mile Hub";
            return RedirectToAction("Parcel");

        }

        //parcel status Changed to Damaged and create return id
        public IActionResult Damaged(string id)
        {
            if(!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            parcel.ReturnParcel = new ReturnParcel
            {
                ParcelId = parcel.Id,
                ReturnDate = DateTime.Now,
                HubId = parcel.HubId,
                MerchantId = parcel.MerchantId
            };

            parcel.Status = "Damaged";
            parcel.DamagedAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Damaged";
            return RedirectToAction("Parcel");

        }

        //Return Create First Mile Hub
        public IActionResult ReturnCreateFirstMileHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return Create First Mile Hub";
            parcel.ReturnCreateFirstMileHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return Create First Mile Hub";
            return RedirectToAction("Parcel");

        }

        //Return On The Way To Sorting Hub
        public IActionResult ReturnOnTheWayToSortingHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return On The Way To Sorting Hub";
            parcel.ReturnOnTheWayToSortingHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return On The Way To Sorting Hub";
            return RedirectToAction("Parcel");

        }


        //Return Received By Sorting Hub
        public IActionResult ReturnReceivedBySortingHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return Received By Sorting Hub";
            parcel.ReturnReceivedBySortingHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return Received By Sorting Hub";
            return RedirectToAction("Parcel");

        }

        //Return On The Way To First Mile Hub
        public IActionResult ReturnOnTheWayToFirstMileHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return On The Way To First Mile Hub";
            parcel.ReturnOnTheWayToFirstMileHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return On The Way To First Mile Hub";
            return RedirectToAction("Parcel");

        }

        //Return Received By First Mile Hub
        public IActionResult ReturnReceivedByFirstMileHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            parcel.Status = "Return Received By First Mile Hub";
            parcel.ReturnReceivedByFirstMileHubAt = DateTime.Now;
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            TempData["success"] = "Parcel Status Changed to Return Received By First Mile Hub";
            return RedirectToAction("Parcel");

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

        [HttpPost]
        public IActionResult AddAdmin(Admin admin)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (admin == null)
            {
                return NotFound();
            }
            //check email exists or not
            var email = _context.Admins.FirstOrDefault(a => a.Email == admin.Email);
            if (email != null)
            {
                TempData["error"] = "Email Already Exists";
                return View(admin);
            }
            //check name already exists
            var name = _context.Admins.FirstOrDefault(a => a.Name == admin.Name);
            if (name != null)
            {
                TempData["error"] = "Name Already Exists";
                return View(admin);
            }
            //check password and confirm password
            if (admin.Password != admin.ConfirmPassword)
            {
                TempData["error"] = "Password and Confirm Password does not match";
                return View(admin);
            }
            if (ModelState.IsValid)
            {
                _context.Admins.Add(admin);
                _context.SaveChanges();
                TempData["success"] = "Admin Added Successfully";
                return RedirectToAction("ApplicationUser");
            }
            else
            {
                return View(admin);
            }
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

        //merchant payment list
        public IActionResult MerchantPaymentList(bool showDuePayments = false)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Payment list for today
            var merchantPaymentsQuery = _context.MerchantPayments.Include(u => u.Merchant).Where(p => p.DateTime.Date == DateTime.Today.Date);

            // Filter by due payments if requested
            if (showDuePayments)
            {
                merchantPaymentsQuery = merchantPaymentsQuery.Where(p => p.DueAmount > 0);
                return View(merchantPaymentsQuery);
            }

            var merchantPayments = merchantPaymentsQuery.ToList();

            if (merchantPayments == null)
            {
                return NotFound();
            }

            return View(merchantPayments);
        }


        //hub payment list
        public IActionResult HubPaymentList(bool showDuePayments = false)
{
    if (!IsAdminLoggedIn())
    {
        return RedirectToAction("Login", "Home");
    }

    // Payment list for today
    IQueryable<HubPayment> hubPaymentsQuery = _context.HubPayments
        .Include(p => p.Hub);

    // Include merchant payments and their associated merchants
    if (showDuePayments)
    {
        hubPaymentsQuery = hubPaymentsQuery
            .Include(p => p.MerchantPayments)
                .ThenInclude(mp => mp.Merchant)
            .Where(p => p.DueAmount > 0);
    }
    else
    {
        hubPaymentsQuery = hubPaymentsQuery
            .Include(p => p.MerchantPayments)
                .ThenInclude(mp => mp.Merchant);
    }

    var hubPayments = hubPaymentsQuery
        .Where(p => p.DateTime.Date == DateTime.Today.Date)
        .ToList();

    if (hubPayments == null)
    {
        return NotFound();
    }

    return View(hubPayments);
}



        public IActionResult RiderPaymentList()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Retrieve rider payments for today
            var riderPayments = _context.riderPayments
                .Include(rp => rp.Rider)
                .Where(rp => rp.PaymentDate.Date == DateTime.Today.Date)
                .ToList();

            if (riderPayments == null)
            {
                return NotFound();
            }

            // Group rider payments by Rider and calculate the total amount for each Rider
            var riderTotalAmounts = riderPayments.GroupBy(rp => rp.RiderId)
                .Select(g => new
                {
                    RiderId = g.Key,
                    RiderName = g.First().Rider.Name,
                    TotalAmount = g.Sum(rp => rp.Amount)
                })
                .ToList();
            //hub receive amount for today
            ViewBag.HubReceivedAmount = _context.HubPayments
                .Where(p => p.DateTime.Date == DateTime.Today.Date)
                .Sum(p => p.AmountReceived);

            ViewBag.AmountCollected = riderPayments.Sum(rp => rp.Amount);
            ViewBag.Due = ViewBag.HubReceivedAmount - ViewBag.AmountCollected;



            return View(riderTotalAmounts);
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
                //check name already exists
                var name = _context.Riders.FirstOrDefault(a => a.Name == rider.Name);
                if (name != null)
                {
                    TempData["error"] = "Name Already Exists";
                    return View(rider);
                }
                //check password and confirm password
                if (rider.Password != rider.ConfirmPassword)
                {
                    TempData["error"] = "Password and Confirm Password does not match";
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

                //name already exists
                var name = _context.Merchants.FirstOrDefault(a => a.Name == merchant.Name);
                if (name != null)
                {
                    TempData["error"] = "Merchant Name Already Exists";
                    return View(merchant);
                }

                //password and confirm password check
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

        //approve merchant
        //public IActionResult ApproveMerchant(string? id)
        //{
        //    if (!IsAdminLoggedIn())
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var merchant = _context.Merchants.Find(id);
        //    if (merchant == null)
        //    {
        //        return NotFound();
        //    }
        //    merchant.Status = "Approved";
        //    _context.Merchants.Update(merchant);
        //    _context.SaveChanges();
        //    TempData["success"] = "Merchant Approved Successfully";
        //    return RedirectToAction("Merchant");
        //}

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

            if (parcels.Any())
            {
                // Update or delete associated parcels as needed
                foreach (var parcel in parcels)
                {
                   
                    parcel.RiderId = null;
                }

                // Save changes after updating or deleting parcels
                _context.SaveChanges();
            }

            //find all delivered parcels associated with the rider
            var deliveredParcels = _context.DeliveredParcels.Where(p => p.RiderId == id).ToList();
            if (deliveredParcels.Any())
            {
                foreach (var parcel in deliveredParcels)
                {
                    parcel.RiderId = null;
                }
                _context.SaveChanges();
            }

            //find all exchange parcels associated with the rider
            var exchangeParcels = _context.ExchangeParcels.Where(p => p.RiderId == id).ToList();
            if (exchangeParcels.Any())
            {
                foreach (var parcel in exchangeParcels)
                {
                    parcel.RiderId = null;
                }
                _context.SaveChanges();
            }

            //find all return parcels associated with the rider
            var returnParcels = _context.ReturnParcels.Where(p => p.RiderId == id).ToList();
            if (returnParcels.Any())
            {
                foreach (var parcel in returnParcels)
                {
                    parcel.RiderId = null;
                }
                _context.SaveChanges();
            }

            // Find all rider payments associated with the rider
            var riderPayments = _context.riderPayments.Where(rp => rp.RiderId == id).ToList();
            if (riderPayments.Any())
            {
                // Delete associated rider payments
                _context.riderPayments.RemoveRange(riderPayments);

                // Save changes after deleting rider payments
                _context.SaveChanges();
            }



            

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

            // Find all merchant payments associated with the merchant
            var merchantPayments = _context.MerchantPayments.Where(mp => mp.MerchantId == id).ToList();

            // If there are associated merchant payments, handle them
            if (merchantPayments.Any())
            {
                // Delete associated merchant payments
                _context.MerchantPayments.RemoveRange(merchantPayments);

                // Save changes after deleting merchant payments
                _context.SaveChanges();
            }

            // Find all parcels associated with the merchant
            var parcels = _context.DeliveredParcels.Where(p => p.MerchantId == id).ToList();

            // If there are associated parcels, handle them
            if (parcels.Any())
            {
                // Update or delete associated parcels as needed
                foreach (var parcel in parcels)
                {
                    // Here you may choose to delete the parcel or update its MerchantId to null
                    // For example:
                    // _context.DeliveredParcels.Remove(parcel);
                    // or
                    parcel.MerchantId = null;
                }

                // Save changes after updating or deleting parcels
                _context.SaveChanges();
            }

            //find all delivered parcels associated with the merchant
            var deliveredParcels = _context.DeliveredParcels.Where(p => p.MerchantId == id).ToList();
            if(deliveredParcels.Any())
            {
                foreach (var parcel in deliveredParcels)
                {
                    parcel.MerchantId = null;
                }
                _context.SaveChanges();
            }

            //find all exchange parcels associated with the merchant
            var exchangeParcels = _context.ExchangeParcels.Where(p => p.MerchantId == id).ToList();
            if (exchangeParcels.Any())
            {
                foreach (var parcel in exchangeParcels)
                {
                    parcel.MerchantId = null;
                }
                _context.SaveChanges();
            }

            //find all return parcels associated with the merchant
            var returnParcels = _context.ReturnParcels.Where(p => p.MerchantId == id).ToList();
            if (returnParcels.Any())
            {
                foreach (var parcel in returnParcels)
                {
                    parcel.MerchantId = null;
                }
                _context.SaveChanges();
            }

            var merchant = _context.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }

            // Delete from AWS S3
            if (merchant.ImageUrl != null)
            {
                string[] split = merchant.ImageUrl.Split("/");
                string key = "Merchant/" + split[split.Length - 1];
                var s3Client = new AmazonS3Client("YOUR_ACCESS_KEY_ID", "YOUR_SECRET_ACCESS_KEY", RegionEndpoint.USEast1);
                var fileTransferUtility = new TransferUtility(s3Client);
                fileTransferUtility.S3Client.DeleteObjectAsync("courierbuckets3", key);
            }

            // Remove the merchant from the database
            _context.Merchants.Remove(merchant);
            _context.SaveChanges();

            TempData["error"] = "Merchant Deleted Successfully";
            return RedirectToAction("Merchant");
        }



        public IActionResult Parcel(DateTime? startDate, DateTime? endDate)
        {

        if (!IsAdminLoggedIn())
        {
            return RedirectToAction("Login", "Home");
        }

        IQueryable<Parcel> parcelsQuery = _context.Parcels
            .Include(m => m.Merchant)
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
            //else
            //{
            //    parcelsQuery = parcelsQuery.Where(x => x.PickupRequestDate.Value.Date == DateTime.Today);
            //}

            var parcels = parcelsQuery.ToList();

        // Pass selected date range to the view
        ViewBag.StartDate = startDate ?? DateTime.Today;
        ViewBag.EndDate = endDate ?? DateTime.Today;
        ViewBag.Riders = _context.Riders.ToList();

        return View(parcels);
    }

        public IActionResult DownloadCsv(DateTime? startDate, DateTime? endDate)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

           
           
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Include(m => m.Merchant)
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
            csvContent.AppendLine("ID,Merchant,Hub,Rider,Pickup Location,Pickup Request Date,Receiver Name,Receiver Address,Receiver Contact,Product Name,Product Weight,Product Price,Product Quantity,Delivery Type,Delivery Charge,Total Price,Status,Payment Status");

            // Data rows
            foreach (var parcel in parcels)
            {
                // Ensure proper formatting of text fields by enclosing them in double quotes
                csvContent.AppendLine($"\"{parcel.Id}\",\"{parcel.Merchant?.Name ?? "Not Assigned"}\",\"{parcel.Hub?.Name ?? "Not Assigned"}\",\"{parcel.Rider?.Name ?? "Not Assigned"}\",\"{parcel.PickupLocation}\",\"{parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}\",\"{parcel.ReceiverName}\",\"{parcel.ReceiverAddress}\",\"{parcel.ReceiverContactNumber}\",\"{parcel.ProductName}\",{parcel.ProductWeight},{parcel.ProductPrice},{parcel.ProductQuantity},\"{parcel.DeliveryType}\",{parcel.DeliveryCharge},{parcel.TotalPrice},\"{parcel.Status}\",\"{parcel.PaymentStatus}\"");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"Parcels_{startDate?.ToString("yyyy-MM-dd")}_{endDate?.ToString("yyyy-MM-dd")}.csv");
        }


        [HttpPost]
        public IActionResult DeleteSelectedParcels(List<string> parcelIds)
        {
            try
            {
                // Find and remove selected parcels from the database
                var parcelsToRemove = _context.Parcels.Where(p => parcelIds.Contains(p.Id)).ToList();
                _context.Parcels.RemoveRange(parcelsToRemove);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                return StatusCode(500, "An error occurred while deleting selected parcels.");
            }
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
                    return RedirectToAction("Index");
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
            var contact = _context.Complain.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Complain.Remove(contact);
            _context.SaveChanges();
            TempData["error"] = "Complain Deleted Successfully";
            return RedirectToAction("Complain");
        }

        //hub

        public IActionResult Hub()
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubs = _context.Hubs.Include(h=>h.Areas).Include(d=>d.District).Include(z=>z.Zones).ToList();
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

            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Areas = _context.Areas.ToList();
            return View();
        }

        //district
        public IActionResult District()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var districts = _context.District.ToList();
            if (districts == null)
            {
                return NotFound();
            }
            return View(districts);
        }

        public IActionResult CreateDistrict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDistrict(District district)
        {
            var name = _context.District.FirstOrDefault(a => a.Name == district.Name);
            if (name != null)
            {
                TempData["error"] = "District Name Already Exists";
                return View(district);
            }
            if (ModelState.IsValid)
            {
                _context.District.Add(district);
                _context.SaveChanges();
                return RedirectToAction("District"); // Redirect to a suitable action
            }
            return View(district);
        }

        //edit district
        public IActionResult EditDistrict(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var district = _context.District.Find(id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        [HttpPost]
        public IActionResult EditDistrict(District district)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (district == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.District.Update(district);
                _context.SaveChanges();
                TempData["success"] = "District Updated Successfully";
                return RedirectToAction("District");
            }
            return View(district);
        }

        //delete district
        public IActionResult DeleteDistrict(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var district = _context.District.Find(id);
            if (district == null)
            {
                return NotFound();
            }
            var zonesToRemove = _context.Zone.Where(z => z.DistrictId == id).ToList();
            _context.Zone.RemoveRange(zonesToRemove);
            _context.District.Remove(district);
            _context.SaveChanges();
            TempData["error"] = "District Deleted Successfully";
            return RedirectToAction("District");
        }

        //zone
        public IActionResult Zone()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var zones = _context.Zone.Include(d => d.District).Include(h => h.Hub).ToList();
            if (zones == null)
            {
                return NotFound();
            }
            return View(zones);
        }

        //delete zone
        
        public IActionResult DeleteZone(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var zone = _context.Zone.Find(id);
            if (zone == null)
            {
                return NotFound();
            }
            var areasToRemove = _context.Areas.Where(a => a.ZoneId == id).ToList();
            _context.Areas.RemoveRange(areasToRemove);
            _context.Zone.Remove(zone);
            _context.SaveChanges();
            TempData["error"] = "Zone Deleted Successfully";
            return RedirectToAction("Zone");
        }

        //create zone
        public IActionResult CreateZone()
        {
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hub = _context.Hubs.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult CreateZone(Zone zone)
        {
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hub = _context.Hubs.ToList();
            //check exists or not

            var name = _context.Zone.FirstOrDefault(a => a.Name == zone.Name);
            if (name != null)
            {
                TempData["error"] = "Zone Name Already Exists";
                return View(zone);
            }
            if (ModelState.IsValid)
            {
                _context.Zone.Add(zone);
                _context.SaveChanges();
                return RedirectToAction("Zone"); 
            }
            return View(zone);
        }

        //Area
        public IActionResult Area()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var areas = _context.Areas.Include(z => z.Zone).Include(d => d.District).Include(h => h.Hub).ToList();
            if (areas == null)
            {
                return NotFound();
            }
            return View(areas);
        }


        public IActionResult CreateArea()
        {
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hubs = _context.Hubs.ToList();
            return View();
        }

        //create area
        [HttpPost]
        public IActionResult CreateArea(Area area)
        {
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hubs = _context.Hubs.ToList();
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            //check exists or not
            var name = _context.Areas.FirstOrDefault(a => a.Name == area.Name);
            if (name != null)
            {
                TempData["error"] = "Area Name Already Exists";
                return View(area);
            }
            if (ModelState.IsValid)
            {
                _context.Areas.Add(area);
                _context.SaveChanges();
                return RedirectToAction("Area");
            }
            return View(area);
        }

        [HttpPost]
        public async Task<IActionResult> BulkCreateArea(Area model, IFormFile csvFile)
        {
            ModelState.Remove("Name"); // Remove validation for the Name field
            if (csvFile == null || csvFile.Length <= 0)
            {
                ModelState.AddModelError("", "Please select a file.");
                return View(); // You can return the view with an error message
            }

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var areas = new List<Area>();
                    string line;
                    bool isFirstLine = true;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue; // Skip the first line
                        }
                        var values = line.Split(',');

                        //check if the area already exists
                        var name = _context.Areas.FirstOrDefault(a => a.Name == values[0]);
                        if (name != null)
                        {
                            TempData["error"] = "Area Name Already Exists";
                            return RedirectToAction("CreateArea");
                        }

                        var area = new Area
                        {
                            Name = values[0], // Assuming the first column is Name
                            DistrictId = model.DistrictId, // Assuming the second column is DistrictId
                            ZoneId = model.ZoneId, // Assuming the third column is ZoneId
                            HubId = model.HubId, // Assuming the fourth column is HubId
                        };
                        areas.Add(area);
                    }

                    // Add areas to the database
                    await _context.Areas.AddRangeAsync(areas);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Areas created successfully.";
                    return RedirectToAction("Area"); // Redirect to the desired page
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing the file.");
                // Log the exception
                return RedirectToAction("Area"); // Redirect to the desired page
            }
        }



        public IActionResult DownloadAreaCsv()
        {
            // Define the CSV content
            // CSV content with headers and sample data
            var csvContent = "Name";
            

            // Create a byte array from the CSV content
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            // Return the CSV file as a downloadable file
            return File(bytes, "text/csv", "Area_Bulk.csv");
        }

        [HttpPost]
        public IActionResult DeleteAreas(List<string> selectedAreas)
        {
            if (selectedAreas != null && selectedAreas.Any())
            {
                var areasToDelete = _context.Areas.Where(a => selectedAreas.Contains(a.Id));
                _context.Areas.RemoveRange(areasToDelete);
                _context.SaveChanges();
                TempData["success"] = "Selected areas deleted successfully.";
            }
            return RedirectToAction("Area"); // Redirect to the page where the table is displayed
        }


        //delete area
        public IActionResult DeleteArea(string? id)
        {
            if (!IsAdminLoggedIn() || Request.Cookies["AdminEmail"] != "flyerbd@gmail.com")
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var area = _context.Areas.Find(id);
            if (area == null)
            {
                return NotFound();
            }
            _context.Areas.Remove(area);
            _context.SaveChanges();
            TempData["error"] = "Area Deleted Successfully";
            return RedirectToAction("Area");
        }



        //create hub
        [HttpPost]
        public IActionResult CreateHub(Hub hub)
        {
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hub = _context.Hubs.ToList();
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (hub == null)
            {
                return NotFound();
            }
            //check name already exists
            var name = _context.Hubs.FirstOrDefault(a => a.Name == hub.Name);
            if (name != null)
            {
                TempData["error"] = "Hub Name Already Exists";
                return View(hub);
            }
            if (ModelState.IsValid)
            {
                _context.Hubs.Add(hub);
                _context.SaveChanges();
                TempData["success"] = "Hub Created Successfully";
                return RedirectToAction("Hub");
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

            try
            {
                // Find all delivered parcels associated with the hub
                var deliveredParcels = _context.DeliveredParcels.Where(dp => dp.HubId == id).ToList();

                // If there are associated delivered parcels, handle them
                if (deliveredParcels.Any())
                {
                    // Update HubId to null for each associated delivered parcel
                    foreach (var parcel in deliveredParcels)
                    {
                        parcel.HubId = null;
                    }
                }

                // Save changes to update delivered parcels
                _context.SaveChanges();

                //find all parcels associated with the hub
                var parcels = _context.Parcels.Where(p => p.HubId == id).ToList();
                if (parcels.Any())
                {
                    foreach(var parcel in parcels)
                    {
                        parcel.HubId = null;
                    }
                }
                _context.SaveChanges();

                // find all exchanged parcels associated with the hub
                var exchangedParcels = _context.ExchangeParcels.Where(ep => ep.HubId == id).ToList();
                if (exchangedParcels.Any())
                {
                    foreach (var parcel in exchangedParcels)
                    {
                        parcel.HubId = null;
                    }
                }
                _context.SaveChanges();

                //find all returned parcels associated with the hub
                var returnedParcels = _context.ReturnParcels.Where(rp => rp.HubId == id).ToList();
                if (returnedParcels.Any())
                {
                    foreach (var parcel in returnedParcels)
                    {
                        parcel.HubId = null;
                    }
                }
                _context.SaveChanges();


               


                // Find all hub payments associated with the hub
                var hubPayments = _context.HubPayments.Where(hp => hp.HubId == id).ToList();

                // If there are associated hub payments, handle them
                if (hubPayments.Any())
                {
                    // Find all merchant payments associated with the hub payments
                    var merchantPayments = _context.MerchantPayments.Where(mp => hubPayments.Select(hp => hp.Id).Contains(mp.HubPaymentId)).ToList();

                    // If there are associated merchant payments, delete them
                    if (merchantPayments.Any())
                    {
                        _context.MerchantPayments.RemoveRange(merchantPayments);
                    }

                    // Delete associated hub payments
                    _context.HubPayments.RemoveRange(hubPayments);
                }

                // Save changes to delete hub payments and associated merchant payments
                _context.SaveChanges();

                // Remove the hub from the database
                _context.Hubs.Remove(hub);
                _context.SaveChanges();

                TempData["error"] = "Hub Deleted Successfully";
                return RedirectToAction("Hub");
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while deleting the hub: " + ex.Message;
                return RedirectToAction("Hub");
            }
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
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Areas = _context.Areas.ToList();
            return View(hub);
        }

        [HttpPost]
        public IActionResult EditHub(Hub hub)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Areas = _context.Areas.ToList();
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

        //edit area
        public IActionResult EditArea(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var area = _context.Areas.Find(id);
            if (area == null)
            {
                return NotFound();
            }
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hubs = _context.Hubs.ToList();
            return View(area);
        }

        [HttpPost]
        public IActionResult EditArea(Area area)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Hubs = _context.Hubs.ToList();
            if (area == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Areas.Update(area);
                _context.SaveChanges();
                TempData["success"] = "Area Updated Successfully";
                return RedirectToAction("Area");
            }
            else
            {
                return View(area);
            }
        }

        //edit zone
        public IActionResult EditZone(string? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            var zone = _context.Zone.Find(id);
            if (zone == null)
            {
                return NotFound();
            }
            ViewBag.Districts = _context.District.ToList();
           
            ViewBag.Hub = _context.Hubs.ToList();
            return View(zone);
        }

        [HttpPost]
        public IActionResult EditZone(Zone zone)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Districts = _context.District.ToList();
            
            ViewBag.Hub = _context.Hubs.ToList();
            if (zone == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Zone.Update(zone);
                _context.SaveChanges();
                TempData["success"] = "Zone Updated Successfully";
                return RedirectToAction("Zone");
            }
            else
            {
                return View(zone);
            }
        }

        public IActionResult AssignParcelAdmin(string id)
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.ToList();

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);
        }

        public IActionResult AssignDeliveryRiderAdmin(string id)
        {

            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.Where(u => u.State == "Available");

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);
        }
        [HttpPost]
        public IActionResult AssignParcel(string id, string riderId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }


            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                //redirect to the assign parcel page
                TempData["error"] = "Please Select A Rider";
                return RedirectToAction("AssignParcel", new { id = id });
            }

            // Assign the rider to the parcel
            parcel.Rider = rider;
            parcel.Status = "Assigned A Rider For Pickup";
            parcel.DispatchDate = DateTime.Now;


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Parcel Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Admin");
        }

        [HttpPost]
        public IActionResult BulkAssignParcel(List<string> parcelIds, string riderId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                TempData["error"] = "Please Select A Rider";
                return RedirectToAction("AssignParcel"); // Redirect to the assign parcel page
            }

            // Assign the rider to each parcel in the list
            foreach (string id in parcelIds)
            {
                var parcel = _context.Parcels.Find(id);
                if (parcel != null)
                {
                    parcel.Rider = rider;
                    parcel.Status = "Assigned A Rider For Pickup";
                    parcel.DispatchDate = DateTime.Now;
                }
            }

            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Parcels Assigned Successfully";

            
            return RedirectToAction("Parcel", "Admin");
        }



        //bulk delete parcels
        [HttpPost]
        public async Task<IActionResult> BulkDeleteParcels(List<string> parcelIds)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find and remove selected parcels from the database asynchronously
            var parcelsToRemove = await _context.Parcels
                .Where(p => parcelIds.Contains(p.Id))
                .ToListAsync();

            _context.Parcels.RemoveRange(parcelsToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction("Parcel");
        }




        [HttpPost]
        public IActionResult AssignDeliveryRider(string id, string riderId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }


            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                TempData["error"] = "Please Select A Rider";
                return RedirectToAction("AssignDeliveryRider", new { id = id });
            }

            // Assign the rider to the parcel
            parcel.Rider = rider;
            parcel.Status = "Assigned For Delivery";
            parcel.DeliveryRiderAssignedAt = DateTime.Now;
            


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Admin");
        }

        [HttpPost]
        public IActionResult BulkAssignDeliveryRider(List<string> parcelIds, string riderId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                TempData["error"] = "Please Select A Rider";
                // Redirect to the bulk assign modal with selected parcel IDs
                return RedirectToAction("AssignDeliveryRider", new { parcelIds = parcelIds });
            }

            // Assign the rider to each parcel in the list
            foreach (string id in parcelIds)
            {
                var parcel = _context.Parcels.Find(id);
                if (parcel != null)
                {
                    parcel.Rider = rider;
                    parcel.Status = "Assigned For Delivery";
                    parcel.DeliveryRiderAssignedAt = DateTime.Now;
                }
            }

            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Parcels Assigned Successfully";

            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Admin");
        }


        //status change to Parcel In Hub
        public IActionResult ParcelInHub(string id)
        {
            if (!IsAdminLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel In Hub";
            parcel.ParcelAtTheHubReceivedAt = DateTime.Now;
            //rider.State = "Available";
            _context.Parcels.Update(parcel);
            //_context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //Status - ReturnParcelInHub
        public IActionResult ReturnParcelHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var returnParcel = _context.Parcels.Find(id);
            returnParcel.Status = "Return Parcel In Hub";
            _context.Parcels.Update(returnParcel);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //assignRiderToMerchant
        public IActionResult AssignRiderToMerchant(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            // Find the parcel by ID
            var parcel = _context.Parcels.Where(p => p.Id == id).Include(m => m.Merchant).Include(s=>s.Store).FirstOrDefault();
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.ToList();

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);

        }

        [HttpPost]
        public IActionResult AssignRiderToMerchant(string id, string riderId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                TempData["error"] = "Please Select A Rider";
                return RedirectToAction("AssignRiderToMerchant", new { id = id });
            }

            // Assign the rider to the parcel
            parcel.Rider = rider;
            parcel.Status = "Returned Assigned Pickup Agent";
            parcel.ReturnedAssignedPickupAgentAt = DateTime.Now;

            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Admin");
        }

        //status - ExchangeParcelHub
        public IActionResult ExchangeParcelHub(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var exchangeParcel = _context.Parcels.Find(id);
            exchangeParcel.Status = "Exchange Parcel In Hub";
            _context.Parcels.Update(exchangeParcel);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //RiderForExchnage
        public IActionResult RiderForExchange(string id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Where(p => p.Id == id).Include(m => m.Merchant).FirstOrDefault();
            if (parcel == null)
            {
                return NotFound();
            }
            var riders = _context.Riders.Where(u => u.State == "Available");
            ViewBag.Riders = riders;
            return View(parcel);
        }

        //add new parcel
        public IActionResult AddNewParcel()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            //parcel with hub information
            ViewBag.HubList = _context.Hubs.ToList();

            ViewBag.Districts = _context.District.ToList();
            ViewBag.Zones = _context.Zone.ToList();
            ViewBag.Area = _context.Areas.ToList();

            ViewBag.Merchants = _context.Merchants.ToList();
            ViewBag.Stores = _context.Stores.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddParcel(Parcel parcel)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            //find merchant according to store id
            var merchant = _context.Merchants.Find(parcel.MerchantId);
            //find merchant id and set
            parcel.MerchantId = merchant.Id;

            //find hub according to store id
            var store = _context.Stores.Find(parcel.StoreId);
            var hub = _context.Hubs.Find(store.HubId);
            //find hub id and set
            parcel.HubId = hub.Id;


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

        // Make an HTTP GET request to fetch parcel information
        #region
        public async Task<IActionResult> GetParcelInfo(string trackingId)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Create a new HttpClient instance
            using (var client = new HttpClient())
            {
                // Set the base address for the HTTP client
                client.BaseAddress = new Uri("https://api.example.com");

                // Send an HTTP GET request to the API endpoint
                var response = await client.GetAsync($"/parcels/{trackingId}");

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response content to a Parcel object
                    var parcel = JsonConvert.DeserializeObject<Parcel>(content);

                    // Pass the parcel object to the view
                    return View(parcel);
                }
                else
                {
                    // Handle the error response
                    return View("Error");
                }
            }
        }
        #endregion




    }
}
