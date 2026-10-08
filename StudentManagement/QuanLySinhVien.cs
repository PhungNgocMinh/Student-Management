using System.Globalization;
using System.Text;

namespace StudentManagement;

public class QuanLySinhVien
{
        private readonly List<SinhVien> _students = new();

    //  Adds a student. Returns false if the ID already exists
    public bool Add(SinhVien student)
    {
        if (GetByID(student.ID) != null)
        {
            return false;
        }
        _students.Add(student);
        return true;
    }

    //  Updates the GPA of a student. Returns false if the ID is not found
    public bool UpdateGPA(string studentID, double newGPA)
    {
        SinhVien? student = GetByID(studentID);
        if (student == null)
        {
            return false;
        }
        student.AverageGPA = newGPA;
        return true;
    }

    //  Removes a student by ID. Returns false if the ID is not found
    public bool Remove(string studentID)
    {
        SinhVien? student = GetByID(studentID);
        if (student == null)
        {
            return false;
        }
        return _students.Remove(student);
    }

    //  Finds a student by ID (case-insensitive). Returns null if not found
    public SinhVien? GetByID(string studentID)
    {
        string id = (studentID ?? string.Empty).Trim();
        return _students.FirstOrDefault(s =>
            string.Equals(s.ID, id, StringComparison.OrdinalIgnoreCase));
    }

    //  Finds all students whose name contains the keyword (ignores case and accents)
    public List<SinhVien> GetByName(string keyword)
    {
        string key = RemoveDiacritics(keyword ?? string.Empty).Trim();
        return _students
            .Where(s => RemoveDiacritics(s.Name).Contains(key, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    //  Returns a copy sorted by GPA, highest first (the original list is unchanged)
    public List<SinhVien> SortByGPA()
    {
        return _students
            .OrderByDescending(s => s.AverageGPA)
            .ThenBy(s => s.Name)
            .ToList();
    }

    //  Returns the students who passed (GPA &gt;= 5)
    public List<SinhVien> GetPassedStudents()
    {
        return _students.Where(s => s.AverageGPA >= 5).ToList();
    }

    //  Returns a copy of the list so outside code cannot modify it directly
    public List<SinhVien> GetList()
    {
        return new List<SinhVien>(_students);
    }

    //  Removes accents (Ex: "Nguyen" == "Nguyễn")
    private static string RemoveDiacritics(string text)
    {
        string normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString()
                 .Normalize(NormalizationForm.FormC)
                 .Replace('đ', 'd')
                 .Replace('Đ', 'D');
    }
}