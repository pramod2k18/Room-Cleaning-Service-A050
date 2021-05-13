using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RoomCleaning.Models;

namespace RoomCleaning.Controllers
{
    public class UserController : Controller
    {
        private AdminDataContext db = new AdminDataContext();

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "ActivationCode")] User user)//del this is email part
        {
            bool Status = false;
            string Message = "";
            //model validation
            if (ModelState.IsValid)
            {
                //Email already exist or not
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();

                db.Users.Add(user);
                db.SaveChanges();
                Status = true;
                Message = "New User Created Successfully";
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string Message = "";
            var entity = db.Users.FirstOrDefault(x => x.Email == login.EmailID);
            if (entity != null)
            {

                if (string.Compare(login.Password, entity.Password) == 0)
                {
                    int timeout = login.RememberMe ? 525600 : 120;//525600 = 1 year
                    var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        Session["UID"] = login.EmailID;
                        return RedirectToAction("UserIndex", "User");
                    }
                }
                else
                {
                    Message = "Wrong Password Entered";
                }

            }
            else
            {
                Message = "User with this email dosen't exist";
            }


            ViewBag.Message = Message;
            return View();
        }


        //User Landing Page
        [Authorize]
        public ActionResult UserIndex()
        {
            return View();
        }
        //Forgot UserID
        [HttpGet]
        public ActionResult ForgotUserId()
        {
            return View();
        }
        //Forgot UserID POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotUserId(ForgotUserid user)
        {
            string message = String.Empty;
            bool Status = false;
            //model validation

            var entity = db.Users.FirstOrDefault(x => x.ContactNumber == user.ContactNumber);
            if (entity != null)
            {
                if (string.Compare(user.Ques1, entity.Ques1) == 0 && string.Compare(user.Ques2, entity.Ques2) == 0 && string.Compare(user.Ques3, entity.Ques3) == 0)
                {
                    Status = true;
                    message = $"User Email-ID is {entity.Email} ";
                }
                else
                {
                    message = "Wrong Answers to the Questions";
                }

            }
            else
            {
                message = "User with this Contact Number doesn't exist";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Forgot Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //Forgot Password POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword forgotpassword, string ReturnUrl = "")
        {
            string message = String.Empty;
            if (ModelState.IsValid)
            {
                User entity = db.Users.FirstOrDefault(x => x.Email == forgotpassword.Email);
                if (entity != null)
                {
                    if (string.Compare(forgotpassword.Ques1, entity.Ques1) == 0 && string.Compare(forgotpassword.Ques2, entity.Ques2) == 0 && string.Compare(forgotpassword.Ques3, entity.Ques3) == 0)
                    {

                        return RedirectToAction("ResetPassword", new { UserId = entity.UserId });

                    }
                    else
                    {
                        message = "Wrong Answers to the Questions";
                    }
                }
                else
                {
                    message = "User ID does not Exist";
                }
            }
            ViewBag.Message = message;
            return View(forgotpassword);
        }

        [HttpGet]
        public ActionResult ResetPassword(int UserId)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int UserId, ResetPassword resetPassword)
        {
            string message = String.Empty;
            if (ModelState.IsValid)
            {
                var entity = db.Users.Find(UserId);
                entity.Password = resetPassword.NewPassword;
                entity.ConfirmPassword = entity.Password;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                message = "Password Reset Sucessfully.";
            }

            ViewBag.Message = message;
            return View(resetPassword);
        }

        //ServiceBooking get
        [HttpGet]
        public ActionResult ServiceBooking()
        {
            return View();
        }

        //ServiceBooking Post
        [HttpPost]
        public ActionResult ServiceBooking([Bind(Exclude = "Cleaner_Id,Status_Admin,Status_Cleaner,Service_Status")] Service serviceobj)
        {

            serviceobj.Service_Status = "";
            serviceobj.Status_Admin = false;
            serviceobj.Status_Cleaner = false;
            serviceobj.Payment = false;
            db.Services.Add(serviceobj);
            db.SaveChanges();

            return RedirectToAction("ServiceDisplayUser");
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        //Ticket Create
        [Authorize]
        public ActionResult TicketCreate()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketCreate([Bind(Include = "Id,Issue,Description,Date,Resolution,Status,UserEmail")] Ticket ticket)
        {
            string Message = "";
            
            if (ModelState.IsValid)
            {
                ticket.UserEmail = Convert.ToString(Session["UID"]);
                ticket.Date = DateTime.Now;
                ticket.Resolution = "Not Resolved";
                ticket.Status = false;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                Message = "Ticket Submited Successfully";

            }

            ViewBag.Message = Message;

            return View(ticket);
        }

        //Tickets View

        public ActionResult TicketView()
        {

            
            string uid = Convert.ToString(Session["UID"]);
            return View(db.Tickets.Where(x => x.UserEmail == uid).ToList());
        }

        //Service Booking user
        [Authorize]
        public ActionResult ServiceDisplayUser()
        {

            bool Status = false;
            string currentUser = User.Identity.Name;
            //var context = new AdminDataContext();
            var UserObj = db.Users.Where(x => x.Email == currentUser).FirstOrDefault();
            if (db.Services.Where(x => x.UserEmail == currentUser).FirstOrDefault() != null)
            {
                ViewBag.Status = true;
                var serviceList = db.Services.Where(x => x.UserEmail == currentUser).ToList();
                return View(serviceList);
            }
            ViewBag.Status = Status;
            ViewBag.Message = "First Add a Service";
            return View();
        }


        //Payment
        [Authorize]
        public ActionResult Payment(int? id)
        {

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            Session["Amount"] = (service.RoomCount * 500);
            service.Payment = true;
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();

            }
            if (service == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("PaymentOption");

        }

        public ActionResult PaymentOption()
        {

            return View();

        }

        //Card Payment
        [Authorize]
        public ActionResult CardPayment()
        {
            ViewBag.Amount = Session["Amount"];
            return View();

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CardPayment([Bind(Include = "Id,cardNumber,ExpMonth,ExpYear,cvv,name,amount,Method")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.amount = Convert.ToDouble(Session["Amount"]);
                payment.Method = "Card";
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("ServiceDisplayUser");
            }

            return View(payment);
        }

        //Wallet Payment

        [Authorize]
        public ActionResult WalletPayment()
        {
            ViewBag.Amount = Session["Amount"];
            return View();

        }



        [NonAction]
        public bool IsEmailExist(string email)
        {

            var v = db.Users.FirstOrDefault(x => x.Email == email);
            if (v != null)
            {
                return true;
            }
            else
                return false;

        }

        //Feedback
        [Authorize]
        public ActionResult Feedback(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            ViewBag.Payment = service.Payment;

            Session["OrderId"] = service.OrderId;
            if (service == null)
            {
                return HttpNotFound();
            }
            else if (service.Payment == false)
            {

                return RedirectToAction("ErrorFeedback");
            }



            return RedirectToAction("CreateFeedback");



        }
        //Error Message
        [Authorize]
        public ActionResult ErrorFeedback()
        {
            string msg = "Payment is Pending and service is not Completed";
            ViewBag.MSG = msg;
            return View();
        }

        //Create Feedback 

        public ActionResult CreateFeedback()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFeedback(Feedback feedback)
        {
            string Message = "";
            
            feedback.OrderId = Convert.ToInt32(Session["OrderId"]);
            feedback.UserId = Convert.ToString(Session["UID"]);
            if (ModelState.IsValid)
            {

                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                Message = "Feedback Submitted Successfully";
            }
            ViewBag.Message = Message;
            return View(feedback);
        }

        //View Feedback 

        public ActionResult FeedbackView()
        {
            string uid = Convert.ToString(Session["UID"]);
            return View(db.Feedbacks.Where(s => s.UserId == uid).ToList());
        }
    }
}