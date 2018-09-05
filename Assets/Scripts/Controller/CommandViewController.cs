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

        public Blink Blink;
        private IRoom _room;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.choseActionEvent += HandlePlayerChoseAction;
            
            var playerShip = ShipFactory.GenerateCommandShip();
            var roomFactory = new RoomFactory();

            _room = roomFactory.GenerateRoom(new RoomTemplate(1, RoomFlavor.Kelp, "trade"), true);
            _room.SetPlayerShip(playerShip);
            
            StartNextRoom(_room, _room);

            StartCoroutine(Blink.BlinkLoop());
        }

        private void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            
            _room.PlayerShip.DialogueContent =  _room.PlayerShip.CalculateDialogue(nextRoom);
            
            foreach(var ent in nextRoom.Entities)
            {
                if (ent != nextRoom.PlayerShip)
                {
                    ent.DialogueContent = ent.CalculateDialogue(nextRoom);
                }
            }

            scrollView.AddCells(CalculateLookText(nextRoom));
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
            var actionsToExecute = new List<IRoomAction>
            {
                playerAction
            };

            var actionResults = new List<string>();

            foreach (var entity in _room.Entities)
            {
                var nextAction = entity.GetNextAction(_room);
                
                //if player warped -> break
                if (nextAction is WarpAction)
                {
                    StartNextRoom(_room.Exits.First(), _room);
                    return;
                }
                
                actionsToExecute.Add(nextAction);
            }

            foreach (var action in actionsToExecute)
            {
                actionResults.AddRange(action.Execute(_room));
            }
            
            scrollView.AddCells(actionResults);

            foreach (var entity in _room.Entities)
            {
                //entity.AfterAction(_room);
            }
            
            foreach (var entity in _room.Entities)
            {
                if (entity != _room.PlayerShip && entity.PrintToScreen)
                {
                    entity.DialogueContent = entity.CalculateDialogue(_room);
                }
            }
            
            _room.PlayerShip.DialogueContent =  _room.PlayerShip.CalculateDialogue(_room);
            
            _room.Tick();
        }
    }
}