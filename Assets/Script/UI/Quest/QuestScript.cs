using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button doingButton;
    [SerializeField] private Button completeButton;

    private Quest quest;

    public Quest MyQuest
    {
        get
        {
            return quest;
        }
        set
        {
            quest = value;
        } 
    }

    //텍스트 갱신
    public void UpdateText(float time = 0f)
    {
        quest.Time -= time;
        if (quest.Time <= 0)
        {
            Player.instance.ShowIntroduce("임무 실패");
            QuestlogUI.instance.RemoveQuest(this);
            return ;
        }

        string objectives = string.Empty;
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.CommandName + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
        }
        foreach (Objective obj in quest.MyKillObjectives)
        {
            objectives += obj.CommandName + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
        }

        descriptionText.text = string.Format("<b>{0}</b>\n\n<size=20><b>내용</b>\n{1}\n\n<b>목표</b>\n{2}\n<b>조건</b>\n{3}<b>랭크</b>\n\n<b>남은 시간</b>\n{4}\n\n<b>보상</b>\n돈 {5},공적치 {6}</size>\n",
            quest.Title, quest.Description, objectives, quest.Rank, TimeConverter.CalculateTime(quest.Time), quest.Money, quest.Contribution);
    
    }
    //퀘스트 수락
    public void AcceptQuest()
    {
        if (QuestlogUI.instance.AcceptQuest(this))
        {
            acceptButton.gameObject.SetActive(false);
            if (MyQuest.IsComplete)
                completeButton.gameObject.SetActive(true);
            else
                doingButton.gameObject.SetActive(true);
        }
        else
            Player.instance.ShowIntroduce("제한 갯수 초과 or 등급 부족");
    }

    //퀘스트 포기
    public void GiveupQuest()
    {
        QuestlogUI.instance.GiveUpQuest(this);
        acceptButton.gameObject.SetActive(true);
        doingButton.gameObject.SetActive(false);
        completeButton.gameObject.SetActive(false);
    }

    //퀘스트 완료
    public void CompleteQuest()
    {
        Player.instance.SetReward(MyQuest.Contribution, MyQuest.Money);
        QuestlogUI.instance.CompleteQuest(this);
    }
}
