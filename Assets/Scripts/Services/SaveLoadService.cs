using TextSpace.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TextSpace.Models;
using TextSpace.Framework;

namespace TextSpace.Services
{
    public interface ISaveManager : IResolvableService
    {
        bool HasSaveFile { get; }
        SaveState SaveFile { get; }
        string SaveFilePath { get; }
        void Delete();
    }

    public class SaveLoadService : ISaveManager
    {
        public string SaveFilePath => UnityEngine.Application.persistentDataPath + "/SaveData.json";
        public bool HasSaveFile => File.Exists(SaveFilePath);
        public SaveState SaveFile { get; private set; }

        public SaveLoadService()
        {
            DeserializeData();
        }

        private void DeserializeData()
        {
            if (HasSaveFile)
            {
                var jsonData = File.ReadAllText(SaveFilePath);
                try
                {
                    SaveFile = JsonConvert.DeserializeObject<SaveState>(jsonData);
                }
                catch(Exception)
                {
                    SaveFile = new InvalidSaveState();
                }
            }
        }

        public void Delete()
        {
            if (HasSaveFile)
            {
                File.Delete(SaveFilePath);

                var metaFile = SaveFilePath + ".meta";
                if (File.Exists(metaFile))
                    File.Delete(metaFile);
            }
        }

        public GameState Load()
        {
            return BuildGameStateFromSaveState(SaveFile);
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
                ExpeditionData = state.Expedition.ToDto(),
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
                CurrentMission = expData.MissionData == null ? null : expData.MissionData.FromDto()
            };
            state.Expedition = exp;

            exp.CmdShip = new CommandShip(expData.ShipData);
            exp.Room = new Room(expData.RoomData);

            state.CurrentTime = DateTime.Now;
            return state;
        }
    }
}
