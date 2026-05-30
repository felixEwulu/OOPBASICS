using System.Text.Json;

Person person = new Person("Felix", "Ewulu", 1998);

var asJson = JsonSerializer.Serialize(person);
Console.WriteLine("As JSON");
Console.WriteLine(asJson);

var personJson = "{\"FirstName\":\"Felix\",\"LastName\":\"Ewulu\",\"YearOfBirth\":1998}\n";

var personFromJson = JsonSerializer.Deserialize<Person>(personJson);

Console.ReadKey(true);

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int YearOfBirth { get; set; }

    public Person(string firstName, string lastName, int yearOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        YearOfBirth = yearOfBirth;
    }
}

public class Pizza
{
    private readonly List<Ingredient> _ingredients = new List<Ingredient>();
    
    public void AddIngredient(Ingredient ingredient) =>
            _ingredients.Add(ingredient);


    public override string ToString() =>
        $"This is a pizza with {string.Join(", ", _ingredients)}";
}

public abstract class Ingredient
{
    protected Ingredient(int priceIfExtraTopping)
    {
        priceIfExtraTopping = priceIfExtraTopping;
    }
    
    public int PriceIfExtraTopping { get; }
    
    // Virtual methods are methods that may be overriden in the derived classes.
    public virtual string Name { get; } = "Some Ingredient";
    
    // Abstract methods can only be defined in abstract classes, they don't have implementations
    public abstract void Prepare();
    
    public override string ToString() => Name;
}

public class Cheddar : Cheese
{
    public int AgedForMonths { get; }
    
    public Cheddar(int priceIfExtraTopping, int agedForMonths)
        : base(priceIfExtraTopping)
    {
        AgedForMonths = agedForMonths;
    }
    
    public override string Name =>
        $"{base.Name}, more specifically, a Cheddar cheese aged for {AgedForMonths} months";

    public override void Prepare()
    {
        Console.WriteLine("Grate and sprinkle over pizza.");
    }

}

public abstract class Cheese : Ingredient
{
    public Cheese(int priceIfExtraTopping) : base(priceIfExtraTopping)
    {}
    
}

public class TomatoSauce : Ingredient
{
    public int TomatosIn100Grams { get; }
    // only virtual overriden methods can be sealed
    public override string Name => "Tomato Sauce";
    

    public TomatoSauce(int priceIfExtraTopping, int tomatoesIn100Grams)
        : base(priceIfExtraTopping)
    {
        TomatosIn100Grams = tomatoesIn100Grams;
    }
    
    public override void Prepare()
    {
        Console.WriteLine("Cook tomatoes with basil, garlic and salt. Spread on pizza.");
    }
}

public class Mozarella : Cheese
{
    public override string Name => "Mozarella";
    
    public override void Prepare()
    {
        throw new NotImplementedException();
    }

    public bool IsLight { get; }
    
    public Mozarella(int priceIfExtraTopping) : base(priceIfExtraTopping)
    {}
}