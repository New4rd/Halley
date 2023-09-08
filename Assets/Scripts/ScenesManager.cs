using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    public Fader fade;
    
    private void Awake()
    {
        Instance = this;
        StartCoroutine(LoadScene(GetSceneName(SceneType.HEADPHONES), () =>
        {
            fade.FadeIn();
            Invoke(nameof(FadeOut), 6f);
        }));
    }

    private void FadeOut() => fade.FadeOut();
    
    public IEnumerator LoadScene(string sceneName, Action onLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        onLoad.Invoke();
    }

    public IEnumerator UnloadScene(string sceneName, Action onLoad)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.None);
        yield return new WaitUntil(() => asyncLoad.isDone);
        onLoad.Invoke();
    }

    public string GetSceneName(SceneType sceneType)
    {
        return sceneType switch
        {
            SceneType.TITLE => "TitleScene",
            SceneType.INTRODUCTION => "IntroductionScene",
            SceneType.SYSTEM => "SystemScene",
            SceneType.LIGHTYEARS => "LightyearScene",
            SceneType.MAIN => "MainScene",
            SceneType.OUTRODUCTION => "Outroduction",
            SceneType.HEADPHONES => "HeadphonesScene",
            _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
        };
    }

    public void HideScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (obj.TryGetComponent<Camera>(out Camera cam)) cam.enabled = false;
            foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
        }
    }
    
    public void DisplayScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (obj.TryGetComponent<Camera>(out Camera cam)) cam.enabled = true;
            foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = true;
            }
        }
    }
    
    public enum SceneType
    {
        TITLE,
        INTRODUCTION,
        MAIN,
        SYSTEM,
        LIGHTYEARS,
        OUTRODUCTION,
        HEADPHONES
    }
}
