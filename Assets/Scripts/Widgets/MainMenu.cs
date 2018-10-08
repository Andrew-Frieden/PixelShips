using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI StartOrContinue;
    [SerializeField] private TextMeshProUGUI Settings;
    [SerializeField] private TextMeshProUGUI Reset;
    [SerializeField] private TextMeshProUGUI SaveData;

    private List<TextMeshProUGUI> menuText;
    private ISaveManager saveManager;

    private void Start () 
	{
        menuText = new List<TextMeshProUGUI>
        {
            StartOrContinue,
            Settings,
            Reset
        };

        saveManager = GameManager.Instance;
        SetupMenu();
	}

    private void SetupMenu()
    {
        if (saveManager.HasSaveFile)
        {
            StartOrContinue.text = "[Continue]";
            Reset.text = "[Reset]";

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
            Reset.text = "";
            SaveData.text = "";
        }
    }
	
    private void StartOrContinueClick()
    {
        GameManager.Instance.StartNewMission();
    }

    private void ResetClick()
    {
        saveManager.ResetSaveData();
        SetupMenu();
    }

    private void MenuSelected(TextMeshProUGUI text)
    {
        if (text == StartOrContinue)
        {
            StartOrContinueClick();
        }
        else if (text == Settings)
        {
            Debug.Log("Settings Selected");
        }
        else if (text == Reset)
        {
            ResetClick();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int result;

        foreach (var menuItem in menuText)
        {
            result = TMP_TextUtilities.FindIntersectingWord(menuItem, eventData.position, UIManager.Instance.UICamera);
            if (result >= 0)
            {
                MenuSelected(menuItem);
                return;
            }
        }
    }
}
