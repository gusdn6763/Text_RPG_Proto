using UnityEngine;

public class PlayerSockets : MonoBehaviour
{
    [SerializeField] private PlayerStatusUIText playerStatusUIText;
    [SerializeField] private PlayerSocket helmet, ear, neck, chest, gloves, belt, handeLeft, handRight,  pants, shose;

    public void EquipArmor(Armor armor)
    {
        switch (armor.GetArmorType)
        {
            case ArmorType.Helmet:
                helmet.EquipItem(armor);
                break;
            case ArmorType.Ear:
                ear.EquipItem(armor);
                break;
            case ArmorType.Neck:
                neck.EquipItem(armor);
                break;
            case ArmorType.Chest:
                chest.EquipItem(armor);
                break;
            case ArmorType.HandLeft:
                handeLeft.EquipItem(armor);
                break;
            case ArmorType.HandRight:
                handRight.EquipItem(armor);
                break;
            case ArmorType.HandBoth:
                handRight.EquipItem(armor);
                break;
            case ArmorType.Gloves:
                gloves.EquipItem(armor);
                break;
            case ArmorType.Pants:
                pants.EquipItem(armor);
                break;
            case ArmorType.Shose:
                shose.EquipItem(armor);
                break;
            case ArmorType.Belt:
                belt.EquipItem(armor);
                break;
        }
        playerStatusUIText.ShowStatus();
    }

    public void UnEquipArmor(Armor armor)
    {
        switch (armor.GetArmorType)
        {
            case ArmorType.Helmet:
                helmet.UnEquipItem();
                break;
            case ArmorType.Ear:
                ear.UnEquipItem();
                break;
            case ArmorType.Neck:
                neck.UnEquipItem();
                break;
            case ArmorType.Chest:
                chest.UnEquipItem();
                break;
            case ArmorType.HandLeft:
                handeLeft.UnEquipItem();
                break;
            case ArmorType.HandRight:
                handRight.UnEquipItem();
                break;
            case ArmorType.HandBoth:
                handeLeft.UnEquipItem();
                handRight.UnEquipItem();
                break;
            case ArmorType.Gloves:
                gloves.UnEquipItem();
                break;
            case ArmorType.Pants:
                pants.UnEquipItem();
                break;
            case ArmorType.Shose:
                shose.UnEquipItem();
                break;
            case ArmorType.Belt:
                belt.UnEquipItem();
                break;
        }
        playerStatusUIText.ShowStatus();
    }

    public Armor HaveArmor(Armor armor)
    {
        switch (armor.GetArmorType)
        {
            case ArmorType.Helmet:
                return helmet.CurrentArmor;
            case ArmorType.Ear:
                return ear.CurrentArmor;
            case ArmorType.Neck:
                return neck.CurrentArmor;
            case ArmorType.Chest:
                return chest.CurrentArmor;
            case ArmorType.HandLeft:
                return handeLeft.CurrentArmor;
            case ArmorType.HandRight:
                return handRight.CurrentArmor;
            case ArmorType.Gloves:
                return gloves.CurrentArmor;
            case ArmorType.Pants:
                return pants.CurrentArmor;
            case ArmorType.Shose:
                return shose.CurrentArmor;
            case ArmorType.Belt:
                return belt.CurrentArmor;
        }
        Debug.LogError("없는 소켓존재 or 잘못된 참조");
        return null;
    }

    public Armor HaveArmor(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Helmet:
                return helmet.CurrentArmor;
            case ArmorType.Ear:
                return ear.CurrentArmor;
            case ArmorType.Neck:
                return neck.CurrentArmor;
            case ArmorType.Chest:
                return chest.CurrentArmor;
            case ArmorType.HandLeft:
                return handeLeft.CurrentArmor;
            case ArmorType.HandRight:
                return handRight.CurrentArmor;
            case ArmorType.HandBoth:
                return handRight.CurrentArmor;
            case ArmorType.Gloves:
                return gloves.CurrentArmor;
            case ArmorType.Pants:
                return pants.CurrentArmor;
            case ArmorType.Shose:
                return shose.CurrentArmor;
            case ArmorType.Belt:
                return belt.CurrentArmor;
        }
        Debug.LogError("없는 소켓존재");
        return null;
    }

    public float GetWeight()
    {
        float weight = 0;

        if (helmet.CurrentArmor)
            weight += helmet.CurrentArmor.Weight;
        if (ear.CurrentArmor)
            weight += ear.CurrentArmor.Weight;
        if (neck.CurrentArmor)
            weight += neck.CurrentArmor.Weight;
        if (chest.CurrentArmor)
            weight += chest.CurrentArmor.Weight;
        if (handeLeft.CurrentArmor)
            weight += handeLeft.CurrentArmor.Weight;
        if (handRight.CurrentArmor)
            weight += handRight.CurrentArmor.Weight;
        if (gloves.CurrentArmor)
            weight += gloves.CurrentArmor.Weight;
        if (pants.CurrentArmor)
            weight += pants.CurrentArmor.Weight;
        if (shose.CurrentArmor)
            weight += shose.CurrentArmor.Weight;
        if (belt.CurrentArmor)
            weight += belt.CurrentArmor.Weight;

        return weight;
    }
}