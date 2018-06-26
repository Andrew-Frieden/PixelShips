using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiningDemoShip
{
    private string name;
    private int shipId;
    private ShipClass shipClass;
    private string owner;
    private CargoHold cargoHold;


    public MiningDemoShip(int ID, string name, ShipClass shipClass, string owner)
    {
        this.name = name;
        this.shipClass = shipClass;
        this.owner = owner;
        this.cargoHold = new CargoHold(40);
        this.shipId = ID;
    }

    public string getStringManifest()
    {
        string manifest = "Manifest for " + this.shipClass + " " + this.name + " under the command of " + this.owner;

        manifest += this.cargoHold.getStringManifest();

        return manifest;
    }

    public IEnumerable<CargoItem> getManifest()
    {
        return this.cargoHold.GetCargoItems();
    }

    public void addItem(CargoItem item)
    {
        this.cargoHold.addItem(item);
    }

    public void removeitem(CargoItem item)
    {
        this.cargoHold.removeItem(item.getItemId(), item.getQuantity());
    }
}

public class CargoHold
{
    private List<CargoItem> cargoItems;
    private int cargoCapacity;
    private int currentCapacity;

    public CargoHold(int capacity)
    {
        this.cargoCapacity = capacity;
        this.cargoItems = new List<CargoItem>();
        this.currentCapacity = 0;
    }


    public void addItem(CargoItem item)
    {
        if (this.canAdditem(item))
        {

            CargoItem foundItem = getItem(item.getItemId());
            if (foundItem != null)
            {
                foundItem.updateQuantity(item.getQuantity());

            }
            else
            {
                this.cargoItems.Add(item);

            }
            this.currentCapacity += item.getTotalWeight();

            Debug.Log("Added " + item.getName() + " with total weight " + item.getTotalWeight() + " to ship inventory");

            Debug.Log("Ship current capacity: " + this.currentCapacity + "/" + this.cargoCapacity);
        }
        else
        {
            Debug.LogWarning("Could not add " + item.getName() + " to inventory, not enough space");
        }

    }

    private bool canAdditem(CargoItem item)
    {
        if (item.getTotalWeight() + this.currentCapacity > this.cargoCapacity)
        {
            return false;
        }

        return true;
    }

    public void removeItem(int itemId, int quantity)
    {

        CargoItem item = getItem(itemId);
        if (item != null)
        {
            if (item.getQuantity() >= quantity)
            {
                item.updateQuantity(quantity * -1);
            }
            else
            {
                Debug.LogError("Item: " + item.getName() + " itemId: " + itemId + " does not have a quantity great than requested" + quantity);
            }

            Debug.Log("Removed " + item.getName() + " with total weight " + (item.getUnitWeight() * quantity) + " from ship inventory");

            Debug.Log("Ship current capacity: " + this.currentCapacity + "/" + this.cargoCapacity);

        }
        else
        {
            Debug.LogError("Item: " + item.getName() + " itemId: " + itemId + " was not found and cannot be removed");
        }

    }

    public bool hasItem(int itemId)
    {
        IEnumerable<CargoItem> query = from s in this.cargoItems
                                       where s.getItemId() == itemId
                                       select s;

        var e = query.FirstOrDefault();

        return (e != null);
    }

    public CargoItem getItem(int itemId)
    {
        IEnumerable<CargoItem> query = from s in this.cargoItems
                                       where s.getItemId() == itemId
                                       select s;

        return query.FirstOrDefault();
    }

    public string getStringManifest()
    {
        string manifest = "\nITEM:\t\t\tQUANTITY\t\tTOTAL WEIGHT";

        foreach (var item in cargoItems)
        {
            manifest += "\n" + item.getName() + "\t\t\t" + item.getQuantity() + "\t\t" + item.getTotalWeight();
        }

        manifest += "\nTotal ship capacity: " + this.currentCapacity + "/" + this.cargoCapacity;

        return manifest;
    }

    public IEnumerable<CargoItem> GetCargoItems()
    {
        IEnumerable<CargoItem> items = this.cargoItems;

        return items;
    }

    public int getCurrentCapacity()
    {
        return this.currentCapacity;
    }

    public int getCargoCapacity()
    {
        return this.cargoCapacity;
    }

}


public enum ShipClass
{
    CAPITAL, FRIGGATE, YACHT
}
