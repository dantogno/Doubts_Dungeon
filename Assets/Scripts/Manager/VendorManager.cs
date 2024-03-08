using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class VendorManager : MonoBehaviour
{
    //Vendor only has one of each item it pulls there are not stacking items for the vendor

    public List<Item> VendorItems;

    public List<Item> AllItems;

    //The number of items avalable forsame at a time
    public int VendorTotalItems = 5;

    InventoryManager IM;

    //Vendor Menu got values
    public string SelectedItemName;
    public bool isAnyChecked;


    // Start is called before the first frame update
    void Start()
    {
        PopulateVendor();

        IM = new InventoryManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void PopulateVendor()//Randomly pull times from out set item list
    {
        int randomIndex = Random.Range(0, AllItems.Count);

        for (int i = 1; i <= VendorTotalItems; i++)
        {
            AddItemToVendor(randomIndex);
        }
    }

    void AddItemToVendor(int randomIndex)
    {
        if (!VendorItems.Contains(AllItems[randomIndex]))
            VendorItems.Add(AllItems[randomIndex]);
        else
        {
            randomIndex = Random.Range(0, AllItems.Count);
            AddItemToVendor(randomIndex);
        } 
    }

    public void BuyItemByName(string name)
    {
        foreach (Item item in AllItems)
        {
            if(item.Name == name)
            {
                Item ItemToBuy = item;
                BuyItem(ItemToBuy);
            }
        }
    }

    public void BuyItem(Item item)
    {
        if(item.Cost <= IM.player.Currancy)
        {
            VendorItems.Remove(item);
            IM.AddItem(item);

            IM.player.Currancy -= item.Cost;
            Debug.Log($"{item.Name} purchased");
        }
        else
            Debug.Log("Not enough currancy");
    }

    //Could be used for chests
    public Item ReturnItem()
    {
        int randomIndex = Random.Range(0, AllItems.Count);

        Debug.Log($"returned: {AllItems[randomIndex].Name}");
        return AllItems[randomIndex];

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
