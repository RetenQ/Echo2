using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonMono<CameraMgr>
{
    public Transform target;

    public Transform TheCamera;
    [Header("����𶯲���")]
    [SerializeField]private bool isShake;
    public float duration;
    public float strength;




    void Start()
    {
        //TheCamera = this.gameObject.GetComponent<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        if (Input.GetKeyDown(KeyCode.K))
        {
            ShakeCamera(duration, strength); 
        }
    }

    void FollowPlayer()
    {
        TheCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }


    //�����֡
    //! ֻ����д������������ã���Ϊû���Թ��Ƿ�ᵼ������
    public void PauseCamera(int _duration)
    {
        StartCoroutine(Pause(_duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60.0f; //����60֡������ʱ�� 
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    //�����

    public void ShakeCamera(float _duration, float _strength)
    {
        if (!isShake)
        {
            StartCoroutine(Shake(_duration, _strength));
        }

    }

    public void ShakeCamera()
    {
        if (!isShake)
        {
            StartCoroutine(Shake(duration, strength));
        }

    }

    IEnumerator Shake(float _duration, float _strength)
    {
        isShake = true;
        Vector3 startPosition = TheCamera.position;

        while (_duration > 0)
        {
            TheCamera.position = Random.insideUnitSphere * _strength + startPosition;
            _duration -= Time.deltaTime;
            yield return null;
        }

        TheCamera.position = startPosition;
        isShake = false;
    }

}
