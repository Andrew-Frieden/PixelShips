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
        private const string HazardsContentFilePath = "/Hazards_Content.json";
        private const string MobsContentFilePath = "/Mobs_Content.json";
        private const string GatherablesContentFilePath = "/Gatherables_Content.json";

        public List<FlexData> Content { get; private set; }
        
        //Dont Delete: Useful for outputing JSON
        //
        //public void SerializeContent(List<FlexData> data)
        //{
        //    Debug.Log($"serializing game content to {ContentFilePath}");
        //    var json = JsonConvert.SerializeObject(data);
        //    File.WriteAllText(ContentFilePath, json);
        //    Debug.Log("serialize complete.");
        //}
        
        public GameContentDto Load()
        {
            var hazardsJsonData = File.ReadAllText(UnityEngine.Application.dataPath + HazardsContentFilePath);
            var hazards =  JsonConvert.DeserializeObject<List<FlexData>>(hazardsJsonData);
            
            var mobsJsonData = File.ReadAllText(UnityEngine.Application.dataPath + MobsContentFilePath);
            var mobs =  JsonConvert.DeserializeObject<List<FlexData>>(mobsJsonData);
            
            var gatherablesJsonData = File.ReadAllText(UnityEngine.Application.dataPath + GatherablesContentFilePath);
            var gatherables =  JsonConvert.DeserializeObject<List<FlexData>>(gatherablesJsonData);

            return new GameContentDto(hazards, mobs, gatherables);
        }
    }
}