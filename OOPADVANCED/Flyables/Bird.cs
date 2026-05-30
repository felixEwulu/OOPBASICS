namespace OOPADVANCED.Flyables;

public interface IFlyable
{
    void Fly();
}

public interface IFuelable
{
    void Fuel();
}

public class Bird : IFlyable
{
    public void Tweet() =>
    Console.WriteLine("Tweet");
    
    public void Fly() =>
    Console.WriteLine("Flying using its wings.");
}

public class Kite : IFlyable
{
    public void Fly() =>
    Console.WriteLine("Flying carried by the wind.");
}

public class Plane : IFlyable, IFuelable
{
    public void Fly() =>
    Console.WriteLine("Flying using jet propulsion.");
    
    public void Fuel() =>
    Console.WriteLine("Filling tanks with jet fuel.");
}