using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상인의 아이템의 정보클래스 -> 아이템과 갯수, 가격, 무한인지 아닌지 체크
/// </summary>
[System.Serializable]
public class MerchantItem
{
    [SerializeField] private Item item;

    [SerializeField] private int count;
    [SerializeField] private int price;

    public Item MyItem { get { return item; } }

    public int Count { get { return count; } set { count = value; } }
    public int Price { get { return price; } }
}
