using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject destination; // 目标物体
    private LineRenderer lineRenderer; // LineRenderer组件
    public string targetTag;
    public float damage;
    public BaseObj LaserUser; 

    [Header("准备状态属性")]
    public Color IniColor;

    public float IniWidth; 

        
    [Header("激活状态属性")]
    public Color ActColor;
    public float ActWidth;

    [Header("Audio")]
    public AudioSource preAudio;
    public AudioSource activeAudio;


    void Start()
    {
        /*        lineRenderer = GetComponent<LineRenderer>();
         *        使用生成+setLaser的时候要手动添加
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

    // 在挂载函数的物体和一个名为destination的gameObject之间绘制一条红色的线
    public void putLaser()
    {
        lineRenderer.material.color = IniColor; // 设置线段颜色

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

    // 在挂载函数的物体和一个名为destination的gameObject之间绘制一个橙色的线，并且打印其中所有的物体的名字
    public void ActiveLaser()
    {
        if (destination != null)
        {
            lineRenderer.material.color = ActColor; // 设置线段颜色
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

            //SetLaser(); // 绘制橙色的线
        }

        activeAudio.Play();

        StartCoroutine(LaserEnd_IE());

    }

    IEnumerator LaserEnd_IE()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject); // 释放后就摧毁

    }
}
