using Common;
using Models.Actions;
using Models.Dtos;
using Models.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Models
{
    public class SaveLoadController
    {
        public static string SaveFilePath = UnityEngine.Application.persistentDataPath + "/SaveData.json";
        public bool HasSaveData => File.Exists(SaveFilePath);
        public SaveState SaveData;

        public void Init()
        {
            if (HasSaveData)
            {
                var jsonData = File.ReadAllText(SaveFilePath);
                try
                {
                    SaveData = JsonConvert.DeserializeObject<SaveState>(jsonData);
                }
                catch(Exception ex)
                {
                    //  deserialize failed
                    SaveData = new InvalidSaveState();
                }
            }
        }

        public void Delete()
        {
            if (HasSaveData)
            {
                File.Delete(SaveFilePath);

                var metaFile = SaveFilePath + ".meta";
                if (File.Exists(metaFile))
                    File.Delete(metaFile);
            }
        }

        public GameState Load()
        {
            return BuildGameStateFromSaveState(SaveData);
        }
        
        public void Save(GameState state)
        {
            //  TODO don't save the bootstraping state
            var saveState = BuildSaveStateFromGameState(state);
            var jsonData = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(SaveFilePath, jsonData);
        }

        //  hack together a valid gamestate
        public GameState CreateNewGameState()
        {
            return new GameState
            {
                CurrentExpedition = new Expedition
                {
                    CmdShip = GameManager.ShipFactory.GenerateCommandShip(GameManager.RoomFactory),
                    Room = (Room) GameManager.RoomFactory.GenerateRoom(new RoomTemplate(10, RoomFlavor.Kelp)),
                    CurrentMission = new Mission { MissionLevel = 1 },
                    Ticks = 0,
                    Jumps = 0
                },
                Home = new Homeworld
                {
                    PlanetName = "Galvanius",
                    DeepestExpedition = 0,
                    HardestMonsterSlainScore = 0
                }
            };
        }

        public GameState CreateBootstrapGameState(bool devSettingsEnabled)
        {
            return new GameState
            {
                CurrentExpedition = new Expedition
                {
                    CmdShip = GameManager.ShipFactory.GenerateCommandShip(GameManager.RoomFactory),
                    Room = (Room)GameManager.RoomFactory.GenerateBootstrapRoom(!devSettingsEnabled),
                    CurrentMission = new Mission { MissionLevel = 0 },
                    Ticks = 0,
                    Jumps = 0
                },
                Home = new Homeworld
                {
                    PlanetName = "",
                    DeepestExpedition = 0,
                    HardestMonsterSlainScore = 0
                }
            };
        }

        private SaveState BuildSaveStateFromGameState(GameState state)
        {
            var saveData = new SaveState
            {
                HomeworldData = state.Home.ToDto(),
                ExpeditionData = state.CurrentExpedition.ToDto(),
                SaveTime = DateTime.Now,
                CmdViewCellData = new List<string>()
            };
            return saveData;
        }

        private GameState BuildGameStateFromSaveState(SaveState saveData)
        {
            var state = new GameState()
            {
                Home = saveData.HomeworldData.FromDto()
            };

            var expData = saveData.ExpeditionData;
            var exp = new Expedition()
            {
                Ticks = expData.Ticks,
                Jumps = expData.Jumps,
                CurrentMission = expData.MissionData.FromDto()
            };
            state.CurrentExpedition = exp;

            exp.CmdShip = new CommandShip(expData.ShipData);
            exp.Room = new Room(expData.RoomData);
            exp.Room.SetPlayerShip(exp.CmdShip);

            //  setup all the dialogue content which requires building actions that have references to entities
            //exp.Room.DialogueContent = expData.RoomData.ContentDto.FromDto(exp.Room);
            //foreach (var dto in expData.RoomData.Entities)
            //{
            //    var entity = exp.Room.Entities.Single(e => e.Id == dto.Id);
            //    entity.DialogueContent = dto.ContentDto.FromDto(exp.Room);
            //}

            //exp.CmdShip.DialogueContent = expData.ShipData.ContentDto.FromDto(exp.Room);
            //foreach (var dto in expData.ShipData.HardwareData)
            //{
            //    var hardware = exp.CmdShip.Hardware.Single(h => h.Id == dto.Id);
            //    hardware.DialogueContent = dto.ContentDto.FromDto(exp.Room);
            //}

            state.CurrentTime = DateTime.Now;
            return state;
        }
    }
}
