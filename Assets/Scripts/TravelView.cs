using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TravelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private CanvasGroup canvasgroup;

    public CelestialType selectedCelestial { get; set; }
    
    public void SetPlanetName(string planetName)
    {
        string format = "Voyager jusqu'à {0} ?";
        string content = string.Format(format, planetName);
        confirmText.text = content;
    }

    public void Validation()
    {
        GameManager.Instance.SetState(GameState.OVERVIEW_ASK);
        CelestialManager.Instance.PlanetFocus = selectedCelestial;
        ScenesManager.Instance.fade.FadeOut();
    }

    public void HidePopup()
    {
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
    }

    public void DisplayPopup()
    {
        canvasgroup.alpha = 1;
        canvasgroup.interactable = true;
        canvasgroup.blocksRaycasts = true;
    }
}
