using System.Diagnostics;
using System.Security.Claims;

namespace SchoolApp.Models;

public class Staff
{
    public int StaffId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PersonalNumber { get; set; }
    public string Role { get; set; } = null!;

    public int? DepartmentId { get; set; }
    public int Salary { get; set; }
    public Department? Department { get; set; }

    public DateTime EmploymentDate { get; set; }

    public ICollection<Class> HomeroomClasses { get; set; } = new List<Class>();
    public ICollection<Grade> GradesGiven { get; set; } = new List<Grade>();
}
