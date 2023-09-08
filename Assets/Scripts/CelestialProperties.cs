using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CelestialProerties", menuName = "CelestialProperties", order = 1)]
public class CelestialProperties : ScriptableObject
{
    public float rotationCoefficient = 1;
    public float revolutionCoefficient = 1;
    [Space]
    public bool normalizeAllRotations;
    public float normalizedRotation = 1;
    [Space]
    public bool normalizeAllRevolutions;
    public float normalizedRevolution = 1;
    [Space]
    public List<CelestialInfos> celestialInfos;
}

[Serializable]
public class CelestialInfos
{
    public CelestialType celestial;
    
    [Header("Rotation & revolution relatives to Earth values")]
    public float rotationPeriod;
    public float revolutionPeriod;
}

public enum CelestialType
{
    SUN,
    MERCURY,
    VENUS,
    EARTH,
    MARS,
    JUPITER,
    SATURN,
    URANUS,
    NEPTUNE,
    EARTH_CLOUDS,
    NONE
}