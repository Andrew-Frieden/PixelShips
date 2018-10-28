using System.Linq;
using Models.Dtos;
using TextEncoding;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private CommandViewController _commandViewController;
        [SerializeField] private TextMeshProUGUI DevToolsContentLoaded;
        
        public void SpawnNewShip()
        {
            //This will re-write the game state
            GameManager.Instance.StartNewExpedition();
            _commandViewController.StartCommandView();
        }

        public void InitContentLoadResults(GameContentDto content)
        {
            var flexDataLoadedText =
                $"<b>FlexData Loaded:</b>{Env.l}" +
                $"Mobs:\t\t{content.Mobs.Count()}{Env.l}" +
                $"Npcs:\t\t{content.Npcs.Count()}{Env.l}" +
                $"Hazards:\t\t{content.Hazards.Count()}{Env.l}" +
                $"Gatherables:\t{content.Gatherables.Count()}{Env.l}" +
                $"Weapons:\t\t{content.Weapons.Count()}{Env.l}" +
                $"Hardware:\t\t{content.Hardware.Count()}{Env.l}";

            var totalFlexData = content.Mobs.Count()
                + content.Npcs.Count()
                + content.Hardware.Count()
                + content.Hazards.Count()
                + content.Gatherables.Count()
                + content.Weapons.Count();
            flexDataLoadedText += $"<b>Total:\t\t{totalFlexData}</b>";

            DevToolsContentLoaded.text = flexDataLoadedText;
        }
    }
}