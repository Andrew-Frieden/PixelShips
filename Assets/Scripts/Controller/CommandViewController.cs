using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Common;
using Models;
using Models.Factories;
using PixelSpace.Models.SharedModels.Helpers;
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
        private IRoom _room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.choseActionEvent += HandlePlayerChoseAction;
            
            var commandShip = ShipFactory.GenerateCommandShip();

            var roomEntityRepository = new RoomEntityRepository();
            var roomEntities = roomEntityRepository.LoadData();

            var roomRepository = new RoomRepository();
            var rooms = roomRepository.LoadData();

            var randomizedRooms = rooms.OrderBy(a => Guid.NewGuid()).ToList();
            _room = randomizedRooms.First();
            _room.SetPlayerShip(commandShip);

            var tabby = (Mob)roomEntities.First();
            tabby.DialogueContent.OptionAAction = new AttackAction(tabby, commandShip, 17);
            tabby.DialogueContent.OptionBAction = new AttackAction(tabby, commandShip, 39);

            _room.AddEntity(tabby);
            
            //TODO: Make a scroll view controller method to handle printing a room and all its entities to cells
            scrollView.AddCells(new List<string>() { _room.GetLookText(), _room.Entities[0].GetLookText() });

            StartCoroutine(Blink.BlinkLoop());
        }

        private void HandleLinkTouchedEvent(string guid)
        {
            var entity = _room.Entities.FirstOrDefault(e => e.Id == guid) ?? (ITextEntity) _room.PlayerShip;
            abController.ShowControl(entity.DialogueContent);
        }

        private void HandlePlayerChoseAction(IRoomAction action)
        {
            //scrollView.AddCells
            action.Execute(_room);

            foreach (var entity in _room.Entities)
            {
                //scrollView.AddCells
                scrollView.AddCells(entity.GetNextAction(_room).Execute(_room));
            }
        }
    }
}