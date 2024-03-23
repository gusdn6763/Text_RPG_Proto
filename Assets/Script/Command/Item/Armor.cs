using UnityEngine;

public enum ArmorType { Helmet, Ear, Neck, Chest, HandLeft, HandRight, HandBoth, Gloves, Pants, Shose, Belt }

public class Armor : Item
{
    [Header("장비부분")]
    [SerializeField] private ArmorType armorType;
    [SerializeField] private Stat requestStat;
    [SerializeField] private Stat armorStat;
    [SerializeField] private float armorHealth;

    [Range(0,1)]
    [SerializeField] private float cutValue = 0;
    [Range(0, 1)]
    [SerializeField] private float stabValue = 0;
    [Range(0, 1)]
    [SerializeField] private float hitValue = 0;

    public ArmorType GetArmorType { get => armorType; }
    public Stat RequestStat { get => requestStat; set => requestStat = value; }
    public Stat ArmorStat { get => armorStat; set => armorStat = value; }
    public float ArmorHealth { get => armorHealth; set => armorHealth = value; }

    public override void BuyItem()
    {
        base.BuyItem();
        CommandActive(Constant.equip, true);
    }
    public void Equip()
    {
        if (Player.instance.Equip(this))
        {
            CommandActiveAll(false);
            CommandActive(Constant.info, true);
            CommandActive(Constant.unequip, true);
        }
    }
    public void UnEquip()
    {
        if (Player.instance.UnEquip(this))
        {
            CommandActiveAll(false);
            CommandActive(Constant.info, true);
            CommandActive(Constant.equip, true);
            CommandActive(Constant.delete, true);
        }
    }
    public override void GetItem()
    {
        base.GetItem();
        CommandActive(Constant.equip, true);
    }
    public override void DropItem()
    {
        base.DropItem();
        CommandActive(Constant.equip, true);
    }

    public float CalculateDamage(AttackType attackType, float damage)
    {
        float totalDamage = damage;
        switch (attackType)
        {
            case AttackType.Cut:
                totalDamage = damage * cutValue;
                break;
            case AttackType.Stab:
                totalDamage = damage * stabValue;
                break;
            case AttackType.Hit:
                totalDamage = damage * hitValue;
                break;
        }
        ArmorHealth -= totalDamage;

        if (ArmorHealth <= 0)
        {
            float remainingDamage = -ArmorHealth;
            ArmorHealth = 0;
            return remainingDamage;
        }
        else
            return 0;
    }
}

