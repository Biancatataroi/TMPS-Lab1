using System;
using System.Collections.Generic;
using System.Linq;

// SINGLETON: one shared student registry within the running application.
public sealed class StudentRegistry
{
    private static readonly Lazy<StudentRegistry> _instance = new(() => new StudentRegistry());
    public static StudentRegistry Instance => _instance.Value;
    private StudentRegistry() { }

    private readonly List<Student> _students = new();
    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public Student? FindById(int id) => _students.FirstOrDefault(s => s.Id == id);
    public int NextId() => _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;

    public void Add(Student student)
    {
        if (FindById(student.Id) is not null)
            throw new InvalidOperationException("A student with this ID already exists.");
        _students.Add(student);
    }

    public bool Remove(int id)
    {
        var student = FindById(id);
        return student is not null && _students.Remove(student);
    }
}
