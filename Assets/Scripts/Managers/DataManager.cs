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

    // 기본 테마 데이터 생성
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
        LoadData(); // 데이터 로드 시도
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(themeList, true);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string fullPath = path + fileName;
        if (File.Exists(fullPath)) // 파일이 존재하면 로드
        {
            string data = File.ReadAllText(fullPath);
            themeList = JsonUtility.FromJson<ThemeList>(data);
        }
        else // 파일이 존재하지 않으면 초기 데이터 설정
        {
            themeList.InitializeThemes();
            SaveData(); // 초기 데이터 저장
        }
    }

    public void UpdateTheme(string themeName)
    {
        bool updated = false;
        foreach (var theme in themeList.themes)
        {
            if (theme.themeName == themeName && !theme.isOpen) // 테마가 해당 이름과 일치하고 잠겨있는 경우
            {
                theme.isOpen = true;  // 테마를 해금
                updated = true;       // 업데이트가 이루어졌음을 표시
                break;
            }
        }

        if (updated)
        {
            SaveData();  // 변경 사항이 있으면 데이터 저장
        }
    }
}
