using System.Collections;
using Models;
using Models.Actions;
using Models.Stats;
using TextEncoding;
using TextSpace.Events;
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

        [SerializeField] private TextTyper _typer;
        private TextTyper sectorTyper
        {
            get
            {
                if (_typer == null)
                    this.GetComponent<TextTyper>();
                return _typer;
            }
        }

		private CommandShip PlayerShip { get; set; }

		//These track the ships 'current' before any actions/tick changes the values
        private int CurrentHull { get; set; }
		private int CurrentMaxHull { get; set; }
		private int CurrentShield { get; set; }
		private int CurrentEnergy { get; set; }

		public void InitializeShipHud(IRoom room)
		{
			PlayerShip = room.PlayerShip;

			CurrentHull = PlayerShip.Hull;
            CurrentMaxHull = PlayerShip.MaxHull;
			CurrentShield = PlayerShip.Stats[StatKeys.Shields];
			CurrentEnergy = PlayerShip.Stats[StatKeys.Energy];
		
			_shield.text = "Shield: " + PlayerShip.Stats[StatKeys.Shields] + "/" + PlayerShip.Stats[StatKeys.MaxShields];
			_hull.text = "Hull: " + PlayerShip.Hull + "/" + PlayerShip.MaxHull;
			_energy.text = "Energy: " + PlayerShip.Stats[StatKeys.Energy] + "/" + PlayerShip.Stats[StatKeys.MaxEnergy];

            UpdateSector(room);
		}

        private void Start()
        {
            UIResponseBroadcaster.UIResponseTagTrigger += RespondToEventTag;
        }

        private void RespondToEventTag(UIResponseTag tag)
        {
            if (PlayerShip == null)
                return;
                
            switch (tag)
            {
                case UIResponseTag.PlayerHullModified:
                    UpdateHull();
                    break;
                case UIResponseTag.PlayerDamaged:
                    UpdateShield();
                    UpdateHull();
                    break;
                case UIResponseTag.PlayerShieldsRecovered:
                    UpdateShield();
                    break;
                case UIResponseTag.PlayerEnergyConsumed:
                    UpdateEnergy();
                    break;
            }
        }

		public void UpdateHull()
		{
			StartCoroutine(UpdateHullText());
		}

		public void UpdateShield()
		{
			StartCoroutine(UpdateShieldText());
		}

		public void UpdateEnergy()
		{
			StartCoroutine(UpdateEnergyText());
		}

        public void UpdateSector(IRoom room)
        {
            _sectorName.text = "<>".Encode(room.GetLinkText(), room.Id, LinkColors.Room);
            sectorTyper.TypeText();
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
			if (CurrentHull > PlayerShip.Hull)
			{
				while (PlayerShip.Hull  < CurrentHull)
				{
					CurrentHull -= 1;
					_hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.MaxHull;
					yield return new WaitForSeconds(0.25f);
				}
			}
			else 
			{
				while (PlayerShip.Hull  > CurrentHull)
				{
					CurrentHull += 1;
					_hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.MaxHull;
					yield return new WaitForSeconds(0.25f);
				}
			}
            
            if (CurrentMaxHull != PlayerShip.MaxHull)
            {
                CurrentMaxHull = PlayerShip.MaxHull;
                _hull.text = "Hull: " + CurrentHull + "/" + PlayerShip.MaxHull;
            }
		}
		
		private IEnumerator UpdateEnergyText()
		{
			if (CurrentEnergy > PlayerShip.Stats[StatKeys.Energy])
			{
				while (PlayerShip.Stats[StatKeys.Energy]  < CurrentEnergy)
				{
					CurrentEnergy -= 1;
					_energy.text = "Energy: " + CurrentEnergy + "/" + PlayerShip.Stats[StatKeys.MaxEnergy];
					yield return new WaitForSeconds(0.25f);
				}
			}
			else
			{
				while (PlayerShip.Stats[StatKeys.Energy]  > CurrentEnergy)
				{
					CurrentEnergy += 1;
					_energy.text = "Energy: " + CurrentEnergy + "/" + PlayerShip.Stats[StatKeys.MaxEnergy];
					yield return new WaitForSeconds(0.25f);
				}
			}
		}
	}
}
