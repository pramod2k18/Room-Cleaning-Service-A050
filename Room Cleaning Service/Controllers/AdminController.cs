using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Net;
using System.Dynamic;
using RoomCleaning.Models;
using Rotativa;

namespace RoomCleaning.Controllers
{
    public class AdminController : Controller
    {

        AdminDataContext db = new AdminDataContext();
        // GET: Admin
        //Login GET
        [HttpGet]
        public ActionResult Login() {

            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin adminLogin, string returnUrl = "") {
            string message = "";
            ViewBag.Message = message;
            using (var context = new AdminDataContext())
            {
                var entity = context.AdminTbls.FirstOrDefault(x => x.AdminID == adminLogin.AdminID);
                if (entity != null)
                {
                    if (string.Compare(entity.Password, adminLogin.Password) == 0)
                    {
                        int timeout = adminLogin.RememberMe ? 525600 : 120;
                        var ticket = new FormsAuthenticationTicket(adminLogin.AdminID, adminLogin.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        } else
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    } else
                    {
                        message = "Password not matching";
                    }
                } else
                {
                    message = "Admin ID not Present";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout

        [Authorize]
        [HttpPost]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }

        //Approve Cleaner
        [Authorize]
        public ActionResult Approve() {
            var context = new AdminDataContext();
            return View(context.Cleaners.ToList());
        }

        [Authorize]
        public ActionResult Edit(int id) {
            var context = new AdminDataContext();
            var std = context.Cleaners.Where(s => s.Id == id).FirstOrDefault();

            return View(std);
        }
        //Editing Admin Approval
        [HttpPost]
        public ActionResult Edit(Cleaner updcleaner) {
            //update student in DB using EntityFramework in real-life application
            var context = new AdminDataContext();
            Cleaner originalcleaner = context.Cleaners.Where(s => s.Id == updcleaner.Id).FirstOrDefault();
            originalcleaner.AdminApproved = updcleaner.AdminApproved;
            originalcleaner.ConfirmPassword = originalcleaner.Password;
            context.Entry(originalcleaner).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Approve");
        }


        //Displaying  Cleaner Details
        [Authorize]
        public ActionResult Details(int? id) {
            var context = new AdminDataContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaner cleaner = context.Cleaners.Find(id);
            if (cleaner == null)
            {
                return HttpNotFound();
            }
            return View(cleaner);
        }

        //Landing Page
        [Authorize]
        public ActionResult Index() {
            return View();
        }

        //Tickets Display

        public ActionResult TicketDisplay() {

            return View(db.Tickets.Where(x => x.Status == false).ToList());
        }

        //Ticket Reply

        public ActionResult TicketReply(int? id) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketReply([Bind(Include = "Id,Issue,Description,Date,Resolution,Status,UserEmail")] Ticket ticket) {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TicketDisplay");
            }
            return View(ticket);
        }

        //Cleaner List

        private List<Cleaner> GetCleaners() {
            var context = new AdminDataContext();
            List<Cleaner> CleanerList = context.Cleaners.ToList();
            return CleanerList;
        }
        //Service List
        private List<Service> GetServices() {
            var context = new AdminDataContext();
            List<Service> ServiceList = context.Services.ToList();
            return ServiceList;
        }
        //Filter Cleaner and Service
        [Authorize]
        public ActionResult Filter(string location) {
            var context = new AdminDataContext();
            ViewBag.Locationservice = (from r in context.Services
                                       select r.Location).Distinct();
            ViewBag.Locationcleaner = (from r in context.Cleaners
                                       select r.Location).Distinct();
            dynamic mymodel = new ExpandoObject();
            mymodel.Cleanerz = from r in context.Cleaners
                               where r.Location == location || location == null || location == ""
                               select r;
            //GetCleaners();
            mymodel.Services = from r in context.Services
                               where r.Location == location || location == null || location == ""
                               select r;
            return View(mymodel);
        }
        //Edit for Service Booking
        public ActionResult EditServiceBooking(int Orderid) {
            var context = new AdminDataContext();
            var selectedservice = context.Services.Where(s => s.OrderId == Orderid).FirstOrDefault();
            return View(selectedservice);
        }
        //Edit for Service Booking POST
        [HttpPost]
        public ActionResult EditServiceBooking(Service serv) {
            var context = new AdminDataContext();
            Service originalservice = context.Services.Where(s => s.OrderId == serv.OrderId).FirstOrDefault();
            originalservice.Status_Admin = serv.Status_Admin;
            originalservice.Cleaner_Id = serv.Cleaner_Id;
            originalservice.Service_Status = serv.Service_Status;
            context.SaveChanges();
            return RedirectToAction("Filter", "Admin");
        }

        //Edit for Cleaner Booking
        public ActionResult EditCleanerlist(int id) {
            var context = new AdminDataContext();
            var selectedcleaner = context.Cleaners.Where(s => s.Id == id).FirstOrDefault();
            return View(selectedcleaner);
        }

        //Edit for Cleaner Booking POST
        [HttpPost]
        public ActionResult EditCleanerlist(Cleaner cler) {
            var context = new AdminDataContext();
            Cleaner originalcleaner = context.Cleaners.Where(s => s.CleanerId == cler.CleanerId).FirstOrDefault();
            originalcleaner.CleanerAssigned = cler.CleanerAssigned;
            originalcleaner.ConfirmPassword = originalcleaner.Password;
            context.SaveChanges();
            return RedirectToAction("Filter");
        }

        //View User Feedback for services 
        public ActionResult FeedbackDisplay() {
            return View(db.Feedbacks.ToList());
        }

        //Generating PDF 

        public ActionResult ServicesList(string location) {
            AdminDataContext db = new AdminDataContext();

            ViewBag.Locationservice = (from r in db.Services
                                       select r.Location).Distinct();

            var slist = db.Services.Where(x => x.Location == location).ToList();
            return View(slist);
        }

        public ActionResult FeedbackList(string location) {
            AdminDataContext db = new AdminDataContext();

            ViewBag.Locationfeedback = (from r in db.Feedbacks
                                        select r.Location).Distinct();
            var flist = db.Feedbacks.Where(x => x.Location == location).ToList();
            return View(flist);

        }

        public ActionResult CleanerList(string location)
        {
            AdminDataContext db = new AdminDataContext();

            ViewBag.LocationCleaner = (from r in db.Cleaners
                                       select r.Location).Distinct();
            var flist = db.Cleaners.Where(x => x.Location == location).ToList();
            return View(flist);

        }


        public ActionResult GenerateCleanerPdf(string loc)
        {

            var report = new ActionAsPdf("CleanerList", new { location = loc });
            return report;
        }

        public ActionResult GenerateServicesPdf(string loc) {

            var report = new ActionAsPdf("ServicesList", new { location = loc });
            return report;
        }


        public ActionResult GenerateFeedbackPdf(string loc) {
            var report = new ActionAsPdf("FeedbackList", new { location = loc });
            return report;
        }

        public ActionResult GeneratePDF() {
            return View();
        }

    }
}
