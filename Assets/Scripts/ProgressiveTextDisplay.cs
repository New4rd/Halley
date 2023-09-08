using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressiveTextDisplay : MonoBehaviour
{
    [SerializeField] private float displaySpeed;
    
    private TextMeshProUGUI _textField;

    private string _currentText;
    private bool _runningDisplay = false;
    
    private void Awake()
    {
        _textField = GetComponent<TextMeshProUGUI>();
    }

    private float _timeBeforeDisplay;
    
    private void Update()
    {
        if (_runningDisplay)
        {
            if (_textField.maxVisibleCharacters < _currentText.Length)
            {
                if (Time.time > _timeBeforeDisplay)
                {
                    _timeBeforeDisplay = Time.time + displaySpeed;
                    _textField.maxVisibleCharacters++;
                    MusicManager.Instance.PlayBoop();
                }
            }
            else
            {
                _runningDisplay = false;
            }
        }
    }

    public void DisplayText(string text)
    {
        string completedText = string.Format(text, GameManager.Instance.PlayerName);
        _textField.maxVisibleCharacters = 0;
        _currentText = completedText;
        _textField.text = completedText;
        _runningDisplay = true;
    }

    public void DisplayAllText()
    {
        _textField.maxVisibleCharacters = _currentText.Length;
        _runningDisplay = false;
    }

    public bool DisplayIsDone()
    {
        return !_runningDisplay;
    }

    public void StopDisplay()
    {
        _textField.text = "";
    }
}
