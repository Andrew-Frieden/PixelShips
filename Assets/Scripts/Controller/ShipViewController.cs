using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Stats;
using UnityEngine;
using static Models.CommandShip;

namespace Controller
{
	public class ShipViewController : MonoBehaviour
	{
		[SerializeField] private TripleViewController _tripleViewController;
		[SerializeField] private ScrollViewController _shipScrollViewController;
	
		private void Start()
		{
			_tripleViewController.OnViewShown += HandleOnViewShown;
			Render();
		}

		private void HandleOnViewShown(int view)
		{
			if (view == (int) TripleView.Right)
			{
				Render();
			}
		}

		private void Render()
		{
			_shipScrollViewController.ClearScreen();
			_shipScrollViewController.AddCells(GetShipCells());
		}

		private IEnumerable<string> GetShipCells()
		{
			var ship = GameManager.Instance.GameState.CommandShip;

			var baseStats = new List<string>
			{
				"Name: " + ship.Values[ShipStats.CaptainName],
				"Hull: " + ship.Stats[StatKeys.Hull] + "/" + ship.Stats[StatKeys.MaxHull],
				"Shields: " + ship.Stats[StatKeys.Shields] + "/" + ship.Stats[StatKeys.MaxShields],
				"Captainship: " + ship.Stats[StatKeys.Captainship],
				"WarpDriveReady: " + (ship.Stats[ShipStats.WarpDriveReady] == 1)
			};
			
			if (ship.Stats[StatKeys.Scrap] > 0)
			{
				baseStats.Add("Scrap: " + ship.Stats[StatKeys.Scrap]);
			}

			if (ship.Stats[StatKeys.Resourcium] > 0)
			{
				baseStats.Add("Resourcium: " + ship.Stats[StatKeys.Resourcium]);
			}
			
			if (ship.Stats[StatKeys.Techanite] > 0)
			{
				baseStats.Add("Techanite: " + ship.Stats[StatKeys.Techanite]);
			}
			
			if (ship.Stats[StatKeys.MachineParts] > 0)
			{
				baseStats.Add("Machine Parts: " + ship.Stats[StatKeys.MachineParts]);
			}
			
			if (ship.Stats[StatKeys.PulsarCoreFragments] > 0)
			{
				baseStats.Add("Pulsar Core Fragments: " + ship.Stats[StatKeys.PulsarCoreFragments]);
			}

			return baseStats;
		}
	}
}
