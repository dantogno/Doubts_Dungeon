using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Stabilizer, Adrenalin, Histamine, Acetylcholine, Endorphins, Noradrenaline, Dopamine }

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField]
    ItemType itemType;

    public string Name;
    public string Description;
    public int Count;
    public bool Stackable;

    public int Cost;

    public Item(ItemType it, string name, string description, int count, bool stackable, int cost)
    {
        this.itemType = it;

        Name = name;
        Description = description;
        Count = count;
        Stackable = stackable;

        Cost = cost;
    }
}
