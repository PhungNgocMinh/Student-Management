namespace StudentManagement;

public class Nguoi
{
    private string _name = string.Empty;
    private DateTime _birthDate;

    //  Full name, must not be empty
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name must not be empty.");
            }
            _name = value.Trim();
        }
    }

    //  Birth date, must not be in the future
    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value.Date > DateTime.Today)
            {
                throw new ArgumentOutOfRangeException(nameof(BirthDate), "Birth date cannot be in the future.");
            }
            _birthDate = value.Date;
        }
    }

    public Nguoi(string name, DateTime birthDate)
    {
        Name = name;
        BirthDate = birthDate;
    }

    //  Returns the person's information; child classes can override it
    public virtual string GetInfo()
    {
        return $"Name: {Name}, Birth date: {BirthDate:dd/MM/yyyy}";
    }
}