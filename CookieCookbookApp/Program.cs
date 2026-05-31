/*   
When the app starts for the first time, it prompts the user to create a new recipe
A recipe is a collection of ingredients.
The user adds the ingredient by typing the Id to the console
After selecting all the ids required, exit the selection by typing an invalid id
A message is printed in the console, saying the recipe has been added.
It then, one after the other, lists the method of preparation of each ingredient added
Each ingredient should have a name, and method of preparation
E.g Wheat Flour. Sieve. Add to other ingredients.

Then press any key to exit.

.....................................

When the app is run for the second time, the recipe created is printed on the screen
It should be saved as a .json file.
I should contain a json array which should have a single string containing the Ids selected i.e ["1, 2, 5"]
We must be ready to store the recipe as both txt and json formats

Consider reusing the stringsTextualRepository class
Consider having a class storing recipes in .txt and another in .json files, both implementing the same interface.
ingredients can be represented similarly as in the Pizzeria app.
An ingredient has an id, name and preparation instructions.
Remember the single responsibility principle.

*/

using System.Text.Json;

const string jsonFilePath = "recipes.json";
const string txtFilePath = "recipes.txt";

IngredientsRegister register = new IngredientsRegister();
var repository = new JsonRecipeRepository(new StringTextualRepository());

CookieCookbookApp app = new CookieCookbookApp(register, repository, jsonFilePath);
app.Run();

Console.ReadKey(true);

public class CookieCookbookApp
{
    private readonly IngredientsRegister _register;
    private readonly IRecipeRepository _repository;
    private readonly string _filePath;

    public CookieCookbookApp(
        IngredientsRegister register,
        IRecipeRepository repository,
        string filePath
        )
    {
        _register = register;
        _repository = repository;
        _filePath = filePath;
    }
    
    public void Run()
    {
        var existingRecipes = _repository.ReadAll(_filePath);

        if (existingRecipes.Count > 0)
        {
            ConsoleInterface.Print("Existing recipes are:");
            ConsoleInterface.Print("");
            
            RecipePrinter.PrintAll(existingRecipes, _register);
            ConsoleInterface.Print("");
            
        }
        
        ConsoleInterface.PrintWelcome();
        ConsoleInterface.PrintAvailableIngredients(_register);
        
        var ids = ConsoleInterface.PromptForIngredientIds(_register);
        var recipe = new Recipe(ids);
        
        _repository.Write(_filePath, recipe);
        
        RecipePrinter.Print(recipe, _register);
    }
}

// =================================================================
// RECIPE
// =================================================================
// A recipe is a list of ingredients ID chosen by the user.
// It is a pure data object - it has no behaviour beyond holding IDs.
// We store IDs (not Ingredient objects) so the recipe can be
// serialized/deserialized without any ingredient logic leaking into storage

public class Recipe
{
    public List<int> IngredientIds { get; }

    public Recipe(List<int> ingredientIds)
    {
        IngredientIds = ingredientIds;
    }
}

// =====================================================================
// RECIPE PRINTER
// =====================================================================
// Separated from ConsoleInterface because printing a recipe is a
// distinct concern from collecting user input.

public static class RecipePrinter
{
    public static void Print(Recipe recipe, IngredientsRegister register)
    {
        foreach (var id in recipe.IngredientIds )
        {
            var ingredient = register.GetById(id);
            if (ingredient is not null)
            {
                Console.WriteLine(ingredient); // calls ingredient.ToString()
            }

        }
    }

    public static void PrintAll(List<Recipe> recipes, IngredientsRegister register)
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            Console.WriteLine($"***** {i + 1} *****");
            Print(recipes[i], register);
            ConsoleInterface.Print("");
        }
    }
}

public static class ConsoleInterface
{
    public static void Print(string message)
    {
        Console.WriteLine(message);
    }
    
    public static void PrintWelcome()
    {
        Console.WriteLine("Create a new cookie recipe. Available ingredients: ");
    }

    public static void PrintInstruction()
    {
        Console.WriteLine("Add an ingredient by it's Id or type anything else if finished.");
    }
    
    public static void PrintAvailableIngredients(IngredientsRegister register)
    {
        foreach (var ingredient in register.All)    
        {
            Console.WriteLine($"{ingredient.Id}. {ingredient.Name}");
        }
    }
    
    // Keep asking for IDs until the user types something that is not
    // a valid integer Or and Id that doesn't exist in the register
    public static List<int> PromptForIngredientIds(IngredientsRegister register)
    {
        var ids = new List<int>();

        while (true)
        {
            PrintInstruction();
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
                break;

            if (register.GetById(id) is null)
            {
                Console.WriteLine($"{id} is not found.");
                break;
            }
            
            ids.Add(id);

        }

        return ids;
    }
}

// =====================================================================
// INGREDIENTS REGISTER
// =====================================================================
// One place that knows every ingredient that exists.
// The app and repositories never hard-code ingredients — they ask here.


public class IngredientsRegister
{
    public IReadOnlyList<Ingredient> All { get; } = new List<Ingredient>
    {
        new WheatFlour(),
        new CoconutFlour(),
        new Butter(),
        new Chocolate(),
        new Sugar(),
        new Cardamom(),
        new Cinnamon(),
        new CocoaPowder()
    };
    
