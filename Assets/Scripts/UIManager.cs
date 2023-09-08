using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Transform planetButtons;
    [SerializeField] private Transform buttonSet;

    private void Awake()
    {
        Instance = this;
        UpdateActiveButtons();
    }

    private void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.GetState() == GameState.OVERVIEW)
            {
                if (planetButtons.GetComponent<CanvasGroup>().interactable) HidePlanetsPanel();
                else ShowPlanetsPanel();
            }
        }
    }

    public void OnAskViewOpened() => GameManager.Instance.SetState(GameState.OVERVIEW_ASK);
    public void OnAskViewCanceled() => GameManager.Instance.SetState(GameState.OVERVIEW);
    
    public void ReinitializeCamera()
    {
        CelestialManager.Instance.ReinitializeCamera();
    }

    public void UpdateActiveButtons()
    {
        for (int i = 0; i < GameManager.Instance.progression; i++)
        {
            buttonSet.GetChild(i).gameObject.SetActive(true);
            buttonSet.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
    
    private void HidePlanetsPanel() => planetButtons.GetComponent<Animator>().SetTrigger("Disappear");
    private void ShowPlanetsPanel() => planetButtons.GetComponent<Animator>().SetTrigger("Appear");
}
