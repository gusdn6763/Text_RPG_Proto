using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerSocket : MonoBehaviour
{
    private RectTransform rectTransform;
    private Text text;
    private Armor currentArmor;

    private string defaultString;

    public Armor CurrentArmor { get => currentArmor; set => currentArmor = value; }

private void Awake()
    {
        rectTransform = GetComponentInChildren<RectTransform>();
        text = GetComponentInChildren<Text>();
        defaultString = text.text;
    }

    public void EquipItem(Armor armor)
    {
        UnEquipItem();
        currentArmor = armor;

        text.text = armor.CommandName;
        armor.transform.SetParent(transform);
        armor.RectTrans.anchoredPosition = Vector3.zero;
    }

    public void UnEquipItem()
    {
        if (currentArmor)
        {
            text.text = defaultString;
            currentArmor = null;
        }
    }
}
