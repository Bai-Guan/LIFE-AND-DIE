using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    public static  TimeManager Instance {  get { return _instance; } }
    private List<Coroutine> activeCoroutines = new List<Coroutine>();
   

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

    }


    // Update is called once per frame
 

   
    //过一段时间后只触发一次
    public void OneTime(float delay,Action callback)
    {
        var coroutine =  StartCoroutine(OneTimeCoroutine(delay, callback));
        activeCoroutines.Add(coroutine);
    }

    //一段时间内每帧持续触发
    public void FrameTime(float delay, Action callback)
    {
        var coroutine = StartCoroutine(FrameTimeCoroutine(delay, callback));
        activeCoroutines.Add(coroutine);
        foreach(Coroutine c in activeCoroutines)
        {
            if(c==null) activeCoroutines.Remove(c);
        }
    }

    public void FrameTime(float delay, Action callback,Action onComplete)
    {
        var coroutine = StartCoroutine(FrameTimeCoroutine(delay, callback, onComplete));
        activeCoroutines.Add(coroutine);
    }

    public void CameraShake(float duration,float pow)
    {
        StartCoroutine(CameraShakeCoroutine(duration, pow));
    }

   private IEnumerator OneTimeCoroutine(float delay,Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
        //协程未处理
        
    }
    private IEnumerator FrameTimeCoroutine(float duration,Action callback)
    {
        float time = 0;
        while (time < duration)
        {
            callback?.Invoke();
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FrameTimeCoroutine(float duration, Action callback,Action OnComplete)
    {
        float time = 0;
        while (time < duration)
        {
            callback?.Invoke();
            time += Time.deltaTime;
            yield return null;
        }
        OnComplete?.Invoke();
    }

    private IEnumerator CameraShakeCoroutine(float duration,float pow)
    {
        Vector3 temp = CameraManager.Instance.gameObject.transform.position;
        Vector3 pos;
        float time = 0;
        while (time < duration)
        {
            pos = CameraManager.Instance.gameObject.transform.position;
            pos = new Vector3(
              UnityEngine.Random.Range(-pow, pow)+ pos.x,
               UnityEngine.Random.Range(-pow, pow) + pos.y,
                pos.z
                );
            time += Time.deltaTime;
            yield return null;
        }
        CameraManager.Instance.gameObject.transform.position = temp;
    }

}
