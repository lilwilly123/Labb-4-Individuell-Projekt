namespace SchoolApp.Models;

public class Class
{
    public int ClassId { get; set; }
    public string Name { get; set; } = null!;

    public int HomeroomTeacherId { get; set; }
    public Staff HomeroomTeacher { get; set; } = null!;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
