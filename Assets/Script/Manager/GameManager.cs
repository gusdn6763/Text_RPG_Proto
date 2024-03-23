using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public delegate void KillConfirmed(MobCommand mobCommand);
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event KillConfirmed killConfirmedEvent;

    [SerializeField] private List<Command> initCommandList = new List<Command>();
    [SerializeField] public MobSpawner mobSpawner;
    [SerializeField] private MoveCommandParent moveCommandParent;
    [SerializeField] private DungeonUI dungeonUI;

    public bool dungeonEnter = false;

    public float actionTime = 60f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        moveCommandParent.MoveLocation(Constant.defaultPosition);
    }

    public void MoveEvent()
    {
        for (int i = 0; i < initCommandList.Count; i++)
            initCommandList[i].IsOn = false;
    }

    //던전 관련부분
    public void DungeonEnter()
    {
        dungeonEnter = true;
        mobSpawner.FirstSpawn();
        mobSpawner.Spent(true);
        mobSpawner.Spawn(true);
        Player.instance.PlayerEnter();
    }

    public void DungeonExit()
    {
        dungeonEnter = false;
        mobSpawner.Spawn(false);
        mobSpawner.Spent(false);
        mobSpawner.ResetAllObj();

        Player.instance.PlayerExit();
        moveCommandParent.MoveLocation(Constant.town);
    }

    public void OnKillConfirmed(MobCommand mobCommand)
    {
        if (killConfirmedEvent != null)
            killConfirmedEvent(mobCommand);
    }

    public void RemoveAllList()
    {
        mobSpawner.ResetAllObj();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "테스트1");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "나감");
    }
}
