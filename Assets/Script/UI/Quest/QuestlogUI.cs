using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

//플레이어 퀘스트 UI  
public class QuestlogUI : UIScript
{
    public static QuestlogUI instance;

    [SerializeField] private List<Quest> questList = new List<Quest>(); //총 퀘스트 갯수

    [SerializeField] private QuestScript questPrefab;
    [SerializeField] private Transform questParent;
    [SerializeField] private Text questCountTxt;                        //현재 받은 퀘스트 텍스트
    [SerializeField] private int maxCount;                              //받을 수 있는 퀘스트 최대 갯수

    [SerializeField] List<QuestScript> acceptQuests = new List<QuestScript>();   //플레이어가 수락한 퀘스트들
    [SerializeField] private List<QuestScript> showQuests = new List<QuestScript>();     //플레이어가 수락하지 않은 남은 퀘스트들

    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        QuestRenewal();
    }

    public void QuestRenewal()
    {
        for(int j = 0; j < showQuests.Count; j++)
            Destroy(showQuests[j].gameObject);

        showQuests.Clear();
        for (int i = 0; i < questList.Count; i++)
        {
            QuestScript qs = Instantiate(questPrefab, questParent);
            qs.MyQuest = new Quest(questList[i]);
            qs.UpdateText();
            showQuests.Add(qs);
        }
    }

    public bool AcceptQuest(QuestScript questScript)
    {
        //갯수 제한 + 플레이어 랭크 제한
        if (acceptQuests.Count < maxCount && questScript.MyQuest.Rank >= Player.instance.Rank)
        {
            showQuests.Remove(questScript);

            acceptQuests.Add(questScript);
            questCountTxt.text = acceptQuests.Count + "/" + maxCount;
            //itemCountChangedEvent는 이벤트 함수로 아이템을 얻을때 실행한다.
            //아이템을 얻을시 퀘스트에 필요한 아이템이면 갯수를 증가시키기 위함    
            foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
            {
                InventoryManager.instance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                //퀘스트를 수락 후 인벤토리에 아이템이 있는지 체크  
                o.UpdateItemCount();   
            }

            foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
                GameManager.instance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);

            return true;
        }

        return false;
    }

    public void GiveUpQuest(QuestScript questScript)
    {
        if (acceptQuests.Contains(questScript))
        {
            foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
                InventoryManager.instance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);

            foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
                GameManager.instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);

            RemoveQuest(questScript);
        }
        else
            Player.instance.ShowIntroduce(Constant.notAcceptQuest);
    }

    public void CompleteQuest(QuestScript questScript)
    {
        if (acceptQuests.Contains(questScript))
        {
            foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
            {
                InventoryManager.instance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete();
            }

            foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
                GameManager.instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);

            RemoveQuest(questScript);
        }
        else
            Debug.LogError("수락하지 않은 퀘스트인데 퀘스트를 완료함");
    }

    //퀘스트 포기 or 완료
    public void RemoveQuest(QuestScript qs)
    {
        acceptQuests.Remove(qs);
        questCountTxt.text = acceptQuests.Count + "/" + maxCount;

        Destroy(qs.gameObject);
    }

    public void UpdateSelected(float time = 0)
    {
        for (int i = acceptQuests.Count - 1; i >= 0; i--)
            acceptQuests[i].UpdateText(time);
    }

    //퀘스트들이 완료됬는지 하나씩 확인한다.  
    public void CheckCompletion()
    {
        foreach (QuestScript qs in acceptQuests)
        {
            if (qs.MyQuest.IsComplete)
                Player.instance.ShowIntroduce(string.Format("{0} (퀘스트 완료)", qs.MyQuest.Title));
        }
    }
}
