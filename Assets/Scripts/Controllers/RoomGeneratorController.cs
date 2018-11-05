using System;
using System.Collections.Generic;
using TextSpace.Framework.IoC;
using TextSpace.Models;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using TextSpace.Services.Factories;
using TMPro;
using UnityEngine;

namespace TextSpace.Controllers
{
	public class RoomGeneratorController : MonoBehaviour
	{
		private GameContentFilterDto _filter;

		private RoomFlavor _selectedRoomFlavor;
		private RoomActorFlavor _selectedRoomActorFlavor;
		
		private TMP_Dropdown.DropdownEvent _flavorFilterDropdownEvent;
		private TMP_Dropdown.DropdownEvent _actorFlavorFilterDropdownEvent;

		[SerializeField] private TMP_Dropdown _flavorFilterDropdown;
		[SerializeField] private TMP_Dropdown _contentTypeFilterDropdown;
		[SerializeField] private TMP_Dropdown _entityChooserDropdown;
		
		private RoomFactoryService RoomFactory => ServiceContainer.Resolve<RoomFactoryService>();
		
		void Start()
		{
			var flavorFilterOptions = new List<TMP_Dropdown.OptionData>();
			var contentTypeFilterOptions = new List<TMP_Dropdown.OptionData>();

			foreach (var flavor in Enum.GetValues(typeof(RoomFlavor)))
			{
				flavorFilterOptions.Add(new TMP_Dropdown.OptionData()
				{
					text = flavor.ToString()
				});
			}

			foreach (var flavor in Enum.GetValues(typeof(RoomActorFlavor)))
			{
				contentTypeFilterOptions.Add(new TMP_Dropdown.OptionData()
				{
					text = flavor.ToString()
				});
			}
			
			_flavorFilterDropdown.options = flavorFilterOptions;
			_contentTypeFilterDropdown.options = contentTypeFilterOptions;
			
			_flavorFilterDropdownEvent = new TMP_Dropdown.DropdownEvent();
			_flavorFilterDropdownEvent.AddListener(OnFlavorFilterSelected);
			_flavorFilterDropdown.onValueChanged = _flavorFilterDropdownEvent;
			
			_actorFlavorFilterDropdownEvent = new TMP_Dropdown.DropdownEvent();
			_actorFlavorFilterDropdownEvent.AddListener(OnActorFlavorFilterSelected);
			_contentTypeFilterDropdown.onValueChanged = _actorFlavorFilterDropdownEvent;
		}

		private void OnFlavorFilterSelected(int selected)
		{
			_selectedRoomFlavor = (RoomFlavor) selected;

			SetEntityDropdownOptions();
		}
		
		private void OnActorFlavorFilterSelected(int selected)
		{
			_selectedRoomActorFlavor = (RoomActorFlavor) selected;

			SetEntityDropdownOptions();
		}

		private void SetEntityDropdownOptions()
		{
			var gameContentFilterDto = new GameContentFilterDto(_selectedRoomFlavor, _selectedRoomActorFlavor);
			var flexData = RoomFactory.GetFlexData(gameContentFilterDto);

			if (_entityChooserDropdown.options != null)
			{
				_entityChooserDropdown.ClearOptions();
			}
			
			var entityChooserOptions = new List<TMP_Dropdown.OptionData>();
			
			foreach (var entity in flexData)
			{
				entityChooserOptions.Add(new TMP_Dropdown.OptionData()
				{
					text = entity.Values[ValueKeys.Name]
				});
			}

			_entityChooserDropdown.options = entityChooserOptions;
		}

		public void AddToRoom()
		{
			
		}
	}
}
