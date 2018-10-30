using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextEncoding;
using TextSpace.Events;
using TextSpace.Services;
using UnityEngine;
using Widgets.Scroller;

namespace TextSpace.Controllers
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
            //TODO: A more organized way to track these event listeners (cmdviewcontroller probably shouldn't know about scrollcells - maybe do something like EventTagBroadcaster)
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.onRoomActionSelect += HandlePlayerChoseAction;
            ScrollCellTextTyper.scrollCellTyperFinishedEvent += HandleScrollCellTyperFinishedEvent;
        }

        public void StartCommandView()
        {
            InitFromGameState();

            UIResponseBroadcaster.Broadcast(UIResponseTag.ViewCmd);
            UIResponseBroadcaster.Broadcast(UIResponseTag.ShowHUD);
            UIResponseBroadcaster.Broadcast(UIResponseTag.ShowNavBar);

            var startingRoom = _room.Entities.Where(n => n is HomeworldNpc).Any();

            _scrollView.AddCells(CalculateLookText(_room, startingRoom));
            StartCoroutine(Blink.BlinkLoop());
        }

        public void BootstrapView()
        {
            InitFromGameState();

            var text = RoomService.ResolveNextTick(_room, new DoNothingAction(_room.PlayerShip));
            _scrollView.AddCells(text);
        }

        private void InitFromGameState()
        {
            _room = GameManager.Instance.GameState.CurrentExpedition.Room;
            _room.SetPlayerShip(GameManager.Instance.GameState.CurrentExpedition.CmdShip);
            _scrollView.ClearScreen();
            _shipHudController.InitializeShipHud(_room);
            RoomService.StartNextRoom(_room, _room);
        }

        private IEnumerable<TagString> CalculateLookText(IRoom room, bool startingRoom = false)
        {
            var lookResults = new List<TagString>();

            if (startingRoom)
            {
                lookResults.Add("Your <> rests in-system, ready to take on the void.".Encode("starship", room.PlayerShip.Id, LinkColors.Player).Tag());
            }
            else
            {
                lookResults.Add(room.PlayerShip.GetLookText());
            }

            lookResults.Add(room.GetLookText());

            foreach (var entity in room.Entities)
            {
                if (!entity.IsHidden)
                {
                    lookResults.Add(entity.GetLookText());
                }
            }

            return lookResults;
        }

        private void HandleLinkTouchedEvent(string guid)
        {
            var entity = _room.FindEntity(guid);
            var content = entity != null ? entity.DialogueContent : DialogueBuilder.EmptyDialogue(_room);
            _abController.ShowControl(content);
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            _scrollView.DimCells();

            var text = RoomService.ResolveNextTick(_room, playerAction);
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

            var nextRoom = GameManager.RoomFactory.GenerateRoom(PlayerShip.WarpTarget);
            RoomService.StartNextRoom(nextRoom, _room);
            _room = nextRoom;

            //  TODO find a better way to update the GameState's current room
            GameManager.Instance.GameState.CurrentExpedition.Room = (Room)_room;
                
            _scrollView.AddCells(CalculateLookText(_room));

            _shipHudController.UpdateSector(_room);
        }
    }
}