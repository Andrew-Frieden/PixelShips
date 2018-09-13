using System.Collections.Generic;
using Models.Stats;
using UnityEngine;

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

			return new List<string>
			{
				"Name: Space Pirate Dave",
				"Hull: " + ship.Stats[StatKeys.Hull],
				"Captainship: " + ship.Stats[StatKeys.Captainship],
				"WarpDriveReady: " + ship.Stats[StatKeys.WarpDriveReady],
				"----------------------",
				"Resourcium: " + ship.Stats[StatKeys.Resourcium],
				"Scrap: " + ship.Stats[StatKeys.Scrap],
			};
		}
	}
}
