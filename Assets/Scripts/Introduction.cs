using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    public static Introduction Instance;

    [SerializeField] private TextHandler textHandler;
    
    private void Awake()
    {
        Instance = this;
    }

    public TextHandler GetTextHandler()
    {
        return textHandler;
    }
}
