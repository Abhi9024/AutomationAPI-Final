using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNet.OData;
using Automation.Service.ViewModel;
using AutomationService.Core.DataAccessAbstractions;
using AutomationService.Core;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Automation.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDataController : Controller
    {
        private IGenericRepo<TestData> _genericRepo;
        private ITestDataRepo _testDataRepo;
        private IMapper _mapper;
        private ILogger<TestDataController> _logger;

        public TestDataController(IGenericRepo<TestData> genericRepo,
            ITestDataRepo testDataRepo, 
            IMapper mapper,
            ILogger<TestDataController> logger)
        {
            _genericRepo = genericRepo;
            _testDataRepo = testDataRepo;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/values
        [EnableQuery]
        [HttpGet("GetTestAllData")]
        public ActionResult<IList<TestDataVM>> Get()
        {
            var result = new List<TestDataVM>();
            try
            {
                result = _mapper.Map<List<TestDataVM>>(_genericRepo.GetAll().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        // GET api/values/5
        [HttpGet("GetTestData/{id}/{userId}")]
        public TestDataVM Get(int id, int userId)
        {
            var result = new TestDataVM();
            try
            {
                var data = _genericRepo.GetById(id);
                if (data != null)
                {
                    data.IsLocked = true;
                    data.LockedByUser = userId;
                    data.UserId = userId;
                    _testDataRepo.UpdateLockedByFlags(data);
                }
                result = _mapper.Map<TestDataVM>(data);
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
                    _testDataRepo.UpdateLockedByFlags(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        // POST api/values
        [HttpPost("AddTestData")]
        public void Post([FromBody]TestDataVM testData)
        {
            try
            {
                var data = _mapper.Map<TestData>(testData);
                _testDataRepo.CreateTestData(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        // PUT api/values/5
        [HttpPut("UpdateTestData/{id}")]
        public void Put(int id, [FromBody]TestDataVM testData)
        {
            try
            {
                var data = _mapper.Map<TestData>(testData);
                _testDataRepo.UpdateTestData(id, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        // DELETE api/values/5
        [HttpDelete("DeleteTestData/{id}/{userId}")]
        public void Delete(int id, int userId)
        {
            try
            {
                _testDataRepo.DeleteTestData(id, userId);
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
