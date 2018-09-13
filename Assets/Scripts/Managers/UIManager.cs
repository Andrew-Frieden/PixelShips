using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu MainMenu;
    [SerializeField] private Canvas TripleView;

    public Camera UICamera;
    
    protected override void Awake()
    {
        base.Awake();
        
        GameManager.Instance.OnGameStateChanged.AddListener(GameStateChangedHandler);
    }
    
    private void GameStateChangedHandler(GameManager.GamePhase currentPhase, GameManager.GamePhase previous)
    {
        if (currentPhase == previous) 
            return;
        
        switch (currentPhase)
        {
            case GameManager.GamePhase.PREGAME:
                MainMenu.gameObject.SetActive(true);
                TripleView.gameObject.SetActive(false);
                break;
            case GameManager.GamePhase.MISSION:
                MainMenu.gameObject.SetActive(false);
                TripleView.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentPhase), currentPhase, null);
        }
    }
}
