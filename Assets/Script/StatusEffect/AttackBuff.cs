using UnityEngine;

public class AttackBuff : StatusEffect
{
    private float attackIncrease;

    public AttackBuff(float duration, Character target, float attackIncrease) : base(duration, target)
    {
        this.attackIncrease = attackIncrease;
    }

    public override void ApplyEffect()
    {
        Debug.Log("Attack buff applied: +" + attackIncrease + " Attack");
    }

    public override void RemoveEffect()
    {
        Debug.Log("Attack buff removed: -" + attackIncrease + " Attack");
    }
}