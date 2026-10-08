using System.Globalization;
using System.Text;

namespace StudentManagement;

public class Program
{
    private static readonly QuanLySinhVien _management = new();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool running = true;
        while (running)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();

            switch (choice)
            {
                case "1": AddStudent(); break;
                case "2": ShowList(); break;
                case "3": FindByID(); break;
                case "4": FindByName(); break;
                case "5": UpdateGPA(); break;
                case "6": RemoveStudent(); break;
                case "7": SortByGPA(); break;
                case "8": FilterPassed(); break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
            }
        }
    }

    // ===================== MENU =====================

    private static void ShowMenu()
    {
        Console.WriteLine("===== STUDENT MANAGEMENT =====");
        Console.WriteLine("1. Add student");
        Console.WriteLine("2. Show student list");
        Console.WriteLine("3. Find student by ID");
        Console.WriteLine("4. Find student by name");
        Console.WriteLine("5. Update average GPA");
        Console.WriteLine("6. Remove student");
        Console.WriteLine("7. Sort by GPA (descending)");
        Console.WriteLine("8. Filter passed students");
        Console.WriteLine("0. Exit");
        Console.Write("Choose a function: ");
    }

    // ===================== FEATURES =====================

    private static void AddStudent()
    {
        Console.WriteLine("--- ADD STUDENT ---");

        string id = InputString("Enter student ID: ");
        if (_management.GetByID(id) != null)
        {
            Console.WriteLine($"Student ID {id.ToUpper()} already exists, cannot add.");
            return;
        }

        string name = InputString("Enter full name: ");
        DateTime birthDate = InputBirthDate("Enter birth date (dd/MM/yyyy): ");
        string classID = InputString("Enter class ID: ");
        double gpa = InputGPA("Enter average GPA (0 - 10): ");

        var student = new SinhVien(id, name, birthDate, classID, gpa);
        if (_management.Add(student))
        {
            Console.WriteLine($"Added successfully! Grade: {student.Grade}.");
        }
        else
        {
            Console.WriteLine("Student ID already exists, cannot add.");
        }
    }

    private static void ShowList()
    {
        Console.WriteLine("--- STUDENT LIST ---");
        PrintTable(_management.GetList());
    }

    private static void FindByID()
    {
        Console.WriteLine("--- FIND BY ID ---");
        string id = InputString("Enter the student ID to find: ");
        SinhVien? student = _management.GetByID(id);

        if (student == null)
        {
            Console.WriteLine("No student found with this ID.");
            return;
        }
        Console.WriteLine(student.GetInfo());
    }

    private static void FindByName()
    {
        Console.WriteLine("--- FIND BY NAME ---");
        string keyword = InputString("Enter a name keyword: ");
        List<SinhVien> result = _management.GetByName(keyword);

        if (result.Count == 0)
        {
            Console.WriteLine("No matching student found.");
            return;
        }
        PrintTable(result);
    }

    private static void UpdateGPA()
    {
        Console.WriteLine("--- UPDATE AVERAGE GPA ---");
        string id = InputString("Enter the student ID to update: ");
        SinhVien? student = _management.GetByID(id);

        if (student == null)
        {
            Console.WriteLine("No student found with this ID.");
            return;
        }

        Console.WriteLine($"Student: {student.Name} - Current GPA: {student.AverageGPA:0.00}");
        double newGPA = InputGPA("Enter new GPA (0 - 10): ");
        _management.UpdateGPA(id, newGPA);
        Console.WriteLine($"Updated successfully! New grade: {student.Grade}.");
    }

    private static void RemoveStudent()
    {
        Console.WriteLine("--- REMOVE STUDENT ---");
        string id = InputString("Enter the student ID to remove: ");

        if (_management.Remove(id))
        {
            Console.WriteLine("Student removed.");
        }
        else
        {
            Console.WriteLine("No student found with this ID.");
        }
    }

    private static void SortByGPA()
    {
        Console.WriteLine("--- STUDENTS SORTED BY GPA (DESCENDING) ---");
        PrintTable(_management.SortByGPA());
    }

    private static void FilterPassed()
    {
        Console.WriteLine("--- PASSED STUDENTS (GPA >= 5) ---");
        PrintTable(_management.GetPassedStudents());
    }

    // ===================== PRINT =====================

    private static void PrintTable(List<SinhVien> students)
    {
        if (students.Count == 0)
        {
            Console.WriteLine("The list is empty.");
            return;
        }

        string header = $"{"ID",-18}{"Name",-28}{"Birth date",-13}{"Class",-18}{"GPA",-8}{"Grade",-6}";
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        foreach (SinhVien s in students)
        {
            Console.WriteLine(
                $"{s.ID,-18}{s.Name,-28}{s.BirthDate.ToString("dd/MM/yyyy"),-13}" +
                $"{s.ClassID,-18}{s.AverageGPA.ToString("0.00"),-8}{s.Grade,-6}");
        }
        Console.WriteLine($"Total: {students.Count} student(s).");
    }

    // ===================== INPUT HELPER =====================

    //  Reads a non-empty string; asks again if the input is empty
    private static string InputString(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            Console.WriteLine("Value must not be empty, please try again.");
        }
    }

    //  Reads a birth date in dd/MM/yyyy format; it cannot be in the future
    private static DateTime InputBirthDate(string message)
    {
        string[] formats = { "dd/MM/yyyy", "d/M/yyyy" };

        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!DateTime.TryParseExact(input?.Trim(), formats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime birthDate))
            {
                Console.WriteLine("Birth date must be in dd/MM/yyyy format, please try again.");
                continue;
            }

            if (birthDate > DateTime.Today)
            {
                Console.WriteLine("Birth date cannot be in the future, please try again.");
                continue;
            }
            return birthDate;
        }
    }

    //  Reads a GPA between 0 and 10 (accepts both '.' and ',')
    private static double InputGPA(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine()?.Trim().Replace(',', '.');

            if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double gpa))
            {
                Console.WriteLine("GPA must be a number, please try again.");
                continue;
            }

            if (double.IsNaN(gpa) || gpa < 0 || gpa > 10)
            {
                Console.WriteLine("Invalid GPA (must be between 0 and 10), please try again.");
                continue;
            }
            return gpa;
        }
    }
}
