using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{    
    public void InputExit()
    {
        Application.Quit();
    }

    public void InputGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InputStoreBtn()
    {
        SceneManager.LoadScene("Store");
    }
    
    public void InputMainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
}
