using Final.Recipes.Ingredients;

namespace Final.Recipes;

public class Recipe
{
    public IEnumerable<Ingredient> Ingredients { get; }
    // To create a recipe for a particular food, you should
    //pass a list of ingredients
    //on recipe object holds a list of ingredients
    public Recipe(IEnumerable<Ingredient> ingredients)
    {
        Ingredients = ingredients;
    }

    public override string ToString()
    {
        var steps = new List<string>();
        foreach (var ingredient in Ingredients)
        {
            steps.Add($"{ingredient.Name}. {ingredient.PreparationInstructions}");
        }
        return string.Join(Environment.NewLine, steps);
    }
    
}