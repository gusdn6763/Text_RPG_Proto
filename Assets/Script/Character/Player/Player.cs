using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    public static Player instance;

    [Header("플레이어UI")]
    private BoxCollider weaponCol;
    [SerializeField] private BoxCollider InteractionCol;
    [SerializeField] private PlayerSockets playerSockets;
    [SerializeField] private DungeonUI dungeonUI;
    [SerializeField] private InfoUI info;

    [Header("플레이어 전투")]
    [SerializeField] private float runValue = 50f;

    [Header("플레이어")]
    [SerializeField] private float hp;
    [SerializeField] private float fatigue;
    [SerializeField] private float hungry;
    [SerializeField] private int money;
    [SerializeField] private string playerName;

    private PlayerRank rank;

    private string currnetLocation;
    private float waitTime = 0;
    private bool isMove = false;
    private bool combatOn = false;
    private bool canAttack = false;
    private bool attacked = false;

    [Header("수면 디버프-테스트용")]
    public float sleep;
    public float debuff2;
    public float debuff3;
    public Weapon debugWeapon;

    public float Hp { get { return hp; } 
        set 
        {
            hp = value;
            if (hp > 100)
                hp = 100;
            info.ShowStatus(GetStatus());
        } 
    }
    public float Fatigue
    {
        get { return fatigue; }
        set
        {
            fatigue = value;
            if (fatigue > 100)
                fatigue = 100;

            if (fatigue <= sleep)
            {
                info.ShowIntroduce("8시간 수면");
                fullHp = 100;
                Fatigue = 100;
                Hungry -= 50;
                info.ShowStatus(GetStatus(480));
            }
            else if (fatigue <= debuff2)
            {
                StatusEffect effect = new StatusBuff(999, this, new Status(0, 0, 0, 0));
                ApplyBuff(effect);
            }
            else if (fatigue <= debuff3)
            {
                StatusEffect effect = new StatusBuff(999, this, new Status(0, 0, 0, 0));
                ApplyBuff(effect);
            }
            info.ShowStatus(GetStatus());
        }
    }
    public float Hungry
    {
        get { return hungry; }
        set
        {
            hungry = value;
            if (hungry > 100)
                hungry = 100;
            info.ShowStatus(GetStatus());
        }
    }
    public int Rank { get { return rank.SetRank(); } }
    public string CurrentLocation
    {
        get
        {
            if (string.IsNullOrEmpty(currnetLocation))
                return Constant.room;
            else
                return currnetLocation;
        }
        set
        {
            currnetLocation = value;
            info.ShowLocation(currnetLocation);
            GameManager.instance.MoveEvent();
        }
    }
    public bool CombatOn
    {
        get { return combatOn; }
        set
        {
            if (combatOn != value)
            {
                waitTime = 0;
                reloading = 0;
                CanAttack = false;
                attacked = false;

                if (combatOn)
                    dungeonUI.CombatEndUi();
                else
                    dungeonUI.CombatStartUi();
            }
            combatOn = value;
        }
    }
    public float RecognitionDistance { get { return recognitionDistance; } }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    public bool Attacked { get { return attacked; } set { attacked = value; } }
    public int Money { get { return money; } set { money = value; } }

    protected override void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        base.Awake();

        rank = GetComponent<PlayerRank>();
        weaponCol = GetComponent<BoxCollider>();
    }

    protected override void Start()
    {
        base.Start();
        ResetDistance();
        info.ShowStatus(GetStatus());
        info.ShowNameRank(playerName, rank.SetRankName());

        dungeonUI.StatusUpdate();
    }

    public void Update()
    {
        if (GameManager.instance.dungeonEnter)
        {
            if (Input.GetKeyDown(KeyCode.W))
                isMove = true;
            if (Input.GetKeyDown(KeyCode.S))
                isMove = false;

            if (isMove)
                transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);

            if (CombatOn)
            {
                if (reloadTime >= waitTime)       //장전
                {
                    CanAttack = false;
                    waitTime += Time.deltaTime;
                    dungeonUI.AttackWait = waitTime / reloadTime;
                }
                else if (reloadTime >= reloading)       //장전
                {
                    CanAttack = true;
                    reloading += Time.deltaTime;
                    dungeonUI.Reloading = reloading / reloadTime;
                }
                else if (Attacked == false && CanAttack && reloading >= waitTime)
                {
                    reloading = 0;
                    CanAttack = false;
                    ShowIntroduce("턴을 놓쳤다");
                }
                else
                {
                    reloading = 0;
                    Attacked = false;
                }
            }
        }
    }

    public (AttackType, float) Attack()
    {
        Attacked = true;

        Armor armor = playerSockets.HaveArmor(ArmorType.HandRight);
        if (armor is Weapon)
        {
            (AttackType attackType, float baseDamage) = ((Weapon)armor).Attack();

            float modifiedDamage = baseDamage * (characterStat.strength * 0.1f);

            return (attackType, modifiedDamage);
        }
        else
        {
            float modifiedDamage = 1 * (characterStat.strength * 0.1f);

            return (AttackType.Hit, modifiedDamage);
        }
    }
    public void Damaged(AttackType attackType, float damage = 0, float agility = 0)
    {
        CombatOn = true;
        // 데미지를 받을 부위를 랜덤으로 선택
        int pos = Random.Range(0, activePart.Count);

        if (activePart[pos].Aviod(agility))
            ShowIntroduce("회피 했다");
        else
        {
            // 부위 체력 감소
            float reduceDamage = part[pos].Damaged(attackType, damage);
            currentHp -= reduceDamage;
            // 풀체력이 0일시 사망
            if (currentHp <= 0)
                Die();

            dungeonUI.StatusUpdate();
            info.ShowStatus(GetStatus());
            ShowIntroduce(reduceDamage + "의 데미지를 받았다.");
        }
    }
    public override void Die()
    {
        base.Die();
        FadeManager.instance.FadeIn(Constant.die, float.MaxValue);
    }
    public void Run()
    {
        if (CanAttack)
        {
            if (Random.Range(0, 100) < runValue + ((characterStat.agility - FindMoustAgility()) * 10))
            {
                GameManager.instance.mobSpawner.ResetAllObj();
                monsters.Clear();
                CombatOn = false;
            }
            else
            {
                CanAttack = false;
            }
        }
        else
        {
            ShowIntroduce(Constant.notRun);
        }
    }

    public float FindMoustAgility()
    {
        float max = 0;
        for (int i = 0; i < monsters.Count; i++)
            max = Mathf.Max(monsters[i].GetComponent<Monster>().CharacterStat.agility, max);
        return max;
    }

    //던전 관련 부분
    public void PlayerEnter()
    {
        CombatOn = false;
        dungeonUI.CombatEndUi();
        dungeonUI.StatusUpdate();
        transform.position = Vector3.zero;
    }
    public void PlayerExit()
    {
        CombatOn = false;
        dungeonUI.CombatEndUi();
    }

    //퀘스트 부분
    public void SetReward(int Contribution, int money)
    {
        Money += money;
        rank.SetContribution(Contribution);
        info.ShowNameRank(playerName, rank.SetRankName());
    }

    //UI 및 상태바 부분
    public void SetStatus(Status status, bool min = false)
    {
        Debug.Log("건강 : " + status.hp + "피로 : " + status.fatigue + "허기 : " + status.hungry + "시간 : " + status.time);
        if (min)
        {
            if (Hp - status.hp <= 0)
                Hp = 0;
            if (Fatigue - status.fatigue <= 0)
                Fatigue = 0;
            if (Hungry - status.hungry <= 0)
                Hungry = 0;
        }
        else
        {
            Hp += status.hp;
            Fatigue += status.fatigue;
            Hungry += status.hungry;
        }
        info.ShowStatus(GetStatus(status.time));
    }
    public Status GetStatus(float time = 0)
    {
        return new Status(hp, fatigue, hungry, time);
    }
    public void ShowIntroduce(string introduce)
    {
        info.ShowIntroduce(introduce);
    }

    //장비 부분
    public bool Equip(Armor armor)
    {
        if (characterStat > armor.RequestStat)
        {
            Armor equipArmor = playerSockets.HaveArmor(armor);
            if (equipArmor)
                UnEquip(equipArmor);

            if (armor is Weapon)
            {
                Weapon weapon = armor as Weapon;
                attackDistance = weapon.WeaponDistance;
                ResetDistance();

                if (armor.GetArmorType == ArmorType.HandBoth)
                {
                    Armor equipLeft = playerSockets.HaveArmor(ArmorType.HandLeft);
                    if (equipLeft)
                        UnEquip(equipLeft);
                }
            }
            else if (armor is Armor)
            {
                for(int i = 0; i < part.Count; i++)
                {
                    if (part[i].armorType == armor.GetArmorType)
                        part[i].PartArmor = armor;
                }
            }
            characterStat += armor.ArmorStat;
            playerSockets.EquipArmor(armor);
            return true;
        }
        else
            ShowIntroduce("장비 요구치 부족");

        return false;
    }
    public bool UnEquip(Armor armor)
    {
        if (armor is Weapon)
        {
            attackDistance = 100;
            ResetDistance();
        }
        else if (armor is Armor)
        {
            for (int i = 0; i < part.Count; i++)
            {
                if (part[i].armorType == armor.GetArmorType)
                    part[i].PartArmor = null;
            }
        }
        characterStat -= armor.ArmorStat;
        playerSockets.UnEquipArmor(armor);
        InventoryManager.instance.SetItem(armor, true);

        return true;
    }
    public void Test()
    {
        Equip(debugWeapon);
    }
    public void ResetDistance()
    {
        InteractionCol.center = new Vector3(0, 0, interactionDistance * 0.5f);
        InteractionCol.size = new Vector3(2000, 300, interactionDistance);

        weaponCol.center = new Vector3(0, 0, attackDistance * 0.5f);
        weaponCol.size = new Vector3(2000, 300, attackDistance);
    }
    public float GetWeight()
    {
        return playerSockets.GetWeight();
    }

    public List<Collider> monsters = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MobCommand>().ActiveOnOff(true);

            if (monsters.Contains(other) == false)
                monsters.Add(other);
            CombatOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MobCommand>().ActiveOnOff(false);

            if (monsters.Contains(other))
                monsters.Remove(other);

            if (monsters.Count == 0)
                CombatOn = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, recognitionDistance * 0.5f), new Vector3(2000, 300, recognitionDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, attackDistance * 0.5f), new Vector3(200, 300, attackDistance));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, interactionDistance * 0.5f), new Vector3(200, 300, interactionDistance));
    }
}
