using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models;
using Models.Actions;
using Models.Dialogue;
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
            //TODO: A more organized way to track these event listeners (cmdviewcontroller probably shouldn't know about scrollcells - maybe do something like EventTagBroadcaster)
            ScrollCell.linkTouchedEvent += HandleLinkTouchedEvent;
            ABDialogueController.onRoomActionSelect += HandlePlayerChoseAction;
            ScrollCellTextTyper.scrollCellTyperFinishedEvent += HandleScrollCellTyperFinishedEvent;
        }

        //Called the first time the player spawns a ship and goes to the command view.
        public void StartCommandView()
        {
            //Get our initial room and ship from the game state
            _room = GameManager.Instance.GameState.CurrentExpedition.Room;
            _room.SetPlayerShip(GameManager.Instance.GameState.CurrentExpedition.CmdShip);
            
            RoomController.StartNextRoom(_room, _room);
            
            //  TODO make this event based so cmdviewcontroller doesn't need to call this or know about shiphudcontroller
            _shipHudController.InitializeShipHud(_room);
            
            _scrollView.ClearScreen();
            _scrollView.AddCells(CalculateLookText(_room));
            
            StartCoroutine(Blink.BlinkLoop());
        }

        private IEnumerable<TagString> CalculateLookText(IRoom room)
        {
            var lookResults = new List<TagString>()
            {
                new TagString()
                {
                    Text = room.PlayerShip.GetLookText().Text,
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = room.GetLookText().Text,
                    Tags = new List<EventTag> { }
                }
            };

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
            _abController.ShowControl(content); // pass room in here so we can calculate IsValid() ?
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            _scrollView.DimCells();

            var text = RoomController.ResolveNextTick(_room, playerAction, _shipHudController, _scrollView);
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
            RoomController.StartNextRoom(nextRoom, _room);
            _room = nextRoom;

            //  TODO find a better way to update the GameState's current room
            GameManager.Instance.GameState.CurrentExpedition.Room = (Room)_room;
                
            _scrollView.AddCells(CalculateLookText(_room));

            _shipHudController.UpdateSector(_room);
        }
    }
}