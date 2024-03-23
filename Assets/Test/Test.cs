using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Status[] status = new Status[7];
    public void one()
    {
        Player.instance.SetStatus(status[0]);
    }
    public void two()
    {
        Player.instance.SetStatus(status[1]);
    }
    public void three()
    {
        Player.instance.SetStatus(status[2]);
    }
    public void four()
    {
        Player.instance.SetStatus(status[3]);
    }
    public void five()
    {
        Player.instance.SetStatus(status[4]);
    }
    public void six()
    {
        Player.instance.SetStatus(status[5]);
    }

    public void seven()
    {
        Player.instance.SetStatus(status[6]);
    }

    public void eight()
    {
        Player.instance.Money += 10;
    }

    public void nine()
    {
        Player.instance.currentHp += 10;
    }
}
