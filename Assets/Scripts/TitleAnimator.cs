using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleAnimator : MonoBehaviour
{
    public GameObject title;
    public GameObject btns;
    void Start()
    {
        DOTween.KillAll();
        title.transform.DOMove(new Vector3(0, 2.7f, -1), 2f).SetEase(Ease.OutBounce);
        btns.GetComponent<RectTransform>().DOAnchorPosY(-4, 0.5f).SetDelay(0.5f);
    }

    public void InputExit()
    {
        Application.Quit();
    }

    public void InputGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
