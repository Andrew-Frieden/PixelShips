using Common;
using Models.Actions;
using Models.Dtos;
using Models.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Models
{
    public class SaveLoadController
    {
        public static string SaveFilePath = UnityEngine.Application.dataPath + "/SaveData.json";
        public bool HasSaveData => File.Exists(SaveFilePath);
        public SaveState SaveData;

        public void Init()
        {
            if (HasSaveData)
            {
                var jsonData = File.ReadAllText(SaveFilePath);
                SaveData = JsonConvert.DeserializeObject<SaveState>(jsonData);
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
            var saveState = BuildSaveStateFromGameState(state);
            var jsonData = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(SaveFilePath, jsonData);
        }
        
        //  testing for development
        public void SerializeContent(List<FlexData> data)
        {
            Debug.Log($"serializing game content to {SaveFilePath}");
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("serialize complete.");
        }

        //  hack together a valid gamestate
        public GameState CreateNewGameState()
        {
            return new GameState
            {
                CurrentExpedition = new Expedition
                {
                    CmdShip = FactoryContainer.ShipFactory.GenerateCommandShip(),
                    Room = (Room)FactoryContainer.RoomFactory.GenerateRoom(new RoomTemplate(1, RoomFlavor.Kelp)),
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

        private SaveState BuildSaveStateFromGameState(GameState state)
        {
            return new SaveState
            {
                Room = state.CurrentExpedition.Room.ToDto(),
                SaveTime = DateTime.Now
            };
        }

        private GameState BuildGameStateFromSaveState(SaveState save)
        {
            var state = new GameState()
            {
              //  Room = (Room)save.Room.FromDto(),
              //  CommandShip = save.CommandShip.FromDto()
            };

            //state.Room.SetPlayerShip(state.CommandShip);

            //  grab all the mob dtos and build mob entities
            //var mobs = new List<Mob>();
            //save.Room.Mobs.ForEach(dto => mobs.Add(new Mob(dto.Description, dto.Link, dto.Hull, null)));

            //  create a collection to put all entities into (mobs, npcs, hazards)
            var entities = new List<IRoomActor>();
            //entities.AddRange(mobs);

            //  add all the entities to the room
            //entities.ForEach(e => state.Room.AddEntity(e));

            var contents = save.Room.GetContent();

            //  setup the DialogueContent for every mob
            //save.Room.Mobs.ForEach(dto => state.Room.FindEntity(dto.Id).DialogueContent = dto.Content.FromDto(state.Room));

            //  setup the DialogueContent for the player ship
            //state.CommandShip.CalculateDialogue(state.Room);

            state.CurrentTime = DateTime.Now;
            return state;
        }
    }
}
