using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField]
    float _scrollSpeed;

    [SerializeField] 
    RectTransform _rectTransform;

    Vector3 _startPosition;

    void Start()
    {
        _startPosition = _rectTransform.anchoredPosition;
    }

    void Update()
    {
        _rectTransform.Translate(Vector3.right * _scrollSpeed * Time.deltaTime);

        if (_rectTransform.anchoredPosition.x > (_startPosition.x + 1028))
        {
            _rectTransform.anchoredPosition = _startPosition;
        }
    }
}
