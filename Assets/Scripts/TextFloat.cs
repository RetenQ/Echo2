using UnityEngine;
using TMPro;

public class TextFloat : MonoBehaviour
{
    public float floatSpeed = 1f; // ���¸������ٶ�
    public float floatHeight = 0.1f; // �����ĸ߶�

    private TMP_Text textMesh;
    private Vector3 originalPosition;

    void Start()
    {
        // ��ȡTextMeshPro - Text UI���
        textMesh = GetComponent<TMP_Text>();
        // ��¼��ʼλ��
        originalPosition = transform.position;
    }

    void Update()
    {
        // �������¸�����ƫ����
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        // Ӧ��ƫ����
        transform.position = originalPosition + new Vector3(0f, yOffset, 0f);
    }
}
