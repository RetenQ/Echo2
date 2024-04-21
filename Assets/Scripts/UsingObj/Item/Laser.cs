using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject destination; // 目标物体
    private LineRenderer lineRenderer; // LineRenderer组件

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

    // 在挂载函数的物体和一个名为destination的gameObject之间绘制一条红色的线
    public void SetLaser()
    {
        if (destination != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, destination.transform.position);
        }
    }

    // 在挂载函数的物体和一个名为destination的gameObject之间绘制一个橙色的线，并且打印其中所有的物体的名字
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

            //SetLaser(); // 绘制橙色的线
        }
    }
}
