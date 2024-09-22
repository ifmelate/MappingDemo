using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MapperlyDemo.Entities;

public class PatientEntity: BaseUserEntity
{
    public bool IsActive { get; set; }

    public string FullName { get; set; } = null!;

 
    public string? Gender { get; set; }


    public DateTime BirthDate { get; set; }

   
    public string? BirthAddress { get; set; }

    public string? Address { get; set; }

    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Phone3 { get; set; }

    public string? Email { get; set; }
    public string? Skype { get; set; }

    public int Number { get; set; }


    public string? Comment { get; set; }


    public DateTime RegistrationDate { get; set; }

    public string? Diagnoses { get; set; }

    public virtual List<VisitEntity> Visits { get; set; } = null!;
}









public class PatientEntityConfiguration : IEntityTypeConfiguration<PatientEntity>
{
    public void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();
  
           

        builder.Property(p => p.Id)
            .HasColumnName("PatientId");

        builder.Property(p => p.FullName)
            .HasMaxLength(100);

        builder.Property(p => p.Gender)
            .HasMaxLength(1)
            .IsFixedLength();
        builder.Property(p => p.BirthAddress)
            .HasMaxLength(100);

        builder.Property(p => p.BirthDate);

  
        RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder)builder, "Patients");



    }

}