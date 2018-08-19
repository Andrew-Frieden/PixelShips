using System;
using System.Collections.Generic;
using Actions;
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
        [SerializeField] private ABDialogueController abController;
        
        private IRoom _room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            
            var commandShip = ShipFactory.GenerateCommandShip();
            
            var tabbyOfficerContent = new ABDialogueContent()
            {
                MainText = @"A tabby officer greets you over the voice comms:

Meow Citizen!

I was in pursuit of two renegade Verdants in this sectorrr but my ship got stuck in the kelp.Can you renderrr me some assistance ? ",
                OptionAText = "Might want to consider this idea",
                OptionBText = "Sounds risky"
            };
            
            var tabbyOfficer = new Mob("A {{ link }} floats here", "Tabby Officer", 2, tabbyOfficerContent);

            tabbyOfficerContent.OptionAAction = new AttackAction(tabbyOfficer, commandShip, 3);
            tabbyOfficerContent.OptionBAction = new AttackAction(tabbyOfficer, commandShip, 5);
            
            var giantNebulaContent = new ABDialogueContent()
            {
                MainText = @"This sector seems to be a dizzying array of star dust and deadly asteroids.",
                OptionAText = "Warp to next sector",
                OptionBText = "This should just be an A or cancel I think"
            };
            
            _room = new Room("You enter into a {{ link }} with many asteroids.", "Giant Nebula", commandShip, new List<IRoomEntity>() { tabbyOfficer }, giantNebulaContent);
            
            scrollView.AddCell(_room);
            scrollView.AddCell(_room.Entities[0]);
        }

        private void HandleLinkTouchedEvent(ITextEntity textEntity)
        {
            abController.ShowControl(textEntity.DialogueContent);
        }

        public void OnPlayerChoseAction()
        {
            var plantContent = new ABDialogueContent();
            
            var anotherMob = new Mob("A {{ link }} floats here", "Potted Plant", 2, plantContent);
            
            scrollView.AddCell(anotherMob);
        }
    }
}