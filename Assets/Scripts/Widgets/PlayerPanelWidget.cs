using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelShips.Widgets;
using PixelShips.Verse;

public enum enumCombatStatus {
	DANGER, CLEAR, WARNING
}

public class PlayerPanelWidget : BaseWidget
{

    private int pHealth;
    private int pMaxHealth;
    private int pShields;
    private int pMaxShields;
    private string pName;
    private string pShipName;
	private enumCombatStatus pStatus;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI shipName;
    public Slider healthBar;
    public TextMeshProUGUI healthBarText;
    public Slider shieldBar;
    public TextMeshProUGUI shieldBarText;


    // Use this for initialization
    void Start()
    {
        pHealth = 50;
        pMaxHealth = 300;
        pShields = 100;
        pMaxShields = 150;
        pName = "Miles The Mighty";
        pShipName = "P. Walter Tugnut";

        setUIValues();

    }

    protected override void OnVerseUpdate(IGameState state)
    {
		pShipName = state.UserShip.Name;
        pHealth = state.UserShip.Hull;
        pMaxHealth = state.UserShip.MaxHull;

        setUIValues();
    }

    private void setUIValues()
    {
        setPlayerName(pName);
        setShipName(pShipName);
        setMaxHealth(pMaxHealth);
        setMaxShield(pMaxShields);
        setCurHealth(pHealth);
        setCurShield(pShields);
        setHealthBarText(pHealth,pMaxHealth);
        setShieldBarText(pShields, pMaxShields);
    }


    private void setHealthBarText(int cur, int max)
    {
        healthBarText.text = cur + "//" + max;
    }

    private void setShieldBarText(int cur, int max)
    {
        shieldBarText.text = cur + "//" + max;
    }

    private void setPlayerName(string name)
    {
        playerName.text = "CMDR " + name;
    }

    private void setShipName(string name)
    {
        shipName.text = "HMS " + name;
    }

    private void setCurHealth(int value)
    {
        healthBar.value = value;
    }

    private void setCurShield(int value)
    {
        shieldBar.value = value;
    }

    private void setMaxHealth(int value)
    {
        healthBar.maxValue = value;
    }

    private void setMaxShield(int value)
    {
        shieldBar.maxValue = value;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
