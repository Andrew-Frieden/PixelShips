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
        private string SaveFilePath = Application.dataPath + "/SaveData.json";

        public GameState Load()
        {
            var jsonData = File.ReadAllText(SaveFilePath);
            var saveState = JsonConvert.DeserializeObject<SaveState>(jsonData);
            return LoadGameState(saveState);
        }
        
        public void Save(GameState state)
        {
            var saveState = BuildSaveState(state);
            var jsonData = JsonConvert.SerializeObject(saveState);
            File.WriteAllText(SaveFilePath, jsonData);
        }

        public GameState CreateNewGameState()
        {
            return new GameState
            {
                CommandShip = FactoryContainer.ShipFactory.GenerateCommandShip(),
                Room = FactoryContainer.RoomFactory.GenerateRoom(new RoomTemplate(5, RoomFlavor.Kelp, "trade"))
            };
        }

        private SaveState BuildSaveState(GameState state)
        {
            return new SaveState
            {
                Room = state.Room.ToDto(),
                SaveTime = DateTime.Now
            };
        }

        private GameState LoadGameState(SaveState save)
        {
            var state = new GameState()
            {
                Room = save.Room.FromDto(),
                CommandShip = save.CommandShip.FromDto()
            };

            state.Room.SetPlayerShip(state.CommandShip);

            //  grab all the mob dtos and build mob entities
            //var mobs = new List<Mob>();
            //save.Room.Mobs.ForEach(dto => mobs.Add(new Mob(dto.Description, dto.Link, dto.Hull, null)));

            //  create a collection to put all entities into (mobs, npcs, hazards)
            var entities = new List<IRoomActor>();
            //entities.AddRange(mobs);

            //  add all the entities to the room
            entities.ForEach(e => state.Room.AddEntity(e));

            var contents = save.Room.GetContent();

            //  setup the DialogueContent for every mob
            //save.Room.Mobs.ForEach(dto => state.Room.FindEntity(dto.Id).DialogueContent = dto.Content.FromDto(state.Room));

            //  setup the DialogueContent for the player ship
            state.CommandShip.CalculateDialogue(state.Room);

            state.CurrentTime = DateTime.Now;
            return state;
        }
    }
}
