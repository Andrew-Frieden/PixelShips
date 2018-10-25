using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StartOrContinue;
    [SerializeField] private TextMeshProUGUI Settings;
    [SerializeField] private TextMeshProUGUI Reset;
    [SerializeField] private TextMeshProUGUI SaveData;

    private ISaveManager saveManager;

    private void Start () 
	{
        saveManager = GameManager.Instance;
        SetupMenu();
	}

    private void SetupMenu()
    {
        if (saveManager.HasSaveFile)
        {
            StartOrContinue.text = "[Continue]";

            if (saveManager.SaveFile is InvalidSaveState)
            {
                SaveData.text = "Invalid Save File Found.";
            }
            else
            {
                var save = saveManager.SaveFile;
                SaveData.text = $"Current Save:{Environment.NewLine}Time:{save.SaveTime}{Environment.NewLine}Path:{saveManager.SavePath}";
            }
        }
        else
        {
            StartOrContinue.text = "[Begin]";
            SaveData.text = "";
        }
    }
	
    public void StartOrContinueClick()
    {
        if (saveManager.HasSaveFile)
        {
            if (saveManager.SaveFile is InvalidSaveState)
            {
                GameManager.Instance.BootstrapNewGame();
            }
            else
            {
                GameManager.Instance.ContinueFromSave();
            }
        }
        else
        {
            GameManager.Instance.BootstrapNewGame();
        }
    }

    public void DevStartClick()
    {
        GameManager.Instance.BootstrapNewGame(true);
    }

    public void SettingsClick()
    {
        Debug.Log("Settings Clicked.");
    }

    public void ResetClick()
    {
        saveManager.ResetSaveData();
        SetupMenu();
    }

}
