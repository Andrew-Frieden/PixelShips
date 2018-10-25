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
            var ship = GameManager.Instance.GameState.CurrentExpedition.CmdShip;

            var shipData = new List<TagString>
            {
                $"--- Ship ---".Tag(),
                $"Captain: {ship.Values[ValueKeys.CaptainName]}".Tag(),
                $"Hull: {ship.Hull}/{ship.MaxHull}".Tag(),
                $"Shields: {ship.Stats[StatKeys.Shields]}/{ship.Stats[StatKeys.MaxShields]}".Tag(),
                $"Warp Drive: {(ship.Stats[StatKeys.WarpDriveReady] == 1 ? "Ready" : "Cold")}".Tag()
            };

            shipData.Add($"--- Weapons ---".Tag());
            shipData.Add($"Light: <>".Encode(ship.LightWeapon, LinkColors.Weapon).Tag());
            shipData.Add($"Heavy: <>".Encode(ship.HeavyWeapon, LinkColors.Weapon).Tag());

            shipData.Add($"--- Hardware ---".Tag());
            int emptySlots = ship.Stats[StatKeys.MaxHardwareSlots];
            foreach (var hardware in ship.Hardware)
            {
                emptySlots--;
                shipData.Add($"<>".Encode(hardware, LinkColors.Gatherable).Tag());
            }
            for (int i = 0; i < emptySlots; i++)
            {
                shipData.Add($"[Empty Hardware Slot]".Tag());
            }

            shipData.Add("--- Cargo ---".Tag());
            shipData.Add($"Credits: {ship.Stats[StatKeys.Credits]}".Tag());
            shipData.Add($"Resourcium: {ship.Resourcium}".Tag());
            shipData.Add($"Scrap: {ship.Scrap}".Tag());
			
			return shipData;
		}
	}
}
