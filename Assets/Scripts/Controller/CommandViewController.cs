using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models;
using Models.Actions;
using Models.Factories;
using UnityEngine;
using Widgets.Scroller;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {   
        [SerializeField] private ScrollViewController scrollView;
        [SerializeField] private ABDialogueController abController;

        private RoomController _roomController;

        public Blink Blink;
        private IRoom _room;

        private bool _warpToNextRoom = false;

        private void Start()
        {
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.onRoomActionSelect += HandlePlayerChoseAction;
            ScrollCellTextTyper.scrollCellTyperFinishedEvent += HandleScrollCellTyperFinishedEvent;
            
            _roomController = new RoomController();
            var playerShip = FactoryContainer.ShipFactory.GenerateCommandShip();

            _room = FactoryContainer.RoomFactory.GenerateRoom(new RoomTemplate(5, RoomFlavor.Kelp, "trade"));
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
            var entity = _room.FindEntity(guid);
            abController.ShowControl(entity.DialogueContent);
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            var text = _roomController.ResolveNextTick(_room, playerAction);

            scrollView.DimCells();
            scrollView.AddCells(text);

            //if Exit is populated -> player is warping
            if (_room.Exit != null)
            {
                _warpToNextRoom = true;
            }
        }

        private void HandleScrollCellTyperFinishedEvent()
        {
            if (!_warpToNextRoom) return;
            
            _warpToNextRoom = false;
            StartCoroutine(WaitAndStartNextRoom());
        }
        
        private IEnumerator WaitAndStartNextRoom()
        {
            yield return new WaitForSecondsRealtime(2.0f);
            
            scrollView.ClearScreen();
            _roomController.StartNextRoom(_room.Exit, _room);
                
            _room = _room.Exit;
                
            scrollView.AddCells(CalculateLookText(_room));
        }
    }
}