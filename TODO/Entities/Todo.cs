namespace TODO.Entities;

public class Todo
{   
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Todo()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public Todo(string title, string description)
    {
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public void MarkAsCompleted()
    {
        IsCompleted = true;
        UpdatedAt = DateTime.Now;
    }
    
    public void MarkAsUncompleted()
    {
        IsCompleted = false;
        UpdatedAt = DateTime.Now;
    }
    
    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTime.Now;
    }
}