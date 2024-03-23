using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Command
{
    [SerializeField] private List<MerchantItem> items = new List<MerchantItem>();

    public List<MerchantItem> Items { get { return items; } set { items = value; } }

    public void Talk()
    {
        Player.instance.ShowIntroduce("대화한다");
    }

    public void SetItemList(StoreUI storeUI)
    {
        storeUI.SetItemList(this);
    }
}
