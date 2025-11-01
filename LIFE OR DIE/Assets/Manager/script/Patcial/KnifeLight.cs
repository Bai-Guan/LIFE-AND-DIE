using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnifeLight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float totalDuration;
    private float currentTimer = 0;
    private Vector3 currentScale;

    private const float InTime = 0.08f;   // 淡入占 8%
    private const float OutTime = 0.25f;   // 淡出占 25%


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 初始透明
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
        currentScale=this.transform.localScale;
    }

    public void Set(Transform spawnTransform, float duration, float size, float rotation)
    {
        transform.position = spawnTransform.position;
        transform.localEulerAngles = new Vector3(0, 0, rotation);
        currentScale.x =currentScale.x/size;
        currentScale.y =currentScale.y/size;
        transform.localScale = currentScale;
        totalDuration = duration;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= totalDuration)
        {
            Destroy(gameObject);
            return;
        }

        float progress = currentTimer / totalDuration;
        float alpha = CalculateAlpha(progress);

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private float CalculateAlpha(float progress)
    {
        if (progress < InTime)                                    // 淡入段
            return Mathf.Lerp(0f, 1f, progress / InTime);

        if (progress < 1f - OutTime)                              // 保持段
            return 1f;

        // 淡出段
        float outProgress = (progress - (1f - OutTime)) / OutTime;
        return Mathf.Lerp(1f, 0f, outProgress);
    }
}
