using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelShips.Widgets;
using PixelShips.Verse;

public enum buttonActions {
	HAIL,SCAN,TARGET,ATTACK,DISMISS,INTERACT
}

public class InteractionPanelWidget :  BaseWidget{

	private string interactionLabel;
	private bool rButtonEnabled;
	private bool mrButtonEnabled;
	private bool mlButtonEnabled;
	private bool lButtonEnabled;

	public TextMeshProUGUI interactionText;
	public TextMeshProUGUI rButtonText;
	public TextMeshProUGUI mrButtonText;
	public TextMeshProUGUI mlButtonText;
	public TextMeshProUGUI lButtonText;

	public Button rButton;
	public Button mrButton;
	public Button mlButton;
	public Button lButton;

	public buttonActions rButtonAction;
	public buttonActions mrButtonAction;
	public buttonActions mlButtonAction;
	public buttonActions lButtonAction;


	// Use this for initialization
	void Start () {
		interactionLabel = "You Selected: [Ship]";
		rButtonAction = buttonActions.DISMISS;
		mrButtonAction = buttonActions.TARGET;
		mlButtonAction = buttonActions.HAIL;
		lButtonAction = buttonActions.SCAN;

		rButtonEnabled = false;
		mrButtonEnabled = true;
		mlButtonEnabled = true;
		lButtonEnabled = true;

		updateUI();
		
	}
	private void setTextMeshText(TextMeshProUGUI textMesh, string text){
		textMesh.text = text;
	}

	private void setButtonEnabled(Button button, bool enabled){
		if (enabled){
			button.gameObject.SetActive(true);
		} else {
			button.gameObject.SetActive(false);
		}
	}

	public void handleButtonClick(Button button){
		Debug.Log( button.ToString() + " Clicked!");
		if (button == lButton){
			Debug.Log( generateButtonText(lButtonAction) + "!");
		}



	}

	private string generateButtonText(buttonActions action){

		switch(action){
			case buttonActions.HAIL :
				return "HAIL";
			case buttonActions.SCAN :
				return "SCAN";
			case buttonActions.ATTACK:
				return "PEW PEW";
			case buttonActions.TARGET:
				return "TARGET";
			case buttonActions.DISMISS:
				return "BAIL";
			case buttonActions.INTERACT:
				return "PRESS BUTTON";
			default:
				return "BROKEN";
		}

	}

	private void updateUI(){
		setTextMeshText(interactionText, interactionLabel);
		setTextMeshText(rButtonText, generateButtonText(rButtonAction));
		setTextMeshText(mrButtonText, generateButtonText(mrButtonAction));
		setTextMeshText(mlButtonText, generateButtonText(mlButtonAction));
		setTextMeshText(lButtonText, generateButtonText(lButtonAction));

		setButtonEnabled(rButton, rButtonEnabled);
		setButtonEnabled(mrButton, mrButtonEnabled);
		setButtonEnabled(mlButton, mlButtonEnabled);
		setButtonEnabled(lButton, lButtonEnabled);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
