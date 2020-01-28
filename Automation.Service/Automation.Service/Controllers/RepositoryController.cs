using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Automation.Service.ViewModel;
using Microsoft.AspNet.OData;
using AutomationService.Core.DataAccessAbstractions;
using AutomationService.Core;
using Microsoft.Extensions.Logging;

namespace Automation.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : Controller
    {
        private IGenericRepo<Repository> _genericRepo;
        private IRepositoryEntityRepo _repositoryEntityRepo;
        private IMapper _mapper;
        private ILogger<RepositoryController> _logger;

        public RepositoryController(IGenericRepo<Repository> genericRepo, 
            IRepositoryEntityRepo repositoryEntityRepo,
            IMapper mapper,
            ILogger<RepositoryController> logger)
        {
            _genericRepo = genericRepo;
            _repositoryEntityRepo = repositoryEntityRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetAllRepository")]
        public IList<RepositoryEntityVM> GetAllRepository()
        {
            var result = new List<RepositoryEntityVM>();
            try
            {
                result = _mapper.Map<List<RepositoryEntityVM>>(_genericRepo.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpGet("GetRepositoryById/{id}/{userId}")]
        public RepositoryEntityVM GetRepositoryById(int id, int userId)
        {
            var result = new RepositoryEntityVM();
            try
            {
                var data = _genericRepo.GetById(id);
                if (data != null)
                {
                    data.IsLocked = true;
                    data.LockedByUser = userId;
                    data.UserId = userId;
                    _repositoryEntityRepo.UpdateLockedByFlags(data);
                }
               result = _mapper.Map<RepositoryEntityVM>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpPut("ResetLockedByField/{id}/{userId}")]
        public void ResetLockedByField(int id, int userId)
        {
            try
            {
                var data = _genericRepo.GetById(id);
                if (data != null)
                {
                    data.IsLocked = null;
                    data.LockedByUser = null;
                    data.UserId = null;
                    _repositoryEntityRepo.UpdateLockedByFlags(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPost("AddRepository")]
        public void AddRepository([FromBody]RepositoryEntityVM repository)
        {
            try
            {
                var data = _mapper.Map<Repository>(repository);
                _repositoryEntityRepo.CreateRepository(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateRepository/{id}")]
        public void UpdateRepository(int id, [FromBody]RepositoryEntityVM repository)
        {
            try
            {
                var data = _mapper.Map<Repository>(repository);
                _repositoryEntityRepo.UpdateRepository(id, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpDelete("DeleteRepository/{id}/{userId}")]
        public void DeleteRepository(int id, int userId)
        {
            try
            {
                _repositoryEntityRepo.DeleteRepository(id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpGet("GetRecordsCount")]
        public int GetRecordsCount()
        {
            var result = 0;
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

        [HttpGet("GetAllLogicalNames")]
        public List<string> GetAllLogicalNames()
        {
            var result = new List<string>();
            try
            {
                result = _repositoryEntityRepo.GetAllLogicalNames();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }
    }
}
