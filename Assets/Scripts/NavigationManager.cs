
using UnityEngine;

using UnityEngine.SceneManagement;
public class NavigationManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject settingCanvas;
    public GameObject gameModeCanvas;
    public GameObject fireMapCanvas;

    public GameObject CurrentCanvas;
    public GameObject TargetCanvas;
    public GameObject settingInGameCanvas;
    public GameObject pauseCanvas;
    public GameObject gameCanvas;
   
    
    bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        isPaused = true;

        gameCanvas.SetActive(false);

        pauseCanvas.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        isPaused = false;

        pauseCanvas.SetActive(false);

        gameCanvas.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
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
        SceneManager.LoadScene("Wakeup");
    }
    public void ExittoMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void Replay()
    {
        FireSpread.currentFireCount = 0;
        // nạp lại Scene đang hoạt động bằng tên
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        // reset
        Time.timeScale = 1f;
    }

    public void SettingInGame()
    {
        pauseCanvas.SetActive(false);

        settingInGameCanvas.SetActive(true);
    }

    public void BackToPause()
    {
        settingInGameCanvas.SetActive(false);

        pauseCanvas.SetActive(true);
    }

  
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
