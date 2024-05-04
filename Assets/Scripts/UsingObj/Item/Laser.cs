using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject destination; // Ŀ������
    private LineRenderer lineRenderer; // LineRenderer���
    public string targetTag;
    public float damage;
    public BaseObj LaserUser; 

    [Header("׼��״̬����")]
    public Color IniColor;

    public float IniWidth; 

        
    [Header("����״̬����")]
    public Color ActColor;
    public float ActWidth;

    [Header("Audio")]
    public AudioSource preAudio;
    public AudioSource activeAudio;


    void Start()
    {
        /*        lineRenderer = GetComponent<LineRenderer>();
         *        ʹ������+setLaser��ʱ��Ҫ�ֶ����
         *        
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            
            putLaser();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveLaser();
        }
    }

    public void setLaser(GameObject _destination , string _targetTag , float _damage ,  BaseObj _user)
    {
        this.destination = _destination;
        this.targetTag = _targetTag;
        this.damage = _damage;
        this.LaserUser= _user;
        lineRenderer = GetComponent<LineRenderer>();

    }

    // �ڹ��غ����������һ����Ϊdestination��gameObject֮�����һ����ɫ����
    public void putLaser()
    {
        lineRenderer.material.color = IniColor; // �����߶���ɫ

        /*        lineRenderer.startColor = InistartColor;
                lineRenderer.endColor = IniendColor;*/
        lineRenderer.startWidth = IniWidth;
        lineRenderer.endWidth = IniWidth;

        if (destination != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, destination.transform.position);
        }

        preAudio.Play();

    }

    // �ڹ��غ����������һ����Ϊdestination��gameObject֮�����һ����ɫ���ߣ����Ҵ�ӡ�������е����������
    public void ActiveLaser()
    {
        if (destination != null)
        {
            lineRenderer.material.color = ActColor; // �����߶���ɫ
            /*            lineRenderer.startColor = ActstartColor;
                        lineRenderer.endColor = ActendColor;*/
            lineRenderer.startWidth = ActWidth;
            lineRenderer.endWidth = ActWidth;

            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, destination.transform.position);

            foreach (RaycastHit2D hit in hits)
            {
                // Debug.Log("Hit object name: " + hit.transform.name);
                if (hit.transform.gameObject.CompareTag(targetTag))
                {
                    hit.transform.gameObject.GetComponent<BaseObj>().Hurt(damage , LaserUser);
                }
            }

            //SetLaser(); // ���Ƴ�ɫ����
        }

        activeAudio.Play();

        StartCoroutine(LaserEnd_IE());

    }

    IEnumerator LaserEnd_IE()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject); // �ͷź�ʹݻ�

    }
}
