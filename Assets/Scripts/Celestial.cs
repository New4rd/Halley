using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Celestial : MonoBehaviour
{
    [SerializeField] private CelestialType celestialType;

    [Header("Self-rotation")]
    [SerializeField] private Transform rotationObject;
    [SerializeField] private bool computeRotation;

    [Header("Rotation around")]
    [SerializeField] private bool computeRevolution;
    [SerializeField] private Transform revolutionCenter;
    
    private CelestialInfos _celestialInfos;
    private float _rotationSpeed;
    private float _revolutionSpeed;
    
    private void Start()
    {
        if (celestialType != CelestialType.EARTH_CLOUDS) CelestialManager.Instance.AddCelestial(this);
        
        _celestialInfos = FindCelestialInfos();
        _rotationSpeed = GameManager.Instance.celestialProperties.normalizeAllRotations
            ? GameManager.Instance.celestialProperties.normalizedRotation
            : _celestialInfos.rotationPeriod;

        _revolutionSpeed = GameManager.Instance.celestialProperties.normalizeAllRevolutions
            ? GameManager.Instance.celestialProperties.normalizedRevolution
            : _celestialInfos.revolutionPeriod;
    }

    private void FixedUpdate()
    {
        if (computeRotation)
        {
            if (rotationObject != null)
            {
                rotationObject.transform.Rotate(
                    Vector3.forward * _rotationSpeed *
                    GameManager.Instance.celestialProperties.rotationCoefficient * Time.deltaTime,
                    Space.Self);
            }

            else
            {
                transform.Rotate(
                    Vector3.forward * _celestialInfos.rotationPeriod *
                    GameManager.Instance.celestialProperties.rotationCoefficient * Time.deltaTime,
                    Space.Self);
            }
        }

        if (computeRevolution)
        {
            transform.RotateAround(
                revolutionCenter.position, Vector3.up,
                _revolutionSpeed * GameManager.Instance.celestialProperties.revolutionCoefficient * Time.deltaTime);
        }
    }
    
    private CelestialInfos FindCelestialInfos()
    {
        return GameManager.Instance.celestialProperties.celestialInfos
            .FirstOrDefault(infos => infos.celestial == celestialType);
    }

    public void MultiplyRevolutionSpeed(float amount)
    {
        _revolutionSpeed *= amount;
    }

    public void MultiplyRotationSpeed(float amount)
    {
        _rotationSpeed *= amount;
    }
    
    public CelestialType GetCelestialType()
    {
        return celestialType;
    }

    public void DisableTrail()
    {
        if (TryGetComponent(out TrailRenderer trail)) trail.enabled = false;
    }

    public void EnableTrail()
    {
        if (TryGetComponent(out TrailRenderer trail)) trail.enabled = true;
    }
}

