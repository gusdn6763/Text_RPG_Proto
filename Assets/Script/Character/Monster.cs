using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum MonsterState
{
    NONE,
    IDLE,
    MOVE,
    RELOAD,
    ATTACK,
    RUN,
    DIE
}

[RequireComponent(typeof(MobCommand))]
[RequireComponent(typeof(LootTable))]
public class Monster : Character
{
    [Header("공격 종류")]
    [SerializeField] private List<AttackList> attackLists = new List<AttackList>();


    protected LootTable lootTable;
    protected MobCommand mobCommand;
    protected MonsterState monsterState = MonsterState.NONE;

    protected override void Awake()
    {
        base.Awake();
        lootTable = GetComponent<LootTable>();
        mobCommand = GetComponent<MobCommand>();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(StateCoroutine());
        StartCoroutine(ActionCoroutine());
    }
    public virtual IEnumerator StateCoroutine()
    {
        while (!isDie)
        {
            if (monsterState == MonsterState.DIE)
                yield break;
            float zDistance = transform.position.z - Player.instance.transform.position.z;

            if (0 >= zDistance)
                Die();
            else if (attackDistance > zDistance)
            {
                if (reloadTime > reloading)
                    monsterState = MonsterState.RELOAD;
                else
                    monsterState = MonsterState.ATTACK;
            }
            else if (recognitionDistance > zDistance)
                monsterState = MonsterState.MOVE;
            yield return null;
        }
    }

    public virtual IEnumerator ActionCoroutine()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.NONE:
                    break;
                case MonsterState.IDLE:
                    break;
                case MonsterState.MOVE:
                    transform.position -= new Vector3(0f, 0f, speed * Time.deltaTime);
                    break;
                case MonsterState.RELOAD:
                    reloading += Time.deltaTime;
                    mobCommand.Images.fillAmount = reloading / reloadTime;
                    break;
                case MonsterState.ATTACK:
                    if (reloading >= reloadTime)
                    {
                        reloading = 0;
                        Player.instance.CombatOn = true;
                        Attack();
                    }
                    break;
                case MonsterState.DIE:
                    break;
            }
            yield return null;
        }
    }

    public virtual void Attack()
    {
        int index = Random.Range(0, attackLists.Count);
        AttackType selectedAttack = attackLists[index].attackType;
        float damage = attackLists[index].damage * (characterStat.strength * 0.1f);

        Player.instance.Damaged(selectedAttack, damage, characterStat.agility);
    }

    public virtual void Damaged()
    {
        if (Player.instance.CanAttack && Player.instance.Attacked == false)
        {
            Player.instance.CanAttack = false;
            Player.instance.Attacked = true;

            (AttackType attackType, float damage) = Player.instance.Attack();

            //데미지를 받을 부위를 랜덤으로 선택
            int pos = Random.Range(0, part.Count);

            if (part[pos].Aviod(Player.instance.CharacterStat.agility))
                CombatTextManager.instance.CreateText(transform.position, damage.ToString(), SCTTYPE.MISS, false);
            else
            {
                //부위 체력 감소
                float reduceDamage = part[pos].Damaged(attackType, damage);

                currentHp -= reduceDamage;
                //풀체력이 0일시 사망
                if (currentHp <= 0)
                    Die();

                CombatTextManager.instance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);
            }
        }
        else
            Player.instance.ShowIntroduce("아직 공격할 수 없다.");
    }

    public override void Die()
    {
        base.Die();
        mobCommand.Images.fillAmount = 1;
        lootTable.RollLoot();
        mobCommand.Die();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, 0, recognitionDistance * 0.5f), new Vector3(recognitionDistance, recognitionDistance, recognitionDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, 0, attackDistance * 0.5f), new Vector3(attackDistance, attackDistance, attackDistance));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, 0, interactionDistance * 0.5f), new Vector3(interactionDistance, interactionDistance, interactionDistance));
    }
}
