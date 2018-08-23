using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Common;
using Models;
using Models.Factories;
using Repository;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {   
        [SerializeField] private ScrollViewController scrollView;
        [SerializeField] private ABDialogueController abController;

        public Blink Blink;
        private IRoom room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            
            var commandShip = ShipFactory.GenerateCommandShip();

            var roomEntityRepository = new RoomEntityRepository();
            var roomEntities = roomEntityRepository.LoadData();

            var roomRepository = new RoomRepository();
            var rooms = roomRepository.LoadData();

            
            var randomizedRooms = rooms.OrderBy(a => Guid.NewGuid()).ToList();
            room = randomizedRooms.First();



            room.SetPlayerShip(commandShip);

            var tabby = (Mob)roomEntities.First();
            tabby.DialogueContent.OptionAAction = new AttackAction(tabby, commandShip, 17);
            tabby.DialogueContent.OptionBAction = new AttackAction(tabby, commandShip, 39);

            room.AddEntity(tabby);
            
            //TODO: Make a scroll view controller method to handle printing a room and all its entities to cells
            scrollView.AddCells(new List<ITextEntity>() { room, room.Entities[0] });

            StartCoroutine(Blink.BlinkLoop());
        }

        private void HandleLinkTouchedEvent(ITextEntity textEntity)
        {
            abController.ShowControl(textEntity.DialogueContent);
        }

        public void OnPlayerChoseAction()
        {
            var plantContent = new ABDialogueContent();
            
            var anotherMob = new Mob("A {{ link }} floats here", "Potted Plant", 2, plantContent);
            var secondMob = new Mob("A very seriously sized {{ link }} hulks off into the distance. Be carefyk if this one", "Space Ogre", 2, plantContent);
            
            scrollView.AddCells(new List<ITextEntity>() { secondMob, anotherMob });
        }
    }
}