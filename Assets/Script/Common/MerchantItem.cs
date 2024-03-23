using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �������� ����Ŭ���� -> �����۰� ����, ����, �������� �ƴ��� üũ
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
