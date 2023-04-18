using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TextureBaseMaterialScrollOffset : MonoBehaviour
{
    [SerializeField] List<Material> scrollMaterial = new List<Material>();
    [SerializeField] private Vector2 scrollPosition = new Vector2(0.1f, 0.1f);

    private void Update()
    {
        foreach (var element in scrollMaterial)
        {
            element.mainTextureOffset += Time.deltaTime * scrollPosition;
        }
    }
}
