using System.Collections;
using Models;
using Models.Stats;
using TMPro;
using UnityEngine;

namespace Controller
{
	public class ShipHudController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _shield;
		[SerializeField] private TextMeshProUGUI _sectorName;
		[SerializeField] private TextMeshProUGUI _hull;
		[SerializeField] private TextMeshProUGUI _energy;

		private CommandShip PlayerShip { get; set; }

		//These track the ships 'current' before any actions/tick changes the values
		private int CurrentHull { get; set; }
		private int CurrentShield { get; set; }

		public void InitializeShipHud(CommandShip ship)
		{
			PlayerShip = ship;
		
			CurrentHull = PlayerShip.Stats[StatKeys.Hull];
			CurrentShield = PlayerShip.Stats[StatKeys.Shields];
		
			_shield.text = "Shield: " + PlayerShip.Stats[StatKeys.Shields] + "/" + PlayerShip.Stats[StatKeys.MaxShields];
			_hull.text = "Hull: " + PlayerShip.Stats[StatKeys.Hull] + "/" + PlayerShip.Stats[StatKeys.MaxHull];
		}

		public void UpdateHull()
		{
			StartCoroutine(UpdateHullText());
		}

		public void UpdateShield()
		{
			StartCoroutine(UpdateShieldText());
		}

		private IEnumerator UpdateShieldText()
		{
			if (CurrentShield > PlayerShip.Stats[StatKeys.Shields])
			{
				while (PlayerShip.Stats[StatKeys.Shields]  < CurrentShield)
				{
					CurrentShield -= 1;
					_shield.text = "Shield: " + CurrentShield + "/" + PlayerShip.Stats[StatKeys.MaxShields];
					yield return new WaitForSeconds(0.25f);
				}
			}
			else
			{
				while (PlayerShip.Stats[StatKeys.Shields]  > CurrentShield)
				{
					CurrentShield += 1;
					_shield.text = "Shield: " + CurrentShield + "/" + PlayerShip.Stats[StatKeys.MaxShields];
					yield return new WaitForSeconds(0.25f);
				}
			}
		}
	
		private IEnumerator UpdateHullText()
		{
			if (CurrentHull > PlayerShip.Stats[StatKeys.Hull])
			{
				while (PlayerShip.Stats[StatKeys.Hull]  < CurrentHull)
				{
					CurrentHull -= 1;
					_hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.Stats[StatKeys.MaxHull];
					yield return new WaitForSeconds(0.25f);
				}
			}
			else
			{
				while (PlayerShip.Stats[StatKeys.Hull]  > CurrentHull)
				{
					CurrentHull += 1;
					_hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.Stats[StatKeys.MaxHull];
					yield return new WaitForSeconds(0.25f);
				}
			}
		}
	}
}
