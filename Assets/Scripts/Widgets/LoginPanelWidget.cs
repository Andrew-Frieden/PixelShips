using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelShips.Widgets;
using PixelShips.Verse;

public class LoginPanelWidget : BaseWidget {

	private string username;
	private string password;
	public TextMeshProUGUI usernameText;
	public TextMeshProUGUI passwordText;


	public void authenticateCredentials(){
		username = usernameText.text;
		password = passwordText.text;

		Debug.Log("Username: "+ username + " Password: " + password);


	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
