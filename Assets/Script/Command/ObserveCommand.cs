using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OberEvent
{
    public List<Item> createList;

    public string eventName;
    public string locationName;
    public int percent;
}
public class ObserveCommand : Command
{
    [SerializeField] private Area spawnArea;
    [SerializeField] private LocationList locationList;
    [SerializeField] private List<OberEvent> oberEvents;

    private List<Location> totalLocation;

    protected override void Start()
    {
        base.Start();
        totalLocation = locationList.GetAllLocation();
    }
    public void ObserveStart()
    {
        Excute();

        //아직 모든 지형이 탐험되지 아니함
        if (FirstObserve())
            return;

        for (int i = 0; i < oberEvents.Count; i++)
        {
            OberEvent oberEvent = oberEvents[i];

            if (Player.instance.CurrentLocation == oberEvent.locationName)
            {
                if (UnityEngine.Random.Range(0, 100) < oberEvent.percent)
                {
                    for (int j = 0; j < oberEvent.createList.Count; j++)
                    {
                        Item item = Instantiate(oberEvent.createList[j]);
                        spawnArea.SetItem(item, true);
                    }
                    Player.instance.ShowIntroduce(oberEvent.eventName);
                    return;
                }
            }
        }

        Player.instance.ShowIntroduce(Constant.nothingFound);
    }

    public bool FirstObserve()
    {
        bool find = false;
        for (int i = 0; i < totalLocation.Count; i++)
        {
            if (Player.instance.CurrentLocation == totalLocation[i].locationName)
            {
                List<string> list = locationList.SearchNearLocationFromName(Player.instance.CurrentLocation);

                MoveCommandParent command = locationList.GetComponent<MoveCommandParent>();
                for (int j = 0; j < list.Count; j++)
                {
                    if (command.FindLocation(list[j]))
                        Player.instance.ShowIntroduce(list[j] + Constant.findLocation);
                }
                totalLocation.RemoveAt(i);
                find = true;
            }
        }

        return find;
    }
}
