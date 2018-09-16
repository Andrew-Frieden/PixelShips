using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Stats;
using TMPro;
using UnityEngine;

public class ShipHudController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _shipName;
	[SerializeField] private TextMeshProUGUI _sectorName;
	[SerializeField] private TextMeshProUGUI _hull;
	[SerializeField] private TextMeshProUGUI _energy;

	private CommandShip PlayerShip { get; set; }

	private int CurrentHull { get; set; }

	public void InitializeShipHud(CommandShip ship)
	{
		PlayerShip = ship;
		CurrentHull = PlayerShip.Stats[StatKeys.Hull];
		_hull.text = "Hull: " + PlayerShip.Stats[StatKeys.Hull] + "/" + PlayerShip.Stats[StatKeys.MaxHull];
	}

	public void UpdateHull(int damage)
	{
		StartCoroutine(UpdateHullText(damage));
	}

	//TODO: This is only for damage, need to implement the 'repair your hull' version
	private IEnumerator UpdateHullText(int damage)
	{
		while (PlayerShip.Stats[StatKeys.Hull]  < CurrentHull)
		{
			CurrentHull -= 1;
			_hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.Stats[StatKeys.MaxHull];
			yield return new WaitForSeconds(0.25f);
		}
	}
}
