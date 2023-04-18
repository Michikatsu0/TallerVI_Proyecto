using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageScroll : MonoBehaviour
{
    [SerializeField] private List<RawImage> _img = new List<RawImage>();
    [SerializeField] Vector2 scrollPosition = new Vector2(0.1f, 0.1f);

    void Update()
    {
        foreach (var element in _img)
        {
            element.uvRect = new Rect(element.uvRect.position + scrollPosition * Time.deltaTime, element.uvRect.size);
        }
    }
}