using UnityEngine;
using UnityEngine.UI;

public class Chest : Area
{
    [SerializeField] protected Text weightText;
    [SerializeField] protected float fullWeight = 100f;
    [SerializeField] protected float currentWeight;

    public override bool SetItem(Item item, bool random = true)
    {
        if (currentWeight + item.Weight > fullWeight)
            Player.instance.ShowIntroduce(Constant.fullWeight);

        return base.SetItem(item, random);
    }

    public override void OnItemCountChanged(Item item)
    {
        base.OnItemCountChanged(item);
        CaculateWeight();
    }

    public void CaculateWeight()
    {
        currentWeight = 0;
        foreach (Transform child in transform)
        {
            Item tmp = child.GetComponent<Item>();
            if (tmp)
                currentWeight += tmp.Weight;
        }

        weightText.text = currentWeight + " / " + fullWeight;
    }
}
