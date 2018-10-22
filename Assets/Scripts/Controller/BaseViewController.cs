using System.Linq;
using Models.Dtos;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class BaseViewController : MonoBehaviour
    {
        [SerializeField] private CommandViewController _commandViewController;
        [SerializeField] private TextMeshProUGUI DevToolsContentLoaded;
        
        public void SpawnNewShip()
        {
            //This will re-write the game state
            GameManager.Instance.StartNewExpedition();
            _commandViewController.StartCommandView();
        }

        public void InitContentLoadResults(GameContentDto gameContentDto)
        {
            DevToolsContentLoaded.text = "Mobs Loaded: " + (gameContentDto.Mobs != null ? gameContentDto.Mobs.Count().ToString() : "0") + "\n" +
                                         "Hazards Loaded: " + (gameContentDto.Hazards != null ? gameContentDto.Hazards.Count().ToString() : "0") + "\n" +
                                         "Gatherables Loaded: " + (gameContentDto.Gatherables != null ? gameContentDto.Gatherables.Count().ToString() : "0") + "\n" +
                                         "Weapons Loaded: " + (gameContentDto.Weapons != null ? gameContentDto.Weapons.Count().ToString() : "0") + "\n" +
                                         "Hardware Loaded: " + (gameContentDto.Hardware != null ? gameContentDto.Hardware.Count().ToString() : "0") + "\n" +
                                         "Npcs Loaded: " + (gameContentDto.Npcs != null ? gameContentDto.Npcs.Count().ToString() : "0");
        }
    }
}