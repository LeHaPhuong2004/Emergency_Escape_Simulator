
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
    public void ExittoMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void Replay()
    {
        FireSpread.currentFireCount = 0;
        // Cách 1: Nạp lại Scene đang hoạt động bằng Tên
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        // Đảm bảo TimeScale quay về 1 nếu bạn có dùng Pause game
        Time.timeScale = 1f;
    }
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
