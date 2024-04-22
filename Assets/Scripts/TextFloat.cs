using UnityEngine;
using TMPro;

public class TextFloat : MonoBehaviour
{
    public float floatSpeed = 1f; // 上下浮动的速度
    public float floatHeight = 0.1f; // 浮动的高度

    private TMP_Text textMesh;
    private Vector3 originalPosition;

    void Start()
    {
        // 获取TextMeshPro - Text UI组件
        textMesh = GetComponent<TMP_Text>();
        // 记录初始位置
        originalPosition = transform.position;
    }

    void Update()
    {
        // 计算上下浮动的偏移量
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        // 应用偏移量
        transform.position = originalPosition + new Vector3(0f, yOffset, 0f);
    }
}
