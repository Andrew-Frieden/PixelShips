using TextSpace.Models;
using System;
using TMPro;
using UnityEngine;
using TextSpace.Framework.IoC;
using TextSpace.Services;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StartOrContinue;
    [SerializeField] private TextMeshProUGUI Settings;
    [SerializeField] private TextMeshProUGUI Reset;
    [SerializeField] private TextMeshProUGUI SaveData;

    private ISaveManager SaveManager => ServiceContainer.Resolve<ISaveManager>();

    private void Start () 
	{
        SetupMenu();
	}

    private void SetupMenu()
    {
        if (SaveManager.HasSaveFile)
        {
            StartOrContinue.text = "[Continue]";

            if (SaveManager.SaveFile is InvalidSaveState)
            {
                SaveData.text = "Invalid Save File Found.";
            }
            else
            {
                var save = SaveManager.SaveFile;
                SaveData.text = $"Current Save:{Environment.NewLine}Time:{save.SaveTime}{Environment.NewLine}Path:{SaveManager.SaveFilePath}";
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
        if (SaveManager.HasSaveFile)
        {
            if (SaveManager.SaveFile is InvalidSaveState)
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
        SaveManager.Delete();
        SetupMenu();
    }

}
