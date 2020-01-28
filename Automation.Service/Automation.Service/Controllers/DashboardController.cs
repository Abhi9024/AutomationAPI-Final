using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Automation.Service.ViewModel;
using AutomationService.Core.DataAccessAbstractions;
using AutomationService.Core;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Automation.Service.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private IGenericRepo<UserTable> _genericRepo;
        private IDashboardRepo _dashboardRepo;
        private IMapper _mapper;
        private ILogger<DashboardController> _logger;

        public DashboardController(IGenericRepo<UserTable> genericRepo,
            IDashboardRepo dashboardRepo,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            _genericRepo = genericRepo;
            _dashboardRepo = dashboardRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/User/GetActiveUsers
        [HttpGet("GetActiveUsers")]
        public int GetActiveUsers()
        {
            int result = 0;
            try
            {
                result = _genericRepo.GetRecordsCount();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpGet("RecordsModifiedCount")]
        public int RecordsModifiedCount()
        {
            int result = 0;
            try
            {
                result = _dashboardRepo.RecordsModified();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }

        [HttpGet("GetLatestFeeds")]
        public string[] GetLatestFeeds()
        {
            var feeds = new List<string>();
            try
            {
                var users = _dashboardRepo.ModifiedFeeds();
                foreach (var user in users)
                {
                    feeds.Add($"{user.ToUpper()} recently Modified/Added an entry.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return feeds.ToArray();
        }

        [HttpGet("GetAllRoles")]
        public List<RoleVM> GetAllRoles()
        {
            var roles = new List<RoleVM>();
            try
            {
                roles = _mapper.Map<List<RoleVM>>(_dashboardRepo.GetAllRoles());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return roles;
        }
    }
}
