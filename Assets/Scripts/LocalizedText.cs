using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [TextArea] public string englishText;
    [TextArea] public string vietnameseText;

    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    private void Start()
    {
        UpdateText();
    }

    private void LateUpdate()
    {
        // Nếu LanguageManager spawn muộn hơn
        // hoặc text chưa được gán thì tự cập nhật lại 1 lần
        if (textUI != null && string.IsNullOrEmpty(textUI.text))
        {
            UpdateText();
        }
    }

    public void UpdateText()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();

        if (textUI == null)
            return;

        if (LanguageManager.Instance == null)
            return;

        textUI.text =
            LanguageManager.Instance.CurrentLanguage == 0
            ? englishText
            : vietnameseText;
    }
}