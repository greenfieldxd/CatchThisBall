using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera _2dCamera;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    public int _totalHits { get; private set; }
    
    private bool _isStarted = false;


    void Update()
    {
        if (!_isStarted) return;
        
        Vector3 mousePos = Input.mousePosition; // позиция мыши в координатах камеры
        Vector3 mouseWorldPos = _2dCamera.ScreenToWorldPoint(mousePos); // позиция мыши в координатах мира

        float xPlatform = mouseWorldPos.x; // значение x координат мыши
        float clampedPlatformX =
            Mathf.Clamp(xPlatform, _minX, _maxX); // обрезаем координаты платформы по x до нужных нам значений
        float yPlatform = transform.position.y;

        transform.position = new Vector3(clampedPlatformX, yPlatform, 0);
    }

    public void StartPlatform()
    {
        _isStarted = true;
    }

    public void ResetPlatform()
    {
        _isStarted = false;
        transform.DOMoveX(0, 1f).SetEase(Ease.InSine);
    }

    public void AppendHit()
    {
        _totalHits += 1;
    }

    public void ResetHits()
    {
        _totalHits = 0;
    }
}