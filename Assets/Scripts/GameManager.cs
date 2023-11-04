using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Source.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    private Ball _ball;
    private Player _player;
    private UIScreen _screen;
    private YandexGame _yandexGame;
    private AudioSource[] _audios;
    
    void Start()
    {
        _ball = FindObjectOfType<Ball>();
        _player = FindObjectOfType<Player>();
        
        _screen = FindObjectOfType<UIScreen>();
        _audios = FindObjectsOfType<AudioSource>();
        _yandexGame = FindObjectOfType<YandexGame>();_screen = FindObjectOfType<UIScreen>();
        _yandexGame = FindObjectOfType<YandexGame>();

        foreach (var source in _audios) source.volume = YandexGame.savesData.isSound ? 1f : 0f;
        _screen.SoundTextUpdate();

        _screen.StartButton.onClick.AddListener(StartGame);
        _screen.ChangeLanguage.onClick.AddListener(ChangeLanguage);
        _screen.ChangeSound.onClick.AddListener(ChangeSound);

        _ball.OnBallDie += StopGame;
    }

    private void StartGame()
    {
        foreach (var hide in _screen.HideGameObjects)
        {
            var button = hide.GetComponent<Button>();
            if (button) button.interactable = false;

            hide.transform.DOScale(0, 0.3f);
        }
        
        _player.StartPlatform();
        _ball.StartBall();
    }

    private void StopGame()
    {
        YandexGame.FullscreenShow();
        
        _player.ResetPlatform();

        foreach (var hide in _screen.HideGameObjects)
        {
            var button = hide.GetComponent<Button>();

            hide.transform.DOScale(1, 1f).OnComplete(() =>
            {
                if (button) button.interactable = true;
            });
        }
    }

    private void ChangeLanguage()
    {
        Debug.Log("Language Click");
        OtherExtensions.TransformPunchScale(_screen.ChangeLanguage.transform);
        _yandexGame.SwitchLanguage();
    }

    private void ChangeSound()
    {
        Debug.Log("Sound Click");
        YandexGame.savesData.isSound = !YandexGame.savesData.isSound;
        
        OtherExtensions.TransformPunchScale(_screen.ChangeSound.transform);
        foreach (var source in _audios) source.volume = YandexGame.savesData.isSound ? 1 : 0;
        _screen.SoundTextUpdate();
        YandexGame.SaveProgress();
    }
}
