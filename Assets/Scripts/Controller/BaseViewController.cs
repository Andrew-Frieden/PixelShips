using UnityEngine;

namespace Controller
{
    public class BaseViewController : MonoBehaviour
    {
        [SerializeField] private CommandViewController _commandViewController;
        
        
        public void SpawnNewShip()
        {
            //This will re-write the game state

            
            GameManager.Instance.StartNewMission();

            _commandViewController.StartCommandView();
        }
    }
}