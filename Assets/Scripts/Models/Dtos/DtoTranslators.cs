using Models.Actions;
using Models.Factories;
using System;
using System.Collections.Generic;

namespace Models.Dtos
{
    public static class DtoTranslators
    {
        #region ToDto
        public static RoomDto ToDto(this IRoom room)
        {
            var roomDto = new RoomDto
            {
                //Mobs = new List<MobDto>()
            };

            foreach (var entity in room.Entities)
            {
                //if (entity is Mob)
                //{
                //    var mob = (Mob)entity;
                //    roomDto.Mobs.Add(mob.ToDto());
                //}
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
                ContentDto = ship.DialogueContent?.ToDto()
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
                if (act == null)
                    continue;

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
                    throw new Exception($"ABContent.ToDto() => unable to convert action to dto: {act.GetType().ToString()} ada");
                }
            }

            return contentDto;
        }

        public static SimpleActionDto ToDto(this SimpleAction simpleAction)
        {
            return new SimpleActionDto
            {
                ActionName = simpleAction.ActionName,
                Stats = simpleAction.Stats,
                Values = simpleAction.Values,
                SourceId = simpleAction.Source.Id,
                TargetId = simpleAction.Target.Id
            };
        }
        
        public static FlexEntityDto ToDto(this FlexEntity entity)
        {
            return new FlexEntityDto
            {
                EntityType = entity.GetType().FullName,
                Id = entity.Id,
                Values = entity.Values,
                Stats = entity.Stats
            };
        }
        
        #endregion

        #region FromDto

        public static IRoom FromDto(this RoomDto dto)
        {
            return new Room(dto.Id, dto.Description, dto.Link);
        }

        public static CommandShip FromDto(this ShipDto dto)
        {
            return new CommandShip(dto.Id, dto.Gathering, dto.Transport, dto.Intelligence, dto.Combat, dto.Speed, dto.Hull);
        }

        /// <summary>
        /// this requires the room to already contain all room entities
        /// </summary>
        public static ABDialogueContent FromDto(this ABContentDto content, IRoom room)
        {
            var actionA = content.OptionAActionSimple.FromDto(room);
            var actionB = content.OptionBActionSimple.FromDto(room);

            return new ABDialogueContent
            {
                MainText = content.MainText,
                OptionAText = content.OptionAText,
                OptionBText = content.OptionBText,
                OptionAAction = actionA,
                OptionBAction = actionB
            };
        }
        #endregion
    }
}