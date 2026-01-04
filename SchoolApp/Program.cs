using Microsoft.EntityFrameworkCore;
using SchoolApp;
using SchoolApp.Models;

using var context = new SchoolContext();

bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("\n=== School Menu ===");
    Console.WriteLine("1. Visa alla studenter");
    Console.WriteLine("2. Visa studenter i en klass");
    Console.WriteLine("3. Lägg till ny student");
    Console.WriteLine("4. Visa personal");
    Console.WriteLine("5. Lägg till personal");
    Console.WriteLine("6. Visa betyg senaste månaden");
    Console.WriteLine("7. Visa aktiva kurser");
    Console.WriteLine("8. Visa antal lärare per avdelning");
    Console.WriteLine("9. Sätt betyg");
    Console.WriteLine("0. Avsluta");
    Console.Write("Val: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowAllStudents(context);
            Console.ReadLine();
            break;
        case "2":
            ShowStudentsInClass(context);
            Console.ReadLine();
            break;
        case "3":
            AddStudent(context);
            Console.ReadLine();
            break;
        case "4":
            ShowStaff(context);
            Console.ReadLine();
            break;
        case "5":
            AddStaff(context);
            Console.ReadLine();
            break;
        case "6":
            ShowGradesLastMonth(context);
            Console.ReadLine();
            break;
        case "7":
            ShowActiveCourses(context);
            Console.ReadLine();
            break;
        case "8":
            ShowTeachers(context);
            Console.ReadLine();
            break;
        case "9":
            SetGrade(context);
            Console.ReadLine();
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Ogiltigt val.");
            Console.ReadLine();
            break;
    }
}

//------------------
//Show all students
//------------------
static void ShowAllStudents(SchoolContext context)
{
    var students = context.Student
       .Include(s => s.Class)
       .Include(s => s.Grades)
           .ThenInclude(g => g.Course)
       .Include(s => s.Grades)
           .ThenInclude(g => g.Teacher)
       .ToList();

    foreach (var s in students)
    {
        Console.WriteLine($"\n{s.FirstName} {s.LastName} – Klass: {s.Class.Name}");

        if (!s.Grades.Any())
        {
            Console.WriteLine("  Inga betyg än.");
            continue;
        }

        foreach (var g in s.Grades)
        {
            Console.WriteLine(
                $"  {g.Course.Name}: {g.GradeValue} " +
                $"({g.GradeDate:yyyy-MM-dd}) " +
                $"– {g.Teacher.FirstName} {g.Teacher.LastName}"
            );
        }
    }
}
//-----------------------
//Show Students in class
//-----------------------
static void ShowStudentsInClass(SchoolContext context)
{
    var classes = context.Class.ToList();
    if (!classes.Any())
    {
        Console.WriteLine("Inga klasser hittades.");
        return;
    }

    Console.WriteLine("Tillgängliga klasser:");
    foreach (var c in classes)
    {
        Console.WriteLine($"{c.ClassId}: {c.Name}");
    }

    Console.Write("Ange ClassId: ");
    if (!int.TryParse(Console.ReadLine(), out int classId))
    {
        Console.WriteLine("Ogiltigt id.");
        return;
    }

    var students = context.Student
        .Where(s => s.ClassId == classId)
        .OrderBy(s => s.LastName)
        .ThenBy(s => s.FirstName)
        .ToList();

    if (!students.Any())
    {
        Console.WriteLine("Inga studenter i den klassen.");
        return;
    }

    Console.WriteLine("Studenter i klassen:");
    foreach (var s in students)
    {
        Console.WriteLine($"{s.StudentId}: {s.FirstName} {s.LastName}");
    }
}
//------------------
//Add Student
//------------------
static void AddStudent(SchoolContext context)
{
    Console.Write("Förnamn: ");
    var firstName = Console.ReadLine() ?? "";

    Console.Write("Efternamn: ");
    var lastName = Console.ReadLine() ?? "";

    Console.Write("Personnummer: ");
    var pnr = Console.ReadLine() ?? "";

    Console.WriteLine("Välj klass (ClassId):");
    foreach (var c in context.Class)
    {
        Console.WriteLine($"{c.ClassId}: {c.Name}");
    }

    if (!int.TryParse(Console.ReadLine(), out int classId))
    {
        Console.WriteLine("Ogiltigt klass-id.");
        return;
    }

    var student = new Student
    {
        FirstName = firstName,
        LastName = lastName,
        PersonalNumber = pnr,
        ClassId = classId
    };

    context.Student.Add(student);
    context.SaveChanges();

    Console.WriteLine("Student sparad.");
}
//------------------
//Show Staff
//------------------
static void ShowStaff(SchoolContext context)
{
    Console.WriteLine("Vill du se (a)lla eller bara en viss roll (t.ex. Lärare)?");
    var answer = Console.ReadLine()?.ToLower();

    IQueryable<Staff> query = context.Staff;

    if (answer == "a")
    {
       
    }
    else
    {
        Console.Write("Ange roll (t.ex. Lärare, Rektor, Administratör): ");
        var role = Console.ReadLine() ?? "";
        query = query.Where(s => s.Role == role);
    }

    foreach (var s in query)
    {
        Console.WriteLine($"{s.StaffId}: {s.FirstName} {s.LastName} ({s.Role})");
    }
}
//------------------
//Add Staff
//------------------
static void AddStaff(SchoolContext context)
{
    Console.Write("Förnamn: ");
    var firstName = Console.ReadLine() ?? "";

    Console.Write("Efternamn: ");
    var lastName = Console.ReadLine() ?? "";

    Console.Write("Personnummer (valfritt): ");
    var pnr = Console.ReadLine();

    Console.Write("Roll (t.ex. Lärare, Rektor): ");
    var role = Console.ReadLine() ?? "";

    var staff = new Staff
    {
        FirstName = firstName,
        LastName = lastName,
        PersonalNumber = string.IsNullOrWhiteSpace(pnr) ? null : pnr,
        Role = role
    };

    context.Staff.Add(staff);
    context.SaveChanges();

    Console.WriteLine("Personal sparad.");
}
//-----------------------
// Show grades last month
//-----------------------
static void ShowGradesLastMonth(SchoolContext context)
{
    var grades = context.GradesLastMonth
        .OrderByDescending(g => g.GradeDate)
        .ToList();

    if (!grades.Any())
    {
        Console.WriteLine("Inga betyg satta den senaste månaden.");
        return;
    }

    Console.WriteLine("\nBetyg satta senaste månaden:\n");

    foreach (var g in grades)
    {
        Console.WriteLine(
            $"{g.GradeDate:yyyy-MM-dd} | " +
            $"{g.StudentFirstName} {g.StudentLastName} | " +
            $"{g.CourseName} | " +
            $"Betyg: {g.GradeValue} | " +
            $"Lärare: {g.TeacherFirstName} {g.TeacherLastName}"
        );
    }
}
//-----------------------
// Show active courses
//-----------------------
static void ShowActiveCourses(SchoolContext context)
{
    var courses = context.Course
        .Where(c => c.IsActive)
        .OrderBy(c => c.Name)
        .ToList();

    foreach (var c in courses)
    {
        Console.WriteLine($"{c.Code} – {c.Name}");
    }
}

