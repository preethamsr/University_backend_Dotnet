using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace University_project_backend.Controllers
{
    public class testingauthController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/testingauth/forall")]
        public IHttpActionResult Get()
        {
            return Ok("The time is "+DateTime.Now.ToString());
        }
        [Authorize]
        [HttpGet]
        [Route("api/testingauth/authenticate")]
        public IHttpActionResult getauthonticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("hello" + identity);
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        [Route("api/testingauth/authorized")]
        public IHttpActionResult getforadmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            return Ok("Hello" + identity.Name + "Role:" + string.Join(",", roles.ToList()));
        }
    }
}
