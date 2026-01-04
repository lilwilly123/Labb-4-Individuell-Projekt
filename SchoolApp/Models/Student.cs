using System.Security.Claims;

namespace SchoolApp.Models;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PersonalNumber { get; set; } = null!;

    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();

}
