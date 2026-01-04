using SchoolApp.Models;

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }

    public string GradeValue { get; set; } = null!;
    public DateTime GradeDate { get; set; }


    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public Staff Teacher { get; set; } = null!;
}
