using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreWebAPI.Models;
using ASPCoreWebAPI.Repository;
using ASPCoreWebAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ASPCoreWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IOptions<MySettingsModel> appSettings;

        private readonly IConfiguration configuration;

        public UserController(IOptions<MySettingsModel> app, IConfiguration configuration)
        {
            appSettings = app;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var data = DbClientFactory<UserDbClient>.Instance.GetAllUsers(appSettings.Value.connString);
                //var data1 = DbClientFactory<UserDbClient>.Instance.GetAllUsers(this.configuration["connectionString"]);
                return Ok(data);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromBody]UsersModel model)
        {
            try
            {
                var msg = new Message<UsersModel>();
                var data = DbClientFactory<UserDbClient>.Instance.SaveUser(model, appSettings.Value.connString);
                if (data == "C200")
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "User saved successfully";

                }
                else if (data == "C201")
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "Email Id already exists";
                }
                else if (data == "C202")
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "Mobile Number already exists";
                }
                return Ok(msg);
            }
            catch (Exception e)
            {

                throw;
            }
           
        }

        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody]UsersModel model)
        {
            try
            {
                var msg = new Message<UsersModel>();
                var data = DbClientFactory<UserDbClient>.Instance.UpdateUser(model, appSettings.Value.connString);
                if (data == "C200")
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "User saved successfully";

                }
                else if (data == "C201")
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "Invalid record";
                }
                return Ok(msg);
            }
            catch (Exception e)
            {

                throw;
            }
            
        }


        [HttpPost]
        [Route("DeleteUser")]
        public IActionResult DeleteUser([FromBody]UsersModel model)
        {
            try
            {
                var msg = new Message<UsersModel>();
                var data = DbClientFactory<UserDbClient>.Instance.DeleteUser(model.Id, appSettings.Value.connString);
                if (data == "C200")
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "User Deleted";
                }
                else if (data == "C203")
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "Invalid record";
                }
                return Ok(msg);
            }
            catch (Exception e)
            {

                throw;
            }
           
        }
    }
}
