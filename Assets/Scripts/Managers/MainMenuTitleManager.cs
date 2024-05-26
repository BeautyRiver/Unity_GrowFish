using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuTitleManager : MonoBehaviour
{
    public GameObject ThemeChangeScreen; // 테마 변경 화면
    public RectTransform ThemeItemRectTransform; // 테마 아이템 스크롤
    public ThemeSelectManager themeSelectManager; // 테마 선택 매니저

    [Header("사운드 설정UI")]
    public GameObject BlackScreen; // 배경(검은 배경)
    public GameObject SoundSettingScreen; // 사운드 설정 화면
    public Slider sfxSlider; // 효과음 슬라이더
    public Slider bgmSlider; // 배경음 슬라이더

    private void Start()
    {        
        SoundManager.Instance.SoundSliderSetting(sfxSlider, bgmSlider); // 사운드 슬라이더 설정
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

    // 사운드 설정 버튼
    public void InputSoundSettingBtn()
    {
        BlackScreen.SetActive(true); // 검은 배경 활성화
        BlackScreen.GetComponent<Image>().DOFade(1,0.5f);
        
        SoundSettingScreen.SetActive(true); // 사운드 설정 화면 활성화
        SoundSettingScreen.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정        
        SoundSettingScreen.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack); // 통통 튀는 효과로 등장
    }

    // 사운드 설정 화면 닫기 버튼
    public void InputSoundSettingCloseBtn()
    {
        BlackScreen.GetComponent<Image>().DOFade(0,0.5f).OnComplete(() => BlackScreen.SetActive(false));
        SoundSettingScreen.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
        .OnComplete(() => SoundSettingScreen.SetActive(false));
    }
    
}
