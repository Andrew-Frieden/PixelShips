using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelSpace.Models.SharedModels
{
    public class RoomState : ISpaceState
    {
        public Room Room { get { return Rooms.Single(); } }

        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Ship> Ships { get; set; }
        public IEnumerable<ShipFeed> Feeds { get; set; }

        public IDictionary<string, Room> RoomMap { get; set; }
        public IDictionary<string, Ship> ShipMap { get; set; }
        public IDictionary<string, ShipFeed> FeedMap { get; set; }

        public IEnumerable<SpaceAction> Actions { get; set; }

        private SpaceActionFactory actionFactory;

        public RoomState(Room room, IEnumerable<Ship> ships, IEnumerable<ShipFeed> feeds, IEnumerable<SpaceActionDbi> actions)
        {
            //  setup the room, actions and feeds collections

            //  initialize map objects
            ShipMap = new Dictionary<string, Ship>();
            RoomMap = new Dictionary<string, Room>();
            FeedMap = new Dictionary<string, ShipFeed>();

            //  setup the room model
            room.Ships = new List<Ship>();
            Rooms = new List<Room> { room };
            RoomMap.Add(room.Id, room);

            //  setup ships models and link with room
            Ships = ships;
            foreach (var ship in Ships)
            {
                room.Ships.Add(ship);
                ship.Room = room;
                ShipMap.Add(ship.Id, ship);
            }

            //  setup feeds models
            Feeds = feeds;
            foreach (var feed in Feeds)
            {
                FeedMap.Add(feed.Id, feed);
            }

            //  setup action models
            actionFactory = new SpaceActionFactory(this);
            var actionList = new List<SpaceAction>();
            foreach (var actionDbi in actions)
            {
                var actionModel = actionFactory.GetModel(actionDbi);
                actionList.Add(actionModel);
            }
            Actions = actionList;
        }


    }
}
