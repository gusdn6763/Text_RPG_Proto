using System.Collections.Generic;
using UnityEngine;


public class MoveCommand : Command
{
    [SerializeField] private bool found = false;
    [SerializeField] private List<Command> activeList;

    public override bool IsOn
    {
        get { return isOn; } 
        set
        {
            if (found == false)
                return ;

            isOn = value;

            gameObject.SetActive(isOn);
        }
    }

    public bool Found { get { return found; } set { found = value; isOn = true; } }
    public override void Excute()
    {
        base.Excute();

        IsOn = false;

        for (int i = 0; i < activeList.Count; i++)
            activeList[i].IsOn = true;
    }
}
