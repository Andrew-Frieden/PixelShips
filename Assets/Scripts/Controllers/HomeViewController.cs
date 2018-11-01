using System.Linq;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;
using TextEncoding;
using TextSpace.Events;
using TMPro;
using UnityEngine;
using TextSpace.Services;
using TextSpace.Framework.IoC;

namespace TextSpace.Controllers
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI DevToolsContentLoaded;
        [SerializeField] private TextMeshProUGUI PlanetName;
        [SerializeField] private TextMeshProUGUI PlanetDescription;
        [SerializeField] private TextMeshProUGUI PlanetStats;

        private ContentLoadService contentLoadSvc => ServiceContainer.Resolve<ContentLoadService>();

        private void Start()
        {
            UIResponseBroadcaster.UIResponseTagTrigger += RespondToUIResponseTag;
            InitContentLoadResults();
            DisplayHomeworld();
        }

        private void RespondToUIResponseTag(UIResponseTag tag)
        {
            if (tag == UIResponseTag.UpdateHomeworld)
                DisplayHomeworld();
        }

        public void StartNewExpedition()
        {
            GameManager.Instance.StartNewExpedition();
        }

        public void DisplayHomeworld()
        {
            var _homeworld = ServiceContainer.Resolve<IHomeworldProvider>().Home;

            PlanetName.text = _homeworld.PlanetName;
            PlanetDescription.text = $"{_homeworld.Description} World";
            PlanetStats.text = 
                $"Conquest:\t\t{0}{Env.l}" +
                $"Expeditions:\t{_homeworld.ExpeditionCount}";
        }

        private void InitContentLoadResults()
        {
            var content = contentLoadSvc.Content;
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