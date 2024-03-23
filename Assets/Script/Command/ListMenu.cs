using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListMenu : MonoBehaviour
{
    private RectTransform rectTransform;
    private Command[] commandList; 
    [SerializeField] private float width;
    [SerializeField] private float height;

    private void Awake()
    {
        int childCount = transform.childCount;

        commandList = new Command[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            Command command = childTransform.GetComponent<Command>();
            if (command)
                commandList[i] = command;
            else
                Debug.LogError("쓸데없는것이 껴있음");
        }
        rectTransform = GetComponent<RectTransform>();
    }

    public void IsActive(bool isOn)
    {
        if (isOn)
        {
            gameObject.SetActive(true);
            int nCount = 0;
            for (int i = 0; i < commandList.Length; i++)
            {
                if (commandList[i].IsOn)
                    nCount++;

                commandList[i].gameObject.SetActive(commandList[i].IsOn);
            }
            ShowList(nCount);
        }
        else
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        int nCount = 0;
        for (int i = 0; i < commandList.Length; i++)
        {
            if (commandList[i].IsOn)
                nCount++;

            commandList[i].gameObject.SetActive(commandList[i].IsOn);
        }

        ShowList(nCount);
    }

    public void ShowList(int nCount = 0)
    {
        StartCoroutine(AnimateSizeChangeCoroutine(width, nCount * 30f, 0.5f));
    }

    private IEnumerator AnimateSizeChangeCoroutine(float width, float height, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newWidth = Mathf.Lerp(0, width, elapsedTime / duration);
            float newHeight = Mathf.Lerp(0, height, elapsedTime / duration);

            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 목표 높이로 설정
        rectTransform.sizeDelta = new Vector2(width, height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}

