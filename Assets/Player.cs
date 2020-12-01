using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera _2dCamera;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    public int _totalHits { get; private set; }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition; // позиция мыши в координатах камеры
        Vector3 mouseWorldPos = _2dCamera.ScreenToWorldPoint(mousePos); // позиция мыши в координатах мира
        //Debug.Log("mousePos: " + mousePos + ", mouseWorldPos: " + mouseWorldPos);

        float xPlatform = mouseWorldPos.x; // значение x координат мыши
        float clampedPlatformX =
            Mathf.Clamp(xPlatform, _minX, _maxX); // обрезаем координаты платформы по x до нужных нам значений
        float yPlatform = transform.position.y;

        transform.position = new Vector3(clampedPlatformX, yPlatform, 0);
    }

    public void AppendHit()
    {
        _totalHits += 1;
    }
}