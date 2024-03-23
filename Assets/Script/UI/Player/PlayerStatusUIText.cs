using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIText : MonoBehaviour
{
    [SerializeField] private Text totalHpTetx;
    [SerializeField] private Text hungryText;
    [SerializeField] private Text fatigueText;
    [SerializeField] private Text HealthText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text intellectText;
    [SerializeField] private Text agilityText;

    private void Start()
    {
        ShowStatus();
    }

    public void ShowStatus()
    {
        totalHpTetx.text = Constant.totalHp + Player.instance.Hp;
        hungryText.text = Constant.hungry + Player.instance.Hungry;
        fatigueText.text = Constant.fatigue + Player.instance.Fatigue;
        HealthText.text = Constant.health + Player.instance.CharacterStat.health;
        strengthText.text = Constant.strength + Player.instance.CharacterStat.strength;
        intellectText.text = Constant.intellect + Player.instance.CharacterStat.intellect;
        agilityText.text = Constant.agility + Player.instance.CharacterStat.agility;
    }
}
