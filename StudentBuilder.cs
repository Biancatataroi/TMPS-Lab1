using System;
using System.Collections.Generic;
using System.Net.Mail;

// BUILDER: validates and assembles a complete student profile step by step.
public class StudentBuilder
{
    private int _id;
    private string _name = "";
    private string _faculty = "";
    private string _group = "";
    private int _studyYear;
    private string _email = "";
    private readonly List<int> _grades = new();

    public StudentBuilder SetId(int id) { _id = id; return this; }
    public StudentBuilder SetName(string name) { _name = name; return this; }
    public StudentBuilder SetFaculty(string faculty) { _faculty = faculty; return this; }
    public StudentBuilder SetGroup(string group) { _group = group; return this; }
    public StudentBuilder SetStudyYear(int year) { _studyYear = year; return this; }
    public StudentBuilder SetEmail(string email) { _email = email; return this; }
    public StudentBuilder AddGrade(int grade)
    {
        if (grade < 1 || grade > 10) throw new ArgumentOutOfRangeException(nameof(grade));
        _grades.Add(grade);
        return this;
    }

    public Student Build()
    {
        if (_id <= 0) throw new ArgumentException("ID must be positive.");
        if (string.IsNullOrWhiteSpace(_name)) throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(_faculty)) throw new ArgumentException("Faculty is required.");
        if (string.IsNullOrWhiteSpace(_group)) throw new ArgumentException("Group is required.");
        if (_studyYear < 1 || _studyYear > 6) throw new ArgumentException("Study year must be 1-6.");
        if (!MailAddress.TryCreate(_email, out var address) || address.Address != _email)
            throw new ArgumentException("A valid email is required.");
        var student = new Student(_id, _name.Trim(), _faculty.Trim(), _group.Trim(), _studyYear, _email.Trim());
        student.Grades.AddRange(_grades);
        return student;
    }
}
