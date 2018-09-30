using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
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
			_shipScrollViewController.AddCells(GetShipData());
		}

		private IEnumerable<TagString> GetShipData()
		{
			var ship = GameManager.Instance.GameState.CommandShip;

            var shipData = new List<TagString>
            {
                new TagString()
                {
                    Text = "Name: " + ship.Values[ShipStats.CaptainName]
                },
                new TagString()
                {
                    Text = "Hull: " + ship.Stats[StatKeys.Hull] + "/" + ship.Stats[StatKeys.MaxHull]
                },
                new TagString($"Shields: {ship.Stats[StatKeys.Shields]}/{ship.Stats[StatKeys.MaxShields]}"),
                new TagString($"Captainship: {ship.Stats[StatKeys.Captainship]}"),
                new TagString()
                {
                    Text = "WarpDriveReady: " + (ship.Stats[ShipStats.WarpDriveReady] == 1)
                },
                new TagString($"Credits: {ship.Stats[StatKeys.Credits]}")
			};

            foreach (var hardware in ship.Hardware)
            {
                shipData.Add($"<>".Encode(hardware, LinkColors.Gatherable).Tag());
            }
			
			if (ship.Stats[StatKeys.Scrap] > 0)
			{
				shipData.Add(new TagString()
				{
					Text = "Scrap: " + ship.Stats[StatKeys.Scrap]
				});
			}

			if (ship.Stats[StatKeys.Resourcium] > 0)
			{
				shipData.Add(new TagString()
				{
					Text = "Resourcium: " + ship.Stats[StatKeys.Resourcium]
				});
			}
			
			if (ship.Stats[StatKeys.Techanite] > 0)
			{
				shipData.Add(new TagString()
				{
					Text = "Techanite: " + ship.Stats[StatKeys.Techanite]
				});
			}
			
			if (ship.Stats[StatKeys.MachineParts] > 0)
			{
				shipData.Add(new TagString()
				{
					Text = "Machine Parts: " + ship.Stats[StatKeys.MachineParts]
				});
			}
			
			if (ship.Stats[StatKeys.PulsarCoreFragments] > 0)
			{	
				shipData.Add(new TagString()
				{
					Text = "Pulsar Core Fragments: " + ship.Stats[StatKeys.PulsarCoreFragments]
				});
			}

			return shipData;
		}
	}
}
