using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    
    private bool _fadeInEnded, _fadeOutEnded;

    public void FadeIn()
    {
        AnimTrigger("FadeIn");
    }
    public void FadeOut()
    {
        AnimTrigger("FadeOut");
    }
    
    // animation event
    public void FadeInEnded()
    {
        switch (GameManager.Instance.GetState())
        {
            case GameState.TITLE:
                Title.Instance.Clickable();
                return;
            
            case GameState.INTRODUCTION:
                Introduction.Instance.GetTextHandler().Init();
                return;
            
            case GameState.CELESTIAL_FOCUS:
                CelestialManager.Instance.GetTextHandler().Init();
                return;
            
            case GameState.LIGHTYEARS:
                LightyearSceneManager.Instance.GetTextHandler().Init();
                return;
            
            case GameState.OUTRODUCTION:
                Introduction.Instance.GetTextHandler().Init();
                return;
        }
    }

    // animation event
    public void FadeOutEnded()
    {
        switch (GameManager.Instance.GetState())
        {
            case GameState.HEADPHONES:
                MusicManager.Instance.StartPlaying();
                StartCoroutine(ScenesManager.Instance.UnloadScene(
                    ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.HEADPHONES), () =>
                    {
                        StartCoroutine(ScenesManager.Instance.LoadScene(
                            ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.TITLE), () =>
                            {
                                GameManager.Instance.SetState(GameState.TITLE);
                                FadeIn();
                            }));
                    }));
                return;
            
            case GameState.TITLE:
                if (GameManager.Instance.introCompleted == 0)
                {
                    StartCoroutine(ScenesManager.Instance.UnloadScene(
                        ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.TITLE),
                        () =>
                        {
                            StartCoroutine(ScenesManager.Instance.LoadScene(
                                ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.INTRODUCTION),
                                () =>
                                {
                                    GameManager.Instance.SetState(GameState.INTRODUCTION);
                                    FadeIn();
                                }));
                        }));
                }
                else
                {
                    StartCoroutine(ScenesManager.Instance.UnloadScene(
                        ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.TITLE),
                        () =>
                        {
                            StartCoroutine(ScenesManager.Instance.LoadScene(
                                ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.SYSTEM),
                                () =>
                                {
                                    GameManager.Instance.SetState(GameState.OVERVIEW);
                                    FadeIn();
                                }));
                        }));
                    return;
                }
                
                return;
            
            case GameState.INTRODUCTION:
                StartCoroutine(ScenesManager.Instance.UnloadScene(
                    ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.INTRODUCTION),
                    () =>
                    {
                        StartCoroutine(ScenesManager.Instance.LoadScene(
                            ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.SYSTEM),
                            () =>
                            {
                                GameManager.Instance.SetIntroDone();
                                GameManager.Instance.SetState(GameState.OVERVIEW);
                                FadeIn();
                            }));
                    }));
                return;
            
            case GameState.OVERVIEW:
                break;
            
            case GameState.OVERVIEW_ASK:
                StartCoroutine(ScenesManager.Instance.LoadScene(
                    ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.LIGHTYEARS),
                    () =>
                    {
                        GameManager.Instance.SetState(GameState.LIGHTYEARS);
                        ScenesManager.Instance.HideScene(ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.SYSTEM));
                        FadeIn();
                    }));
                return;
            
            case GameState.LIGHTYEARS:
                StartCoroutine(ScenesManager.Instance.UnloadScene(
                    ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.LIGHTYEARS),
                    () =>
                    {
                        if (CelestialManager.Instance.PlanetFocus == CelestialType.EARTH_CLOUDS)
                        {
                            GameManager.Instance.SetState(GameState.OUTRODUCTION);
                            StartCoroutine(ScenesManager.Instance.UnloadScene(
                                ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.SYSTEM), () =>
                                {
                                    StartCoroutine(ScenesManager.Instance.LoadScene(
                                        ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.OUTRODUCTION), () =>
                                        {
                                            StartCoroutine(MusicManager.Instance.StartDecreasingVolume(() => MusicManager.Instance.StartPlayingOutroMusic()));
                                            FadeIn();
                                        }));
                                }));
                        }
                        
                        else
                        {
                            GameManager.Instance.SetState(GameState.CELESTIAL_FOCUS);
                            CelestialManager.Instance.IncreaseRevolutionSpeed(1f);
                            ScenesManager.Instance.DisplayScene(
                                ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.SYSTEM));
                            CelestialManager.Instance.CenterCameraOnObject(
                                CelestialManager.Instance.GetActiveCelestial());
                            FadeIn();
                        }
                    }));
                return;
            
            case GameState.CELESTIAL_FOCUS:
                if ((int)CelestialManager.Instance.PlanetFocus == GameManager.Instance.progression
                    && CelestialManager.Instance.PlanetFocus != CelestialType.EARTH_CLOUDS)
                {
                    GameManager.Instance.IncreaseProgression();
                }
                GameManager.Instance.SetState(GameState.OVERVIEW);
                CelestialManager.Instance.ReinitializeCamera();
                CelestialManager.Instance.ReinitializeRevolutionCoefficient();
                UIManager.Instance.UpdateActiveButtons();
                FadeIn();
                return;

            case GameState.OUTRODUCTION:
            {
                StartCoroutine(ScenesManager.Instance.UnloadScene(
                    ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.OUTRODUCTION), () =>
                    {
                        StartCoroutine(ScenesManager.Instance.LoadScene(
                            ScenesManager.Instance.GetSceneName(ScenesManager.SceneType.TITLE),
                            () =>
                            {
                                GameManager.Instance.SetState(GameState.TITLE);
                                FadeIn();
                            }));
                    }));
                return;
            }
                
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    void AnimTrigger(string triggerName)
    {
        foreach(AnimatorControllerParameter p in fadeAnimator.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                fadeAnimator.ResetTrigger(p.name);
        fadeAnimator.SetTrigger(triggerName);
    }
}
