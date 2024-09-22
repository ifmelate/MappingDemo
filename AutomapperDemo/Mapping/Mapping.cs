using AutoMapper;
using AutomapperDemo.Entities;
using AutomapperDemo.Models;

namespace AutomapperDemo.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PatientEntity, GetPatientDossierDto>()
            .ForMember(s=> s.BirthDate, options
                => options.MapFrom(x => DateOnly.FromDateTime(x.BirthDate))
                )
            .ForMember(s=> s.Identifier, options
                => options.MapFrom(x => x.Number))
            
            .ForMember(s=> s.VisitsSummary, options
                => options.MapFrom(x => new VisitsSummary
                {
                    VisitsCount = x.Visits.Count,
                    HardDiseasesCount = x.Visits.Count(f=> f.VisitType == VisitTypeEnum.OldSessionHardDisease 
                      || f.VisitType == VisitTypeEnum.NewSessionHardDisease),  
                    RepeatableDiseasesCount = x.Visits.Count(f=> f.VisitType == VisitTypeEnum.OldSessionRepeatableDisease 
                      || f.VisitType == VisitTypeEnum.NewSessionRepeatableDisease)
                }))
            ;

        
        
        
        
        
        CreateMap<VisitEntity, GetPatientVisitDto>()
            .ForMember(dest => dest.VisitType,
                options =>
                    options.MapFrom(x => x.VisitType.ToString()));
        
        
        CreateMap<BaseUserEntity, BaseDto>()
            .ForMember(x => x.Status, m
                => m.MapFrom(s => s.IsDeleted ? Status.Archived : Status.Active))
            .IncludeAllDerived()
           ;
    }
}