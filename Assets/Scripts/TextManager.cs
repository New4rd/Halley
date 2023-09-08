using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;

    [SerializeField] private TextAsset introductionText;   
    [SerializeField] private TextAsset outroductionText;
    [SerializeField] private List<CelestialTexts> celestialTextsList;

    private void Awake()
    {
        Instance = this;
    }

    public CelestialTexts GetCelestialTexts(CelestialType type)
    {
        return celestialTextsList.FirstOrDefault(texts => texts.type == type);
    }

    public string[] GetFocusTextLines(CelestialType type)
    {
        string text = GetCelestialTexts(type).focusTextFile.text;
        return Regex.Split(text, "\n");
    }
    
    public string[] GetLightyearTextLines(CelestialType type)
    {
        string text = GetCelestialTexts(type).lightyearTextFile.text;
        return Regex.Split(text, "\n");
    }

    public string[] GetIntroductionTextLines()
    {
        string text = introductionText.text;
        return Regex.Split(text, "\n");
    }
    
    public string[] GetOutroductionTextLines()
    {
        string text = outroductionText.text;
        return Regex.Split(text, "\n");
    }
}

[Serializable]
public class CelestialTexts
{
    public CelestialType type;
    public TextAsset focusTextFile;
    public TextAsset lightyearTextFile;
}
