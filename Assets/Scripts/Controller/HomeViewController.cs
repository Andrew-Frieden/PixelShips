﻿using System.Linq;
using Models;
using Models.Actions;
using Models.Dtos;
using TextEncoding;
using TextSpace.Events;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private CommandViewController _commandViewController;
        [SerializeField] private TextMeshProUGUI DevToolsContentLoaded;

        [SerializeField] private TextMeshProUGUI PlanetName;
        [SerializeField] private TextMeshProUGUI PlanetDescription;
        [SerializeField] private TextMeshProUGUI PlanetStats;

        private void Start()
        {
            UIResponseBroadcaster.UIResponseTagTrigger += RespondToUIResponseTag;
        }

        private void RespondToUIResponseTag(UIResponseTag tag)
        {
            if (tag == UIResponseTag.UpdateHomeworld)
                DisplayHomeworld();
        }

        private Homeworld _homeworld;
        public void Init(Homeworld world)
        {
            _homeworld = world;
            DisplayHomeworld();
        }

        public void StartNewExpedition()
        {
            GameManager.Instance.StartNewExpedition();
        }

        public void DisplayHomeworld()
        {
            if (_homeworld == null)
                return;

            PlanetName.text = _homeworld.PlanetName;
            PlanetDescription.text = $"{_homeworld.Description} World";
            PlanetStats.text = 
                $"Conquest:\t\t{0}{Env.l}" +
                $"Expeditions:\t{_homeworld.ExpeditionCount}";
        }

        public void InitContentLoadResults(GameContentDto content)
        {
            var flexDataLoadedText =
                $"<b>FlexData Loaded</b>{Env.l}" +
                $"Mobs:\t\t{content.Mobs.Count()}{Env.l}" +
                $"Npcs:\t\t{content.Npcs.Count()}{Env.l}" +
                $"Hazards:\t\t{content.Hazards.Count()}{Env.l}" +
                $"Gatherables:\t{content.Gatherables.Count()}{Env.l}" +
                $"Weapons:\t\t{content.Weapons.Count()}{Env.l}" +
                $"Hardware:\t\t{content.Hardware.Count()}{Env.l}" + 
                $"Mineables:\t{content.Mineables.Count()}{Env.l}";

            var totalFlexData = content.Mobs.Count()
                + content.Npcs.Count()
                + content.Hardware.Count()
                + content.Hazards.Count()
                + content.Gatherables.Count()
                + content.Weapons.Count()
                + content.Mineables.Count();
            flexDataLoadedText += $"<b>Total:\t\t{totalFlexData}</b>";

            DevToolsContentLoaded.text = flexDataLoadedText;
        }
    }
}