using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : UIScript
{
    [SerializeField] private Command waist;
    [SerializeField] private Command endDungeon;
    [SerializeField] private Command run;
    [SerializeField] private Image reloadingImage;
    [SerializeField] private Image attackWaitingImage;
    [SerializeField] private Text headText;
    [SerializeField] private Text chestText;
    [SerializeField] private Text leftArmText;
    [SerializeField] private Text rightArmText;
    [SerializeField] private Text leffLegText;
    [SerializeField] private Text rightLegText;

    public float Reloading { set { reloadingImage.fillAmount = value; } }
    public float AttackWait { set { attackWaitingImage.fillAmount = value; } }

    public void CombatStartUi()
    {
        waist.IsOn = true;
        endDungeon.IsOn = false;
        run.IsOn = true;
    }

    public void CombatEndUi()
    {
        Reloading = 0;
        AttackWait = 0;

        waist.IsOn = false;
        endDungeon.IsOn = true;
        run.IsOn = false;
    }

    public void StatusUpdate()
    {
        int count = Player.instance.part.Count;
        for (int i = 0; i < count; i++)
        {
            Part part = Player.instance.part[i];

            switch (part.partType)
            {
                case PartType.Head:
                    headText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
                case PartType.Chest:
                    chestText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
                case PartType.LeftArm:
                    leftArmText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
                case PartType.RightArm:
                    rightArmText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
                case PartType.LeftLeg:
                    leffLegText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
                case PartType.RightLeg:
                    rightLegText.text = part.CurrentHp + "/" + part.fullHp;
                    break;
            }
        }
    }
}
