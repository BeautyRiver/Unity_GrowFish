using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ThemeData
{
    public string themeName;
    public bool isOpen;
}
public class DataManager : Singleton<DataManager>
{   
    public ThemeData themeData = new ThemeData();
    private string path;
    private string fileName = "ThemeDataSave";
    public int nowSlot;
    protected override void Awake()
    {
        base.Awake();  // 기반 클래스의 Awake 호출
        path = Application.persistentDataPath + "/";
    }
    private void Start()
    {
        
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(themeData);
        File.WriteAllText(path + fileName + nowSlot.ToString(), data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName + nowSlot.ToString());
        themeData = JsonUtility.FromJson<ThemeData>(data);
    }
}
