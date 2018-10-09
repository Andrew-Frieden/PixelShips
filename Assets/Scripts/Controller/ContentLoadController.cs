﻿using System.Collections.Generic;
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
            var hazardsJsonData = Resources.Load<TextAsset>(BaseFolderPath + HazardsContentFilePath);
            var hazards =  JsonConvert.DeserializeObject<List<FlexData>>(hazardsJsonData.text);
            
            var mobsJsonData = Resources.Load<TextAsset>(BaseFolderPath + MobsContentFilePath);
            var mobs =  JsonConvert.DeserializeObject<List<FlexData>>(mobsJsonData.text);
            
            var gatherablesJsonData =Resources.Load<TextAsset>(BaseFolderPath + GatherablesContentFilePath);
            var gatherables =  JsonConvert.DeserializeObject<List<FlexData>>(gatherablesJsonData.text);

            return new GameContentDto(hazards, mobs, gatherables);
        }
    }
}