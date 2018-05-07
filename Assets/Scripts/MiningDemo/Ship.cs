using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship {

    private string name;
    private int shipID;
    private ShipClass shipClass;
    private string owner;
    private CargoHold cargoHold;


    public Ship (int ID, string name, ShipClass shipClass, string owner)
    {
        this.name = name;
        this.shipClass = shipClass;
        this.owner = owner;
        this.cargoHold = new CargoHold(40);
        this.shipID = ID;
    }
}

public class CargoHold
{
    private List<CargoItem> cargoItems;
    private int cargoCapacity;
    private int currentCapacity;

    public CargoHold (int capacity)
    {
        this.cargoCapacity = capacity;
        this.cargoItems = new List<CargoItem>();
        this.currentCapacity = 0;
    }

    //public Dictionary

    public void addItem(CargoItem item, int quantity) 
    {
        bool addError = false;

        for (int i = 0; i < quantity; i++)
        {
            if (item.size + this.currentCapacity >= this.cargoCapacity)
            {
                addError = true;
            }
            else
            {
                this.cargoItems.Add(item);
                this.currentCapacity += item.size;
            }
        }
        if (addError)
        {
            //throw new IllegalArgumentException("Could not add some items to inventory, not enough space");
        }
    }

    public void removeItem(int itemId, int quantity)
    {
        bool removeError = false;

        List<CargoItem> tempList = this.cargoItems.
            
            (x => x.getItemId() == itemId);

        if (temoList)
        {

        }
 
        if (addError)
        {
           // throw new IllegalArgumentException("Could not remove some items to inventory, none exist");
        }

    }

}

public class CargoItem
{
    private string name;
    private int quantity;
    private int itemId;
    private int unitWeight;

    public CargoItem(int itemId, int quantity, int unitWeight, string name)
    {
        this.itemId = itemId;
        this.name = name;

        if (quantity > 0)
        {
            this.quantity = quantity;
        } else
        {
           // throw new IllegalArgumentException("Item: " + name + " itemId: " + itemId+ " may not have a quantity less than 1");
            this.quantity = 1;
        }

        if (unitWeight >= 0)
        {
            this.unitWeight = unitWeight;
        } else
        {
           // throw new IllegalArgumentException("Item: " + name + " itemId: " + itemId + " may not have a weight less than or equal to 0");
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

    public void updateQuantity(int changeInQuantity)
    {
        int sum = this.quantity + changeInQuantity;
            

        if (sum >= 0)
        {
            this.quantity = sum;
        } else
        {
           // throw new IllegalArgumentException("Item: " + this.name + " itemId: " + this.itemId + " may not have a quantity less than 1");
        }

    }


}





public enum ShipClass
{
    CAPITAL,FRIGGATE,YACHT
}
