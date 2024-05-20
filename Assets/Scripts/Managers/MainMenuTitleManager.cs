using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuTitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen;
    public RectTransform ThemeItemRectTransform;
    public ThemeSelectManager themeSelectManager;

    private void Start()
    {
        SoundManager.Instance.ChangePlayListClip("MainMenu_bgm");
    }
    // 게임 종료 버튼
    public void InputExitBtn()
    {
        Application.Quit();
    }

    // 게임 시작 버튼
    public void InputGameStartBtn()
    {               
       SceneManager.LoadScene("GameScene_DefaultTheme");                
    }
    // 상점 화면으로 이동 버튼
    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    // 테마 변경 화면 활성화 버튼
    public void InputThemeScreenOnBtn()
    {      
        themeSelectManager.UpdateThemeMainMenu(); // 테마 선택 버튼 업데이트  
        ThemeChangeScreen.SetActive(true); // 오브젝트 활성화
        SetPositionX(ThemeItemRectTransform, 0); // rect(스크롤) 초기 위치로 설정         
        ThemeChangeScreen.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정
        // 통통 튀는 효과로 등장
        ThemeChangeScreen.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }
    
    // rect(스크롤) 위치 설정
    public void SetPositionX(RectTransform rectTransform, float newX)
    {
        Vector2 newPosition = rectTransform.anchoredPosition;
        newPosition.x = newX;
        rectTransform.anchoredPosition = newPosition;
    }

    // 테마 변경 화면 비활성화 버튼
    public void InputThemeScreenOffBtn()
    {
        // 통통 튀는 효과로 사라짐
        ThemeChangeScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => ThemeChangeScreen.SetActive(false)); // 애니메이션 완료 후 비활성화
    }

    
}
