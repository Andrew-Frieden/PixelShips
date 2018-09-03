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

        public void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            
            //var tabby = (IRoomActor) nextRoom.Entities[0];
            //tabby.DialogueContent.OptionAAction = new AttackAction(nextRoom.PlayerShip, tabby, 17);
            //tabby.DialogueContent.OptionBAction = new CreateDelayedAttackActorAction(nextRoom.PlayerShip, tabby, 3, 39);
            
            foreach(var ent in nextRoom.Entities)
            {
                if (ent != nextRoom.PlayerShip)
                {
                    ent.DialogueContent = ent.CalculateDialogue(nextRoom);
                }
            }

            nextRoom.PlayerShip.DialogueContent.MainText = "Your ship looks like a standard frigate.";
            nextRoom.PlayerShip.DialogueContent.OptionAText = "Create a shield.";
            nextRoom.PlayerShip.DialogueContent.OptionBText = "Spin up your warp drive.";
            
            nextRoom.PlayerShip.DialogueContent.OptionAAction = new CreateShieldActorAction(nextRoom.PlayerShip, null, 3, 5);
            nextRoom.PlayerShip.DialogueContent.OptionBAction = new CreateWarpDriveActorAction(nextRoom.PlayerShip, 2);

            //TODO: Make a scroll view controller method to handle printing a room and all its entities to cells
            scrollView.AddCells(new List<string>() { nextRoom.GetLookText(), nextRoom.PlayerShip.GetLookText(), nextRoom.Entities[0].GetLookText() });
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
                entity.AfterAction(_room);
            }
            
            foreach (var entity in _room.Entities)
            {
                if (entity != _room.PlayerShip)
                {
                    entity.DialogueContent = entity.CalculateDialogue(_room);
                }
            }
            
            _room.Tick();
        }
    }
}