using Final.Recipes;
using Final.Recipes.Ingredients;

RecipesApp recipesApp = new RecipesApp(
    new Repository(),
    new UserInteraction(new IngredientsRegister())
    );
recipesApp.Run("recipes.txt");


// The Orchestrator class
public class RecipesApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;

    public RecipesApp(
        Repository repository, 
        UserInteraction userInteraction)
    {
        _repository = repository;
        _userInteraction = userInteraction;
    }
   
    
    
    public void Run(string filePath)
    {
        // gets all the recipes saved in a particular file
        List<Recipe> allRecipes = _repository.Read(filePath);
        // The Print method should be IEnumerable type because we are not modifying the list
        _userInteraction.PrintExistingRecipes(allRecipes);

        _userInteraction.PromptToCreateRecipe();

        IEnumerable<Ingredient> ingredients = _userInteraction.ReadIngredientsFromUser();

        if (ingredients.Count() > 0)
        {
            var recipe = new Recipe(ingredients);
            allRecipes.Add(recipe);
            _repository.Write(filePath, allRecipes);
            _userInteraction.ShowMessage("Recipe added:");
            _userInteraction.ShowMessage(recipe.ToString());
        }
        else
        {
            _userInteraction.ShowMessage("No ingredients have been selected. Recipe will not be saved.");
        }

        _userInteraction.Exit();
    }
}

internal interface IUserInteraction
{
    void PrintExistingRecipes(IEnumerable<Recipe> allRecipes);
    void PromptToCreateRecipe();
    IEnumerable<Ingredient> ReadIngredientsFromUser();
    void ShowMessage(string message);
    void Exit();
}

internal interface IRepository
{
    List<Recipe> Read(string filePath);
    void Write(string filePath, IEnumerable<Recipe> allRecipes);
}

public class UserInteraction: IUserInteraction
{
    private readonly IngredientsRegister _ingredientsRegister;

    public UserInteraction(IngredientsRegister ingredientsRegister)
    {
        _ingredientsRegister = ingredientsRegister;
    }
    
    
    
    public void PrintExistingRecipes(IEnumerable<Recipe> allRecipes)
    {
        if (allRecipes.Count() > 0)
        {
            Console.WriteLine("Existing recipes are:" + Environment.NewLine);

            int counter = 1;
            foreach (var recipe in allRecipes)
            {
                Console.WriteLine($"*****{counter}*****");
                Console.WriteLine(recipe);
                Console.WriteLine();
                ++counter;
            }
        }
    }

    public void PromptToCreateRecipe()
    {
        Console.WriteLine("Create a new cookie recipe! Available ingredients are:");

        foreach (var ingredient in _ingredientsRegister.All)
        {
            Console.WriteLine(ingredient);
        }
    }
    public IEnumerable<Ingredient> ReadIngredientsFromUser()
    {
        bool shalStop = false;
        var ingredients = new List<Ingredient>();

        while (!shalStop)
        {
            Console.WriteLine("Add an ingredient by it's ID, or type anything else to exit");
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int id))
            {
                var selectedIngredient = _ingredientsRegister.GetById(id);
                if (selectedIngredient is not null)
                {
                    ingredients.Add(selectedIngredient);
                }
            }
            else
            {
                shalStop = true;
            }
        }
        return ingredients;
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void Exit()
    {
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey(true);
    }
}

public class IngredientsRegister
{
    public IEnumerable<Ingredient> All { get; } = new List<Ingredient>
    {
        new WheatFlour(),
        new SpeltFlour(),
        new Cinnamon(),
        new Sugar(),
        new CocoaPowder(),
        new Cardamom(),
        new Chocolate(),
        new Butter()
    };

    public Ingredient GetById(int id)
    {
        foreach (var ingredient in All)
        {
            if (ingredient.Id == id) return ingredient;
        }
        return null;
    }
}

public class Repository : IRepository
{
    public List<Recipe> Read(string filePath)
    {
        return new List<Recipe>
        {
            new Recipe(new List<Ingredient>
            {
                new WheatFlour(),
                new Butter(),
                new Sugar()
            }),
            new Recipe(new List<Ingredient>
            {
                new WheatFlour(),
                new Butter(),
                new Sugar()
            })
        };
    }

    public void Write(string filePath, IEnumerable<Recipe> allRecipes)
    {
        throw new NotImplementedException();
    }
}