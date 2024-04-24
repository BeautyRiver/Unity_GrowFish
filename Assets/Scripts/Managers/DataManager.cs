using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class ThemeData
{
    public string themeName;
    public bool isOpen;

    public ThemeData(string name, bool open)
    {
        themeName = name;
        isOpen = open;
    }
}

[System.Serializable]
public class ThemeList
{
    public List<ThemeData> themes = new List<ThemeData>();

    // �⺻ �׸� ������ ����
    public void InitializeThemes()
    {
        themes.Add(new ThemeData("Default", true));
        themes.Add(new ThemeData("Halloween", false));
        themes.Add(new ThemeData("Paper", false));
    }
}

public class DataManager : Singleton<DataManager>
{
    public ThemeList themeList = new ThemeList();
    private string path;
    private string fileName = "ThemeData.json";

    protected override void Awake()
    {
        base.Awake();
        path = Application.persistentDataPath + "/";
        LoadData(); // ������ �ε� �õ�
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(themeList, true);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string fullPath = path + fileName;
        if (File.Exists(fullPath)) // ������ �����ϸ� �ε�
        {
            string data = File.ReadAllText(fullPath);
            themeList = JsonUtility.FromJson<ThemeList>(data);
        }
        else // ������ �������� ������ �ʱ� ������ ����
        {
            themeList.InitializeThemes();
            SaveData(); // �ʱ� ������ ����
        }
    }

    public void UpdateTheme(string themeName)
    {
        bool updated = false;
        foreach (var theme in themeList.themes)
        {
            if (theme.themeName == themeName && !theme.isOpen) // �׸��� �ش� �̸��� ��ġ�ϰ� ����ִ� ���
            {
                theme.isOpen = true;  // �׸��� �ر�
                updated = true;       // ������Ʈ�� �̷�������� ǥ��
                break;
            }
        }

        if (updated)
        {
            SaveData();  // ���� ������ ������ ������ ����
        }
    }
}
