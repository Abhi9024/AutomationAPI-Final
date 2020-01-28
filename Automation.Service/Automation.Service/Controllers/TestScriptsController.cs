using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
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
    public class TestScriptsController : ControllerBase
    {
        private IGenericRepo<TestScripts> _genericRepo;
        private IGenericRepo<TestScripts_Map> _genericRepo2;
        private ITestScriptsRepo _testScriptsRepo;
        private IMapper _mapper;
        private ILogger<TestScriptsController> _logger;

        public TestScriptsController(IGenericRepo<TestScripts> genericRepo,
            IGenericRepo<TestScripts_Map> genericRepo2,
            ITestScriptsRepo testScriptsRepo, 
            IMapper mapper,
            ILogger<TestScriptsController> logger)
        {
            _genericRepo = genericRepo;
            _genericRepo2 = genericRepo2;
            _testScriptsRepo = testScriptsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetScripts/{userId}")]
        public ActionResult<IList<TestScriptVM>> GetScripts(int userId)
        {
            var result = new List<TestScriptVM>();
            try
            {
                result = new List<TestScriptVM>();
                var testScriptMapData = GetAllTestScriptsMap();
                var mappedIds = testScriptMapData.Where(k => k.UserId == userId).Select(k => k.MasterTestScriptID).ToList();
                var filteredList = _mapper.Map<List<TestScriptVM>>(_testScriptsRepo.GetFilteredTestScripts(mappedIds));

                var testScriptsMappedList = _mapper.Map<List<TestScriptVM>>(testScriptMapData);
                result.AddRange(testScriptsMappedList);
                result.AddRange(filteredList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }

        [EnableQuery]
        [HttpGet("GetAllKeywordsMap")]
        public IList<TestScript_MapVM> GetAllTestScriptsMap()
        {
            var result = new List<TestScript_MapVM>();
            try
            {
                result = _mapper.Map<List<TestScript_MapVM>>(_genericRepo2.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpGet("GetScript/{id}/{userId}")]
        public TestScriptVM Get(int id, int userId)
        {
            var result = new TestScriptVM();
            try
            {
                result = _mapper.Map<TestScriptVM>(_genericRepo.GetById(id));
                var mappedData = _mapper.Map<TestScript_MapVM>(_testScriptsRepo.GetMappedTestScript(id, userId));
                if (mappedData != null)
                    result = _mapper.Map<TestScriptVM>(mappedData);
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
                    _testScriptsRepo.UpdateLockedByFlags(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            
        }

        [HttpPost("AddScript")]
        public void Post([FromBody]TestScriptVM scripts)
        {
            try
            {
                var data = _mapper.Map<TestScripts>(scripts);
                _testScriptsRepo.CreateScript(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateScriptAdmin/{id}")]
        public void UpdateScriptAdmin(int id, [FromBody]TestScriptVM scripts)
        {
            try
            {
                var data = _mapper.Map<TestScripts>(scripts);
                _testScriptsRepo.UpdateScript(id, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateScript/{id}")]
        public void Put(int id, [FromBody]TestScriptVM scripts)
        {
            try
            {
                var mapVM = _mapper.Map<TestScript_MapVM>(scripts);

                var testScriptMapEntity = _mapper.Map<TestScripts_Map>(mapVM);
                testScriptMapEntity.MasterTestScriptID = id;

                var mappedData = _testScriptsRepo.GetMappedTestScript(id, scripts.UserId);

                if (mappedData != null)
                {
                    _testScriptsRepo.UpdateScriptMap(scripts.UserId, testScriptMapEntity);
                }
                else
                {
                    _testScriptsRepo.CreateScriptMap(testScriptMapEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpDelete("DeleteScript/{id}/{userId}")]
        public void Delete(int id, int userId)
        {
            try
            {
                var mappedData = _testScriptsRepo.GetMappedTestScript(id, userId);
                if (mappedData == null)
                {
                    _testScriptsRepo.DeleteScript(id, userId);
                }
                else
                {
                    _testScriptsRepo.DeleteScriptMap(userId, id);
                }
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
    }
}
