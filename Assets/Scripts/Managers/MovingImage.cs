using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingImage : MonoBehaviour
{

    public RectTransform canvas; // UI Canvas������  
    public Sprite imageSprite; // Ҫʹ�õ�Image��Sprite  
    public Vector2 spawnPosition; // Image������λ��  
    public float moveSpeed = 100f; // Image���ƶ��ٶ�  

    private Image movingImage; // ��̬������Image  
    private float disappearanceTime = 1f; // Image��ʧ��ʱ�䣨�룩  

    void Start()
    {
        // �����µ�Image��������λ�ú�Sprite  
        GameObject newImageObj = new GameObject("Moving Image");
        movingImage = newImageObj.AddComponent<Image>();
        movingImage.sprite = imageSprite;
        movingImage.rectTransform.SetParent(canvas, false);
        movingImage.rectTransform.anchoredPosition = spawnPosition;

        // ��ʼЭ�����ƶ�Image����1���������  
        StartCoroutine(MoveAndDestroy(movingImage, disappearanceTime));
    }

    IEnumerator MoveAndDestroy(Image img, float delay)
    {
        // ʹImageһֱ�����ƶ�  
        while (true)
        {
            img.rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

            // ����Ƿ�������Image��ʱ��  
            if (delay <= 0)
            {
                Destroy(img.gameObject);
                break;
            }

            // ����ʣ��ʱ��  
            delay -= Time.deltaTime;

            yield return null;
        }
    }
}