using System.Collections;
using UnityEngine;


public class DamageOverTimeDebuff : StatusEffect
{
    private float damagePerSecond;
    private Coroutine damageCoroutine;

    public DamageOverTimeDebuff(float duration, Character target, float damagePerSecond) : base(duration, target)
    {
        this.damagePerSecond = damagePerSecond;
    }

    public override void ApplyEffect()
    {
        // �ֱ������� ���ظ� ������ �ڷ�ƾ ����
        damageCoroutine = target.StartCoroutine(InflictDamageOverTime());
        Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // ����� ���� �� ���� �ڷ�ƾ �ߴ�
        if (damageCoroutine != null)
        {
            target.StopCoroutine(damageCoroutine);
        }
        Debug.Log("DoT debuff removed");
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damagePerSecond);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);

            duration -= 1f;
        }
    }

    private void DealDamageToTarget(float damage)
    {
        // ������ ���ظ� ������ �κ�
        // �� ���ÿ����� Debug.Log�� ����Ͽ�����, �����δ� ��󿡰� ���ظ� ������ ��
        Debug.Log("Dealing " + damage + " damage to the target");
    }
}