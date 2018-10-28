using Models.Dtos;
using Models.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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

            state.CurrentTime = DateTime.Now;
            return state;
        }
    }
}
