using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [TextArea]
    public string englishText;

    [TextArea]
    public string vietnameseText;

    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();

        Debug.Log("LocalizedText Awake: " + gameObject.name);
    }

    private void Start()
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        Debug.Log(
            "Language = " +
            LanguageManager.Instance.CurrentLanguage);

        if (LanguageManager.Instance.CurrentLanguage == 0)
        {
            textUI.text = englishText;
            Debug.Log("Set English");
        }
        else
        {
            textUI.text = vietnameseText;
            Debug.Log("Set Vietnamese");
        }
    }

}