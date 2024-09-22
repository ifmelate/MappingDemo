using AutoFixture;
using MapperlyDemo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MapperlyDemo.Stubs;

public class TestDbContext: DbContext
{
    
    public DbSet<PatientEntity> Patients { get; set; }

    public DbSet<VisitEntity> Visits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TestInMemoryDb");
        optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
        optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PatientEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VisitEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    public async Task RecreateDatabaseAsync()
    {
        await Database.EnsureDeletedAsync();
        await Database.EnsureCreatedAsync();
        await FillWithData();
    }

    private async Task FillWithData()
    {
        var patient = new Fixture().Build<PatientEntity>()
            .Without(s=> s.Visits)
            .Create(); 
        await Patients.AddAsync(patient);
        var visit1 = new Fixture().Build<VisitEntity>()
            .Without(s=> s.Patient)
            .With(s=> s.VisitType, VisitTypeEnum.NewSessionHardDisease).Create();
        visit1.Patient = patient;
        await Visits.AddAsync(visit1);
        var visit2 = new Fixture().Build<VisitEntity>()
            .Without(s=> s.Patient)
            .With(s=> s.VisitType, VisitTypeEnum.NewSessionRepeatableDisease)
            .With(s=> s.IsDeleted, false)
            .Create();
        visit2.Patient = patient;
        await Visits.AddAsync(visit2);
        
        var visit3 = new Fixture().Build<VisitEntity>()
            .Without(s=> s.Patient)
            .With(s=> s.VisitType, VisitTypeEnum.OldSessionRepeatableDisease)
            .With(s=> s.IsDeleted, true)
            .Create();
        visit3.Patient = patient;
        await Visits.AddAsync(visit3);
        
        await SaveChangesAsync();
    }
}

