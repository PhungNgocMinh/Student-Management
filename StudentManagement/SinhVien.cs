namespace StudentManagement;

public class SinhVien : Nguoi
{
        private string _id = string.Empty;
    private string _classID = string.Empty;
    private double _averageGPA;

    //  Student ID, must not be empty. Stored in upper case.</summary>
    public string ID
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Student ID must not be empty.");
            }
            _id = value.Trim().ToUpper();
        }
    }

    //  Class ID, must not be empty. Stored in upper case
    public string ClassID
    {
        get => _classID;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Class ID must not be empty.");
            }
            _classID = value.Trim().ToUpper();
        }
    }

    //  Average GPA, only accepts values from 0 to 10
    public double AverageGPA
    {
        get => _averageGPA;
        set
        {
            if (double.IsNaN(value) || value < 0 || value > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(AverageGPA), "GPA must be between 0 and 10.");
            }
            _averageGPA = value;
        }
    }

    //  Letter grade, always calculated from the current GPA
    public char Grade => Classify();

    public SinhVien(string id, string name, DateTime birthDate, string classID, double averageGPA)
        : base(name, birthDate)
    {
        ID = id;
        ClassID = classID;
        AverageGPA = averageGPA;
    }

    //  Returns the letter grade for the current GPA
    public char Classify()
    {
        if (AverageGPA >= 9.0) return 'A';
        if (AverageGPA >= 8.0) return 'B';
        if (AverageGPA >= 6.5) return 'C';
        if (AverageGPA >= 5.0) return 'D';
        if (AverageGPA >= 4.0) return 'E';
        return 'F';
    }

    //Override: adds student details to the Nguoi information
    public override string GetInfo()
    {
        return $"ID: {ID}, {base.GetInfo()}, Class: {ClassID}, " +
               $"Average GPA: {AverageGPA:0.00}, Grade: {Grade}";
    }
}