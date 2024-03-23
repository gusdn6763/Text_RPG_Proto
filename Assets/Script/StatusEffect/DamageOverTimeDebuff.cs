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
        // 주기적으로 피해를 입히는 코루틴 시작
        damageCoroutine = target.StartCoroutine(InflictDamageOverTime());
        Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // 디버프 종료 시 피해 코루틴 중단
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
            // 피해를 입히는 부분
            DealDamageToTarget(damagePerSecond);

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
            yield return new WaitForSeconds(1f);

            duration -= 1f;
        }
    }

    private void DealDamageToTarget(float damage)
    {
        // 실제로 피해를 입히는 부분
        // 이 예시에서는 Debug.Log를 사용하였지만, 실제로는 대상에게 피해를 입혀야 함
        Debug.Log("Dealing " + damage + " damage to the target");
    }
}