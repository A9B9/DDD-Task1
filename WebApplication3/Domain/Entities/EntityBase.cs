namespace WebApplication3.Domain.Entities;

public class EntityBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; private set; }
    public  DateTime UpdatedAt { get; private set; }
}