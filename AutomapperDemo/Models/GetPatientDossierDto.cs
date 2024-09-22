namespace AutomapperDemo.Models;

public class GetPatientDossierDto: BaseDto
{
    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly BirthDate { get; set; }
    public int Identifier { get; set; }
    public string? BirthAddress { get; set; }

    public string? Address { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? Phone3 { get; set; }

    public string? Email { get; set; }

    public string? Skype { get; set; }

    public string? Comment { get; set; }

    public string? Diagnoses { get; set; }
    public VisitsSummary VisitsSummary { get; set; } = null!;
}

public class VisitsSummary
{
    public int VisitsCount { get; set; }
    public int HardDiseasesCount { get; set; }
    public int RepeatableDiseasesCount { get; set; }
}