using MapperlyDemo.Entities;
using MapperlyDemo.Models;
using Riok.Mapperly.Abstractions;

namespace MapperlyDemo.Mapping;

[Mapper]
public static partial class Mapping
{
       
    [MapDerivedType(typeof(PatientEntity), typeof(GetPatientDossierDto))]
    [MapDerivedType<VisitEntity, GetPatientVisitDto>]
    private static partial BaseDto MapToBaseDto(this BaseUserEntity entity);
    private static Status MapToStatus(object source)
    {
        return ((BaseUserEntity)source).IsDeleted ? Status.Archived : Status.Active;
    }
    
    [MapProperty(nameof(PatientEntity.Number), nameof(GetPatientDossierDto.Identifier))]
    [MapPropertyFromSource(nameof(GetPatientDossierDto.VisitsSummary), Use = nameof(MapToVisitsSummary))]
    [MapPropertyFromSource(nameof(BaseDto.Status), Use = nameof(MapToStatus))]
    public static partial GetPatientDossierDto ToDossierDto(this PatientEntity entity);
    

    private static VisitsSummary MapToVisitsSummary(this PatientEntity entity)
        => new VisitsSummary
        {
            VisitsCount = entity.Visits.Count,
            HardDiseasesCount = entity.Visits
                .Count(v => v.VisitType == VisitTypeEnum.NewSessionHardDisease ||
                            v.VisitType == VisitTypeEnum.OldSessionHardDisease),
            RepeatableDiseasesCount = entity.Visits
                .Count(v => v.VisitType == VisitTypeEnum.NewSessionRepeatableDisease ||
                            v.VisitType == VisitTypeEnum.OldSessionRepeatableDisease)
        };
    
    public static partial IQueryable<GetPatientDossierDto> ProjectToDossierDto(this IQueryable<PatientEntity> entity);
    
    
    [MapPropertyFromSource(nameof(BaseDto.Status), Use = nameof(MapToStatus))]
    public static partial GetPatientVisitDto ToDto(this VisitEntity entity);

    public static partial IQueryable<GetPatientVisitDto> ProjectToDto(this IQueryable<VisitEntity> entity);

    
}




