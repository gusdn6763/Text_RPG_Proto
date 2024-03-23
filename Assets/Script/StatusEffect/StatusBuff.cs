using UnityEngine;

public class StatusBuff : StatusEffect
{
    private Status status;

    public StatusBuff(float duration, Character target, Status status) : base(duration, target)
    {
    }

    public override void ApplyEffect()
    {
        Player.instance.ShowIntroduce("무언가의 디버프");
    }

    public override void RemoveEffect()
    {

    }
}