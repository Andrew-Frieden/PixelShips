using System;
using System.Collections.Generic;
using Models.Actions;
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

		private IEnumerable<StringTagContainer> GetShipCells()
		{
			var ship = GameManager.Instance.GameState.CommandShip;

            var baseStats = new List<StringTagContainer>
            {
                new StringTagContainer()
                {
                    Text = "Name: " + ship.Values[ShipStats.CaptainName]
                },
                new StringTagContainer()
                {
                    Text = "Hull: " + ship.Stats[StatKeys.Hull] + "/" + ship.Stats[StatKeys.MaxHull]
                },
                new StringTagContainer($"Shields: {ship.Stats[StatKeys.Shields]}/{ship.Stats[StatKeys.MaxShields]}"),
                new StringTagContainer($"Captainship: {ship.Stats[StatKeys.Captainship]}"),
                new StringTagContainer()
                {
                    Text = "WarpDriveReady: " + (ship.Stats[ShipStats.WarpDriveReady] == 1)
                },
                new StringTagContainer($"Credits: {ship.Stats[StatKeys.Credits]}")
			};
			
			if (ship.Stats[StatKeys.Scrap] > 0)
			{
				baseStats.Add(new StringTagContainer()
				{
					Text = "Scrap: " + ship.Stats[StatKeys.Scrap]
				});
			}

			if (ship.Stats[StatKeys.Resourcium] > 0)
			{
				baseStats.Add(new StringTagContainer()
				{
					Text = "Resourcium: " + ship.Stats[StatKeys.Resourcium]
				});
			}
			
			if (ship.Stats[StatKeys.Techanite] > 0)
			{
				baseStats.Add(new StringTagContainer()
				{
					Text = "Techanite: " + ship.Stats[StatKeys.Techanite]
				});
			}
			
			if (ship.Stats[StatKeys.MachineParts] > 0)
			{
				baseStats.Add(new StringTagContainer()
				{
					Text = "Machine Parts: " + ship.Stats[StatKeys.MachineParts]
				});
			}
			
			if (ship.Stats[StatKeys.PulsarCoreFragments] > 0)
			{	
				baseStats.Add(new StringTagContainer()
				{
					Text = "Pulsar Core Fragments: " + ship.Stats[StatKeys.PulsarCoreFragments]
				});
			}

			return baseStats;
		}
	}
}
