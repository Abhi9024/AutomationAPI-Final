using AutoMapper;
using Automation.Service.ViewModel;
using AutomationService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automation.Service.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<BaseEntity, BaseEntityVM>().ReverseMap();

            CreateMap<TestScripts, TestScriptVM>().ReverseMap();
            CreateMap<TestScripts_Map, TestScript_MapVM>().ReverseMap();
            CreateMap<TestScript_MapVM, TestScriptVM>()
                .ForMember(dest=>dest.ID,opt=>opt.MapFrom(src=>src.MasterTestScriptID))
                .ReverseMap();

            CreateMap<ModuleController, ModuleControllerVM>().ReverseMap();
            CreateMap<ModuleController_Map, ModuleController_MapVM>().ReverseMap();
            CreateMap<ModuleController_MapVM, ModuleControllerVM>().ReverseMap();

            CreateMap<TestController, TestControllerVM>().ReverseMap();
            CreateMap<TestController_Map, TestController_MapVM>().ReverseMap();
            CreateMap<TestController_MapVM, TestControllerVM>().ReverseMap();

            CreateMap<BrowserVMExec, BrowserControllerVM>()
                .ForMember(dest => dest.Exec, opt => opt.MapFrom(src => src.Run))
                .ReverseMap();

            CreateMap<KeywordLibrary, KeywordEntityVM>().ReverseMap();
            CreateMap<KeywordLibrary_Map, KeywordEntity_MapVM>().ReverseMap();
            CreateMap<KeywordEntity_MapVM, KeywordEntityVM>()
                .ForMember(dest=>dest.ID,opt=>opt.MapFrom(src=>src.MasterKeywordID))
                .ReverseMap();

            CreateMap<Repository, RepositoryEntityVM>().ReverseMap();

            CreateMap<TestData, TestDataVM>().ReverseMap();

            CreateMap<UserTable, UserVM>().ReverseMap();
            CreateMap<UserRole, RoleVM>().ReverseMap();
        }
    }
}
