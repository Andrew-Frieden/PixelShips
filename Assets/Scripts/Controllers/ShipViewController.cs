using System.Collections.Generic;
using TextSpace.Models.Actions;
using TextSpace.Models.Stats;
using TextEncoding;
using UnityEngine;
using TextSpace.Services;
using TextSpace.Framework.IoC;

namespace TextSpace.Controllers
{
	public class ShipViewController : MonoBehaviour
	{
		[SerializeField] private TripleViewController _tripleViewController;
		[SerializeField] private ScrollViewController _shipScrollViewController;

        private BootstrapService BootStrapSvc => ServiceContainer.Resolve<BootstrapService>();
	
		private void Start()
		{
			_tripleViewController.OnViewShown += HandleOnViewShown;
		}

		private void HandleOnViewShown(int view)
		{
			if (view == (int) TripleView.Right)
				Render();
		}

		private void Render()
		{
			_shipScrollViewController.ClearScreen();
			_shipScrollViewController.AddCells(GetShipData());
		}

		private IEnumerable<TagString> GetShipData()
		{
            var ship = GameManager.Instance.GameState.CurrentExpedition.CmdShip;

            if (ship == null || ship == BootStrapSvc.GameState.CurrentExpedition.CmdShip)
                return new[] 
                {
                    "Searching...".Tag(),
                    ".".Tag(),
                    ".".Tag(),
                    ".".Tag(),
                    ".".Tag(),
                    ".".Tag(),
                    ".".Tag(),
                    "No systems found.".Tag(),
                    "Construct a starship at your homeworld.".Tag()
                };

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
            shipData.Add($"Resourcium: {ship.Resourcium}".Tag());
            shipData.Add($"Scrap: {ship.Scrap}".Tag());
			
			return shipData;
		}
	}
}
