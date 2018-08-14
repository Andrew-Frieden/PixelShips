using System.Collections.Generic;
using Models;
using Models.Factories;
using UnityEngine;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {
        private void Awake()
        {
            var mob = new Mob("An angry pirate", 2);
            
            Debug.Log("Mob Created.");
            
            var commandShip = ShipFactory.GenerateCommandShip();
            
            Debug.Log("Command Ship Created.");
            
            var room = new Room(commandShip, new List<IRoomEntity>() { mob });
            
            Debug.Log("Room Created.");
        }
    }
}