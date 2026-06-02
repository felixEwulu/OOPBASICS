

public class Employee
{
    public int ID { get; set; } 
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
}

public class EmployeeDataAccessLogic
{
    public Employee GetEmployeeDetails(int id)
    {
        Employee emp = new Employee()
        {
            ID = id,
            Name = "Pranaya",
            Department = "IT",
            Salary = 1500
        };
        return emp;
    }
}