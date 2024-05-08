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

    // 기본 테마 데이터 생성
    public void InitializeThemes()
    {
        themes.Add(new ThemeData("DefaultTheme", true, true));
        themes.Add(new ThemeData("PaperTheme", false , false));
        themes.Add(new ThemeData("HalloweenTheme", false, false));
    }
}

public class DataManager : Singleton<DataManager> 
{
    public ThemeList themeList = new ThemeList(); // 테마 데이터 리스트
    private string path; // 파일 경로
    private string fileName = "ThemeData.json"; // 파일 이름

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
    public void SelectTheme(string themeName)
    {
        bool updated = false;
        foreach (var theme in themeList.themes)
        {
            if (theme.themeName == themeName) // 테마가 해당 이름과 일치하는 경우
            {
                if (!theme.isSelect) // 현재 선택되지 않았다면
                {
                    theme.isSelect = true;  // 테마를 선택
                    updated = true;         // 업데이트 표시
                }
            }
            else
            {
                if (theme.isSelect) // 다른 테마가 선택되어 있다면
                {
                    theme.isSelect = false; // 선택 해제
                    updated = true;         // 업데이트 표시
                }
            }
        }
        if (updated)
        {
            SaveData();  // 변경 사항이 있으면 데이터 저장
        }
    }

    // 디버그용 JSON 초기화
    public void DebugJsonInit()
    {
        themeList.themes.Clear();
        themeList.InitializeThemes();

        SaveData();
    }
}
