using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    public Transform targetA; // 第一个目标点
    public Transform targetB; // 第二个目标点
    public Transform targetC; // 第三个目标点
    public float delaySec = 1f; // 延迟时间

    private LineRenderer lineRenderer; // LineRenderer组件

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        StartCoroutine(StartRaycast());
        StartCoroutine(StartExtraRaycast());
    }

    IEnumerator StartRaycast()
    {
        yield return new WaitForSeconds(delaySec);

        RaycastHit2D[] hits = Physics2D.LinecastAll(targetA.position, targetB.position);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Hit object name: " + hit.transform.name);
        }

        // 更新LineRenderer的位置
        lineRenderer.SetPosition(0, targetA.position);
        lineRenderer.SetPosition(1, targetB.position);
    }

    IEnumerator StartExtraRaycast()
    {
        yield return new WaitForSeconds(delaySec);

        Vector2 direction = (targetC.position - targetA.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(targetA.position, direction);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Extra Hit object name: " + hit.transform.name);
        }

    }
}
/*
    private void OnDrawGizmos()
    {
        // 在编辑器中绘制Gizmos
        if (targetA != null && targetC != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetA.position, targetC.position);
        }
    }
}*/
