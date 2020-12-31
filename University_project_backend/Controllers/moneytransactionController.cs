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
        public class transactionController : ApiController
        {
        Contextclass contextclass = new Contextclass();

            [System.Web.Http.HttpPost]
            public HttpResponseMessage moneytransaction([FromBody] moneytransfer moneytransfers,string data)
            {
            try
            {
                using (Contextclass contextclass = new Contextclass())
                {
                    var id = contextclass.user_Details.Where(x => x.Activationcode == data).Select(x => x.ID).FirstOrDefault();
                    moneytransfers.Account_id = id;
                    moneytransfers.Date = DateTime.Now.Date;
                    var balance = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Balance).FirstOrDefault();
                    var balanceverification = balancechecking(balance, moneytransfers.Amount);
                    if (balanceverification == true)
                    {
                        var updatebalance = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).FirstOrDefault();
                        var new_balance = balance - moneytransfers.Amount;
                        updatebalance.Balance = new_balance;
                        contextclass.Entry(updatebalance).State = System.Data.Entity.EntityState.Modified;
                        contextclass.SaveChanges();

                        contextclass.moneytransfers.Add(moneytransfers);
                        contextclass.SaveChanges();

                        var reciver_balance_update = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).FirstOrDefault();
                        var present_receiver_balance = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).Select(x => x.Balance).FirstOrDefault();
                        var latest_balance = present_receiver_balance + moneytransfers.Amount;
                        reciver_balance_update.Balance = latest_balance;
                        contextclass.Entry(reciver_balance_update).State = System.Data.Entity.EntityState.Modified;
                        contextclass.SaveChanges();

                        var sender_account_number = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Account_number).FirstOrDefault();
                        var sender_emai_address = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Email_address).FirstOrDefault();
                        var sender_name = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Name).FirstOrDefault();
                        var email_address_client = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).Select(x => x.Email_address).FirstOrDefault();
                        SendEmailToReciver(sender_emai_address, sender_name, sender_account_number, moneytransfers.Amount, email_address_client, latest_balance);

                        var immediate_balance = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Balance).FirstOrDefault();
                        var reciver_email = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).Select(x => x.Email_address).FirstOrDefault();
                        var reciver_account_number = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).Select(x => x.Account_number).FirstOrDefault();
                        var receiver_name = contextclass.user_Details.Where(x => x.Account_number == moneytransfers.To_account).Select(x => x.Name).FirstOrDefault();
                        var email_address_sender = contextclass.user_Details.Where(x => x.ID == moneytransfers.Account_id).Select(x => x.Email_address).FirstOrDefault();
                        SendEmailTosender(reciver_email, receiver_name, reciver_account_number, immediate_balance, email_address_sender, moneytransfers.Amount);

                        var message = Request.CreateResponse(HttpStatusCode.Accepted);
                        return message;
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent);
                    }


                    
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);


            }
            }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Account_number_search(string account_number)
        {
            var search=contextclass.user_Details.Where(x => x.Account_number.StartsWith(account_number)).Select(x=>x.Account_number).ToList();
            if(search.Count==0)
            {
                return Request.CreateResponse(HttpStatusCode.OK,"Not Found");
                
            }else
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, search);
            }

        }

        public bool balancechecking(int balance,int sending_amount )
        {
            if (sending_amount<=balance)
            {
                return true;
            }
            else return false;
        }




        public void SendEmailTosender(string emailId, string Name, string account_number, int balance,string email_address_to_sender,int amount)
        {
            var link = "Localhost/4200";
            // var link = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, GenarateUserVerificationLink);
            var fromMail = new MailAddress("projectu889@gmail.com", "Preetham"); // set your email  
            var fromEmailpassword = "ranjitha716"; // Set your password
            var toEmail = new MailAddress(email_address_to_sender);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Sent";
            Message.Body = "<br/>Sent" +
                           "<br/>Name" +" "+ Name +
                           "<br/>Account Number" + " " + account_number +
                           "<br/>Email Address" + " " + emailId +
                           "<br/>Amount"+""+amount+
                           "<br/> Account Balance" + " " + balance +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }



        public void SendEmailToReciver(string emailId, string Name, string account_number,int amount,string email_address,int latest_balance)
            {
                var link = "Localhost/4200";
               // var link = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, GenarateUserVerificationLink);
                var fromMail = new MailAddress("projectu889@gmail.com", "Preetham"); // set your email  
                var fromEmailpassword = "ranjitha716"; // Set your password
                var toEmail = new MailAddress(email_address);

                var smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = "Recived";
                Message.Body = "<br/>Recived" +
                               "<br/>Name"+" "+Name+
                               "<br/> Account Number</h4>"+" "+account_number+
                               "<br/>Email Address"+" "+emailId+
                               "<br/>Amount"+" "+amount+
                               "<br>Account Balance"+""+latest_balance+
                               "<br/><br/><a href=" + link + ">" + link + "</a>";
                Message.IsBodyHtml = true;
                smtp.Send(Message);
            }

              [System.Web.Http.HttpGet]
              public HttpResponseMessage transactiondetails(string activationcode)
               {
            try { 
            var id = contextclass.user_Details.Where(x => x.Activationcode == activationcode).Select(x => x.ID).FirstOrDefault();
                if (id != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, contextclass.moneytransfers.Where(x => x.Account_id == id).OrderBy(x=>x.Date).ThenBy(x=>x.Date).ToList());
                }
                else return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        }



    }

