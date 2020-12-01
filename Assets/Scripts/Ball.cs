using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [Header("Config Ball")]
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [SerializeField] private float _minY;
    [SerializeField] private float _duration;
    [SerializeField] private float _jumpPower;

    [Header("References")] 
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private Animation _textAnimation;
    [SerializeField] private Animation _flashAnimation;
    [SerializeField] private Text _textScore;

    private CircleCollider2D _collider;
    private Player _player;
    private Tween _tween;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _collider = GetComponent<CircleCollider2D>();

        StartAnimation();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _tween.Kill();
            
            HitEffect();
            RandomJump();
        }
        else if (col.CompareTag("Floor"))
        {
            _collider.enabled = false;
            _tween.Kill();

            _textScore.text = "You Lose!";
            Die();
        }
    }

    private void HitEffect()
    {
        _player.AppendHit();
        _textScore.text = _player._totalHits.ToString();
        _textAnimation.Play("ScoreAnimation");
        _flashAnimation.Play("FlashAnimation");
        
        _audio.pitch = Random.Range(1.5f, 2f);
        _audio.PlayOneShot(_hitSound);
    }

    private void Die()
    {
        transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            StartAnimation();
            _player.ResetHits();
            _textScore.text = _player._totalHits.ToString();

            _audio.pitch = 2.5f;
            _audio.PlayOneShot(_dieSound);
        });
        
    }

    private void StartAnimation()
    {
        Sequence startAnim = DOTween.Sequence();

        startAnim.Append(transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase(Ease.InSine));
        startAnim.Append(transform.DOScale(Vector3.one , 0.5f).SetEase(Ease.InSine));
        startAnim.AppendCallback(() =>
        {
            _collider.enabled = true;
            RandomJump();
        });
    }

    private void RandomJump()
    {
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0) _tween = transform.DOJump(new Vector2(Random.Range(_minX, _maxX), _minY), _jumpPower, 1, _duration);
        else _tween = transform.DOJump(new Vector2(Random.Range(-_minX, -_maxX), _minY), _jumpPower, 1, _duration);
    }
}
