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
    public class KeywordsController : ControllerBase
    {
        private IGenericRepo<KeywordLibrary> _genericRepo;
        private IGenericRepo<KeywordLibrary_Map> _genericRepo2;
        private IKeywordEntityRepo _keywordRepo;
        private IMapper _mapper;
        private ILogger<KeywordsController> _logger;

        public KeywordsController(IGenericRepo<KeywordLibrary> genericRepo,
            IGenericRepo<KeywordLibrary_Map> genericRepo2,
            IKeywordEntityRepo keywordRepo,
            IMapper mapper,
            ILogger<KeywordsController> logger)
        {
            _genericRepo = genericRepo;
            _genericRepo2 = genericRepo2;
            _keywordRepo = keywordRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetAllKeywords/{userId}")]
        public IList<KeywordEntityVM> GetAllKeywords(int userId)
        {
            var result = new List<KeywordEntityVM>();
            try
            {
                var keywordsMapData = GetAllKeywordsMap();
                var mappedIds = keywordsMapData.Where(k => k.UserId == userId).Select(k => k.MasterKeywordID).ToList();
                result = _mapper.Map<List<KeywordEntityVM>>(_keywordRepo.GetFilteredKeywords(mappedIds));
                var keywordsMappedList = _mapper.Map<List<KeywordEntityVM>>(keywordsMapData);
                result.AddRange(keywordsMappedList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }

            return result;
        }

        [EnableQuery]
        [HttpGet("GetAllKeywordsMap")]
        public IList<KeywordEntity_MapVM> GetAllKeywordsMap()
        {
            var result = new List<KeywordEntity_MapVM>();
            try
            {
                result = _mapper.Map<List<KeywordEntity_MapVM>>(_genericRepo2.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }

        [HttpGet("GetKeywordById/{id}/{userId}")]
        public KeywordEntityVM GetKeywordById(int id, int userId)
        {
            var result = new KeywordEntityVM();
            try
            {
                result = _mapper.Map<KeywordEntityVM>(_genericRepo.GetById(id));
                var mappedData = _mapper.Map<KeywordEntity_MapVM>(_keywordRepo.GetMappedKeywordLibrary(id, userId));
                if (mappedData != null)
                    result = _mapper.Map<KeywordEntityVM>(mappedData);
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
                    _keywordRepo.UpdateLockedByFlags(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPost("AddKeyword")]
        public void AddKeyword([FromBody]KeywordEntityVM keyword)
        {
            try
            {
                var data = _mapper.Map<KeywordLibrary>(keyword);
                _keywordRepo.CreateKeyword(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpPut("UpdateKeywordAdmin/{id}")]
        public void UpdateKeywordAdmin(int id, [FromBody]KeywordEntityVM keyword)
        {
            try
            {
                var data = _mapper.Map<KeywordLibrary>(keyword);
                _keywordRepo.UpdateKeyword(id, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }


        [HttpPut("UpdateKeyword/{id}")]
        public void UpdateKeyword(int id, [FromBody]KeywordEntityVM keyword)
        {
            try
            {
                var mapVM = _mapper.Map<KeywordEntity_MapVM>(keyword);
                var keywordMapEntity = _mapper.Map<KeywordLibrary_Map>(mapVM);
                keywordMapEntity.MasterKeywordID = id;

                var mappedData = _keywordRepo.GetMappedKeywordLibrary(id, keyword.UserId);

                if (mappedData != null)
                {
                    _keywordRepo.UpdateKeywordMap(keyword.UserId, keywordMapEntity);
                }
                else
                {
                    _keywordRepo.CreateKeyword_Map(keywordMapEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
        }

        [HttpDelete("DeleteKeyword/{id}/{userId}")]
        public void DeleteKeyword(int id, int userId)
        {
            try
            {
                var mappedData = _keywordRepo.GetMappedKeywordLibrary(id, userId);
                if (mappedData == null)
                {
                    _keywordRepo.DeleteKeyword(id, userId);
                }
                else
                {
                    _keywordRepo.DeleteKeywordMap(userId, id);
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

        [HttpGet("GetAllFunctionNames")]
        public List<string> GetAllFunctionNames()
        {
            var result = new List<string>();
            try
            {
                result = _keywordRepo.GetAllFunctionNames();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Source: {ex.Source}, StackTrace: {ex.StackTrace} ,  Message: {ex.Message}");
            }
            return result;
        }
    }
}
