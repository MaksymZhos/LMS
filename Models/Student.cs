namespace MyWebApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // Initialized with default value
        public string Password { get; set; } = string.Empty;  // Initialized with default value
        public float GPA { get; set; }
    }
}
