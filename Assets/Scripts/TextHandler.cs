using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    [SerializeField] private DisplaySpace displaySpace;
    [SerializeField] private ProgressiveTextDisplay textDisplay;

    [Header("Intro parameters")]
    [SerializeField] private Button validateButton;
    [SerializeField] private int inputFieldNb;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textField;
    
    private string[] lines;

    private int _displayedLine;

    private bool _displayStarted, _displayEnded, _fadeOutDone, _trigLoop;
    private float _initialTextTime = 5f, _postTextTime = 5f;
    
    public void Init()
    {
        switch (displaySpace)
        {
            case DisplaySpace.INTRODUCTION:
                lines = TextManager.Instance.GetIntroductionTextLines();
                break;
            case DisplaySpace.OUTRO:
                lines = TextManager.Instance.GetOutroductionTextLines();
                break;
            case DisplaySpace.LIGHTYEAR:
                lines = TextManager.Instance.GetLightyearTextLines(CelestialManager.Instance.PlanetFocus);
                break;
            case DisplaySpace.FOCUS: lines = TextManager.Instance.GetFocusTextLines(CelestialManager.Instance.PlanetFocus);
                break;
        }
        
        _initialTextTime = 2f;
        _postTextTime = 2f;
        _displayedLine = 0;

        _displayStarted = false;
        _displayEnded = false;
        _fadeOutDone = false;

        _trigLoop = true;
    }
    
    private void Update()
    {
        if (!_trigLoop) return;
        
        if ((GameManager.Instance.GetState() == GameState.LIGHTYEARS && displaySpace == DisplaySpace.LIGHTYEAR) ||
            (GameManager.Instance.GetState() == GameState.CELESTIAL_FOCUS && displaySpace == DisplaySpace.FOCUS) ||
            (GameManager.Instance.GetState() == GameState.INTRODUCTION && displaySpace == DisplaySpace.INTRODUCTION) ||
            (GameManager.Instance.GetState() == GameState.OUTRODUCTION && displaySpace == DisplaySpace.OUTRO))

        {
            if (_displayEnded && !_fadeOutDone)
            {
                _postTextTime -= Time.deltaTime;
                if (_postTextTime < 0)
                {
                    _fadeOutDone = true;
                    _trigLoop = false;
                    ScenesManager.Instance.fade.FadeOut();
                }
            }

            if (!_displayStarted)
            {
                _initialTextTime -= Time.deltaTime;
                if (_initialTextTime < 0)
                {
                    AskForNextLine();
                    _displayStarted = true;
                }
            }

            else
            {
                if (textDisplay.DisplayIsDone())
                {
                    if ((Input.touchCount > 0  && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
                    {
                        if (_displayedLine == inputFieldNb && displaySpace == DisplaySpace.INTRODUCTION)
                        {
                            InitializeTextField();
                        }
                        else AskForNextLine();
                    }
                    
                }

                else
                {
                    if ((Input.touchCount > 0  && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
                    {
                        textDisplay.DisplayAllText();
                    }
                }
            }
        }
    }
    
    public void EndLoop()
    {
        _trigLoop = false;
    }

    public void OnButtonNextLine()
    {
        if (inputField != null)
        {
            OnNameEntered(textField.text);

            if (textField.text.Contains("alle"))
            {
                textDisplay.DisplayText("Ce nom est déjà pris par mon amour. Désolé.");
                return;
            }

            if (textField.text.Contains("axim"))
            {
                textDisplay.DisplayText("Un joli nom... Mais il est déjà pris par mon créateur. Désolé.");
                return;
            }
            
            validateButton.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            AskForNextLine();
        }
    }

    private void AskForNextLine()
    {
        if (_displayedLine == lines.Length)
        {
            textDisplay.StopDisplay();
            _displayEnded = true;
        }
        else 
        {
            textDisplay.DisplayText(lines[_displayedLine]);
            _displayedLine++;
        }
    }

    private void InitializeTextField()
    {
        inputField.gameObject.SetActive(true);
    }

    private void OnNameEntered(string name)
    {
        string correctName = name.Replace("\n", " ");
        GameManager.Instance.SetName(correctName);
        validateButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }

    public void CheckName()
    {
        validateButton.gameObject.SetActive(textField.text.Length != 0);
    }
    
    enum DisplaySpace {
        LIGHTYEAR,
        FOCUS,
        INTRODUCTION,
        OUTRO
    }
}
