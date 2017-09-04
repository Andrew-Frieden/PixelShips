using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPanelWidget : MonoBehaviour {

	private int pHealth;
	private int pMaxHealth;
	private int pShields;
	private int pMaxShields;
	private string pName;
	private string pShipName;

	public TextMeshProUGUI playerName;
	public TextMeshProUGUI shipName;
	public Slider healthBar;
	public TextMeshProUGUI healthBarText;
	public Slider shieldBar;
	public TextMeshProUGUI shieldBarText;


	// Use this for initialization
	void Start () {
		pHealth = 50;
		pMaxHealth = 300;
		pShields = 100;
		pMaxShields = 150;
		pName = "Miles The Mighty";
		pShipName = "HMS P. Walter Tugnut";

		setPlayerName(pName);
		setShipName(pShipName);
		setMaxHealth(pMaxHealth);
		setMaxShield(pMaxShields);
		setCurHealth(pHealth);
		setCurShield(pShields);
		setHealthBarText(pHealth + "//" + pMaxHealth);
		setShieldBarText(pShields + "//" + pMaxShields);

	}

	void setHealthBarText(string text){
		healthBarText.text =  text;
	}

	void setShieldBarText(string text){
		shieldBarText.text =  text;
	}
	
	void setPlayerName(string name){
		playerName.text = name;
	}

	void setShipName(string name){
		shipName.text = name;
	}

	void setCurHealth(int value){
		healthBar.value = value;
	}

	void setCurShield(int value){
		shieldBar.value = value;
	}

	void setMaxHealth(int value){
		healthBar.maxValue = value;
	}

	void setMaxShield(int value){
		shieldBar.maxValue = value;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
