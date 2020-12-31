using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Http;
using System.Data.Entity;
using University_project_backend.Models;
using System.Net.Http;
using System.Net;
using System.Net.Mail;


namespace University_project_backend.Controllers
{
    public class HomeController : ApiController
    {
        Contextclass contextclass = new Contextclass();
        public IEnumerable<User_details> Get()
        {

            using (Contextclass contextclass = new Contextclass())
            {
                return contextclass.user_Details.ToList();
            }
        }


        public HttpResponseMessage Post([FromBody] User_details Details)
        {

            try
            {
                using (Contextclass contextclass = new Contextclass())
                {
                     var emailverfication = emailexits(Details.Email_address);
                      if (emailverfication)
                      {
                          var Res = Request.CreateResponse(HttpStatusCode.OK);
                          return Res;
                      }
                      Details.Activationcode = Guid.NewGuid().ToString();
                      var account_number = Account_number_generator();
                     // Details.Account_type = "saving";
                      Details.Account_number = account_number;
                      Details.Balance = 000;
                      Details.Date_of_creation = DateTime.Now.Date;
                      Details.Loan = "NO";
                      Details.Loan_amount = 000;
                      Details.Loan_status = "Not Approved";
                      Details.Verified = "NO";
                      contextclass.user_Details.Add(Details);
                      contextclass.SaveChanges();
                     SendEmailToUser(Details.Email_address,Details.Activationcode);
                    var Message = Request.CreateResponse(HttpStatusCode.Created, Details);
                    Message.Headers.Location = new Uri(Request.RequestUri + Details.ID.ToString());
                            return Message;
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        //login verification

        [System.Web.Http.HttpPost]
         public HttpResponseMessage login([FromBody] Login login)
        {
            bool Isvalid = contextclass.user_Details.Any(x => x.Email_address == login.Email_address && x.Verified == "YES" &&
              x.Password == login.Password);
            if (Isvalid)
            {
                var activationcode = contextclass.user_Details.Where(x => x.Email_address == login.Email_address).Select(s => s.Activationcode).Single();
                var message = Request.CreateResponse(HttpStatusCode.OK, activationcode);
                return message;
                
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent);
            
        }



        // user verfication


        [System.Web.Http.HttpPut]
        public HttpResponseMessage user_verification(string activationcode)
        {
            var verify = contextclass.user_Details.Where(x => x.Activationcode == activationcode).FirstOrDefault();
            if (verify != null)
            {
            verify.Verified = "YES";
            contextclass.Entry(verify).State = System.Data.Entity.EntityState.Modified;
            contextclass.SaveChanges();
            var message = Request.CreateResponse(HttpStatusCode.Accepted);
            return message;
            }
            else
                {
                     var mesaage = Request.CreateResponse(HttpStatusCode.NotFound);
                     return mesaage;
                 }

        }

          
        //Email method 

        public void SendEmailToUser(string emailId,string activationCode)
        {
            var GenarateUserVerificationLink = "/Userverification/Userverification?activationcode="+ activationCode;
            var link = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, GenarateUserVerificationLink);
            var fromMail = new MailAddress("projectu889@gmail.com", "Preetham"); // set your email  
            var fromEmailpassword = "ranjitha716"; // Set your password
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Registration Completed-Demo";
            Message.Body = "<br/> Your registration completed succesfully." +
                           "<br/> please click on the below link for account verification" +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }



        //Existing email checking

        public bool emailexits(string eMail)
        {
            var check = contextclass.user_Details.Where(email => email.Email_address == eMail).FirstOrDefault();
            return check != null;

        }



        //Random Account number generator

       public string Account_number_generator()
        {
            string num = "012345678";
            int len = num.Length;
            string account_number = string.Empty;
            int account_degit = 5;
            int getindex;
            string finaldegit;
            for (var i = 0; i < account_degit; i++)
            {
                do
                {
                    getindex = new Random().Next(0, len);
                    finaldegit = num.ToCharArray()[getindex].ToString();
                } while (account_number.IndexOf(finaldegit) != -1);
                account_number += finaldegit;
                
                

            }
            return 6+account_number;

        }

        //account details on home page

        [System.Web.Http.HttpGet]
        public HttpResponseMessage account_details(string activationcode)
        {
          
            return Request.CreateResponse(HttpStatusCode.OK,
                contextclass.user_Details.Where(x => x.Activationcode == activationcode).ToList());
        }
    }
}
