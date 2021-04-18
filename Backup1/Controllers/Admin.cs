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
using System.Web.Http.Cors;

namespace University_project_backend.Controllers
{
    [DisableCors]
    public class AdminController : ApiController
        
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize(Roles = "Admin")]

        public HttpResponseMessage Account_list()
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var account_list=contextclass.user_Details.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, account_list);
                }

            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage search(string search_criteria)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var result = contextclass.user_Details.Where(x => x.Account_number.StartsWith(search_criteria) || x.Name.StartsWith(search_criteria)
                      || x.Email_address.StartsWith(search_criteria)).ToList();
                    return Request.CreateResponse(HttpStatusCode.Accepted, result);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage userdetails(int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var data = contextclass.user_Details.Where(x => x.ID == id).ToList();
                    return Request.CreateResponse(HttpStatusCode.Accepted, data);
                }

            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage transaction( int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                { var transaction = contextclass.moneytransfers.Where(x => x.Account_id == id).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, transaction);
                }
                

            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage loandetails(int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var result = contextclass.loans.Where(x => x.Account_ID == id).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Account_delete(int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var position = contextclass.user_Details.Where(x => x.ID == id).FirstOrDefault();
                    contextclass.Entry(position).State = EntityState.Deleted;
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex);
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage loan()
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var loan_list = contextclass.loans.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, loan_list);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [System.Web.Http.HttpGet]
        public HttpResponseMessage loan_approve(int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var temp = contextclass.loans.Where(x => x.LoanID == id).FirstOrDefault();
                    temp.Date_of_approval = DateTime.Now.Date;
                    temp.Status = true;
                    contextclass.SaveChanges();
                    var user_id = contextclass.loans.Where(x => x.LoanID == id).Select(x => x.Account_ID).FirstOrDefault();
                    var user_update = contextclass.user_Details.Where(x => x.ID == user_id).FirstOrDefault();
                    var new_balance = user_update.Balance + temp.Loan_amount;
                    user_update.Balance = new_balance;
                    user_update.Loan = "YES";
                    user_update.Loan_amount = temp.Loan_amount;
                    user_update.Loan_status = "APPROVED";
                    contextclass.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Modeifyuser([FromBody] Modifyuserdata modifyuserdata,int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var position = contextclass.user_Details.Where(x => x.ID == id).FirstOrDefault();
                    if(modifyuserdata.Email_address!=null)
                    {
                        position.Email_address = modifyuserdata.Email_address;
                    }
                    if(modifyuserdata.Name!=null)
                    {
                        position.Name = modifyuserdata.Name;
                    }
                    if(modifyuserdata.Balance!=0)
                    {
                        position.Balance =position.Balance+ modifyuserdata.Balance;
                    }
                    if(modifyuserdata.DOB!=DateTime.MinValue)
                    {
                        position.DOB = modifyuserdata.DOB;
                    }
                    if(modifyuserdata.Password!=null)
                    {
                        position.Password = modifyuserdata.Password;
                    }
                    if(modifyuserdata.Account_number!=null)
                    {
                        position.Account_number = modifyuserdata.Account_number;
                    }
                    contextclass.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

            }
            catch(Exception ex)
             {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.HttpDelete]
        public HttpResponseMessage UserDelete(int id)
        {
            try
            {
                using(Contextclass contextclass=new Contextclass())
                {
                    var transferlist = contextclass.moneytransfers.Where(x => x.Account_id == id).Select(x => x.Account_id).ToList();
                    if(transferlist.Count!=0)
                    {
                        foreach(var item in transferlist)
                        {
                            var delete = contextclass.moneytransfers.Where(x => x.Account_id == item).FirstOrDefault();
                            contextclass.moneytransfers.Remove(delete);
                            contextclass.SaveChanges();
                            
                        }
                       
                    }
                    var loan_list = contextclass.loans.Where(x => x.Account_ID == id).Select(x => x.Account_ID).ToList();
                    if(loan_list.Count!=0)
                    {
                        foreach( var item in loan_list)
                        {
                            var delete = contextclass.loans.Where(x => x.Account_ID == item).FirstOrDefault();
                            contextclass.loans.Remove(delete);
                            contextclass.SaveChanges();
                        }
                    }
                    var user_details_list = contextclass.user_Details.Where(x => x.ID == id).Select(x => x.ID).ToList();
                    if(user_details_list.Count!=0)
                    {
                        foreach(var item in user_details_list)
                        {
                            var delete = contextclass.user_Details.Where(x => x.ID == item).FirstOrDefault();
                            contextclass.user_Details.Remove(delete);
                            contextclass.SaveChanges();
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        

    }
}
