using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoItem
{
    private string name;
    private int quantity;
    private int itemId;
    private int unitWeight;


    

    public CargoItem(int itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;

        CargoItem item = validItems.validItemsDict[itemId];
        if (item == null)
        {
            Debug.LogError(itemId + " is not a valid itemId");
        }
        else
        {
            this.name = item.getName();
            this.unitWeight = item.getUnitWeight();
        }

    }

    public CargoItem(int itemId, int quantity, int unitWeight, string name)
    {
        this.itemId = itemId;
        this.name = name;

        if (quantity > 0)
        {
            this.quantity = quantity;
        }
        else
        {
            this.quantity = 1;
            Debug.LogError("Item: " + name + " itemId: " + itemId + " may not have a quantity less than 1");
        }

        if (unitWeight >= 0)
        {
            this.unitWeight = unitWeight;
        }
        else
        {
            Debug.LogError("Item: " + name + " itemId: " + itemId + " may not have a weight less than or equal to 0");
            this.quantity = 1;
        }
    }

    public int getItemId()
    {
        return this.itemId;
    }

    public string getName()
    {
        return this.name;
    }

    public int getQuantity()
    {
        return this.quantity;
    }

    public int getUnitWeight()
    {
        return this.unitWeight;
    }

    public int getTotalWeight()
    {
        return this.unitWeight * this.quantity;
    }

    public void updateQuantity(int changeInQuantity)
    {
        int sum = this.quantity + changeInQuantity;


        if (sum >= 0)
        {
            this.quantity = sum;
        }
        else
        {
            Debug.LogError("Item: " + this.name + " itemId: " + this.itemId + " may not have a quantity less than 1");

        }

    }


}

public static class validItems {
    public static Dictionary<int, CargoItem> validItemsDict = new Dictionary<int, CargoItem>
    {
        { 1, new CargoItem(1, 1, 2, "Rocks")},
        { 2, new CargoItem(2, 1, 1, "Metal")},
        { 3, new CargoItem(3, 1, 100, "SuperHeavyObject")},
        { 4, new CargoItem(4, 1, 1, "Ship Fuel")},
        { 5, new CargoItem(5, 1, 20, "Concentrated Dark Matter")}
    };
}

