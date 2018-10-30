using System;
using Common;
using TextSpace.Controllers;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private Canvas TripleView;
    [SerializeField] private CommandViewController cmdCtrl;

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
                TripleView.gameObject.SetActive(true);
                MainMenu.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentPhase), currentPhase, null);
        }
    }
}
