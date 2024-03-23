using UnityEngine;

public abstract class StatusEffect
{
    protected float duration;
    protected Character target;

    public float Duration { get => duration; set => duration = value; }

    public StatusEffect(float duration, Character target)
    {
        this.duration = duration;
        this.target = target;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}