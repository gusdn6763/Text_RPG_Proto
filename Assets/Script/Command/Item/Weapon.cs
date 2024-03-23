using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Cut,
    Stab,
    Hit
}

[System.Serializable]
public class AttackList
{
    public AttackType attackType;
    public float damage;
}

public class Weapon : Armor
{
    [Header("무기부분")]
    [SerializeField] private List<AttackList> attackLists = new List<AttackList>();
    [SerializeField] private float weaponDistance;

    public float WeaponDistance { get { return weaponDistance; } }

    public (AttackType, float) Attack()
    {
        AttackList attackList = attackLists[Random.Range(0, attackLists.Count)];
        return (attackList.attackType, attackList.damage);
    }
}
