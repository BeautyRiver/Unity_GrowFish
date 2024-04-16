using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static bool DoDestoryObj = true;  // �� ��ȯ �� �������� ���θ� �����ϴ� �÷���

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
        return instance;
        }
    }
    protected virtual void Awake()
    {
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.gameObject.transform.root.gameObject);
        }
        if(DoDestoryObj == true)
            DontDestroyOnLoad(this.gameObject);   
    }
}
