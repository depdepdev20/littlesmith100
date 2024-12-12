using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public void DisplayResources(int currentChapter)
    {
        foreach (var ingredient in ingredients)
        {
            if (ingredient.unlockChapter <= currentChapter && ingredient.is_unlocked) //apakah sudah diunlock & spy urut
            {
                Debug.Log($"Resource: {ingredient.name}, Quantity: {ingredient.quantity}");
            }
        }
    }

    public void AddResource(string ingredientName, int amount)
    {
        Ingredient ingredient = ingredients.Find(i => i.name == ingredientName);
        if (ingredient != null)
        {
            ingredient.AddQuantity(amount);
        }
        else
        {
            Debug.LogError("Ingredient not found!");
        }
    }

    public Ingredient GetIngredient(string name)
    {
        return ingredients.Find(i => i.name == name);
    }
}
