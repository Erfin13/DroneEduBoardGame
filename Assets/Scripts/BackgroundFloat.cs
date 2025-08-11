using UnityEngine;

public class BackgroundFloat : MonoBehaviour
{
    public RectTransform target; // 背景 RectTransform
    public float scaleUp = 1.05f; // 放大倍數 (1.05 = 放大5%)
    public float duration = 2f;   // 放大和縮回的時間

    private Vector3 originalScale;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        originalScale = target.localScale;

        // 開始循環放大縮小
        StartCoroutine(FloatLoop());
    }

    System.Collections.IEnumerator FloatLoop()
    {
        while (true)
        {
            // 放大
            yield return ScaleTo(scaleUp, duration);
            // 回到原大小
            //yield return ScaleTo(1f, duration);
        }
    }

    System.Collections.IEnumerator ScaleTo(float targetMultiplier, float time)
    {
        Vector3 startScale = target.localScale;
        Vector3 endScale = originalScale * targetMultiplier;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
    }
}
