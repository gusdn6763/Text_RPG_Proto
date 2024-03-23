using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCommand : Command
{
    [SerializeField] private List<Item> giveItems = new List<Item>();

    public void Give()
    {
        IsOn = false;
        FadeManager.instance.FadeInImmediately(Constant.guildFirst);
        for(int i = 0; i < giveItems.Count; i++)
        {
            Item item = Instantiate(giveItems[i]);
            InventoryManager.instance.SetItem(item);
        }
        Player.instance.SetReward(1, 0);
    }
}
