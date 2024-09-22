using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutomapperDemo.Entities;

public class VisitEntity: BaseUserEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }

    public bool IsCompleted { get; set; }

    public Guid PatientId { get; set; }
    public virtual PatientEntity Patient { get; set; } = null!;

    public VisitTypeEnum VisitType { get; set; }

}

public enum VisitTypeEnum
{
    NotVisit = 0,
    NewSessionHardDisease = 1,
    NewSessionRepeatableDisease = 2,
    OldSessionHardDisease = 3,
    OldSessionRepeatableDisease = 4
}







public class VisitEntityConfiguration : IEntityTypeConfiguration<VisitEntity>
{
      
    public void Configure(EntityTypeBuilder<VisitEntity> builder)
    {
        builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();
    

        builder.Property(p => p.Id)
            .HasColumnName("VisitId");

        builder.HasOne(d => d.Patient).WithMany(p => p.Visits).HasForeignKey(s => s.PatientId);

        builder.ToTable("Visits");
    }
}  