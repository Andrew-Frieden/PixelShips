using System.Collections.Generic;
using System.IO;
using Models;
using Models.Dtos;
using Newtonsoft.Json;
using UnityEngine;

namespace Controller
{
    public class ContentLoadController
    {
        private const string BaseFolderPath = "Content";
        
        private const string HazardsContentFilePath = "/Hazards";
        private const string MobsContentFilePath = "/Mobs";
        private const string GatherablesContentFilePath = "/Gatherables";
        private const string WeaponsContentFilePath = "/Weapons";
        private const string HardwareContentFilePath = "/Hardware";
        private const string NpcContentFilePath = "/Npcs";

        //  may want to avoid loading all possible content into memory at once
        public GameContentDto Load()
        {
            var hazardsJsonData = Resources.Load<TextAsset>(BaseFolderPath + HazardsContentFilePath);
            var hazards =  JsonConvert.DeserializeObject<List<FlexData>>(hazardsJsonData.text);
            
            var mobsJsonData = Resources.Load<TextAsset>(BaseFolderPath + MobsContentFilePath);
            var mobs =  JsonConvert.DeserializeObject<List<FlexData>>(mobsJsonData.text);
            
            var gatherablesJsonData =Resources.Load<TextAsset>(BaseFolderPath + GatherablesContentFilePath);
            var gatherables =  JsonConvert.DeserializeObject<List<FlexData>>(gatherablesJsonData.text);

            if (gatherables == null)
                gatherables = new List<FlexData>();
            
            var weaponsJsonData = Resources.Load<TextAsset>(BaseFolderPath + WeaponsContentFilePath);
            var weapons =  JsonConvert.DeserializeObject<List<FlexData>>(weaponsJsonData.text);

            var hardwareData = Resources.Load<TextAsset>(BaseFolderPath + HardwareContentFilePath);
            var hardware = JsonConvert.DeserializeObject<List<FlexData>>(hardwareData.text);

            var npcData = Resources.Load<TextAsset>(BaseFolderPath + NpcContentFilePath);
            var npcs = JsonConvert.DeserializeObject<List<FlexData>>(npcData.text);

            return new GameContentDto(hazards, mobs, gatherables, weapons, hardware, npcs);
        }
    }
}