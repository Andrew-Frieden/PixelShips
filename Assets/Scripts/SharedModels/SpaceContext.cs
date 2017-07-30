using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class SpaceContext
    {
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Ship> Ships { get; set; }
        public IEnumerable<ShipFeed> ShipFeeds { get; set; }
        public IEnumerable<SpaceActor> Actors { get; set; }

        public SpaceContext(
            IEnumerable<RoomDto> roomDtos,
            IEnumerable<ShipDto> shipDtos,
            IEnumerable<ShipFeedDto> feedDtos,
            IEnumerable<SpaceActorDto> actorDtos)
        {
            Rooms = SetupRooms(roomDtos);
            Ships = SetupShips(shipDtos);
            ShipFeeds = SetupFeeds(feedDtos);
            Actors = SetupActors(actorDtos);
        }

        private IEnumerable<ShipFeed> SetupFeeds(IEnumerable<ShipFeedDto> dtos)
        {
            return dtos.Select(d => d.FromDto());
        }

        private IEnumerable<Ship> SetupShips(IEnumerable<ShipDto> dtos)
        {
            var ships = new List<Ship>();

            foreach (var dto in dtos)
            {
                var nextShip = BuildDisconnectedShip(dto);
                nextShip.Room = Rooms.Single(r => r.Id == dto.RoomId);
                ships.Add(nextShip);
            }

            return ships;
        }

        private IEnumerable<SpaceActor> SetupActors(IEnumerable<SpaceActorDto> dtos)
        {
            var actionFactory = new SpaceActionFactory(this);
            var actors = new List<SpaceActor>();

            foreach (var dto in dtos)
            {
                var actor = new SpaceActor
                {
                    Id = dto.Id,
                    ActorType = dto.ActorType,
                    LastResolved = dto.LastResolved,
                    Version = dto.Version,
                    Actions = dto.Actions.Select(a => actionFactory.GetSpaceAction(a)).ToList()
                };
                actors.Add(actor);
            }
            return actors;
        }

        private IEnumerable<Room> SetupRooms(IEnumerable<RoomDto> dtos)
        {
            var roomModels = dtos.Select(r => BuildDisconnectedRoom(r)).ToList();
            foreach (var r in dtos)
            {
                var nextRoom = roomModels.Single(model => model.Id == r.Id);
                foreach (var exitId in r.Exits)
                {
                    var nextExit = roomModels.Single(model => model.Id == exitId);
                    nextRoom.Exits.Add(nextExit);
                }
            }
            return roomModels;
        }

        private Room BuildDisconnectedRoom(RoomDto dto)
        {
            return new Room()
            {
                Id = dto.Id,
                Description = dto.Description,
                X = dto.X,
                Y = dto.Y,
                Exits = new List<Room>()
            };
        }

        private Ship BuildDisconnectedShip(ShipDto dto)
        {
            return new Ship()
            {
                Hull = dto.Hull,
                Id = dto.Id,
                Version = dto.Version,
                LastJump = dto.LastJump,
                MaxHull = dto.MaxHull,
                Name = dto.Name
            };
        }
    }
}
