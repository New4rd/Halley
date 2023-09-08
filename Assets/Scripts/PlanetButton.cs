using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetButton : MonoBehaviour
{
    [SerializeField] private TravelView travelView;
    [SerializeField] private CelestialType celestialType;
    [SerializeField] private string displayText;

    public void OnClick()
    {
        travelView.DisplayPopup();
        travelView.SetPlanetName(displayText);
        travelView.selectedCelestial = celestialType;
    }
}
