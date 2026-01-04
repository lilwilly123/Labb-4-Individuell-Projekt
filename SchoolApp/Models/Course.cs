using System.Diagnostics;

namespace SchoolApp.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool IsActive { get; set; }

    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
