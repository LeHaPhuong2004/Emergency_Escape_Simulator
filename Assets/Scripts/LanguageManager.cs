using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public int CurrentLanguage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CurrentLanguage = PlayerPrefs.GetInt("Language", 0);
    }

    public void SetLanguage(int languageIndex)
    {
        CurrentLanguage = languageIndex;

        PlayerPrefs.SetInt("Language", languageIndex);
        PlayerPrefs.Save();
    }
}