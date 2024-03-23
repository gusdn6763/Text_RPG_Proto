using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoUI : UIScript
{
    [SerializeField] private Text statusText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text locationText;
    [SerializeField] private Text timeText;
    [SerializeField] private IntroduceText messagePrefab;
    [SerializeField] private Transform messagePosition;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowStatus(Status status)
    {
        statusText.text = "건강 : " + status.hp + "\n" + "피로 : " + status.fatigue + "\n" + "허기 : " + status.hungry;

        timeText.text = TimeConverter.AddTime(status.time);
    }

    public void ShowNameRank(string name, string rank)
    {
        nameText.text = name + " / " + rank;
    }

    public void ShowLocation(string location)
    {
        locationText.text = location;
    }

    public void ShowIntroduce(string introduce)
    {
        IntroduceText go = Instantiate(messagePrefab, messagePosition);
        go.text.text = introduce;
    }
}
