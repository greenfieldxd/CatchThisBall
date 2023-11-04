using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Source.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Terrain[] terrains;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private ParticleSystem effectBig;
    [SerializeField] private GameColor[] gameColors;
    [SerializeField] private Camera cam;
    
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
        
        _screen.StartButton.onClick.AddListener(StartGame);
        _screen.ChangeLanguage.onClick.AddListener(ChangeLanguage);
        _screen.ChangeSound.onClick.AddListener(ChangeSound);
        _screen.ChangeColor.onClick.AddListener(ClickColorButton);

        YandexGame.GetDataEvent += UpdateData;
        YandexGame.RewardVideoEvent += ChangeRandomColor;
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

        if (_player._totalHits > YandexGame.savesData.bestScore)
        {
            YandexGame.savesData.bestScore = _player._totalHits;
            YandexGame.NewLeaderboardScores("leader", YandexGame.savesData.bestScore);
            _screen.BestScoreText.text = YandexGame.savesData.bestScore.ToString();
            YandexGame.SaveProgress();
        }
        
        _player.ResetHits();

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
        OtherExtensions.TransformPunchScale(_screen.ChangeLanguage.transform);
        _yandexGame.SwitchLanguage();
    }

    private void ChangeSound()
    {
        YandexGame.savesData.isSound = !YandexGame.savesData.isSound;
        
        OtherExtensions.TransformPunchScale(_screen.ChangeSound.transform);
        foreach (var source in _audios) source.volume = YandexGame.savesData.isSound ? 1 : 0;
        _screen.SoundTextUpdate();
        YandexGame.SaveProgress();
    }

    private void ClickColorButton()
    {
        YandexGame.RewVideoShow(0);
        OtherExtensions.TransformPunchScale(_screen.ChangeColor.transform);
    }

    private void ChangeRandomColor(int id)
    {
        var gameColor = gameColors.First(x => x.type == YandexGame.savesData.colorType);
        var list = gameColors.Where(x => x != gameColor);
        
        Random random = new Random();
        var newGameColor = list.OrderBy(x => random.Next()).ToArray().First();
        YandexGame.savesData.colorType = newGameColor.type;
        YandexGame.SaveProgress();

        cam.backgroundColor = newGameColor.camColor;
        
        ParticleSystem.MainModule main1 = effect.main;
        main1.startColor = gameColor.effectColor; 
        
        ParticleSystem.MainModule main2 = effectBig.main;
        main2.startColor = gameColor.bigEffectColor; 
        
        foreach (var terrain in terrains)
        {
            terrain.materialTemplate = newGameColor.mat;
        }

        foreach (var mesh in meshRenderers)
        {
            mesh.material = newGameColor.mat;
        }
    }

    private void LoadGameColor()
    {
        var gameColor = gameColors.First(x => x.type == YandexGame.savesData.colorType);
        
        cam.backgroundColor = gameColor.camColor;

        ParticleSystem.MainModule main1 = effect.main;
        main1.startColor = gameColor.effectColor; 
        
        ParticleSystem.MainModule main2 = effectBig.main;
        main2.startColor = gameColor.bigEffectColor; 
        
        foreach (var terrain in terrains)
        {
            terrain.materialTemplate = gameColor.mat;
        }

        foreach (var mesh in meshRenderers)
        {
            mesh.material = gameColor.mat;
        }
    }
    

    private void UpdateData()
    {
        LoadGameColor();
        foreach (var source in _audios) source.volume = YandexGame.savesData.isSound ? 1f : 0f;
        _screen.SoundTextUpdate();
        _screen.BestScoreText.text = YandexGame.savesData.bestScore.ToString();
    }

    [Serializable]
    public class GameColor
    {
        public ColorType type;
        public Material mat;
        public Color effectColor;
        public Color bigEffectColor;
        public Color camColor;
    }
}
