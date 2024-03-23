using UnityEngine;

public delegate void ItemCountChanged(Item item);
public class Area : MonoBehaviour
{
    protected RectTransform rectTransform;

    public event ItemCountChanged itemCountChangedEvent;

    public RectTransform RectTrans { get => rectTransform; set => rectTransform = value; }

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual bool SetItem(Item item, bool random = true)
    {
        Item existingItem = GetItem(item.CommandName);

        // 이미 아이템이 있으며, 새 아이템이 아닐시
        if (existingItem && existingItem != item)
        {
            if (existingItem is Armor)
                SetNewItem(item, random);
            else
            {
                existingItem.MergeItems(item);
                existingItem.CurrnetArea = this;
            }
        }
        else // 기존 공간에 아이템이 없으면
            SetNewItem(item, random);

        return true;
    }
    private void SetNewItem(Item item, bool random)
    {
        item.RectTrans.SetParent(rectTransform);
        if (random)
        {
            float minY = -rectTransform.rect.height / 2;
            float maxX = rectTransform.rect.width;
            float maxY = rectTransform.rect.height / 2;

            float randomX = Random.Range(0, maxX);
            float randomY = Random.Range(minY, maxY);
            item.RectTrans.anchoredPosition = new Vector2(randomX, randomY);
            Debug.Log(randomX + "" + randomY);
        }
        item.CurrnetArea = this;
    }
    public Item GetItem(string type)
    {
        // 자식 오브젝트들을 검색
        foreach (Transform child in transform)
        {
            Item item = child.GetComponent<Item>();
            if (item && string.IsNullOrEmpty(item.CommandName) == false)
            {
                if (item.CommandName == type)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public int GetItemCount(string type)
    {
        Item item = GetItem(type);

        if (item)
            return item.StackSize;

        return 0;
    }

    public virtual void OnItemCountChanged(Item item)
    {        
        if (itemCountChangedEvent != null)
            itemCountChangedEvent(item);
    }

    public virtual void ItemCommandChange(Item item)
    {
        item.DropItem();
    }
}