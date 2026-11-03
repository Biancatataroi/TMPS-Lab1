using System;

StudentRegistry registry = StudentRegistry.Instance;
registry.Add(new StudentBuilder().SetId(1).SetName("Ana Popescu")
    .SetFaculty("FCIM").SetGroup("FAF-242").SetStudyYear(2)
    .SetEmail("ana@example.com").AddGrade(8).AddGrade(9).Build());
registry.Add(new StudentBuilder().SetId(2).SetName("Alex Rusu")
    .SetFaculty("FCIM").SetGroup("FAF-242").SetStudyYear(2)
    .SetEmail("alex@example.com").AddGrade(7).AddGrade(10).Build());

GradeManager gradeManager = new(new AverageCalculator());

static string ReadRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string value = (Console.ReadLine() ?? "").Trim();
        if (value.Length > 0) return value;
        Console.WriteLine("This field is required.");
    }
}

static int ReadInt(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
            return value;
        Console.WriteLine($"Enter a number between {min} and {max}.");
    }
}

Student? ChooseStudent()
{
    int id = ReadInt("Student ID: ", 1, int.MaxValue);
    Student? student = registry.FindById(id);
    if (student is null) Console.WriteLine("Student not found.");
    return student;
}

static void PrintProfile(Student student)
{
    Console.WriteLine($"ID: {student.Id} | Name: {student.Name}");
    Console.WriteLine($"Faculty: {student.Faculty} | Group: {student.Group} | Year: {student.StudyYear}");
    Console.WriteLine($"Email: {student.Email}");
    Console.WriteLine("Grades: " + (student.Grades.Count == 0 ? "none" : string.Join(", ", student.Grades)));
}

while (true)
{
    Console.WriteLine("\n=== STUDENT GRADE MANAGEMENT SYSTEM - LAB 1 ===");
    Console.WriteLine("1. View all students");
    Console.WriteLine("2. Add new student");
    Console.WriteLine("3. Add grade");
    Console.WriteLine("4. View student profile");
    Console.WriteLine("5. Update student information");
    Console.WriteLine("6. Delete student");
    Console.WriteLine("7. Generate academic report");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");
    string? choice = Console.ReadLine();
    if (choice == "0") break;

    switch (choice)
    {
        case "1":
            if (registry.Students.Count == 0) Console.WriteLine("No students yet.");
            foreach (var s in registry.Students) Console.WriteLine($"{s.Id}: {s.Name} ({s.Group})");
            break;
        case "2":
            try
            {
                int id = registry.NextId();
                var builder = new StudentBuilder()
                    .SetId(id)
                    .SetName(ReadRequired("Name: "))
                    .SetFaculty(ReadRequired("Faculty: "))
                    .SetGroup(ReadRequired("Group: "))
                    .SetStudyYear(ReadInt("Study year (1-6): ", 1, 6))
                    .SetEmail(ReadRequired("Email: "));
                Student created = builder.Build();
                registry.Add(created);
                Console.WriteLine($"Student added with ID {created.Id}.");
            }
            catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
            break;
        case "3":
        {
            Student? student = ChooseStudent();
            if (student is null) break;
            int grade = ReadInt("Grade (1-10): ", 1, 10);
            gradeManager.AddGrade(student, grade);
            Console.WriteLine("Grade added successfully.");
            break;
        }
        case "4":
        {
            Student? student = ChooseStudent();
            if (student is not null) PrintProfile(student);
            break;
        }
        case "5":
        {
            Student? student = ChooseStudent();
            if (student is null) break;
            Console.WriteLine("Enter new data (leave empty to keep current value).");
            Console.Write($"Name [{student.Name}]: ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) student.Name = name.Trim();
            Console.Write($"Faculty [{student.Faculty}]: ");
            string? faculty = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(faculty)) student.Faculty = faculty.Trim();
            Console.Write($"Group [{student.Group}]: ");
            string? group = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(group)) student.Group = group.Trim();
            Console.Write($"Email [{student.Email}]: ");
            string? email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (System.Net.Mail.MailAddress.TryCreate(email.Trim(), out var parsed) && parsed.Address == email.Trim())
                    student.Email = email.Trim();
                else Console.WriteLine("Invalid email: previous email kept.");
            }
            Console.WriteLine("Student information updated.");
            break;
        }
        case "6":
        {
            Student? student = ChooseStudent();
            if (student is null) break;
            Console.Write($"Delete {student.Name}? (yes/no): ");
            if (string.Equals(Console.ReadLine(), "yes", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine(registry.Remove(student.Id) ? "Student deleted." : "Could not delete student.");
            else Console.WriteLine("Deletion cancelled.");
            break;
        }
        case "7":
        {
            Student? student = ChooseStudent();
            if (student is null) break;
            if (student.Grades.Count == 0)
            {
                Console.WriteLine("Add at least one grade before generating a report.");
                break;
            }
            Console.WriteLine("1. Average Grade Report");
            Console.WriteLine("2. Highest Grade Report");
            Console.WriteLine("3. Pass/Fail Report");
            int type = ReadInt("Choose report: ", 1, 3);
            AcademicReportCreator creator = type switch
            {
                1 => new AverageReportCreator(),
                2 => new HighestGradeReportCreator(),
                _ => new PassFailReportCreator()
            };
            Console.WriteLine("\n=== ACADEMIC REPORT ===");
            PrintProfile(student);
            Console.WriteLine(creator.GenerateReport(student));
            break;
        }
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}
