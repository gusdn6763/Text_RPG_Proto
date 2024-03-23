using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] private Area area;
    [SerializeField] private List<Loot> loots = new List<Loot>();
    [SerializeField] private List<Item> droppedItems = new List<Item>();


    public List<Item> DroppedItems { get { return droppedItems; } set { droppedItems = value; } }

    //·çÆÃ È®·ü  
    public void RollLoot()
    {
        foreach (Loot loot in loots)
        {
            int roll = Random.Range(0, 100);

            if (roll <= loot.DropChance)
            {
                DroppedItems.Add(Instantiate(loot.Item));
            }
        }
    }
}
