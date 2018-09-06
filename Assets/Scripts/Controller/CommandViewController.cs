using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models;
using Models.Actions;
using Models.Factories;
using Repository;
using UnityEngine;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {   
        [SerializeField] private ScrollViewController scrollView;
        [SerializeField] private ABDialogueController abController;

        private RoomController _roomController;

        public Blink Blink;
        private IRoom _room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.choseActionEvent += HandlePlayerChoseAction;
            
            _roomController = new RoomController();
            var playerShip = FactoryContainer.ShipFactory.GenerateCommandShip();

            _room = FactoryContainer.RoomFactory.GenerateRoom(new RoomTemplate(1, RoomFlavor.Kelp, "trade"), true);
            _room.SetPlayerShip(playerShip);
            
            _roomController.StartNextRoom(_room, _room);
            
            scrollView.AddCells(CalculateLookText(_room));

            StartCoroutine(Blink.BlinkLoop());
        }

        private IEnumerable<string> CalculateLookText(IRoom room)
        {
            var lookText = new List<string>
            {
                room.PlayerShip.GetLookText(),
                room.GetLookText()
            };
            room.Entities.ForEach(e => lookText.Add(e.GetLookText()));
            return lookText;
        }

        private void HandleLinkTouchedEvent(string guid)
        {
            var entity = _room.Entities.FirstOrDefault(e => e.Id == guid) ?? (ITextEntity) _room.PlayerShip;
            abController.ShowControl(entity.DialogueContent);
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            //if player warped -> break
            if (playerAction is WarpAction)
            {
                return;
            }

            var actionResults = _roomController.GetActionResults(playerAction, _room);
            
            scrollView.AddCells(actionResults);
            
            _roomController.DoCleanup(_room);
            _roomController.CalculateNewDialogues(_room);
            
            _room.Tick();
        }
    }
}