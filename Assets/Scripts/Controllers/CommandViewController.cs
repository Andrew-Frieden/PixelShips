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
using TextSpace.Framework.IoC;
using TextSpace.Services.Factories;

namespace TextSpace.Controllers
{
    public class CommandViewController : MonoBehaviour
    {
        public Blink Blink;
        [SerializeField] private ScrollViewController _scrollView;
        [SerializeField] private ABDialogueController _abController;
        [SerializeField] private ShipHudController _shipHudController;
        

        private CommandShip PlayerShip => ServiceContainer.Resolve<IShipProvider>().Ship;
        private IRoom Room => ServiceContainer.Resolve<IRoomProvider>().Room;

        private RoomFactoryService RoomFactory => ServiceContainer.Resolve<RoomFactoryService>();
        private RoomService RoomService => ServiceContainer.Resolve<RoomService>();

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
            StartRoom();

            UIResponseBroadcaster.Broadcast(UIResponseTag.ViewCmd);
            UIResponseBroadcaster.Broadcast(UIResponseTag.ShowHUD);
            UIResponseBroadcaster.Broadcast(UIResponseTag.ShowNavBar);
            UIResponseBroadcaster.Broadcast(UIResponseTag.UpdateHomeworld);

            var startingRoom = Room.Entities.Where(n => n is HomeworldNpc).Any();

            _scrollView.AddCells(CalculateLookText(Room, startingRoom));
            StartCoroutine(Blink.BlinkLoop());
        }

        public void BootstrapView()
        {
            StartRoom();

            var text = RoomService.ResolveNextTick(Room, new DoNothingAction(PlayerShip));
            _scrollView.AddCells(text);
        }

        private void StartRoom()
        {
            _scrollView.ClearScreen();
            _shipHudController.InitializeShipHud();

            RoomService.StartRoom();
        }

        private IEnumerable<TagString> CalculateLookText(IRoom room, bool startingRoom = false)
        {
            var lookResults = new List<TagString>();

            if (startingRoom)
            {
                lookResults.Add("Your <> rests in-system, ready to take on the void.".Encode("starship", PlayerShip.Id, LinkColors.Player).Tag());
            }
            else
            {
                lookResults.Add(PlayerShip.GetLookText());
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
            var entity = Room.FindEntity(guid);
            var content = entity != null ? entity.DialogueContent : DialogueBuilder.EmptyDialogue(Room);
            _abController.ShowControl(content);
        }

        private void HandlePlayerChoseAction(IRoomAction playerAction)
        {
            _scrollView.DimCells();

            var text = RoomService.ResolveNextTick(Room, playerAction);
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
             
            RoomService.StartRoom();

            _scrollView.ClearScreen();
            _scrollView.AddCells(CalculateLookText(Room));
            _shipHudController.UpdateSector();
        }
    }
}