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
    public class FeatureController : ControllerBase
    {
        private IGenericRepo<ModuleController> _genericRepo;
        private IGenericRepo<TestController> _genericRepo2;
        private IGenericRepo<BrowserVMExec> _genericRepo3;
        private ITestControllerRepo _testControllerRepo;
        private IGenericRepo<ModuleController_Map> _moduleControllerMapRepo;
        private IGenericRepo<TestController_Map> _testControllerMapRepo;
        private IMapper _mapper;
        private ILogger<FeatureController> _logger;

        public FeatureController(IGenericRepo<ModuleController> genericRepo,
            IGenericRepo<ModuleController_Map> moduleControllerMapRepo,
            IGenericRepo<TestController> genericRepo2,
            IGenericRepo<TestController_Map> testControllerMapRepo,
            IGenericRepo<BrowserVMExec> genericRepo3,
            ITestControllerRepo testControllerRepo,
            IMapper mapper,
            ILogger<FeatureController> logger)
        {
            _genericRepo = genericRepo;
            _genericRepo2 = genericRepo2;
            _genericRepo3 = genericRepo3;
            _testControllerRepo = testControllerRepo;
            _moduleControllerMapRepo = moduleControllerMapRepo;
            _testControllerMapRepo = testControllerMapRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetAllModuleController/{userId}")]
        public IList<ModuleControllerVM> GetAllModuleController(int userId)
        {
            var moduleData = new List<ModuleControllerVM>();
            try
            {
                var mapData = GetAllModuleControllerMap();
                moduleData = _mapper.Map<List<ModuleControllerVM>>(_genericRepo.GetAll());

                foreach (var item in mapData)
                {
                    if (item.UserId == userId)
                    {
                        foreach (var value in moduleData.Where(m => m.ID == item.RefId))
                        {
                            value.ModuleID = item.ModuleID;
                            value.MachineSequenceID = item.MachineSequenceID;
                            value.Run = item.Run;
                            value.LockedByUser = item.LockedByUser;
                            value.UserId = item.UserId;
                            value.CreatedOn = item.CreatedOn;
                            value.UpdatedOn = item.UpdatedOn;
                            value.MachineID = item.MachineID;
                            value.ModuleSeqID = item.ModuleSeqID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return moduleData;
        }

        [EnableQuery]
        [HttpGet("GetAllModuleControllerMap")]
        public IList<ModuleController_MapVM> GetAllModuleControllerMap()
        {
            var result = new List<ModuleController_MapVM>();
            try
            {
                result = _mapper.Map<List<ModuleController_MapVM>>(_moduleControllerMapRepo.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }


            return result;
        }

        [HttpGet("GetModuleRecordsCount")]
        public int GetModuleRecordsCount()
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

        [HttpGet("GetAllModuleID")]
        public List<string> GetAllModuleID()
        {
            var result = new List<string>();
            try
            {
                result = _testControllerRepo.GetAllModuleID();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }


            return result;
        }

        [EnableQuery]
        [HttpGet("GetAllTestController/{userId}")]
        public IList<TestControllerVM> GetAllTestController(int userId)
        {

            var result = new List<TestControllerVM>();
            try
            {
                var mapData = GetAllTestControllerMap();
                result = _mapper.Map<List<TestControllerVM>>(_genericRepo2.GetAll());
                foreach (var item in mapData)
                {
                    if (item.UserId == userId)
                    {
                        foreach (var value in result.Where(m => m.ID == item.RefId))
                        {
                            value.TestCaseID = item.TestCaseID;
                            value.TestScriptDescription = item.TestScriptDescription;
                            value.Run = item.Run;
                            value.JiraID = item.JiraID;
                            value.SequenceID = item.SequenceID;
                            value.LockedByUser = item.LockedByUser;
                            value.UserId = item.UserId;
                            value.Browsers = item.Browsers;
                            value.CreatedOn = item.CreatedOn;
                            value.UpdatedOn = item.UpdatedOn;
                            value.FeatureID = item.FeatureID;
                            value.Iterations = item.Iterations;
                            value.StepsCount = item.StepsCount;
                            value.TestScriptName = item.TestScriptName;
                            value.TestType = item.TestType;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [EnableQuery]
        [HttpGet("GetAllTestControllerMap")]
        public IList<TestController_MapVM> GetAllTestControllerMap()
        {
            var result = new List<TestController_MapVM>();
            try
            {
                result = _mapper.Map<List<TestController_MapVM>>(_testControllerMapRepo.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }

        [HttpGet("GetTestControllerRecordsCount")]
        public int GetTestControllerRecordsCount()
        {
            var result = 0;
            try
            {
                result = _genericRepo2.GetRecordsCount();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }


        [EnableQuery]
        [HttpGet("GetAllBrowserController")]
        public IList<BrowserControllerVM> GetAllBrowserVMExec()
        {
            var result = new List<BrowserControllerVM>();
            try
            {
                result = _mapper.Map<List<BrowserControllerVM>>(_genericRepo3.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }

        [HttpGet("GetBrowserRecordsCount")]
        public int GetBrowserRecordsCount()
        {
            var result = 0;
            try
            {
                result = _genericRepo3.GetRecordsCount();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpGet("GetModuleControllerById/{id}/{userId}")]
        public ModuleControllerVM GetController1ById(int id, int userId)
        {
            var result = new ModuleControllerVM();
            try
            {
                var data = _genericRepo.GetById(id);
                result = _mapper.Map<ModuleControllerVM>(data);
                var mappedData = _mapper.Map<ModuleController_MapVM>(_testControllerRepo.GetMappedModuleData(userId, data, id));
                if (mappedData != null)
                    result = _mapper.Map<ModuleControllerVM>(mappedData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        //[HttpGet("GetModuleControllerMapById/{id}")]
        //public ModuleController_MapVM GetModuleControllerMapById(int id)
        //{
        //    return _mapper.Map<ModuleController_MapVM>(_moduleControllerMapRepo.GetById(id));
        //}

        [HttpGet("GetTestControllerById/{id}/{userId}")]
        public TestControllerVM GetController2ById(int id, int userId)
        {
            var result = new TestControllerVM();
            try
            {
                var data = _genericRepo2.GetById(id);
                result = _mapper.Map<TestControllerVM>(data);
                var mappedData = _mapper.Map<TestController_MapVM>(_testControllerRepo.GetMappedTestControllerData(userId, data, id));
                if (mappedData != null)
                    result = _mapper.Map<TestControllerVM>(mappedData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        //[HttpGet("GetTestControllerMapById/{id}")]
        //public TestController_MapVM GetTestControllerMapById(int id)
        //{
        //    return _mapper.Map<TestController_MapVM>(_testControllerMapRepo.GetById(id));
        //}

        [HttpGet("GetBrowserControllerById/{id}")]
        public BrowserControllerVM GetController3ById(int id)
        {
            var result = new BrowserControllerVM();
            try
            {
                result = _mapper.Map<BrowserControllerVM>(_genericRepo3.GetById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpPost("AddModuleController")]
        public void AddModuleController([FromBody]ModuleControllerVM controller1)
        {
            try
            {
                var data = _mapper.Map<ModuleController>(controller1);
                _testControllerRepo.CreateController1(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPost("AddTestController")]
        public void AddTestController([FromBody]TestControllerVM controller2)
        {
            try
            {
                var data = _mapper.Map<TestController>(controller2);
                _testControllerRepo.CreateController2(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPost("AddBrowserController")]
        public void AddBrowserController([FromBody]BrowserControllerVM controller3)
        {
            try
            {
                var data = _mapper.Map<BrowserVMExec>(controller3);
                _testControllerRepo.CreateController3(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateModuleController/{id}")]
        public void UpdateModuleController(int id, [FromBody]ModuleControllerVM testController1)
        {
            try
            {
                var data = _mapper.Map<ModuleController>(testController1);
                var mappedData = _testControllerRepo.GetMappedModuleData(testController1.UserId, data, id);

                if (mappedData != null)
                {
                    var mappedUpdate = _mapper.Map<ModuleController_MapVM>(testController1);
                    var mapData = _mapper.Map<ModuleController_Map>(mappedUpdate);
                    mapData.RefId = id;
                    _testControllerRepo.UpdateController1Map(testController1.UserId, mapData);
                }
                else
                {
                    _testControllerRepo.CreateController1Map(testController1.UserId, data, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateTestController/{id}")]
        public void UpdateTestController(int id, [FromBody]TestControllerVM testController2)
        {
            try
            {
                var data = _mapper.Map<TestController>(testController2);
                var mappedData = _testControllerRepo.GetMappedTestControllerData(testController2.UserId, data, id);

                if (mappedData != null)
                {
                    var mappedUpdate = _mapper.Map<TestController_MapVM>(testController2);
                    var mapData = _mapper.Map<TestController_Map>(mappedUpdate);
                    mapData.RefId = id;
                    _testControllerRepo.UpdateController2Map(testController2.UserId, mapData);
                }
                else
                {
                    _testControllerRepo.CreateController2Map(testController2.UserId, data, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateBrowserController/{id}")]
        public void UpdateBrowserController(int id, [FromBody]BrowserControllerVM testController3)
        {
            try
            {
                var data = _mapper.Map<BrowserVMExec>(testController3);
                _testControllerRepo.UpdateController3(id, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }


        [HttpDelete("DeleteModuleController/{id}/{userId}")]
        public void DeleteModuleController(int id, int userId)
        {
            try
            {
                var testController1 = _genericRepo.GetById(id);
                var mappedData = _testControllerRepo.GetMappedModuleData(userId, testController1, id);
                if (mappedData == null)
                {
                    _testControllerRepo.DeleteController1(id);
                }
                else
                {
                    _testControllerRepo.DeleteController1Map(userId, testController1.ModuleID, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpDelete("DeleteTestController/{id}/{userId}")]
        public void DeleteTestController(int id, int userId)
        {
            try
            {
                var testController2 = _genericRepo2.GetById(id);
                var mappedData = _testControllerRepo.GetMappedTestControllerData(userId, testController2, id);

                if (mappedData == null)
                {
                    _testControllerRepo.DeleteController2(id);
                }
                else
                {
                    _testControllerRepo.DeleteController2Map(userId, testController2.TestCaseID, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpDelete("DeleteBrowserController/{id}/{userId}")]
        public void DeleteBrowserController(int id, int userId)
        {
            try
            {
                _testControllerRepo.DeleteController3(id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }
    }
}
