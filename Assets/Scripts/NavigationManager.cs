
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
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
    public GameObject asktutorial;
    
    bool isPaused = false;

    [Header("Loading")]
    public GameObject loadingPanel;
    public Slider loadingSlider;
    private void Start()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteKey("TutorialAsked");
#endif
    }
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
        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        isPaused = true;

        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Tắt tay khi pause
        Interaction interaction = FindFirstObjectByType<Interaction>();
        if (interaction != null && interaction.handModel != null)
        {
            interaction.handModel.SetActive(false);
        }
    }
    public void Resume()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        isPaused = false;

        pauseCanvas.SetActive(false);

        gameCanvas.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Play()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        if (PlayerPrefs.GetInt("TutorialAsked", 0) == 0)
        {
            menuCanvas.SetActive(false);
            asktutorial.SetActive(true);
        }
        else
        {
            menuCanvas.SetActive(false);
            gameModeCanvas.SetActive(true);
        }
    }
    public void Setting()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        menuCanvas.SetActive(false);
        settingCanvas.SetActive(true);
       
    }
    public void BackToMenu()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        CurrentCanvas.SetActive(false);
        TargetCanvas.SetActive(true);
    }

    public void SwitchMode()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        gameModeCanvas.SetActive(false);
        fireMapCanvas.SetActive(true);
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (displayedProgress < 1f)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                targetProgress,
                Time.deltaTime * 0.4f
            );

            loadingSlider.value = displayedProgress;

            if (displayedProgress >= 1f && operation.progress >= 0.9f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ChooseMap1()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        StartCoroutine(LoadSceneAsync("Wakeup"));
    }

    public void ChooseMap2()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        StartCoroutine(LoadSceneAsync("Office"));
    }
    public void ExittoMenu()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuScene");
    }
    public void tutorial()
    {

        

        SceneManager.LoadScene("Tutorial");
    }

    public void TutorialYes()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        PlayerPrefs.SetInt("TutorialAsked", 1);
        PlayerPrefs.Save();

        StartCoroutine(LoadSceneAsync("Tutorial"));
    
    }

    public void TutorialNo()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        PlayerPrefs.SetInt("TutorialAsked", 1);
        PlayerPrefs.Save();

        asktutorial.SetActive(false);
        gameModeCanvas.SetActive(true);
    }
    public void Replay()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        FireSpread.currentFireCount = 0;
        // nạp lại Scene đang hoạt động bằng tên
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        // reset
        Time.timeScale = 1f;
    }

    public void SettingInGame()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        pauseCanvas.SetActive(false);

        settingInGameCanvas.SetActive(true);
    }

    public void BackToPause()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        settingInGameCanvas.SetActive(false);

        pauseCanvas.SetActive(true);
    }

  
    public void Exit()
    {

        AudioManager.instance.PlaySFX(AudioManager.instance.clickButton);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
