using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCommandParent : Command
{
    private LocationList locationList;

    protected override void Awake()
    {
        base.Awake();
        locationList = GetComponent<LocationList>();
    }

    public void MoveLocation(string locationName)
    {
        Player.instance.CurrentLocation = locationName;

        foreach (Command childCommand in allChildCommands)
            if (locationName == childCommand.CommandName)
                Active(childCommand);
    }

    public virtual void Active(Command command)
    {
        //주의 MoveCommand IsOn은 override
        foreach (Command childCommand in allChildCommands)
            childCommand.IsOn = true;

        Player.instance.CurrentLocation = command.CommandName;

        command.Excute();
        ReCaculateStatus();
    }

    public bool FindLocation(string locationName)
    {
        foreach (Command childCommand in allChildCommands)
        {
            if (childCommand.CommandName == locationName)
            {
                if (childCommand is MoveCommand)
                {
                    if ((childCommand as MoveCommand).Found)
                        return false;

                    (childCommand as MoveCommand).Found = true;
                    return true;
                }
                else
                {
                    Debug.LogError("잘못된 커맨드 존재");
                }
            }
        }
        return false;
    }

    public override void CommandActive(string locationName, bool on)
    {
        foreach (Command childCommand in allChildCommands)
        {
            if (childCommand.CommandName == locationName)
                childCommand.IsOn = on;
        }
    }

    public void ReCaculateStatus()
    {
        List<Tuple<Location, Status>> status = locationList.CaculateAllPathsStatusFromName(Player.instance.CurrentLocation);

        for (int i = 0; i < allChildCommands.Count; i++)
        {
            Command childCommand = allChildCommands[i];

            foreach (var tuple in status)
            {
                Location location = tuple.Item1;
                Status locationStatus = tuple.Item2;

                if (childCommand.CommandName == location.locationName)
                {
                    childCommand.MyStatus = locationStatus;
                    break;
                }
            }
        }
    }
}
