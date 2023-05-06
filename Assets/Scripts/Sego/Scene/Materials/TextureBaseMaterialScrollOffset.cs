using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TextureBaseMaterialScrollOffset : MonoBehaviour
{
    [SerializeField] List<Material> scrollMaterial = new List<Material>();
    public float offSetX, offSetY;
    private Vector2 scrollPosition;

    private void Update()
    {
        scrollPosition.x = offSetX * Time.deltaTime;
        scrollPosition.y = offSetY * Time.deltaTime;

        foreach (var element in scrollMaterial)
            element.mainTextureOffset += scrollPosition;
    }
} 