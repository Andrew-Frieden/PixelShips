using Models.Actions;
using Models.Factories;
using System;
using System.Collections.Generic;

namespace Models.Dtos
{
    public static partial class DtoTranslators
    {
        #region ToDto
        public static RoomDto ToDto(this Room room)
        {
            var dto = new RoomDto
            {
                Id = room.Id,
                Description = room.Description,
                Flavor = room.Flavor,
                LookText = room.LookText,
                Name = room.Name,
                Entities = new List<FlexEntityDto>(),
                ExitDtos = new List<RoomTemplateDto>(),
                ContentDto = room.DialogueContent?.ToDto()
            };

            foreach (var exit in room.Exits)
            {
                dto.ExitDtos.Add(exit.ToDto());
            }

            foreach (var entity in room.Entities)
            {
                if (entity is FlexEntity)
                {
                    dto.Entities.Add(((FlexEntity)entity).ToDto());
                }
                else
                {
                    throw new NotSupportedException("Room.ToDto() found a unsupported entity in the room.");
                }
            }

            return dto;
        }

        public static RoomTemplateDto ToDto(this RoomTemplate template)
        {
            return new RoomTemplateDto
            {
                ActorFlavors = new List<RoomActorFlavor>(template.ActorFlavors),
                PowerLevel = template.PowerLevel,
                Flavor = template.Flavor
            };
        }

        public static ShipDto ToDto(this CommandShip ship)
        {
            var dto = new ShipDto
            {
                Id = ship.Id,
                Stats = ship.Stats,
                Values = ship.Values,
                ContentDto = ship.DialogueContent?.ToDto(),
                HardwareData = new List<FlexEntityDto>(),
                LightWeapon = ship.LightWeapon.ToDto(),
                HeavyWeapon = ship.HeavyWeapon.ToDto()
            };

            foreach (var h in ship.Hardware)
            {
                dto.HardwareData.Add(h.ToDto());
            }

            return dto;
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
                ActionType = simpleAction.GetType().FullName,
                Stats = simpleAction.Stats,
                Values = simpleAction.Values,
                SourceId = simpleAction.Source?.Id,
                TargetId = simpleAction.Target?.Id
            };
        }
        
        public static FlexEntityDto ToDto(this FlexEntity entity)
        {
            return new FlexEntityDto
            {
                EntityType = entity.GetType().FullName,
                Id = entity.Id,
                Values = entity.Values,
                Stats = entity.Stats,
                ContentDto = entity.DialogueContent?.ToDto()
            };
        }

        public static ExpeditionDto ToDto(this Expedition exp)
        {
            var dto = new ExpeditionDto
            {
                Jumps = exp.Jumps,
                Ticks = exp.Ticks,
                MissionData = exp.CurrentMission.ToDto(),
                ShipData = exp.CmdShip.ToDto(),
                RoomData = exp.Room.ToDto()
            };
            return dto;
        }

        public static HomeworldDto ToDto(this Homeworld home)
        {
            return new HomeworldDto
            {
                DeepestExpedition = home.DeepestExpedition,
                HardestMonsterSlainScore = home.HardestMonsterSlainScore,
                PlanetName = home.PlanetName
            };
        }

        public static MissionDto ToDto(this Mission m)
        {
            return new MissionDto
            {
                MissionLevel = m.MissionLevel
            };
        }
        #endregion

        #region FromDto
        public static Homeworld FromDto(this HomeworldDto dto)
        {
            return new Homeworld
            {
                DeepestExpedition = dto.DeepestExpedition,
                HardestMonsterSlainScore = dto.HardestMonsterSlainScore,
                PlanetName = dto.PlanetName
            };
        }

        public static Mission FromDto(this MissionDto dto)
        {
            return new Mission
            {
                MissionLevel = dto.MissionLevel
            };
        }

        public static IRoom FromDto(this RoomDto dto)
        {
            //return new Room(dto.Id, dto.Description, dto.Link);
            throw new NotImplementedException();
        }

        public static CommandShip FromDto(this ShipDto dto)
        {
            throw new NotImplementedException();
            //return new CommandShip(dto.Id, dto.Gathering, dto.Transport, dto.Intelligence, dto.Combat, dto.Speed, dto.Hull);
        }

        /// <summary>
        /// this requires the room to already contain all room entities
        /// </summary>
        public static ABDialogueContent FromDto(this ABContentDto dto, IRoom room)
        {
            IRoomAction actionA = null;
            IRoomAction actionB = null;

            if (dto.OptionAActionSimple != null)
                actionA = dto.OptionAActionSimple.FromDto(room);

            if (dto.OptionBActionSimple != null)
                actionB = dto.OptionBActionSimple.FromDto(room);

            var content = new ABDialogueContent
            {
                MainText = dto.MainText,
                OptionAText = dto.OptionAText,
                OptionBText = dto.OptionBText,
                OptionAAction = actionA,
                OptionBAction = actionB
            };
            content.CalculateMode();
            return content;
        }
        #endregion
    }
}