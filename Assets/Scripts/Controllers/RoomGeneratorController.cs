using System;
using System.Collections.Generic;
using TextSpace.Framework.IoC;
using TextSpace.Models;
using TextSpace.Services.Factories;
using TMPro;
using UnityEngine;

namespace TextSpace.Controllers
{
	public class RoomGeneratorController : MonoBehaviour
	{
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
			
			//_flavorFilterDropdown.onValueChanged
		}
	}
}
