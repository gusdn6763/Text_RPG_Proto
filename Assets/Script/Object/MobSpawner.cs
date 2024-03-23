using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Spawn
{
    public Command command = null;
    [Range(0, 100)]
    public float percent = 0;
}

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform createRectTransform;
    [SerializeField] private RectTransform itemRectTransform;
    [SerializeField] private RectTransform mobRectTransform;
    public List<Spawn> spawnList = new List<Spawn>();

    [SerializeField] private float createSpeed = 5f;
    [SerializeField] private Status status;
    [SerializeField] private float statusSpentTime;
    [Header("테스트용")]
    [SerializeField] private float firstSpawnMin = 500f;
    [SerializeField] private float firstSpawnMax = 1000f;
    [SerializeField] private int firstMinCount = 1;
    [SerializeField] private int firstMaxCount = 3;

    private Coroutine spawnCoroutine;
    private Coroutine spentCoroutine;
    private Status saveStatus = new Status(0, 0, 0, 0);

    private float elapsedTimeSpawn = 0f;
    private float elapsedTimeSpent = 0f;
    private bool save = false;

    public void FirstSpawn()
    {
        int spawnCount = 0;
        int count = Random.Range(firstMinCount, firstMaxCount);
        while (spawnCount != count)
        {
            foreach (Spawn spawn in spawnList)
            {
                if (UnityEngine.Random.Range(0, 100) < spawn.percent)
                {
                    Vector3 randomSpawnPosition = Vector3.zero;
                    // 랜덤한 Z 위치 설정
                    float randomZ = Random.Range(firstSpawnMin, firstSpawnMax); // 최소 Z 위치와 100 단위로 증가하도록 수정
                    if (spawn.command is Item)
                    {
                        randomSpawnPosition = new Vector3(
                            Random.Range(itemRectTransform.rect.min.x, itemRectTransform.rect.max.x),
                            Random.Range(-itemRectTransform.rect.height, 0), randomZ);
                    }
                    else if (spawn.command is MobCommand)
                    {
                        randomSpawnPosition = new Vector3(
                            Random.Range(mobRectTransform.rect.min.x, mobRectTransform.rect.max.x),
                            Random.Range(-mobRectTransform.rect.height, 0), randomZ);
                    }
                    else
                        Debug.LogError("잘못된 스폰");

                    Command obj = Instantiate(spawn.command);

                    obj.ActiveOnOff(false);
                    obj.transform.localPosition = randomSpawnPosition;
                    obj.transform.SetParent(createRectTransform);
                    spawnCount++;
                    break;
                }
            }
        }
    }

    public void Spawn(bool on)
    {
        if (on && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
        else if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;       
        }
    }

    public void Spent(bool on)
    {
        if (on && spentCoroutine == null)
        {
            spentCoroutine = StartCoroutine(StartSpentCoroutine());
        }
        else if (spentCoroutine != null)
        {
            StopCoroutine(spentCoroutine);
            spentCoroutine = null;
        }
    }

    public IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            elapsedTimeSpawn += Time.deltaTime;
            if (elapsedTimeSpawn >= createSpeed)
            {
                elapsedTimeSpawn -= createSpeed;

                foreach (Spawn spawn in spawnList)
                {
                    if (UnityEngine.Random.Range(0, 100) < spawn.percent)
                    {
                        Vector3 randomItemPosition = Vector3.zero;
                        if (spawn.command is Item)
                        {
                            randomItemPosition = new Vector3(
                                Random.Range(itemRectTransform.rect.min.x, itemRectTransform.rect.max.x),
                                Random.Range(-itemRectTransform.rect.height, 0), itemRectTransform.transform.position.z);
                        }
                        else if (spawn.command is MobCommand)
                        {
                            randomItemPosition = new Vector3(
                                Random.Range(mobRectTransform.rect.min.x, mobRectTransform.rect.max.x),
                                Random.Range(-mobRectTransform.rect.height, 0), mobRectTransform.transform.position.z);
                        }
                        else
                            Debug.LogError("잘못된 스폰");

                        Command obj = Instantiate(spawn.command);

                        obj.ActiveOnOff(false);
                        obj.transform.localPosition = randomItemPosition;
                        obj.transform.SetParent(createRectTransform);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator StartSpentCoroutine()
    {
        while (true)
        {
            elapsedTimeSpent += Time.deltaTime;
            if (elapsedTimeSpent >= statusSpentTime)
            {
                elapsedTimeSpent -= statusSpentTime;

                if (Player.instance.CombatOn)
                {
                    saveStatus += status;
                    save = true;
                }
                else
                {
                    if (save)
                    {
                        Player.instance.SetStatus(saveStatus);
                        saveStatus = new Status(0, 0, 0, 0);
                        save = false;
                    }

                    Player.instance.SetStatus(status);
                }
            }
            yield return null;
        }
    }


    public void ResetAllObj()
    {
        foreach (RectTransform child in createRectTransform)
            Destroy(child.gameObject);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        ClampPercentValues();
    }
    public void ClampPercentValues()
    {
        float totalPercent = 0;

        // Calculate total percent
        foreach (Spawn spawn in spawnList)
        {
            totalPercent += spawn.percent;
        }

        // If total percent exceeds 100, adjust it
        if (totalPercent > 100)
        {
            float difference = totalPercent - 100;

            // Adjust the first spawn's percent
            Spawn mostHigh = FindMostHigh();
            mostHigh.percent -= difference;

            // Clamp all values to [0, 100]
            foreach (Spawn spawn in spawnList)
            {
                spawn.percent = Mathf.Clamp(spawn.percent, 0, 100);
            }
        }
    }
    private Spawn FindMostHigh()
    {
        Spawn highestPercentSpawn = null;
        float highestPercent = float.MinValue;
        foreach (Spawn spawn in spawnList)
        {
            if (spawn.percent > highestPercent)
            {
                highestPercent = spawn.percent;
                highestPercentSpawn = spawn;
            }
        }
        return highestPercentSpawn;
    }
#endif
}
