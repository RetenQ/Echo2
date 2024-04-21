using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject destination; // Ŀ������
    private LineRenderer lineRenderer; // LineRenderer���

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetLaser();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveLaser();
        }
    }

    // �ڹ��غ����������һ����Ϊdestination��gameObject֮�����һ����ɫ����
    public void SetLaser()
    {
        if (destination != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, destination.transform.position);
        }
    }

    // �ڹ��غ����������һ����Ϊdestination��gameObject֮�����һ����ɫ���ߣ����Ҵ�ӡ�������е����������
    public void ActiveLaser()
    {
        if (destination != null)
        {
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;

            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, destination.transform.position);

            foreach (RaycastHit2D hit in hits)
            {
                Debug.Log("Hit object name: " + hit.transform.name);
            }

            //SetLaser(); // ���Ƴ�ɫ����
        }
    }
}
