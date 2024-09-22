namespace MapperlyDemo.Entities;

public abstract class BaseUserEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsDeleted { get; set; }
}