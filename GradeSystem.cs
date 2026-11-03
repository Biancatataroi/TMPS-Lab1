using System;
using System.Collections.Generic;
using System.Linq;

// SRP: Student stores student data; it does not calculate reports.
public class Student
{
    public int Id { get; }
    public string Name { get; set; }
    public string Faculty { get; set; }
    public string Group { get; set; }
    public int StudyYear { get; set; }
    public string Email { get; set; }
    public List<int> Grades { get; } = new();

    public Student(int id, string name, string faculty, string group, int studyYear, string email)
    {
        Id = id;
        Name = name;
        Faculty = faculty;
        Group = group;
        StudyYear = studyYear;
        Email = email;
    }
}

// OCP/DIP from Lab 0: calculators can be extended via this abstraction.
public interface IGradeCalculator
{
    double Calculate(List<int> grades);
}

public class AverageCalculator : IGradeCalculator
{
    public double Calculate(List<int> grades) => grades.Average();
}

public class HighestCalculator : IGradeCalculator
{
    public double Calculate(List<int> grades) => grades.Max();
}

public class GradeManager
{
    private readonly IGradeCalculator _calculator;
    public GradeManager(IGradeCalculator calculator) => _calculator = calculator;

    public void AddGrade(Student student, int grade)
    {
        if (grade < 1 || grade > 10)
            throw new ArgumentOutOfRangeException(nameof(grade), "Grade must be between 1 and 10.");
        student.Grades.Add(grade);
    }

    public double GetResult(Student student)
    {
        if (student.Grades.Count == 0)
            throw new InvalidOperationException("Student has no grades.");
        return _calculator.Calculate(student.Grades);
    }
}
