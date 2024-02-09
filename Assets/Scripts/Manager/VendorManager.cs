using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorManager : MonoBehaviour
{
    public List<Item> VendorItems = new List<Item>();

    public List<Item> AllItems = new List<Item>();

    public int VendorTotalItems = 5;

    

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

    }

    void PopulateVendor()
    {
        int randomIndex = Random.Range(0, AllItems.Count);

        for (int i = 1; i <= VendorTotalItems; i++)
        {
            VendorItems.Add(AllItems[randomIndex]);
        }
    }


}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int Count; 
    public bool Stackable;

    public Item(string name, string description, int count, bool stackable)
    {
        Name = name;
        Description = description;
        Count = count;
        Stackable = stackable;
    }
}

public class InventoryManager
{
    public List<Item> PlayerInventory = new List<Item>();

    public void AddItem(Item item)
    {
        if (item.Stackable)
        {
            if(CheckForItem(item)) 
            {
                index = GetItemIndex(item);
                if (index >= 0)
                {
                    PlayerInventory[index].Count++;
                }
            }
            else
            {
                PlayerInventory.Add(item);
            }
        }
        else
        {
            PlayerInventory.Add(item);
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
                    PlayerInventory[index].Count--;
                }
            }
            else
            {
                PlayerInventory.Remove(item);
            }
        }
        else
        {
            PlayerInventory.Remove(item);
        }
    }

    int index = -1;
    public bool CheckForItem(Item item)
    {
        if (PlayerInventory.Count == 0){
            return false;
        }
        else if (PlayerInventory.Contains(item)){
            return true;
        }
        else{
            return false;
        }
       
    }

    public int GetItemIndex(Item item)
    {
        foreach(Item i in PlayerInventory) {
            if (i.Name == item.Name)
            {
                return PlayerInventory.IndexOf(i);
            }
        }

        return -1;
    }
}
