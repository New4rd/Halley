using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CelestialProperties celestialProperties;
    private GameState _state = GameState.HEADPHONES;

    public string PlayerName { get; private set; }
    public int progression { get; private set; } = 1;
    public int introCompleted { get; private set; } = 0;
    
    private void Awake()
    {
        Instance = this;
        if (!PlayerPrefs.HasKey("progression")) PlayerPrefs.SetInt("progression", 1);
        if (!PlayerPrefs.HasKey("name")) PlayerPrefs.SetString("name", "");
        if (!PlayerPrefs.HasKey("introDone")) PlayerPrefs.SetInt("introDone", 0);
    }

    public void SetIntroDone()
    {
        introCompleted = 1;
        PlayerPrefs.SetInt("introDone", 1);
    }
    
    public void SetName(string name)
    {
        PlayerName = name;
        PlayerPrefs.SetString("name", name);
    }

    public void IncreaseProgression()
    {
        progression++;
        PlayerPrefs.SetInt("progression", progression);
    }
     
    public GameState GetState()
    {
        return _state;
    }

    public void SetState(GameState state)
    {
        _state = state;
    }
}

public enum GameState
{
    TITLE,
    INTRODUCTION,
    OVERVIEW,
    OVERVIEW_ASK,
    LIGHTYEARS,
    CELESTIAL_FOCUS,
    OUTRODUCTION,
    HEADPHONES
}
