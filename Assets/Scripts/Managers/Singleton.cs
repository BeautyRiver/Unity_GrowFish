using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public  bool DoDestroyOnLoad = true;  // �� ��ȯ �� ��ü�� �������� ���θ� �����ϴ� �÷���

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
            Destroy(this.gameObject);  // �ߺ� �ν��Ͻ� �ı�
            return;
        }

        instance = this as T;

        if (DoDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);  // �� ��ȯ �� �ı����� �ʵ��� ����
        }

        Application.targetFrameRate = 60;
    }
}
