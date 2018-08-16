using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu MainMenu;
    [SerializeField] private TripleView TripleView;

    public Camera UICamera;
    
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
                TripleView.gameObject.SetActive(false);
                break;
            case GameManager.GameState.MISSION:
                MainMenu.gameObject.SetActive(false);
                TripleView.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }
}
