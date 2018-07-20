using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button _startButton;
	[SerializeField] private Button _optionsButton;
	
	private void Start () 
	{
		_startButton.onClick.AddListener(OnClickStartButton);
		_optionsButton.onClick.AddListener(OnClickOptionsButton);
	}
	
	private void OnClickStartButton()
	{
		GameManager.Instance.StartMission();
	}
	
	private void OnClickOptionsButton()
	{
		Debug.Log("Options Button Clicked");
	}
}
