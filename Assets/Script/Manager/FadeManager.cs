using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : UIScript
{
    public static FadeManager instance;

    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private float fadeSpeed = 0.01f;
    [SerializeField] private Color imageColor;
    [SerializeField] private Color textColor;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);
    }

    public void SkipButton()
    {
        imageColor.a = 0;
        textColor.a = 0;

        StopAllCoroutines();
        OpenClose(false);
    }

    public void FadeInImmediately(string textName)
    {
        OpenClose(true);
        text.text = textName;

        imageColor.a = 1;
        textColor.a = 1;
        image.color = imageColor;
        text.color = textColor;
    }


    public void FadeIn(string textName, float time = 3)
    {
        OpenClose(true);
        text.text = textName;

        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(time));
    }

    public IEnumerator FadeInCoroutine(float time)
    {

        while (imageColor.a < 1f)
        {
            imageColor.a += 0.01f;
            image.color = imageColor;

            textColor.a += 0.01f;
            text.color = textColor;

            yield return new WaitForSeconds(fadeSpeed);
        }
        yield return new WaitUntil(() => imageColor.a >= 1f);
        yield return new WaitForSeconds(time);
        FadeOut(time);
    }
    public void FadeOut(float time = 2)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(time));
    }
    IEnumerator FadeOutCoroutine(float time)
    {
        while (imageColor.a > 0f)
        {
            imageColor.a -= 0.01f;
            image.color = imageColor;

            textColor.a -= 0.01f;
            text.color = textColor;

            yield return new WaitForSeconds(fadeSpeed);
        }
        yield return new WaitUntil(() => imageColor.a <= 0f);

        OpenClose(false);
    }
}