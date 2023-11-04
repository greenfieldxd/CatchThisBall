using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIScreen : MonoBehaviour
{
    [field:SerializeField] public Button StartButton { get; private set; }
    [field:SerializeField] public Button ChangeColor { get; private set; }
    [field:SerializeField] public Button ChangeSound { get; private set; }
    [field:SerializeField] public Button ChangeLanguage { get; private set; }
    [field:SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
    [field:SerializeField] public TextMeshProUGUI BestScoreText { get; private set; }
    [field:SerializeField] public GameObject SoundOn { get; private set; }
    [field:SerializeField] public GameObject SoundOff { get; private set; }
    [field:SerializeField] public GameObject[] HideGameObjects { get; private set; }

    public void SoundTextUpdate()
    {
        SoundOn.SetActive(YandexGame.savesData.isSound);
        SoundOff.SetActive(!YandexGame.savesData.isSound);
    }
}