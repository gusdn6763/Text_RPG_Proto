using System;
using UnityEngine;

public class Item : Command
{
    [SerializeField] private float weight = 1;
    [SerializeField] private int stackSize = 1;
    [SerializeField] private int sellingPrice = 0;

    private int price;
    private Area area;

    public float Weight { get { return weight; } }
    public int StackSize { get { return stackSize; } set { stackSize = value; } }
    public int Price { get { return price; } set { price = value; } }

    public Area CurrnetArea { get { return area; } 
        set
        {
            if (area)
                area.OnItemCountChanged(this);

            area = value; 
            area.OnItemCountChanged(this);
            area.ItemCommandChange(this);
        } 
    }

    protected override void Start()
    {
        base.Start();

        if (stackSize > 1)
            text.text = commandName + " : " + StackSize + " 개";
    }

    //버튼

    public void Buy()
    {
        if (Player.instance.Money > price)
        {
            if (InventoryManager.instance.SetItem(this, true))
            {
                Player.instance.Money -= price;
            }
        }
    }
    public void Show()
    {

    }
    public void Use()
    {
        Player.instance.SetStatus(status);
        Throw(1);
    }
    public void Get()
    {
        if (InventoryManager.instance.SetItem(this, true))
        {

        }
        else
            Player.instance.ShowIntroduce(Constant.full);
    }
    public void Throw(int count)
    {
        float defaultWeight = stackSize / weight;

        stackSize -= count;
        weight -= defaultWeight * count;

        text.text = commandName + " : " + StackSize + " 개";

        if (StackSize < 0)
            Debug.LogError("갯수가 마이너스됨");

        //아이템을 버리거나 사용시 해당공간의 특정 이벤트 실행 =>ex)인벤토리 사이즈, 퀘스트 갯수등
        if (CurrnetArea)
            CurrnetArea.OnItemCountChanged(this);

        if (StackSize <= 0)
            Destroy(this.gameObject);
    }
    public void Sell()
    {
        Player.instance.Money += sellingPrice;
    }

    public void MergeItems(Item item)
    {
        stackSize += item.StackSize;
        weight += item.Weight;
        text.text = commandName + " : " + StackSize + " 개";
        Destroy(item.gameObject);
    }

    //활성화 여부
    public virtual void GetItem()
    {
        CommandActiveAll(false);
        CommandActive(Constant.info, true);
        CommandActive(Constant.use, true);
        CommandActive(Constant.delete, true);
    }
    public void SellingItem()
    {
        CommandActiveAll(false);
        CommandActive(Constant.info, true);
        CommandActive(Constant.buy, true);
    }
    public virtual void BuyItem()
    {
        CommandActiveAll(false);
        CommandActive(Constant.info, true);
        CommandActive(Constant.use, true);
        CommandActive(Constant.delete, true);
        Throw(1);
    }
    public virtual void DropItem()
    {
        CommandActiveAll(false);
        CommandActive(Constant.info, true);
        CommandActive(Constant.use, true);
        CommandActive(Constant.get, true);
        CommandActive(Constant.delete, true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionDistance"))
        {
            ActiveOnOff(true);
        }
    }
}
