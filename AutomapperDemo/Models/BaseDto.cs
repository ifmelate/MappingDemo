namespace AutomapperDemo.Models;

public class BaseDto
{
   public Guid Id { get; set; }
   public Guid UserId { get; set; }
   
   public Status Status { get; set; }
}

public enum Status
{
   Active,
   Archived
}