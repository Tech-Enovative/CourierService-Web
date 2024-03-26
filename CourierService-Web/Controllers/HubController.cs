using Amazon.S3.Model;
using CourierService_Web.Data;
using CourierService_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CourierService_Web.Controllers
{
    public class HubController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HubController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void UpdateLayout()
        {
            //pickup request count
            var pickupRequestCount = _context.Parcels.Where(p => p.Status == "Pickup Request").Count();
            ViewBag.PickupRequestCount = pickupRequestCount;

            //total percel count according to hubId
            var hubId = Request.Cookies["HubId"];
            var totalParcelCount = _context.Parcels.Where(p => p.HubId == hubId).Count();
            ViewBag.TotalParcelCount = totalParcelCount;

            //delivered parcel count according to hubId
            var deliveredParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.DeliveryId != null).Count();
            ViewBag.DeliveredParcelCount = deliveredParcelCount;

           

            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);
            //all parcel list for today
            var todayParcelList = _context.Parcels
                .Where(p => p.HubId == hubId && p.PickupRequestDate >= todayStart && p.PickupRequestDate < tomorrowStart).Include(u => u.Merchant).Include(u => u.Rider)
                .ToList();
            ViewBag.TodayParcelList = todayParcelList;

            //all parcel list for today count
            var todayParcelCount = todayParcelList.Count;
            ViewBag.TodayParcelCount = todayParcelCount;

            //return parcel count according to hubId
            var returnParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.ReturnId !=null).Count();
            ViewBag.ReturnParcelCount = returnParcelCount;

            //exchange parcel count according to hubId
            var exchangeParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.ExchangeId !=null).Count();
            ViewBag.ExchangeParcelCount = exchangeParcelCount;

            //parcel in hub count according to hubId
            var parcelInHubCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Parcel In Hub").Count();
            ViewBag.ParcelInHubCount = parcelInHubCount;

            //parcel Assigned A Rider For Pickup count according to hubId
            var parcelAssignedRiderForPickupCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Assigned A Rider For Pickup").Count();
            ViewBag.ParcelAssignedRiderForPickupCount = parcelAssignedRiderForPickupCount;

            //parcel Assigned For Delivery count according to hubId
            var parcelAssignedForDeliveryCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Assigned For Delivery").Count();
            ViewBag.ParcelAssignedForDeliveryCount = parcelAssignedForDeliveryCount;

            //total amount calculation from rider payment today according to hubId
            var totalAmount = _context.riderPayments
                .Where(r => r.Parcel.HubId == hubId && r.PaymentDate.Date == DateTime.Today.Date)
                .Sum(r => r.Amount);
            ViewBag.TotalAmount = totalAmount;
            

            //received amount calculation from hub payment today according to hubId
            var receivedAmount = _context.HubPayments
                .Where(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date)
                .Sum(h => h.AmountReceived);
            ViewBag.ReceivedAmount = receivedAmount;

            //due amount calculation from rider payment today according to hubId
            var dueAmount = _context.riderPayments
                .Where(h => h.Parcel.HubId == hubId && h.PaymentDate.Date == DateTime.Today.Date)
                .Sum(h => h.Amount);
            ViewBag.DueAmount = dueAmount;

            //due amount calculation from hub payment today according to hubId
            var dueAmountHub = _context.HubPayments
                .Where(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date)
                .Sum(h => h.DueAmount);
            ViewBag.DueAmountHub = dueAmountHub;

            //total amount calculation by hubId in hub payment
            var totalAmountHub = _context.HubPayments
                .Where(h => h.HubId == hubId)
                .Sum(h => h.TotalAmount);
            ViewBag.TotalAmountHub = totalAmountHub;

            



            



        }

        //ishublogged in
        public bool IsHubLoggedIn()
        {
            if (Request.Cookies["HubId"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Action to pay to merchant
        public IActionResult PayToMerchant(string merchantId)
        {
           
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

           
            var hubId = Request.Cookies["HubId"];

            // Calculate total amount collected for the merchant by summing up the total amounts from HubPayments
            var totalAmountCollected = _context.riderPayments.Where(h => h.Parcel.HubId == hubId && h.PaymentDate.Date == DateTime.Today.Date && h.Parcel.MerchantId == merchantId)
                .Sum(h => h.Amount);
            
            var merchant = _context.Merchants.FirstOrDefault(m => m.Id == merchantId);

            if (merchant == null)
            {
                // Handle if merchant is not found
                TempData["error"] = "Merchant not found.";
                return RedirectToAction("Index"); // Redirect to appropriate action
            }

            // Update merchant's payment information
            var merchantPayment = new MerchantPayment
            {
                MerchantId = merchantId,
                TotalAmount = totalAmountCollected,
                AmountPaid = totalAmountCollected,
                DueAmount = 0,
                DateTime = DateTime.Now,
                HubPaymentId = _context.HubPayments.FirstOrDefault(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date)?.Id
            };
            

            // Create a HubPayment for this payment
            
            

            try
            {
                _context.MerchantPayments.Add(merchantPayment);
                _context.SaveChanges();

                //parcels to update for today only
                var parcelsToUpdate = _context.Parcels
                    .Where(p => p.MerchantId == merchantId && p.HubId == hubId && p.PaymentStatus != "Paid To Merchant" && p.PickupRequestDate.Value.Date == DateTime.Today.Date)
                    .ToList();
            

                foreach (var parcel in parcelsToUpdate)
                {
                    parcel.PaymentStatus = "Paid To Merchant";
                }
                _context.SaveChanges();

                TempData["success"] = "Payment to merchant processed successfully.";
                return RedirectToAction("Index"); // Redirect to appropriate action
            }
            catch (Exception ex)
            {
                // Handle exception
                TempData["error"] = "An error occurred while processing the payment to merchant: " + ex.Message;
                return RedirectToAction("Index"); // Redirect to appropriate action
            }
        }



        //FullPayment
        [HttpPost]
        public IActionResult FullPayment()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            


            var hubId = Request.Cookies["HubId"];
            var hub = _context.HubPayments.FirstOrDefault(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date);

            //check if hub payment is already done
            if (hub != null)
            {
                TempData["error"] = "Payment Already Received";
                return RedirectToAction("Index");
            }

            // Total amount received in rider payments today according to hubId
            var totalAmount = _context.riderPayments
                .Where(r => r.Parcel.HubId == hubId && r.PaymentDate.Date == DateTime.Today.Date)
                .Sum(r => r.Amount);

            var hubPayment = new HubPayment
            {
                HubId = hubId,
                TotalAmount = totalAmount,
                AmountReceived = totalAmount,
                DueAmount = 0,
                DateTime = DateTime.Now,
                //RiderPayments = _context.riderPayments.Where(r => r.Parcel.HubId == hubId && r.PaymentDate.Date == DateTime.Today.Date).ToList()
            };

            //



            _context.HubPayments.Add(hubPayment);

            _context.SaveChanges();
            TempData["success"] = "Full Payment Received Successfully";
            return RedirectToAction("Index");
        }

        //partial payment
        public IActionResult PartialPayment(int amount)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (amount <= 0)
            {
                TempData["error"] = "Amount must be greater than 0";
                return RedirectToAction("Index");
            }

            var hubId = Request.Cookies["HubId"];

            var totalAmount = _context.riderPayments
               .Where(r => r.Parcel.HubId == hubId && r.PaymentDate.Date == DateTime.Today.Date)
               .Sum(r => r.Amount);


            //hub existing payment
            var hubPayment = _context.HubPayments.FirstOrDefault(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date);
            if (hubPayment == null)
            {
                var newPayment = new HubPayment
                {
                    HubId = hubId,
                    TotalAmount = totalAmount,
                    AmountReceived = amount,
                    DueAmount = totalAmount - amount,
                    DateTime = DateTime.Now
                };
                _context.HubPayments.Add(newPayment);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                var dueAmount = hubPayment.DueAmount;
                if (dueAmount == 0)
                {
                    TempData["error"] = "No Due Amount";
                    return RedirectToAction("Index");
                }
                if (amount > dueAmount)
                {
                    TempData["error"] = "Amount must be less than or equal to due amount";
                    return RedirectToAction("Index");
                }
                hubPayment.AmountReceived += amount;
                hubPayment.DueAmount = dueAmount - amount;
            }
            _context.SaveChanges();
            TempData["success"] = "Partial Payment Received Successfully";
            return RedirectToAction("Index");
        }

        //Not Received
        [HttpPost]
        public IActionResult NotReceived()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var hubId = Request.Cookies["HubId"];

            var hubPayment = _context.HubPayments.FirstOrDefault(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date);
            if (hubPayment == null)
            {
                TempData["error"] = "No Payment Received Today";
                return RedirectToAction("Index");
            }

            if (hubPayment.DueAmount == 0)
            {
                TempData["error"] = "No Due Amount";
                return RedirectToAction("Index");
            }

            hubPayment.AmountReceived = 0;
            hubPayment.DueAmount = hubPayment.TotalAmount;
            _context.SaveChanges();
            TempData["success"] = "Payment Not Received";
            return RedirectToAction("Index");
        }

        //RiderPayment List according to hubId
        public IActionResult RiderPaymentList()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var hubId = Request.Cookies["HubId"];
            // Rider payment list according to hubId today
            var riderPayments = _context.riderPayments
                .Where(rp => rp.Parcel.HubId == hubId && rp.PaymentDate.Date == DateTime.Today.Date)
                .Include(rp => rp.Parcel)
                .Include(rp => rp.Rider)
                .ToList();

            // Group rider payments by Rider and calculate the total amount for each Rider
            var riderTotalAmounts = riderPayments.GroupBy(rp => rp.RiderId)
                .Select(g => new
                {
                    RiderId = g.Key,
                    RiderName = g.First().Rider.Name,
                    TotalAmount = g.Sum(rp => rp.Amount)
                })
                .ToList();

            // Amount collected by rider today according to hubId
            ViewBag.AmountCollected = riderPayments.Sum(rp => rp.Amount);

            // Hub received amount by hub today
            var hubReceivedAmount = _context.HubPayments
                .Where(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date)
                .Sum(h => h.AmountReceived);
            ViewBag.HubReceivedAmount = hubReceivedAmount;

            // Due
            ViewBag.DueAmount = ViewBag.AmountCollected - ViewBag.HubReceivedAmount;

            return View(riderTotalAmounts);
        }


        //merchant payment list according to hubId
        public IActionResult MerchantPaymentList(bool showDuePayments = false)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var hubId = Request.Cookies["HubId"];

                // Retrieve hub payment Id for today
                var hubPaymentId = _context.HubPayments
                    .FirstOrDefault(h => h.HubId == hubId && h.DateTime.Date == DateTime.Today.Date)?.Id;

                // Log the retrieved hubId for debugging
                Console.WriteLine("HubId from cookie: " + hubId);

                // Query for merchant payments according to hubId and date
                var merchantPaymentsQuery = _context.MerchantPayments
                    .Where(m => m.HubPaymentId == hubPaymentId && m.DateTime.Date == DateTime.Today.Date);

                // Include merchant details
                merchantPaymentsQuery = merchantPaymentsQuery.Include(m => m.Merchant);

                // Filter by due payments if requested
                if (showDuePayments)
                {
                    merchantPaymentsQuery = merchantPaymentsQuery.Where(p => p.DueAmount > 0);
                }

                var merchantPayments = merchantPaymentsQuery.ToList();

                // Log the count of retrieved merchant payments for debugging
                Console.WriteLine("Merchant payments count: " + merchantPayments.Count);

                return View(merchantPayments);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine("Error occurred: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }






        public IActionResult Index()
        {
            UpdateLayout();
            return View();
        }

        public IActionResult Parcel(DateTime? startDate, DateTime? endDate)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var hubId = Request.Cookies["HubId"];
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Where(x => x.HubId == hubId)
                .Include(u => u.Rider)
                .Include(m => m.Merchant)
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
            // Pass selected date range to the view
            ViewBag.StartDate = startDate ?? DateTime.Today;
            ViewBag.EndDate = endDate ?? DateTime.Today;
            

            return View(parcels);
        }

        public IActionResult DownloadCsv(DateTime? startDate, DateTime? endDate)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var hubId = Request.Cookies["HubId"];

            // Get the list of parcels based on the selected date range
            IQueryable<Parcel> parcelsQuery = _context.Parcels
                .Where(x => x.HubId == hubId)
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
                csvContent.AppendLine($"\"{parcel.Id}\",\"{parcel.Merchant?.Name}\",\"{parcel.Hub?.Name ?? "Not Assigned"}\",\"{parcel.Rider?.Name ?? "Not Assigned"}\",\"{parcel.PickupLocation}\",\"{parcel.PickupRequestDate?.ToString("M/d/yyyy, h:mm tt")}\",\"{parcel.ReceiverName}\",\"{parcel.ReceiverAddress}\",\"{parcel.ReceiverContactNumber}\",\"{parcel.ProductName}\",{parcel.ProductWeight},{parcel.ProductPrice},{parcel.ProductQuantity},\"{parcel.DeliveryType}\",{parcel.DeliveryCharge},{parcel.TotalPrice},\"{parcel.Status}\",\"{parcel.PaymentStatus}\"");
            }

            // Return CSV file
            return File(Encoding.UTF8.GetBytes(csvContent.ToString()), "text/csv", $"Parcels_{startDate?.ToString("yyyy-MM-dd")}_{endDate?.ToString("yyyy-MM-dd")}.csv");
        }


        //Return parcel

        //Status - ReturnParcelInHub
        public IActionResult ReturnParcelHub(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var returnParcel = _context.Parcels.Find(id);
            returnParcel.Status = "Return Parcel In Hub";
            _context.Parcels.Update(returnParcel);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //status - ExchangeParcelHub
        public IActionResult ExchangeParcelHub(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var exchangeParcel = _context.Parcels.Find(id);
            exchangeParcel.Status = "Exchange Parcel In Hub";
            _context.Parcels.Update(exchangeParcel);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }
        //assign a parcel
        public IActionResult AssignParcel(string id)
        {

            if (!IsHubLoggedIn())
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

        public IActionResult AssignDeliveryRider(string id)
        {

            if (!IsHubLoggedIn())
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
            if (!IsHubLoggedIn())
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
            parcel.DispatchDate = DateTime.Now.Date;


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Parcel Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Hub");
        }

        [HttpPost]
        public IActionResult AssignDeliveryRider(string id, string riderId)
        {
            if (!IsHubLoggedIn())
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
            //parcel.DispatchDate = DateTime.Now.Date;


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Hub");
        }

        //assignRiderToMerchant
        public IActionResult AssignRiderToMerchant(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            // Find the parcel by ID
            var parcel = _context.Parcels.Where(p=>p.Id == id).Include(m=>m.Merchant).FirstOrDefault();
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
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                TempData["error"] = "Please Select A Rider";
                return RedirectToAction("AssignRiderToMerchant", new { id = id });
            }
            parcel.Rider = rider;
            parcel.Status = "Assigned For Return Parcel";
            //parcel.DispatchDate = DateTime.Now.Date;
            _context.SaveChanges();
            TempData["success"] = "Assigned Rider For Return Parcel Successfully";
            return RedirectToAction("Parcel", "Hub");
        }

        //RiderForExchnage
        public IActionResult RiderForExchange(string id)
        {
            if (!IsHubLoggedIn())
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
        [HttpPost]
        public IActionResult RiderForExchange(string id, string riderId)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return NotFound();
            }
            parcel.Rider = rider;
            parcel.Status = "Assigned For Exchange Parcel";
            _context.SaveChanges();
            TempData["success"] = "Assigned Rider For Exchange Parcel Successfully";
            return RedirectToAction("Parcel", "Hub");
        }


        //status change to Parcel In Hub
        public IActionResult ParcelInHub(string id)
        {
            if (!IsHubLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            //var riderId = HttpContext.Request.Cookies["RiderId"];
            ////find rider by riderId
            //var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel In Hub";
            parcel.DeliveryDate = DateTime.Now.Date;
            //rider.State = "Available";
            _context.Parcels.Update(parcel);
            //_context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //Profile
        public IActionResult Profile()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            var hub = _context.Hubs.FirstOrDefault(h => h.Id == hubId);
            if (hub == null)
            {
                return NotFound();
            }
            return View(hub);
        }

        //delivery parcel list
        public IActionResult DeliveredParcelList(DateTime? selectedDate)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            IQueryable<Parcel> deliveredParcelsQuery = _context.Parcels
                .Where(x => x.HubId == hubId && x.DeliveryId != null)
                .Include(x => x.DeliveryParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);

            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }

            deliveredParcelsQuery = deliveredParcelsQuery.Where(x => x.DeliveryParcel.DeliveryDate.Date == selectedDate.Value.Date);
            var deliveredParcels = deliveredParcelsQuery.ToList();
            return View(deliveredParcels);
        }

        //return parcel list
        public IActionResult ReturnParcelList(DateTime? selectedDate)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            IQueryable<Parcel> returnParcelsQuery = _context.Parcels
                .Where(x => x.HubId == hubId && x.ReturnId != null)
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

        //exchange parcel list
        public IActionResult ExchangeParcelList(DateTime? selectedDate)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            IQueryable<Parcel> exchangeParcelsQuery = _context.Parcels
                .Where(x => x.HubId == hubId && x.ExchangeId != null)
                .Include(x => x.ExchangeParcel)
                .Include(x => x.Rider)
                .Include(h => h.Hub);
            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today;
            }
            exchangeParcelsQuery = exchangeParcelsQuery.Where(x => x.ExchangeParcel.ExchangeDate.Date == selectedDate.Value.Date);
            var exchangeParcels = exchangeParcelsQuery.ToList();
            return View(exchangeParcels);
        }

        public IActionResult PaymentInHand(string? id)
        {
            if (!IsHubLoggedIn())
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

            parcel.PaymentInHand = "PaymentInHand";
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Payment In Hand";
            return RedirectToAction("Parcel");
        }

        public IActionResult PaymentNotInHand(string? id)
        {
            if (!IsHubLoggedIn())
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

            parcel.PaymentInHand = "PaymentNotInHand";
            TempData["success"] = "Payment Not In Hand";
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Payment Payment In Hand";
            return RedirectToAction("Parcel");
        }

        //PaymentMerchant
        public IActionResult PaymentMerchant(string? id)
        {
            if (!IsHubLoggedIn())
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

            parcel.PaymentStatus = "Paid To Merchant";
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Payment Merchant";
            return RedirectToAction("Parcel");
        }

        //PaymentDeliveryCharge
        public IActionResult PaymentDeliveryCharge(string? id)
        {
            if (!IsHubLoggedIn())
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

            parcel.PaymentStatus = "Paid Delivery Charge To Merchant";
            _context.Parcels.Update(parcel);
            _context.SaveChanges();

            TempData["success"] = "Payment Delivery Charge";
            return RedirectToAction("Parcel");
        }
    }
}
