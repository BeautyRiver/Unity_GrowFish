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

    // �׸� ���� ��
    public void SelectTheme(string themeName)
    {
        DataManager dt = DataManager.Instance;
        dt.SelectTheme(themeName); // �׸� ��� ����
        UpdateThemeMainMenu(); // �׸� ���� ��ư ������Ʈ
    }

    // �׸� ���� ��ư ������Ʈ
    public void UpdateThemeMainMenu()
    {
        DataManager dt = DataManager.Instance;
        dt.LoadData();

        for (int i = 0; i < dt.themeList.themes.Count && i < themesBtn.Length; i++) 
        {
            ThemeData currentTheme = dt.themeList.themes[i]; // ���� �׸�
            Button currentBtn = themesBtn[i]; // ���� ��ư

            currentBtn.interactable = currentTheme.isOpen; // �����ִ� �׸��� ���� ����

            Transform checkMark = currentBtn.transform.GetChild(0); // üũ ǥ��
            Image themeImage = currentBtn.transform.GetChild(1).GetComponent<Image>(); // �׸� �̹���

            checkMark.gameObject.SetActive(currentTheme.isSelect); // ���õ� �׸��� üũ ǥ��
            themeImage.color = currentTheme.isOpen ? enabledColor : disabledColor; // �����ִ� �׸��� Ȱ��ȭ
        }
    }

    public void UpdateThemeStore()
    {
        int count = 0;
        DataManager dt = DataManager.Instance;
        dt.LoadData();
        for (int i = 1; i < dt.themeList.themes.Count; i++) // �ε��� 1���� ����
        {
            var item = dt.themeList.themes[i];
            themesBtn[count].interactable = !item.isOpen; // �������� ���� �׸��� ���� ����
            themesBtn[count].transform.GetChild(0).GetComponent<Image>().color = !item.isOpen ? enabledColor : disabledColor;
            count++;
        }

    }
}

