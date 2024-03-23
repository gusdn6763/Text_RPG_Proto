using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Quest
{
    public Quest(Quest quest)
    {
        title = quest.title;
        descripiton = quest.descripiton;
        time = quest.time;
        rank = quest.rank;
        money = quest.money;
        contribution = quest.contribution;
        collectObjectives = quest.collectObjectives?.Select(co => new CollectObjective
        {
            Amount = co.Amount,
            CommandName = co.CommandName,
            CurrentAmount = 0
        }).ToArray();

        killObjectives = quest.killObjectives?.Select(ko => new KillObjective
        {
            Amount = ko.Amount,
            CommandName = ko.CommandName,
            CurrentAmount = 0
        }).ToArray();
    }


    [SerializeField] private string title;                          //이름
    [SerializeField] private string descripiton;                    //상세설명
    [SerializeField] private CollectObjective[] collectObjectives;  //아이템 수집
    [SerializeField] private KillObjective[] killObjectives;        //몹죽이기
    [SerializeField] private float time;                            //기간
    [SerializeField] private int rank;                              //수락 조건
    [SerializeField] private int money;                             //돈
    [SerializeField] private int contribution;                      //공적치

    public string Title { get => title; set => title = value; }
    public string Description { get => descripiton; set => descripiton = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }
    public float Time { get => time; set => time = value; }
    public int Money { get => money; set => money = value; }
    public int Rank { get => rank; set => rank = value; }
    public int Contribution { get => contribution; set => contribution = value; }

    //퀘스트에서 요구하는 아이템들을 체크함
    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

//퀘스트에서 요구하는 아이템
[System.Serializable]
public abstract class Objective
{
    [SerializeField] private int amount;

    private int currentAmount;

    [SerializeField] private string commandName;

    public int Amount { get => amount; set => amount = value; }
    public int CurrentAmount { get => currentAmount; set => currentAmount = value; }

    public string CommandName { get => commandName; set => commandName = value; }

    //퀘스트를 완료하기에 알맞은 양인지 확인
    public bool IsComplete
    {
        get
        {
            return CurrentAmount >= Amount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    //아이템을 얻을때마다 요구하는 아이템의 수량이 변경되어있는지 체크한다. 
    public void UpdateItemCount(Item item)
    {
        string itemName = item.CommandName;

        if (CommandName.ToLower() == itemName.ToLower())
        {
            CurrentAmount = InventoryManager.instance.GetItemCount(CommandName);

            if (CurrentAmount <= Amount)
                Player.instance.ShowIntroduce(string.Format("{0}: {1}/{2}", CommandName, CurrentAmount, Amount));

            QuestlogUI.instance.UpdateSelected();        //퀘스트 텍스트 갱신
            QuestlogUI.instance.CheckCompletion();        //퀘스트 완료 여부 체크
        }
    }

    public void UpdateItemCount()
    {
        CurrentAmount = InventoryManager.instance.GetItemCount(CommandName);
        Player.instance.ShowIntroduce(string.Format("{0}: {1}/{2}", CommandName, CurrentAmount, Amount));

        QuestlogUI.instance.UpdateSelected();        //퀘스트 텍스트 갱신
        QuestlogUI.instance.CheckCompletion();        //퀘스트 완료 여부 체크
    }

    //퀘스트 완료시 아이템 삭제
    public void Complete()
    {
        Item item = InventoryManager.instance.GetItem(CommandName);

        item.Throw(Amount);
    }
}


[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(MobCommand character)
    {
        if (CommandName == character.CommandName)
        {
            if (CurrentAmount < Amount)
            {
                CurrentAmount++;
                Player.instance.ShowIntroduce(string.Format("{0}: {1}/{2}", character.CommandName, CurrentAmount, Amount));

                QuestlogUI.instance.UpdateSelected();         //퀘스트 텍스트 갱신
                QuestlogUI.instance.CheckCompletion();        //퀘스트 완료 여부 체크
            }
        }
    }
}


