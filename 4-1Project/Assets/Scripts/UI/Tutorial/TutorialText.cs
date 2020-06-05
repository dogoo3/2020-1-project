using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    Color32 alpha;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        alpha = spriterenderer.color;
    }

    private void OnEnable()
    {
        alpha.a = 0;
    }

    private void Update()
    {
        if (alpha.a < 255)
        {
            alpha.a += 5;
            spriterenderer.color = alpha;
        }
    }
}
