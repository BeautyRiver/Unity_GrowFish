using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeTransParent : MonoBehaviour
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
        UpdateThemeSelect();
    }

    public void UpdateThemeSelect()
    {        
        if (menu == Menu.Main)
        {
            int count = 0;

            DataManager dt = DataManager.Instance;
            dt.LoadData();
            foreach (var item in dt.themeList.themes)
            {
                themesBtn[count].interactable = item.isOpen;
                themesBtn[count].transform.GetChild(0).GetComponent<Image>().color = item.isSelect ? enabledColor : disabledColor;
                count++;
            }
        }
    }
}
