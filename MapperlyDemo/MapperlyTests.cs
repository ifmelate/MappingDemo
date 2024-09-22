using FluentAssertions;
using MapperlyDemo.Mapping;
using MapperlyDemo.Models;
using MapperlyDemo.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MapperlyDemo;

public class MapperlyTests
{
    private readonly ServiceProvider _serviceProvider;

    public MapperlyTests()
    {
        var services = new ServiceCollection();
        services.AddTransient<TestDbContext>(c=> new TestDbContext());
        _serviceProvider = services.BuildServiceProvider();
    }
    [Fact]
    public async Task MappingUsingStaticExtension()
    {
        await _serviceProvider.GetRequiredService<TestDbContext>().RecreateDatabaseAsync();
        //arrange
        var context = _serviceProvider.GetRequiredService<TestDbContext>();
        var patientEntity = await context.Patients.Include(s=> s.Visits).FirstAsync();
        
        //act
        var patientDto = patientEntity.ToDossierDto();
        
        
        //assert
        patientDto.FullName.Should().Be(patientEntity.FullName);
        patientDto.Id.Should().Be(patientEntity.Id);
        patientDto.Address.Should().Be(patientEntity.Address);
        patientDto.BirthAddress.Should().Be(patientEntity.BirthAddress);
        patientDto.BirthDate.Should().Be(DateOnly.FromDateTime(patientEntity.BirthDate));
        patientDto.Gender.Should().Be(patientEntity.Gender);
        patientDto.Comment.Should().Be(patientEntity.Comment);
        patientDto.Diagnoses.Should().BeEquivalentTo(patientEntity.Diagnoses);
        patientDto.Email.Should().Be(patientEntity.Email);
        patientDto.Phone1.Should().Be(patientEntity.Phone1);
        patientDto.Phone2.Should().Be(patientEntity.Phone2);
        patientDto.Phone3.Should().Be(patientEntity.Phone3);
        patientDto.Skype.Should().Be(patientEntity.Skype);
        patientDto.VisitsSummary.Should().NotBeNull();
        patientDto.VisitsSummary.VisitsCount.Should().Be(3);
        patientDto.VisitsSummary.HardDiseasesCount.Should().Be(1);
        patientDto.VisitsSummary.RepeatableDiseasesCount.Should().Be(2);
        patientDto.Status.Should().Be(patientEntity.IsDeleted ? Status.Archived : Status.Active);
    }
    
    [Fact]
    public async Task MappingUsingStaticExtension_WithProjection()
    {
        await _serviceProvider.GetRequiredService<TestDbContext>().RecreateDatabaseAsync();
        //arrange
        var context = _serviceProvider.GetRequiredService<TestDbContext>();
        var patientsQueryable = context.Patients.Include(s => s.Visits);
        var patients = await patientsQueryable.ToListAsync();
        //act
        
        var patientDtos = await patientsQueryable.ProjectToDossierDto().ToListAsync();
        
        //assert
        foreach (var patientDto in patientDtos)
        {
            var patientEntity = patients.First(s => s.Id == patientDto.Id);
            patientDto.FullName.Should().Be(patientEntity.FullName);
            patientDto.Id.Should().Be(patientEntity.Id);
            patientDto.Address.Should().Be(patientEntity.Address);
            patientDto.BirthAddress.Should().Be(patientEntity.BirthAddress);
            patientDto.BirthDate.Should().Be(DateOnly.FromDateTime(patientEntity.BirthDate));
            patientDto.Gender.Should().Be(patientEntity.Gender);
            patientDto.Comment.Should().Be(patientEntity.Comment);
            patientDto.Diagnoses.Should().BeEquivalentTo(patientEntity.Diagnoses);
            patientDto.Email.Should().Be(patientEntity.Email);
            patientDto.Phone1.Should().Be(patientEntity.Phone1);
            patientDto.Phone2.Should().Be(patientEntity.Phone2);
            patientDto.Phone3.Should().Be(patientEntity.Phone3);
            patientDto.Skype.Should().Be(patientEntity.Skype);
            patientDto.VisitsSummary.Should().NotBeNull();
            patientDto.VisitsSummary.VisitsCount.Should().Be(3);
            patientDto.VisitsSummary.HardDiseasesCount.Should().Be(1);
            patientDto.VisitsSummary.RepeatableDiseasesCount.Should().Be(2);
            patientDto.Status.Should().Be(patientEntity.IsDeleted ? Status.Archived : Status.Active);
        }
        
    }
    
    [Fact]
    public async Task MappingUsingStaticExtension_CombinedWithManualMapping_WithProjection()
    {
        await _serviceProvider.GetRequiredService<TestDbContext>().RecreateDatabaseAsync();
        //arrange
        var context = _serviceProvider.GetRequiredService<TestDbContext>();
        var visitQueryable = context.Visits.Include(s => s.Patient);
        var visits = await visitQueryable.ToListAsync();
        //act
        
        var visitDtos = await visitQueryable.ProjectToDto().ToListAsync();
        var testVisitDto = visits[0].ToDto();
        //assert
        foreach (var visitDto in visitDtos)
        {
            var visitEntity = visits.First(s => s.Id == visitDto.Id);
            visitDto.Id.Should().Be(visitEntity.Id);
            visitDto.PatientId.Should().Be(visitEntity.PatientId);
            visitDto.CreatedDate.Should().Be(visitEntity.CreatedDate);
            visitDto.VisitType.Should().Be(visitEntity.VisitType.ToString());
            visitDto.IsCompleted.Should().Be(visitEntity.IsCompleted);
            visitDto.PatientDiagnoses.Should().Be(visitEntity.Patient.Diagnoses);
            visitDto.PatientFullName.Should().Be(visitEntity.Patient.FullName);
            visitDto.Status.Should().Be(visitEntity.IsDeleted ? Status.Archived : Status.Active);
        }
        
    }
}