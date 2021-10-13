﻿// 배경화면에 대한 설정

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    public GameObject gameBackground;
    private SpriteRenderer gameBackgroundSpriteRenderer;

    void Start()
    {
        gameBackgroundSpriteRenderer = gameBackground.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(gameBackgroundSpriteRenderer, 0.005f));
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer, float amount)
    {
        Color color = spriteRenderer.color;
        while(color.a > 0.0f)   // color는 투명도
        {
            color.a -= amount;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(amount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
