using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class StoreTitleManager : MonoBehaviour
{
    public GameObject popUpBackground;
    public GameObject popUp;
    public TextMeshProUGUI popUpText;
    public int selectedThemeIdx;
    private ThemeSelectManager themeSelectManager;

    [Header("사운드 설정UI")]
    public GameObject BlackScreen; // 배경(검은 배경)
    public GameObject SoundSettingScreen; // 사운드 설정 화면
    public Slider sfxSlider; // 효과음 슬라이더
    public Slider bgmSlider; // 배경음 슬라이더

    // 메인 화면으로 이동 버튼
    private void Awake()
    {
        themeSelectManager = gameObject.GetComponent<ThemeSelectManager>();
        SoundManager.Instance.SoundSliderSetting(sfxSlider, bgmSlider); // 사운드 슬라이더 설정

        SoundManager.Instance.ChangePlayListClip("Shop_bgm");
    }
    public void InputMainScreenBtn()
    {
        SceneManager.LoadScene("MainScreen");
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
    
    // 광고 시청으로 테마 잠금 해제 버튼
    public void UnlockThemeSawAdBtn(int idx)
    {
        selectedThemeIdx = idx; // 선택된 테마 인덱스 저장
        RewardsBanner.Instance.ShowRewardedAd(DataManager.Instance.themeNames[selectedThemeIdx]); // 광고 시청
        // OpenPopUp(); // 팝업창 활성화
    }

    // 팝업창 활성화
    public void OpenPopUp()
    {
        popUpText.text = string.Format("{0}스킨 획득이\n완료되었습니다!", DataManager.Instance.themeNames[selectedThemeIdx]); // 텍스트 설정
        popUpBackground.SetActive(true); // 팝업 배경 활성화
        popUp.SetActive(true); // 팝업 활성화
        popUp.transform.localScale = Vector3.zero; // 초기 스케일을 0으로 설정        

        // 통통 튀는 효과로 등장, 구매 사운드 재생
        popUp.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() => SoundManager.Instance.PlaySound("BuySound"));
        themeSelectManager.UpdateThemeStore(); // 상점 테마 선택 버튼 업데이트
    }
    // 팝업 닫기 버튼
    public void ClosePopUpBtn()
    {
        themeSelectManager.UpdateThemeStore(); // 상점 테마 선택 버튼 업데이트
        // 통통 튀는 효과로 사라짐
        popUp.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                popUp.SetActive(false);
                popUpBackground.SetActive(false);
            }); // 애니메이션 완료 후 비활성화
    }

    // 테마 즉시 장착 버튼
    public void EquipThemeBtn()
    {
        DataManager.Instance.SelectTheme(DataManager.Instance.themeNames[selectedThemeIdx]);
        ClosePopUpBtn(); // 팝업창 닫기
    }
}
