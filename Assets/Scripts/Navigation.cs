
using UnityEngine;

using UnityEngine.SceneManagement;
public class Navigation : MonoBehaviour

   
{
    public GameObject menuCanvas;
    public GameObject settingCanvas;


   
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Setting()
    {
        menuCanvas.SetActive(false);
        settingCanvas.SetActive(true);
       
    }
    public void BackToMenu()
    {
        settingCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
