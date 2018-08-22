using System;
using System.Collections.Generic;

namespace Models
{
    public class SaveLoadController
    {
        public GameState Load()
        {
            var saveState = new SaveState();
            //  deserialize savestate from json
            return BuildGameState(saveState);
        }
        
        public void Save(GameState state)
        {
            var saveState = BuildSaveState(state);
            //  serialize and persist saveState
        }
        
        private GameState BuildGameState(SaveState save)
        {
            var state = new GameState();
            
            
            
            return state;
        }
        
        private SaveState BuildSaveState(GameState state)
        {
            var save = new SaveState
            {
                Room = state.Room.ToDto(),
                SaveTime = DateTime.Now
            };

            return save;
        }
        
    }
    
    public static class DtoHelpers
    {
        public static RoomDto ToDto(this Room room)
        {
            var roomDto = new RoomDto
            {
                PlayerShip = room.PlayerShip.ToDto(),
                Mobs = new List<MobDto>()
            };
            
            foreach (var entity in room.Entities)
            {
                if (entity is Mob)
                {
                    var mob = (Mob)entity;
                    roomDto.Mobs.Add(mob.ToDto());
                }
                //else if (entity is Hazard)
                //{
                //    var hazard = (Hazard)entity;
                //    roomDto.Hazards.Add(hazard.ToDto());
                //}
            }

            return roomDto;
        }
        
        public static ShipDto ToDto(this CommandShip ship)
        {
            return new ShipDto
            {
                Id = ship.Id,
                Combat = ship.Combat,
                Gathering = ship.Gathering,
                Hull = ship.Hull,
                Speed = ship.Speed,
                Transport = ship.Transport,
                Intelligence = ship.Intelligence,
                ContentDto = ship.DialogueContent.ToDto()
            };
        }
        
        public static MobDto ToDto(this Mob mob)
        {
            return new MobDto
            {
                Id = mob.Id,
                Hull = mob.Hull,
                Description = mob.Description,
                Link = mob.Link,
                Content = mob.DialogueContent.ToDto()
            };
        }
        
        public static ABContentDto ToDto(this ABDialogueContent content)
        {
            var contentDto = new ABContentDto
            {
                MainText = content.MainText,
                OptionAText = content.OptionAText,
                OptionBText = content.OptionBText
            };

            var actionModels = new List<IRoomAction>() { content.OptionAAction, content.OptionBAction };
            foreach (var act in actionModels)
            {
                if (act is SimpleAction)
                {
                    var simple = (SimpleAction)act;
                    contentDto.AddSimpleAction(simple.ToDto());
                }
                //else if (act is ComplexAction)
                //{
                //    var complex = (ComplexAction)act;
                //    contentDto.AddComplexAction(complex.ToDto());
                //}
                else
                {
                    throw new Exception("ABContent.ToDto() => unable to convert action to dto.");
                }
            }

            return contentDto;
        }
        
        public static SimpleActionDto ToDto(this SimpleAction simpleAction)
        {
            return new SimpleActionDto();
        }
    }
    
    public static class RoomActionFactory
    {
        public static IRoomAction GetSimpleAction(SimpleActionDto actionDto)
        {
            throw new NotImplementedException();
        }
    }
}
