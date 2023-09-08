using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public static Title Instance;
    
    private bool _clickable;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && _clickable) || Input.GetKeyDown(KeyCode.Space))
        {
            ScenesManager.Instance.fade.FadeOut();
            Unclickable();
        }
    }

    public void Clickable() => _clickable = true;
    private void Unclickable() => _clickable = false;
}
