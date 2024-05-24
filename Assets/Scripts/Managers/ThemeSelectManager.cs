using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelectManager : MonoBehaviour
{

    public Button[] themesBtn;
    Color enabledColor = new Color(1, 1, 1, 1);
    Color disabledColor = new Color(1, 1, 1, 0.4f);
    public enum Menu
    {
        Main,
        Store,
    };
    public Menu menu;
    private void OnEnable()
    {
        if (menu == Menu.Main)
        {
            UpdateThemeMainMenu();
        }
        else if (menu == Menu.Store)
        {
            UpdateThemeStore();
        }
    }

    // 테마 선택 시
    public void SelectTheme(string themeName)
    {
        DataManager dt = DataManager.Instance;
        dt.SelectTheme(themeName); // 테마 잠금 해제
        UpdateThemeMainMenu(); // 테마 선택 버튼 업데이트
    }

    // 테마 선택 버튼 업데이트
    public void UpdateThemeMainMenu()
    {
        DataManager dt = DataManager.Instance;
        dt.LoadData();

        for (int i = 0; i < dt.themeList.themes.Count && i < themesBtn.Length; i++) 
        {
            ThemeData currentTheme = dt.themeList.themes[i]; // 현재 테마
            Button currentBtn = themesBtn[i]; // 현재 버튼

            currentBtn.interactable = currentTheme.isOpen; // 열려있는 테마만 선택 가능

            Transform checkMark = currentBtn.transform.GetChild(0); // 체크 표시
            Image themeImage = currentBtn.transform.GetChild(1).GetComponent<Image>(); // 테마 이미지

            checkMark.gameObject.SetActive(currentTheme.isSelect); // 선택된 테마에 체크 표시
            themeImage.color = currentTheme.isOpen ? enabledColor : disabledColor; // 열려있는 테마만 활성화
        }
    }

    public void UpdateThemeStore()
    {
        int count = 0;
        DataManager dt = DataManager.Instance;
        dt.LoadData();
        for (int i = 1; i < dt.themeList.themes.Count; i++) // 인덱스 1부터 시작
        {
            var item = dt.themeList.themes[i];
            themesBtn[count].interactable = !item.isOpen; // 열려있지 않은 테마만 구매 가능
            themesBtn[count].transform.GetChild(0).GetComponent<Image>().color = !item.isOpen ? enabledColor : disabledColor;
            count++;
        }

    }
}

