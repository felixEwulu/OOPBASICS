MedicalAppointment medicalAppointment = new MedicalAppointment(
    "Felix Ewulu", new DateTime(2026, 01, 01));

medicalAppointment.Reschedule(new DateTime(2026, 05, 01));
Console.ReadKey(true);


class Person
{
    public string Name { get; set; }
    public int YearOfBirth { get; set; }
    public Person(string name, int yearOfBirth)
    {
        Name = name;
        YearOfBirth = yearOfBirth;
    }
}

class MedicalAppointment
{
    private string _patientName;
    private DateTime _date;
    public MedicalAppointment(string  patientName, DateTime date)
    {
        _patientName = patientName;
        _date = date;
    }

    // When this constructor is called, it first executes the code from th constructor
    // with two same-typed parameter, before executing this.
    public MedicalAppointment(string patientName) :
        this(patientName, 7)
    {
    }
    
    public MedicalAppointment(string  patientName, int daysFromNow)
    {
        _patientName = patientName;
        _date = DateTime.Now.AddDays(daysFromNow);
    }
    
    public DateTime GetDate() => _date;

    public void Reschedule(DateTime date)
    {
        _date = date;
        var printer = new MedicalAppointmentPrinter();
        printer.Print(this);
    }

    public void Reschedule(int month, int day)
    {
        _date = new DateTime(_date.Year, month, day);
    }
}

class MedicalAppointmentPrinter
{
    public void Print(MedicalAppointment medicalAppointment)
    {
        Console.WriteLine($"Appointment will take place on {medicalAppointment.GetDate()}");
    }
}

class Rectangle
{
    // A readonly field can only be assigned at the declaration or in the constructor
    public readonly int Width;
    public readonly int Height;
    
    // Immutability means that once an object is created, it will never be modified.
    // make fields readonly if possible.
    // const modifier can be applied to both variables and fields.
    // const fields must be assigned a value at declaration
    // const fields are compile-time constant.
    
}