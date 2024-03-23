using System;
using UnityEngine;

public enum PartType
{
    Head,
    Chest,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg
}

[System.Serializable]
public class Part
{
    private Character parentCharacter;
    [SerializeField] private Armor armor;
    private bool broken = false;

    public Action<Part, float> brokenPart;

    public ArmorType armorType;
    public PartType partType;
    public float fullHp;
    public float currentHp;
    public float avoidability;          //È¸ÇÇÀ²

    public Character ParentCharacter { get { return parentCharacter; } set { parentCharacter = value; } }
    public Armor PartArmor { get { return armor; } set { armor = value; } }
    public bool Broken { get { return broken; } set { broken = value; } }
    public float CurrentHp { get { return currentHp; } set { currentHp = value; } }

    public bool Aviod(float enemyAgility)
    {
        if ((ParentCharacter.CharacterStat.agility - enemyAgility) + avoidability > UnityEngine.Random.Range(0, 100))
            return true;
        else
            return false;
    }

    public float Damaged(AttackType attackType, float damage)
    {
        if (armor)
            damage = armor.CalculateDamage(attackType, damage);
        else
            CurrentHp -= damage;

        if (currentHp <= 0)
        {
            if (brokenPart != null)
            {
                broken = true;
                brokenPart(this, damage);
            }
        }
        
        return damage;
    }
}
