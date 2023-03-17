using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAutoResize : MonoBehaviour
{
    public float x = 1;
    public float y = 1;
    public float scale = 1;
    private void Start()
    {
        GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(x * scale, y * scale);
    }
}
