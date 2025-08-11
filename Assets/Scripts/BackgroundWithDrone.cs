using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BackgroundDroneController : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        public Sprite sprite;
        [Min(0.5f)] public float holdSeconds = 5f;
    }

    [Header("背景設定")]
    public List<Slide> slides = new();
    [Min(0.2f)] public float fadeSeconds = 1.2f;
    public bool randomOrder = false;
    public bool loop = true;
    public Image imgA;
    public Image imgB;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("無人機設定")]
    public RectTransform drone;
    public Vector2 heightRange = new Vector2(-200, 200);
    public float flySpeed = 300f; // 單位/秒
    public float offsetOutside = 200f; // 飛出畫面的距離

    private int index = 0;
    private bool usingA = true;
    private bool playing = false;

    void Awake()
    {
        SetupImage(imgA);
        SetupImage(imgB);

        if (slides.Count > 0)
        {
            imgA.sprite = slides[0].sprite;
            imgA.color = Color.white;
            imgB.color = new Color(1, 1, 1, 0);
        }
    }

    void Start()
    {
        if (slides.Count > 0)
        {
            playing = true;
            StartCoroutine(Run());
        }
    }

    void SetupImage(Image img)
    {
        img.type = Image.Type.Simple;
        img.preserveAspect = false;
        img.rectTransform.anchorMin = Vector2.zero;
        img.rectTransform.anchorMax = Vector2.one;
        img.rectTransform.offsetMin = Vector2.zero;
        img.rectTransform.offsetMax = Vector2.zero;
        img.rectTransform.localScale = Vector3.one;
    }

    IEnumerator Run()
    {
        while (playing)
        {
            yield return StartCoroutine(FlyDroneAcross());

            // 切換背景
            yield return StartCoroutine(FadeToNext());

            if (!loop && index == slides.Count - 1)
            {
                playing = false;
                yield break;
            }
        }
    }

    IEnumerator FlyDroneAcross()
    {
        RectTransform canvasRect = drone.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        int mode = Random.Range(0, 5); // 0~4 五種飛行模式
        Vector2 startPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;

        switch (mode)
        {
            case 0: // 左 → 右
                startPos = new Vector2(-canvasWidth / 2 - offsetOutside, Random.Range(heightRange.x, heightRange.y));
                endPos = new Vector2(canvasWidth / 2 + offsetOutside, startPos.y);
                break;
            case 1: // 右 → 左
                startPos = new Vector2(canvasWidth / 2 + offsetOutside, Random.Range(heightRange.x, heightRange.y));
                endPos = new Vector2(-canvasWidth / 2 - offsetOutside, startPos.y);
                break;
            case 2: // 左下 → 右上
                startPos = new Vector2(-canvasWidth / 2 - offsetOutside, -canvasHeight / 4);
                endPos = new Vector2(canvasWidth / 2 + offsetOutside, canvasHeight / 4);
                break;
            case 3: // 右上 → 左下
                startPos = new Vector2(canvasWidth / 2 + offsetOutside, canvasHeight / 4);
                endPos = new Vector2(-canvasWidth / 2 - offsetOutside, -canvasHeight / 4);
                break;
            case 4: // 波浪曲線
                startPos = new Vector2(-canvasWidth / 2 - offsetOutside, 0);
                endPos = new Vector2(canvasWidth / 2 + offsetOutside, 0);
                Vector3[] path = new Vector3[]
                {
                startPos,
                new Vector3(0,  100, 0),
                new Vector3(canvasWidth / 4, -100, 0),
                new Vector3(canvasWidth / 2,  100, 0),
                endPos
                };
                yield return drone.DOPath(path, Vector2.Distance(startPos, endPos) / flySpeed, PathType.CatmullRom)
                                  .SetEase(Ease.Linear)
                                  .WaitForCompletion();
                yield break;
        }

        drone.anchoredPosition = startPos;
        float distance = Vector2.Distance(startPos, endPos);
        float flyTime = distance / flySpeed;

        yield return drone.DOAnchorPos(endPos, flyTime)
                          .SetEase(Ease.Linear)
                          .WaitForCompletion();
    }


    IEnumerator FadeToNext()
    {
        if (randomOrder && slides.Count > 1)
        {
            int rnd;
            do { rnd = Random.Range(0, slides.Count); } while (rnd == index);
            index = rnd;
        }
        else
        {
            index = (index + 1) % slides.Count;
        }

        Image from = usingA ? imgA : imgB;
        Image to = usingA ? imgB : imgA;
        usingA = !usingA;

        to.sprite = slides[index].sprite;
        to.color = new Color(1, 1, 1, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, fadeSeconds);
            float a = ease.Evaluate(t);
            to.color = new Color(1, 1, 1, a);
            from.color = new Color(1, 1, 1, 1f - a);
            yield return null;
        }

        to.color = Color.white;
        from.color = new Color(1, 1, 1, 0);
    }
}
