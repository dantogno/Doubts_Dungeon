using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorManager : MonoBehaviour
{
    //Vendor only has one of each item it pulls there are not stacking items for the vendor

    public List<Item> VendorItems = new List<Item>();

    public List<Item> AllItems = new List<Item>();

    //The number of items avalable forsame at a time
    public int VendorTotalItems = 5;

    InventoryManager IM = new InventoryManager();


    // Start is called before the first frame update
    void Start()
    {
        PopulateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void PopulateItems()
    {
        //Items can be put in by code or put in through the edior
    }

    void PopulateVendor()//Randomly pull times from out set item list
    {
        int randomIndex = Random.Range(0, AllItems.Count);

        for (int i = 1; i <= VendorTotalItems; i++)
        {
            VendorItems.Add(AllItems[randomIndex]);
        }
    }

    public void BuyItem(Item item)
    {
        if(item.Cost <= IM.player.Currancy)
        {
            VendorItems.Remove(item);
            IM.AddItem(item);

            IM.player.Currancy -= item.Cost;
        }
    }

    //Could be used for chests
    public Item ReturnItem()
    {
        int randomIndex = Random.Range(0, AllItems.Count);
        return AllItems[randomIndex];
    }

}

enum ItemType { }

//Have to consider how I am coding debuf items
[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int Count; 
    public bool Stackable;

    public int Cost;

    public Item(string name, string description, int count, bool stackable, int cost)
    {
        Name = name;
        Description = description;
        Count = count;
        Stackable = stackable;

        Cost = cost;
    }
}

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void AddItem(Item item)
    {
        if (item.Stackable)
        {
            if(CheckForItem(item)) 
            {
                index = GetItemIndex(item);
                if (index >= 0)
                {
                    player.Inventory[index].Count++;
                }
            }
            else
            {
                player.Inventory.Add(item);
            }
        }
        else
        {
            player.Inventory.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (item.Stackable)
        {
            if (CheckForItem(item))
            {
                index = GetItemIndex(item);
                if (index >= 0)
                {
                    player.Inventory[index].Count--;
                }
            }
            else
            {
                player.Inventory.Remove(item);
            }
        }
        else
        {
            player.Inventory.Remove(item);
        }
    }

    int index = -1;
    public bool CheckForItem(Item item)
    {
        if (player.Inventory.Count == 0){
            return false;
        }
        else if (player.Inventory.Contains(item)){
            return true;
        }
        else{
            return false;
        }
       
    }

    public int GetItemIndex(Item item)
    {
        foreach(Item i in player.Inventory) {
            if (i.Name == item.Name)
            {
                return player.Inventory.IndexOf(i);
            }
        }

        return -1;
    }
}
