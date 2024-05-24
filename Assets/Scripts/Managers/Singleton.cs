using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public  bool DoDestroyOnLoad = true;  // 씬 전환 시 객체를 유지할지 여부를 결정하는 플래그

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);  // 중복 인스턴스 파괴
            return;
        }

        instance = this as T;

        if (DoDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);  // 씬 전환 시 파괴되지 않도록 설정
        }

        Application.targetFrameRate = 60;
    }
}
