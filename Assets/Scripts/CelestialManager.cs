using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CelestialManager : MonoBehaviour
{
    public static CelestialManager Instance;

    [SerializeField] private Transform system;
    [SerializeField] private TextHandler _textHandler;

    private List<Celestial> _celestials = new List<Celestial>();
    
    public GameObject camera;
    
    private Vector3 _initialCameraPos;
    private Quaternion _initialCameraRot;
    
    public CelestialType PlanetFocus { get; set; } = CelestialType.NONE;

    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        InitialAcceleration();
    }


    private float _revolutionFactor;
    public void IncreaseRevolutionSpeed(float amount)
    {
        _revolutionFactor = 1 / amount;
        foreach (Celestial celestial in _celestials)
        {
            celestial.MultiplyRevolutionSpeed(amount);
        }
    }

    public void ReinitializeRevolutionCoefficient()
    {
        foreach (Celestial celestial in _celestials)
        {
            celestial.MultiplyRevolutionSpeed(_revolutionFactor);
            if (celestial.TryGetComponent(out TrailRenderer trail))
                trail.enabled = true;
        }
    }

    private void InitialAcceleration()
    {
        IncreaseRevolutionSpeed(100f);
        Invoke(nameof(ReinitializeRevolutionCoefficient), 3f);
    }
    
    public void CenterCameraOnObject(GameObject gameObject)
    {
        GameManager.Instance.SetState(GameState.CELESTIAL_FOCUS);
        
        _initialCameraPos = camera.transform.position;
        _initialCameraRot = camera.transform.rotation;
        
        camera.transform.parent = gameObject.transform.GetChild(0);
        camera.transform.localPosition = Vector3.zero + (gameObject.transform.GetChild(0).right * gameObject.transform.parent.localScale.x * 10f);
        camera.transform.LookAt(gameObject.transform);
        
        DisableAllTrails();
    }

    private void EnableAllTrails()
    {
        foreach (Celestial celestial in _celestials)
        {
            celestial.EnableTrail();
        }
    }
    
    private void DisableAllTrails()
    {
        foreach (Celestial celestial in _celestials)
        {
            celestial.DisableTrail();
        }
    }
    
    public void ReinitializeCamera()
    {
        camera.transform.SetParent(null);
        camera.transform.position = _initialCameraPos;
        camera.transform.rotation = _initialCameraRot;

        GameManager.Instance.SetState(GameState.OVERVIEW);
        EnableAllTrails();
    }

    public void AddCelestial(Celestial celestial)
    {
        _celestials.Add(celestial);
    }

    public GameObject GetActiveCelestial()
    {
        return GetCelestialByType(PlanetFocus);
    }
    
    private GameObject GetCelestialByType(CelestialType type)
    {
        return (
            from celestial in system.GetComponentsInChildren<Celestial>()
            where celestial.GetCelestialType() == type
            select celestial.gameObject).FirstOrDefault();
    }
    
    public TextHandler GetTextHandler()
    {
        return _textHandler;
    }
}
