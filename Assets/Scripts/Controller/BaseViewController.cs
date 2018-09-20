using Models;
using UnityEngine;

namespace Controller
{
    public class BaseViewController : MonoBehaviour
    {
        [SerializeField] private CommandViewController _commandViewController;
        
        private IRoom _room;
        private SaveLoadController _saveLoadController;
        
        public void SpawnNewShip()
        {
            //This will re-write the game state
            GameManager.Instance.StartNewMission();
            
            //Place the new ship in the room - TODO: This should be somewhere else
            var playerShip = GameManager.Instance.GameState.CommandShip;
            _room = GameManager.Instance.GameState.Room;
            _room.SetPlayerShip(playerShip);
            
            _commandViewController.StartCommandView(_room);
        }
    }
}