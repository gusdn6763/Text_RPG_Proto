using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceText : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float moveSpeed;
    public Text text;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FadeOut());

    }

    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;

        float rate = 1.0f / lifeTime;

        float progress = 0.0f;

        while (progress < 1.0)
        {
            transform.position += new Vector3(0, moveSpeed, 0);
            Color tmp = text.color;

            tmp.a = Mathf.Lerp(startAlpha, 0, progress);

            text.color = tmp;

            progress += rate * Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