//-----------------------
// Show Teachers
//-----------------------
static void ShowTeachers(SchoolContext context)
{
    var result = context.Class
        .Select(c => new
        {
            ClassName = c.Name,
            TeacherCount = c.Students
                .SelectMany(s => s.Grades)
                .Select(g => g.TeacherId)
                .Distinct()
                .Count()
        })
        .OrderBy(c => c.ClassName)
        .ToList();

    Console.WriteLine("Klasser och antal lärare:\n");

    foreach (var c in result)
    {
        Console.WriteLine($"{c.ClassName}: {c.TeacherCount} lärare");
    }
}
//-----------------------
// Set Grade
//-----------------------
static void SetGrade(SchoolContext context)
{
    using var transaction = context.Database.BeginTransaction();

    try
    {
        var classes = context.Class.ToList();
        Console.WriteLine("Välj klass:");
        foreach (var c in classes)
            Console.WriteLine($"{c.ClassId}: {c.Name}");

        if (!int.TryParse(Console.ReadLine(), out int classId))
            throw new Exception("Felaktigt klass id.");

  
        var students = context.Student
            .Where(s => s.ClassId == classId)
            .ToList();

        if (!students.Any())
            throw new Exception("Inga studenter i klassen.");

        Console.WriteLine("Välj student:");
        foreach (var s in students)
            Console.WriteLine($"{s.StudentId}: {s.FirstName} {s.LastName}");

        if (!int.TryParse(Console.ReadLine(), out int studentId))
            throw new Exception("Felaktigt student id.");

        var courses = context.Course.Where(c => c.IsActive).ToList();
        Console.WriteLine("Välj kurs:");
        foreach (var c in courses)
            Console.WriteLine($"{c.CourseId}: {c.Name}");

        if (!int.TryParse(Console.ReadLine(), out int courseId))
            throw new Exception("Felaktigt kurs id.");

        var teachers = context.Staff
            .Where(s => s.Role == "Lärare")
            .ToList();

        Console.WriteLine("Välj lärare:");
        foreach (var t in teachers)
            Console.WriteLine($"{t.StaffId}: {t.FirstName} {t.LastName}");

        if (!int.TryParse(Console.ReadLine(), out int teacherId))
            throw new Exception("Felaktigt lärar-id.");


        Console.Write("Ange betyg (A–F): ");

        var gradeValue = (Console.ReadLine() ?? throw new Exception("Betyg saknas.")).ToUpper();
        if (!new[] { "A", "B", "C", "D", "E", "F" }.Contains(gradeValue))
            throw new Exception("Ogiltigt betyg. Ange A–F.");

        var grade = new Grade
        {
            StudentId = studentId,
            CourseId = courseId,
            TeacherId = teacherId,
            GradeValue = gradeValue,
            GradeDate = DateTime.Now
        };

        context.Grade.Add(grade);
        context.SaveChanges();

        transaction.Commit();
        Console.WriteLine("Betyg satt.");
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine($"Fel: {ex.Message}");
    }
}



