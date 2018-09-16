using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models;
using Models.Actions;
using Models.Dtos;
using Models.Factories;
using Models.Stats;
using UnityEngine;
using Widgets.Scroller;
using static Models.CommandShip;

namespace Controller
{
    public class CommandViewController : MonoBehaviour
    {   
        [SerializeField] private ScrollViewController _scrollView;
        [SerializeField] private ABDialogueController _abController;
        [SerializeField] private ShipHudController _shipHudController;
        
        public Blink Blink;
        private IRoom _room;

        private CommandShip PlayerShip => _room.PlayerShip;

        private bool _warpToNextRoom = false;

        private void Start()
        {
            //TODO: A more organized way to track these event listeners
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.onRoomActionSelect += HandlePlayerChoseAction;
            ScrollCellTextTyper.scrollCellTyperFinishedEvent += HandleScrollCellTyperFinishedEvent;
            SimpleAction.onPlayerTookDamageEvent += HandlePlayerTookDamageEvent;
            RoomController.onRoomHealEvent += HandleRoomHealEvent;

            var playerShip = GameManager.Instance.GameState.CommandShip;
            _room = GameManager.Instance.GameState.Room;
            _room.SetPlayerShip(playerShip);
            
            //TODO: abstract the stats lookup
            _shipHudController.InitializeShipHud(playerShip);
            
            RoomController.StartNextRoom(_room, _room);

            _scrollView.AddCells(CalculateLookText(_room));

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
            _abController.ShowControl(entity.DialogueContent);
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            var text = RoomController.ResolveNextTick(_room, playerAction);

            _scrollView.DimCells();
            _scrollView.AddCells(text);

            //if Exit is populated -> player is warping
            if (PlayerShip.WarpTarget != null)
            {
                _warpToNextRoom = true;
            }

            //if Player is dead -> disable all interactions
            if (PlayerShip.IsDestroyed)
            {
                _scrollView.DisableInteractions();
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
            yield return new WaitForSecondsRealtime(1.0f);
            
            _scrollView.ClearScreen();

            var nextRoom = FactoryContainer.RoomFactory.GenerateRoom(PlayerShip.WarpTarget);
            RoomController.StartNextRoom(nextRoom, _room);
            _room = nextRoom;
                
            _scrollView.AddCells(CalculateLookText(_room));
        }

        private void HandlePlayerTookDamageEvent()
        {
            _shipHudController.UpdateShield();
            _shipHudController.UpdateHull();
            _scrollView.Shake();
        }

        private void HandleRoomHealEvent()
        {
            _shipHudController.UpdateShield();
        }
    }
}