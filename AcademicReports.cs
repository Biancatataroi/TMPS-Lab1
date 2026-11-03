using System;

// FACTORY METHOD: each concrete creator selects the report type.
public interface IAcademicReport
{
    string Generate(Student student);
}

public class AverageReport : IAcademicReport
{
    public string Generate(Student student)
    {
        var result = new GradeManager(new AverageCalculator()).GetResult(student);
        return $"Average grade: {result:F2}";
    }
}

public class HighestGradeReport : IAcademicReport
{
    public string Generate(Student student)
    {
        var result = new GradeManager(new HighestCalculator()).GetResult(student);
        return $"Highest grade: {result:F2}";
    }
}

public class PassFailReport : IAcademicReport
{
    public string Generate(Student student)
    {
        var average = new GradeManager(new AverageCalculator()).GetResult(student);
        // Demonstration rule: an average of at least 5 means PASS.
        return $"Average: {average:F2} | Status: {(average >= 5 ? "PASS" : "FAIL")}";
    }
}

public abstract class AcademicReportCreator
{
    public abstract IAcademicReport CreateReport(); // factory method

    public string GenerateReport(Student student)
    {
        IAcademicReport report = CreateReport();
        return report.Generate(student);
    }
}

public class AverageReportCreator : AcademicReportCreator
{
    public override IAcademicReport CreateReport() => new AverageReport();
}

public class HighestGradeReportCreator : AcademicReportCreator
{
    public override IAcademicReport CreateReport() => new HighestGradeReport();
}

public class PassFailReportCreator : AcademicReportCreator
{
    public override IAcademicReport CreateReport() => new PassFailReport();
}
