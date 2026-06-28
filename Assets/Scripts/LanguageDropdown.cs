using TMPro;
using UnityEngine;

public class LanguageDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    [Header("Texts To Update")]
    public LocalizedText[] localizedTexts;

    private void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("Language", 0);
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index)
    {
        LanguageManager.Instance.SetLanguage(index);

        foreach (LocalizedText text in localizedTexts)
        {
            if (text != null)
                text.UpdateText();
        }

        Debug.Log("Language Changed: " + dropdown.options[index].text);
    }
}