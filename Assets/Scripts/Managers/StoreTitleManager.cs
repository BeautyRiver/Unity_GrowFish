using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;



public class StoreTitleManager : MonoBehaviour
{
    public GameObject popUpBackground;
    public GameObject popUp;
    public TextMeshProUGUI popUpText;
    public int selectedThemeIdx;
    private ThemeSelectManager themeSelectManager;

    // ���� ȭ������ �̵� ��ư
    private void Awake()
    {
        themeSelectManager = gameObject.GetComponent<ThemeSelectManager>();
        SoundManager.Instance.ChangePlayListClip("Shop_bgm");
    }
    public void InputMainScreenBtn()
    {
        SceneManager.LoadScene("MainScreen");
    }

    // ���� ��û���� �׸� ��� ���� ��ư
    public void UnlockThemeSawAdBtn(int idx)
    {
        selectedThemeIdx = idx; // ���õ� �׸� �ε��� ����
        RewardsBanner.Instance.ShowRewardedAd(DataManager.Instance.themeNames[selectedThemeIdx]); // ���� ��û
        OpenPopUp(); // �˾�â Ȱ��ȭ
    }

    // �˾�â Ȱ��ȭ
    public void OpenPopUp()
    {
        popUpText.text = string.Format("{0}��Ų ȹ����\n�Ϸ�Ǿ����ϴ�!", DataManager.Instance.themeNames[selectedThemeIdx]); // �ؽ�Ʈ ����
        popUpBackground.SetActive(true); // �˾� ��� Ȱ��ȭ
        popUp.SetActive(true); // �˾� Ȱ��ȭ
        popUp.transform.localScale = Vector3.zero; // �ʱ� �������� 0���� ����        
        
        // ���� Ƣ�� ȿ���� ����, ���� ���� ���
        popUp.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() => SoundManager.Instance.PlaySound("BuySound"));
        themeSelectManager.UpdateThemeStore(); // ���� �׸� ���� ��ư ������Ʈ
    }
    // �˾� �ݱ� ��ư
    public void ClosePopUpBtn()
    {
        themeSelectManager.UpdateThemeStore(); // ���� �׸� ���� ��ư ������Ʈ
        // ���� Ƣ�� ȿ���� �����
        popUp.transform.DOScale(0, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                popUp.SetActive(false);
                popUpBackground.SetActive(false);
            }); // �ִϸ��̼� �Ϸ� �� ��Ȱ��ȭ
    }

    // �׸� ��� ���� ��ư
    public void EquipThemeBtn()
    {
        DataManager.Instance.SelectTheme(DataManager.Instance.themeNames[selectedThemeIdx]);        
        ClosePopUpBtn(); // �˾�â �ݱ�
    }
}
