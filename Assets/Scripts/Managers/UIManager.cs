using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu MainMenu;
    
    protected override void Awake()
    {
        base.Awake();
        
        GameManager.Instance.OnGameStateChanged.AddListener(GameStateChangedHandler);
    }
    
    private void GameStateChangedHandler(GameManager.GameState currentState, GameManager.GameState previous)
    {
        if (currentState == previous) 
            return;
        
        switch (currentState)
        {
            case GameManager.GameState.PREGAME:
                MainMenu.gameObject.SetActive(true);
                break;
            case GameManager.GameState.MISSION:
                MainMenu.gameObject.SetActive(false);
                break;
            case GameManager.GameState.ENCOUNTER:
                break;
            case GameManager.GameState.SPACESTATION:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }
}
