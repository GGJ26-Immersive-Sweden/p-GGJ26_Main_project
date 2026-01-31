using UnityEngine;
using TMPro;
using System;

public class InputFieldController : MonoBehaviour
{
    //
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameState _gameState;

    private void OnEnable()
    {
        _inputField.onValueChanged.AddListener(OnInputValueChangedHandler);
        GameEventManager.OnJoinRequestFailed += OnJoinRequestFailedHandler;
    }


    private void OnDisable()
    {
        _inputField.onValueChanged.RemoveListener(OnInputValueChangedHandler);
        GameEventManager.OnJoinRequestFailed -= OnJoinRequestFailedHandler;
    } 

    private void OnInputValueChangedHandler(string code)
    {
        if(_inputField)
        {
            //Debug.Log($"Updating: {code}");
            _gameState.SessionId = code;
        }
        
    }
    private void OnJoinRequestFailedHandler()
    {
        if(_inputField)
        {
            _inputField.text = "Ups, please try again";
            _gameState.SessionId = "";
        }
    }

}