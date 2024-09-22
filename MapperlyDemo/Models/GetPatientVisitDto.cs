
namespace MapperlyDemo.Models;

public class GetPatientVisitDto: BaseDto
{
    public DateTime CreatedDate { get; set; }

    public bool IsCompleted { get; set; }

    public Guid PatientId { get; set; }
        
    public string VisitType { get; set; } = null!;

    public string PatientFullName { get; set; } = null!;
    public string? PatientDiagnoses { get; set; }
 
}