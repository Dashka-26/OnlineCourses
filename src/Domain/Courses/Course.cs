namespace Domain.Courses;

public class Course
{
    // Тепер Id має тип CourseId
    public CourseId Id { get; }
    public DateTime CreatedAt { get; }
    
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public Level Level { get; private set; }

    private Course(CourseId id, string title, decimal price, bool isActive, Level level, DateTime createdAt)
        => (Id, Title, Price, IsActive, Level, CreatedAt) = (id, title, price, isActive, level, createdAt);

    public static Course New(CourseId id, string title, decimal price, bool isActive, Level level)
        => new(id, title, price, isActive, level, DateTime.UtcNow);

    public void UpdateDetails(string title, decimal price, bool isActive, Level level)
        => (Title, Price, IsActive, Level) = (title, price, isActive, level);
}