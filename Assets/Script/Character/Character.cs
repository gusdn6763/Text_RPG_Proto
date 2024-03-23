using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("�����κ�")]
    [SerializeField] public List<Part> part = new List<Part>();
    [SerializeField] public List<Part> activePart = new List<Part>();
    [SerializeField] protected Stat characterStat;
    [SerializeField] protected float recognitionDistance;   //�νİŸ�
    [SerializeField] protected float attackDistance;        //���ݰŸ�
    [SerializeField] protected float interactionDistance;   //��ȣ�ۿ�
    [SerializeField] protected float fullHp;
    [SerializeField] protected float Accuracy;              //���߷�

    protected List<StatusEffect> activeEffects = new List<StatusEffect>();  //�����

    [Header("Ȯ�ο�-���迹��")]
    public float reloading;         //���� �غ�ð� ����
    public float reloadTime;         //���� �غ�ð�
    public float currentHp;
    public float speed;
    public bool isDie = false;

    public Stat CharacterStat { get { return characterStat; } }

    protected virtual void Awake()
    {
        StatToData();
    }

    protected virtual void Start()
    {
        for (int i = 0; i < part.Count; i++)
        {
            part[i].brokenPart += BrokenPart;
            part[i].ParentCharacter = this;
            part[i].CurrentHp = part[i].fullHp;
        }
        activePart = part;
    }

    public virtual void StatToData()
    {
        float partHP = 0;
        for (int i = 0; i < part.Count; i++)
            partHP += part[i].fullHp;

        fullHp = currentHp = (characterStat.health * 10) + partHP;
        speed = characterStat.agility * 10;
        reloadTime = GameManager.instance.actionTime / characterStat.agility;
    }

    public virtual void BrokenPart(Part part, float damage)
    {
        switch (part.partType)
        {
            case PartType.Head:
                Die();
                break;
            case PartType.Chest:
                Die();
                break;
            case PartType.LeftArm:
                attackDistance /= attackDistance;
                break;
            case PartType.RightArm:
                attackDistance /= attackDistance;
                break;
            case PartType.LeftLeg:
                speed /= speed;
                break;
            case PartType.RightLeg:
                speed /= speed;
                break;
        }
    }
    public virtual void Die()
    {
        isDie = true;
        StopAllCoroutines();
    }

    //���� ����
    public void ApplyBuff(StatusEffect effect)
    {
        effect.ApplyEffect();
        activeEffects.Add(effect);
        StartCoroutine(RemoveEffectAfterDuration(effect));
    }
    private IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        yield return new WaitForSeconds(effect.Duration);
        effect.RemoveEffect();
        activeEffects.Remove(effect);
    }
    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in activeEffects)
        {
            effect.RemoveEffect();
        }
        activeEffects.Clear();
    }
}
