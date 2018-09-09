using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipHudController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _shipName;
	[SerializeField] private TextMeshProUGUI _sectorName;
	[SerializeField] private TextMeshProUGUI _hull;
	[SerializeField] private TextMeshProUGUI _energy;
	
	public float CurrentHull { get; private set; }

	public void InitializeShipHud(int hull)
	{
		CurrentHull = hull;
		_hull.text = "Current Hull: " + CurrentHull.ToString();
	}

	public void UpdateHull(int newHull)
	{
		StartCoroutine(UpdateText(newHull));
	}

	private IEnumerator UpdateText(int newHull)
	{
		while (newHull  < CurrentHull)
		{
			CurrentHull -= 1;
			CurrentHull = Mathf.Clamp(CurrentHull, newHull, CurrentHull);
			_hull.text = "Current Hull: " + CurrentHull.ToString();
			yield return new WaitForSeconds(1 / (CurrentHull - newHull));
		}
		
		while (newHull > CurrentHull)
		{
			CurrentHull += 1;
			CurrentHull = Mathf.Clamp(CurrentHull, 0f, newHull);
			_hull.text = "Current Hull: " + CurrentHull.ToString();
			yield return new WaitForSeconds(1 / (CurrentHull - newHull));
		}
	}
}
