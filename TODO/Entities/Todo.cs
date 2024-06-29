using System.ComponentModel.DataAnnotations;

namespace TODO.Entities;

public class Todo
{   
    public int Id { get; init;}
    public string Title { get; private set;}
    public string Description { get; private set;}
    public bool IsCompleted { get; private set;}
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; private set; }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Todo()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public Todo(string title, string description)
    {
        if (string.IsNullOrEmpty(title))
            throw new ValidationException("Title is required");
        if (string.IsNullOrEmpty(description))
            throw new ValidationException("Description is required");
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
        if (string.IsNullOrEmpty(title))
            throw new ValidationException("Title is required");
        if (string.IsNullOrEmpty(description))  
            throw new ValidationException("Description is required");
        Title = title;
        Description = description;
        UpdatedAt = DateTime.Now;
    }
}