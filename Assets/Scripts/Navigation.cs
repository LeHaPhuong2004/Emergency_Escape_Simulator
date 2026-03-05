
using UnityEngine;

using UnityEngine.SceneManagement;
public class Navigation : MonoBehaviour

   
{
    public GameObject menuCanvas;
    public GameObject settingCanvas;
    public GameObject gameModeCanvas;
    public GameObject fireMapCanvas;

    public GameObject CurrentCanvas;
    public GameObject TargetCanvas;

    public void Play()

    {
        menuCanvas.SetActive(false);
        gameModeCanvas.SetActive(true);
       
    }
    public void Setting()
    {
        menuCanvas.SetActive(false);
        settingCanvas.SetActive(true);
       
    }
    public void BackToMenu()
    {
        CurrentCanvas.SetActive(false);
        TargetCanvas.SetActive(true);
    }
    public void SwitchMode()
    {
        gameModeCanvas.SetActive(false);
        fireMapCanvas.SetActive(true);
    }
    public void ChooseMapAndPlay()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
