using System;
using System.Collections.Generic;
using Models;
using Models.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {   
        [SerializeField] private ScrollViewController scrollView;
        
        private IRoom _room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
        }
        
        private void Awake()
        {
            var mob = new Mob("A {{ link }} floats here", "Space Barbarian", 2);
            
            Debug.Log("Mob Created.");
            
            var commandShip = ShipFactory.GenerateCommandShip();
            
            Debug.Log("Command Ship Created.");
            
            _room = new Room("You enter into a {{ link }} with many asteroids.", "Giant Nebula", commandShip, new List<IRoomEntity>() { mob });
            
            Debug.Log("Room Created.");
            
            Debug.Log("Room look text: " + _room.GetLookText());
            
            scrollView.AddCell(_room);
            scrollView.AddCell(_room.Entities[0]);
        }

        private void HandleLinkTouchedEvent(ITextEntity textEntity)
        {
            Debug.Log("Clicked link for entity with Id: " +  textEntity.Id);
        }

        public void OnPlayerChoseAction()
        {
            var anotherMob = new Mob("A {{ link }} floats here", "Potted Plant", 2);
            
            scrollView.AddCell(anotherMob);
        }
    }
}