using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightyearSceneManager : MonoBehaviour
{
    public static LightyearSceneManager Instance;

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
