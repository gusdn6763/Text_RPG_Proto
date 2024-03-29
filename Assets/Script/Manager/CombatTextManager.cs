﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE { DAMAGE, HEAL, MISS }

public class CombatTextManager : MonoBehaviour
{
    public static CombatTextManager instance;

    [SerializeField] private GameObject combatTextPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 텍스트를 생성하는 함수
    /// </summary>
    /// <param name="position">생성할 위치</param>
    /// <param name="text">텍스트</param>
    /// <param name="type">어떠한 타입의 텍스트인지</param>
    /// <param name="crit">크리일경우 빨간색</param>
    public void CreateText(Vector3 position, string text, SCTTYPE type, bool crit)
    {
        //플레이어 위치에 알맞게 생성
        position.y += 10f;
        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;

        //숫자 앞의 부호표시
        string before = string.Empty;
        string after = string.Empty;
        switch (type)
        {
            case SCTTYPE.DAMAGE:
                before = "-";
                sct.color = Color.red;
                break;
            case SCTTYPE.HEAL:
                before = "+";
                sct.color = Color.green;
                break;
            case SCTTYPE.MISS:
                before = "MISS";
                sct.color = Color.yellow;
                break;
        }

        sct.text = before + text + after;

        //크리티컬일시
        if (crit)
        {
            //sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
