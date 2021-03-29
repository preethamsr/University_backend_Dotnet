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
using System.IO;

namespace University_project_backend.Controllers
{
    public class LoanController : ApiController
    {
        [System.Web.Http.HttpPost]
      public HttpResponseMessage Loansubmit([FromBody] Loan ln, string activationcode)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var id = contextclass.user_Details.Where(x => x.Activationcode == activationcode).Select(x => x.ID).FirstOrDefault();
                    ln.Date_of_apply = DateTime.Now.Date;
                    ln.Account_ID = id;
                    contextclass.loans.Add(ln);
                    contextclass.SaveChanges();
                    int Total_Income = ln.Income + ln.Co_applicant_income;
                    var loan_id = ln.LoanID;
                    int Marital_status;
                    int Education;
                    int Employed;
                    int Area;
                    int gender;
                    if(ln.Gender== "Male")
                    {
                        gender = 1;
                    }
                    else { gender = 0; }
                    if (ln.Marital_status== "Married")
                    {
                        Marital_status = 0;
                    }
                    else
                    {
                        Marital_status = 1;
                    }
                    if(ln.Education== "Graduate")
                    {
                        Education = 0;
                    }
                    else
                    {
                        Education = 1;
                    }
                    if(ln.Employment== "Employed")
                    {
                        Employed = 0;
                    }
                    else
                    {
                         Employed = 1;
                    }
                    if(ln.Area== "Urban")
                    {
                        Area = 2;
                    }
                   else if(ln.Area== "Rural")
                    {
                         Area = 0;
                    }
                    else
                    {
                        Area = 1;
                    }
                    string url = string.Format("http://127.0.0.1:5001/?Marital_status="+Marital_status+"&Dependents="+ln.Dependents+"&Education="+Education+"&Employment="+Employed+"&Income="+ln.Income+ "&Total_Income="+Total_Income+"&LoanAmount="+ln.Loan_amount+"&Area="+Area+"&Term="+ln.Tenure+ "&gender="+gender+ "&credit_History="+ln.Credit_History+"");
                    WebRequest requestobj = WebRequest.Create(url);
                    requestobj.Method = "GET";
                    HttpWebResponse responseobj = null;
                    responseobj = (HttpWebResponse)requestobj.GetResponse();
                    string result;
                    using(Stream stream=responseobj.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream);
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    if(result== "Not_Approved")
                    {
                        
                        return Request.CreateResponse(HttpStatusCode.NotFound,result);
                     
                    }
                    else
                    {
                        var Loanstatus = contextclass.loans.Where(x => x.LoanID== loan_id).FirstOrDefault();
                        Loanstatus.Status = true;
                        Loanstatus.Date_of_approval = DateTime.Now.Date;
                        contextclass.SaveChanges();
                        var balace = contextclass.user_Details.Where(x => x.ID == id).Select(x => x.Balance).FirstOrDefault();
                        var balance_details = contextclass.user_Details.Where(x => x.ID == id).FirstOrDefault();
                        var loan_amount =balace+ln.Loan_amount;
                        balance_details.Balance = loan_amount;
                        balance_details.Loan_status = "Approved";
                        balance_details.Loan_amount = ln.Loan_amount;
                        balance_details.Loan = "YES";
                        contextclass.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.Accepted, result);
                    }
                    

                    return Request.CreateResponse(HttpStatusCode.Accepted,result+"thank you");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
            


        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage loan_detail(string activationcode)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var id = contextclass.user_Details.Where(x => x.Activationcode == activationcode).Select(x => x.ID).FirstOrDefault();
                    var Details = contextclass.loans.Where(x => x.Account_ID == id & x.Status==true).ToList();
                    return Request.CreateResponse(HttpStatusCode.Accepted, Details);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,ex);
            }
        }
     }
}
