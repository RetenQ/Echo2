using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonMono<CameraMgr>
{
    public Transform target;

    public Transform TheCamera;
    [Header("相机震动参数")]
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


    //画面顿帧
    //! 只是先写在这里，谨慎调用，因为没测试过是否会导致乱轴
    public void PauseCamera(int _duration)
    {
        StartCoroutine(Pause(_duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60.0f; //按照60帧来计算时间 
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    //相机震动

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
