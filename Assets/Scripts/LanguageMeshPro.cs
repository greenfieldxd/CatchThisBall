using TMPro;
using UnityEngine;
using YG;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LanguageMeshPro : MonoBehaviour
{
    [SerializeField] string ru;
    [SerializeField] string en;

    TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        YandexGame.SwitchLangEvent += SwitchLanguage;
        
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        SwitchLanguage(YandexGame.savesData.language);
    }

    private void OnDestroy() => YandexGame.SwitchLangEvent -= SwitchLanguage;

    public void SwitchLanguage(string lang)
    {
        switch (lang)
        {
            case "ru":
                _textMeshPro.text = ru;
                break;
            default:
                _textMeshPro.text = en;
                break;
        }
    }
}