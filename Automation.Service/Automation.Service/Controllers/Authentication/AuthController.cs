using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Automation.Service.ViewModel;
using AutoMapper;
using AutomationService.Core.DataAccessAbstractions;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Automation.Service.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthProvider _authProvider;
        private IMapper _mapper;

        public AuthController(IAuthProvider authProvider,IMapper mapper)
        {
            _authProvider = authProvider;
            _mapper = mapper;
        }
        
        // POST api/values
        [HttpPost("Login")]
        public UserVM Login([FromBody]UserVM user)
        {
            var userData = _authProvider.ValidateLogin(user.UserName,user.Password);
            return  _mapper.Map<UserVM>(userData);
        }


        // POST api/values
        [HttpPost("CreateUser")]
        public string CreateUser([FromBody]UserVM user)
        {
            _authProvider.CreateUser(user.UserName, user.Password,user.RoleId);
            return @"User Created Succesfully!";
        }
        
    }
}
