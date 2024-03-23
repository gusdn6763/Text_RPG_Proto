using UnityEngine;

[System.Serializable]
public class Loot
{
    [SerializeField] private Item item;
    [Range(0f, 100f)]
    [SerializeField] private float dropChance;

    public Item Item { get => item; }
    public float DropChance { get => dropChance; }
}