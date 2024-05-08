using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class ThemeData
{
    public string themeName;
    public bool isOpen;
    public bool isSelect;

    public ThemeData(string name, bool open, bool select)
    {
        themeName = name;
        isOpen = open;
        isSelect = select;
    }    
}

[System.Serializable]
public class ThemeList
{
    public List<ThemeData> themes = new List<ThemeData>();

    // �⺻ �׸� ������ ����
    public void InitializeThemes()
    {
        themes.Add(new ThemeData("DefaultTheme", true, true));
        themes.Add(new ThemeData("PaperTheme", false , false));
        themes.Add(new ThemeData("HalloweenTheme", false, false));
    }
}

public class DataManager : Singleton<DataManager> 
{
    public ThemeList themeList = new ThemeList(); // �׸� ������ ����Ʈ
    private string path; // ���� ���
    private string fileName = "ThemeData.json"; // ���� �̸�

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
    public void SelectTheme(string themeName)
    {
        bool updated = false;
        foreach (var theme in themeList.themes)
        {
            if (theme.themeName == themeName) // �׸��� �ش� �̸��� ��ġ�ϴ� ���
            {
                if (!theme.isSelect) // ���� ���õ��� �ʾҴٸ�
                {
                    theme.isSelect = true;  // �׸��� ����
                    updated = true;         // ������Ʈ ǥ��
                }
            }
            else
            {
                if (theme.isSelect) // �ٸ� �׸��� ���õǾ� �ִٸ�
                {
                    theme.isSelect = false; // ���� ����
                    updated = true;         // ������Ʈ ǥ��
                }
            }
        }
        if (updated)
        {
            SaveData();  // ���� ������ ������ ������ ����
        }
    }

    // ����׿� JSON �ʱ�ȭ
    public void DebugJsonInit()
    {
        themeList.themes.Clear();
        themeList.InitializeThemes();

        SaveData();
    }
}
