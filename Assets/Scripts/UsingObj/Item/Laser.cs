using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    public Transform targetA; // ��һ��Ŀ���
    public Transform targetB; // �ڶ���Ŀ���
    public Transform targetC; // ������Ŀ���
    public float delaySec = 1f; // �ӳ�ʱ��

    private LineRenderer lineRenderer; // LineRenderer���

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

        // ����LineRenderer��λ��
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
        // �ڱ༭���л���Gizmos
        if (targetA != null && targetC != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetA.position, targetC.position);
        }
    }
}*/