    // Returns null if no ingredient with that ID exists.
    public Ingredient? GetById(int id) =>
        All.FirstOrDefault(i => i.Id == id);
}

// =====================================================================
// REPOSITORY INTERFACE
// =====================================================================
// Both the JSON and TXT repositories implement this contract.
// The app only depends on this interface, never on a concrete class.
// This means you can swap storage formats without touching the app.
public interface IRecipeRepository
{
    // Returns null when no file exists yet
   List<Recipe> ReadAll(string filePath);
    void Write(string filePath, Recipe recipe);
}

// =====================================================================
// STRINGS TEXTUAL REPOSITORY
// =====================================================================
// A low-level helper responsible ONLY for reading and writing plain
// text files as arrays of strings (one string per line).
// Neither the JSON nor the TXT repository talks to the file system
// directly — they delegate to this class.

public class StringTextualRepository
{
    public string[] ReadAllLines(string filePath)
    {
        if(!File.Exists(filePath))
            return Array.Empty<string>();
        
        return File.ReadAllLines(filePath);
    }
    
    public void WriteAllLines(string filePath, IEnumerable<string> lines)
    {
        File.WriteAllLines(filePath, lines);
    }
}

// =====================================================================
// JSON RECIPE REPOSITORY
// =====================================================================
// Stores and loads the recipe as a JSON file.
//
// Format on disk:
//   ["1, 3, 5"]
//
// That is a JSON array containing a SINGLE string with comma-separated IDs.
// This matches the spec exactly.

public class JsonRecipeRepository : IRecipeRepository
{
    private readonly StringTextualRepository _stringsRepo;
    
    public JsonRecipeRepository(StringTextualRepository stringsRepo)
    {
        _stringsRepo = stringsRepo;
    }

    public List<Recipe> ReadAll(string filePath)
    {
        var lines = _stringsRepo.ReadAllLines(filePath);

        if (lines.Length == 0)
            return new List<Recipe>();
        
        var json = string.Join(Environment.NewLine, lines);
        
        // The JSON is a string array with one element: "1,2,3"
        var array = JsonSerializer.Deserialize<List<string>>(json);

        if (array is null || array.Count == 0) return new List<Recipe>();

        return array.Select(entry => new Recipe(ParseIds(entry))).ToList();
    }

    public void Write(string filePath, Recipe recipe)
    {
        // Turn [1,2,3] -> "1,2,3"
        var idsString = string.Join(", ", recipe.IngredientIds);
        
        // Read existing entries first
        var existing = new List<string>();
        var lines = _stringsRepo.ReadAllLines(filePath);
        if (lines.Length > 0)
        {
            var existingArray = JsonSerializer
                .Deserialize<List<string>>(string.Join("", lines));
            if (existingArray is not null)
            {
                existing.AddRange(existingArray);
            }
        }
        existing.Add(idsString);
        
        // Wrap in a JSON array with one element -> ["1, 2, 3"]
        var json = JsonSerializer.Serialize(existing,
            new JsonSerializerOptions { WriteIndented = true });
        
        _stringsRepo.WriteAllLines(filePath, new[] { json });
    }
    
    private static List<int> ParseIds(string raw) =>
        raw.Split(',')
            .Select(s => s.Trim())
            .Where(s => int.TryParse(s, out _))
            .Select(int.Parse).ToList();

}

// =====================================================================
// INGREDIENT HIERARCHY
// =====================================================================
// Base class uses virtual Prepare() with a default implementation.
// Intermediate abstract classes (Flour, Spice) chain base.Prepare().
// Concrete classes just set Name and Id — no logic needed there.

public abstract class Ingredient
{
    public string Name { get; init; } = string.Empty;
     public int Id {get; init;}
    
    // Default step that every ingredients end with.
    public virtual string Prepare() => "Add to other ingredients.";
    
    // e.g. "Wheat Flour. Sieve. Add to other ingredients."
    public override string ToString() =>
        $"{Name}. {Prepare()}";
    
}

public abstract class Flour : Ingredient
{

    public override string Prepare() =>
        $"Sieve. {base.Prepare()}";
}
public abstract class Spice : Ingredient
{

    public override string Prepare() =>
        $"Take half a teaspoon. {base.Prepare()}";
}

public class WheatFlour: Flour
{
    public WheatFlour() { Name = "Wheat Flour"; Id = 1; }
}
public class CoconutFlour: Flour
{
    public CoconutFlour() { Name = "Coconut Flour"; Id = 2; }    
}
public class Butter: Ingredient
{
    public Butter() { Name = "Butter"; Id = 3; }
    
    public override string Prepare() =>
        $"Melt on low heat. {base.Prepare()}";
}
public class Chocolate: Ingredient
{
    public Chocolate() { Name = "Chocolate"; Id = 4; } 
    
    public override string Prepare() =>
        $"Melt in a water bath. {base.Prepare()}";
    
}
public class Sugar: Ingredient
{
    public Sugar() { Name = "Sugar"; Id = 5; } 
 

}
public class Cardamom: Spice
{
    public Cardamom() { Name = "Cardamon"; Id = 6; } 

}
public class Cinnamon: Spice
{
    public Cinnamon() { Name = "Cinnamon"; Id = 7; } 

}
public class CocoaPowder: Ingredient
{
    public CocoaPowder() { Name = "CocoaPowder"; Id = 8; } 
 
}