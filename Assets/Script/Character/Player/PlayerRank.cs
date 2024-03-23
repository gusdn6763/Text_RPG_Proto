using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rank
{
    public string rankName;
    public int rank;
    public int requestContribution;
}


public class PlayerRank : MonoBehaviour
{
    [SerializeField] private List<Rank> rankList = new List<Rank>();
    [SerializeField] private int currentContribution = 0;

    public void SetContribution(int contributionScore)
    {
        currentContribution += contributionScore;
    }

    public string SetRankName()
    {
        foreach (var rank in rankList)
        {
            if (currentContribution >= rank.requestContribution)
            {
                return rank.rankName;
            }
        }
        return Constant.defaultRank;
    }

    public int SetRank()
    {
        foreach (var rank in rankList)
        {
            if (currentContribution >= rank.requestContribution)
            {
                return rank.rank;
            }
        }
        return 99;
    }
}