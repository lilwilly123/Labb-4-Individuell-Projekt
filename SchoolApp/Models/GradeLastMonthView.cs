namespace SchoolApp.Models;

public class GradeLastMonthView
{
    public int GradeId { get; set; }
    public DateTime GradeDate { get; set; }

    public string StudentFirstName { get; set; } = null!;
    public string StudentLastName { get; set; } = null!;

    public string CourseName { get; set; } = null!;
    public string GradeValue { get; set; } = null!;

    public string TeacherFirstName { get; set; } = null!;
    public string TeacherLastName { get; set; } = null!;
}
