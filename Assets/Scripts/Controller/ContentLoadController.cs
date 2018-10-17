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
        
        public GameContentDto Load()
        {
            var hazardsJsonData = Resources.Load<TextAsset>(BaseFolderPath + HazardsContentFilePath);
            var hazards =  JsonConvert.DeserializeObject<List<FlexData>>(hazardsJsonData.text);
            
            var mobsJsonData = Resources.Load<TextAsset>(BaseFolderPath + MobsContentFilePath);
            var mobs =  JsonConvert.DeserializeObject<List<FlexData>>(mobsJsonData.text);
            
            var gatherablesJsonData =Resources.Load<TextAsset>(BaseFolderPath + GatherablesContentFilePath);
            var gatherables =  JsonConvert.DeserializeObject<List<FlexData>>(gatherablesJsonData.text);
            
            var weaponsJsonData = Resources.Load<TextAsset>(BaseFolderPath + WeaponsContentFilePath);
            var weapons =  JsonConvert.DeserializeObject<List<FlexData>>(weaponsJsonData.text);

            return new GameContentDto(hazards, mobs, gatherables, weapons);
        }
    }
}