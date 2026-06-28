using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingSlider;

    private bool isLoading;

    public void LoadMap(string sceneName)
    {

        if (isLoading)
        {
          
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
  
        isLoading = true;

        if (loadingPanel == null)
            Debug.LogError("loadingPanel NULL");
        else
        {
      
            loadingPanel.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError("[SceneLoader] Scene load failed! Scene not found?");
            yield break;
        }

        Debug.Log("[SceneLoader] AsyncOperation created");

        while (!operation.isDone)
        {
            Debug.Log(
                $"[SceneLoader] Progress: {operation.progress} Done: {operation.isDone}");

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingSlider != null)
                loadingSlider.value = progress;
            else
                Debug.LogWarning("[SceneLoader] loadingSlider NULL");

            yield return null;
        }

        Debug.Log("[SceneLoader] Scene loaded successfully");

        isLoading = false;
    }
}