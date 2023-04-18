using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TextureScrollOffset : MonoBehaviour
{
    [SerializeField] float scrollX = 0.1f;
    [SerializeField] float scrollY = 0.1f;
    [SerializeField] List<Material> scrollMaterial = new List<Material>();

    private Vector2 scrollPosition;
    private float offSetX = 0f;
    private float offSetY = 0f;

    private void Update()
    {
        offSetX = Time.time * scrollX;
        offSetY = Time.time * scrollY;

        scrollPosition.x = offSetX;
        scrollPosition.y = offSetY;

        foreach (var element in scrollMaterial)
        {
            element.mainTextureOffset = scrollPosition;
        }
    }
}
