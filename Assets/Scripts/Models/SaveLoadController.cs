﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Models
{
    public class SaveLoadController
    {
        private string SaveFilePath = Application.dataPath + "/SaveData.json";

        public GameState Load()
        {
            var jsonData = File.ReadAllText(SaveFilePath);
            var saveState = JsonConvert.DeserializeObject<SaveState>(jsonData);
            return BuildGameState(saveState);
        }
        
        public void Save(GameState state)
        {
            var saveState = BuildSaveState(state);
            var jsonData = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(SaveFilePath, jsonData);
        }
        
        private GameState BuildGameState(SaveState save)
        {
            var state = new GameState();

            var room = new Room();
            var playerShip = new CommandShip();

            state.CurrentTime = DateTime.Now;
            return state;
        }
        
        private SaveState BuildSaveState(GameState state)
        {
            return new SaveState
            {
                Room = state.Room.ToDto(),
                SaveTime = DateTime.Now
            };
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
                ContentDto = ship.DialogueContent?.ToDto()
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
                SourceId = simpleAction.Source.Id,
                TargetId = simpleAction.Target.Id
            };
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
