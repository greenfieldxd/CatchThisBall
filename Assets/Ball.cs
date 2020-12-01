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
    [SerializeField] private Animation _textAnimation;
    [SerializeField] private Text _textScore;
   
    private Player _player;
    private Rigidbody2D _rb;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
        
        if (col.CompareTag("Player"))
        {
            HitEffect();

            int randomValue = Random.Range(0, 2);

            if (randomValue == 0) transform.DOJump(new Vector2(Random.Range(_minX, _maxX), _minY), _jumpPower, 1, _duration);
            else transform.DOJump(new Vector2(Random.Range(-_minX, -_maxX), _minY), _jumpPower, 1, _duration);
        }
        else if (col.CompareTag("Floor"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HitEffect()
    {
        _player.AppendHit();
        _textScore.text = _player._totalHits.ToString();
        _textAnimation.Play("ScoreAnimation");
        
        _audio.pitch = Random.Range(1f, 1.3f);
        _audio.PlayOneShot(_hitSound);
    }
}
