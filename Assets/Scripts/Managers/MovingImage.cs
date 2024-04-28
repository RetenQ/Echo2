using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingImage : MonoBehaviour
{

    public RectTransform canvas; // UI Canvas的引用  
    public Sprite imageSprite; // 要使用的Image的Sprite  
    public Vector2 spawnPosition; // Image的生成位置  
    public float moveSpeed = 100f; // Image的移动速度  

    private Image movingImage; // 动态创建的Image  
    private float disappearanceTime = 1f; // Image消失的时间（秒）  

    void Start()
    {
        // 创建新的Image并设置其位置和Sprite  
        GameObject newImageObj = new GameObject("Moving Image");
        movingImage = newImageObj.AddComponent<Image>();
        movingImage.sprite = imageSprite;
        movingImage.rectTransform.SetParent(canvas, false);
        movingImage.rectTransform.anchoredPosition = spawnPosition;

        // 开始协程来移动Image并在1秒后销毁它  
        StartCoroutine(MoveAndDestroy(movingImage, disappearanceTime));
    }

    IEnumerator MoveAndDestroy(Image img, float delay)
    {
        // 使Image一直向左移动  
        while (true)
        {
            img.rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

            // 检查是否到了销毁Image的时间  
            if (delay <= 0)
            {
                Destroy(img.gameObject);
                break;
            }

            // 减少剩余时间  
            delay -= Time.deltaTime;

            yield return null;
        }
    }
}