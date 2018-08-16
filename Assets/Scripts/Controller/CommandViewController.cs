using System;
using System.Collections.Generic;
using Models;
using Models.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class CommandViewController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI ZoneText;
        [SerializeField] private TextMeshProUGUI MobText;
        private IRoom _room;
        
        private void Awake()
        {
            var mob = new Mob("A {{ link }} floats here", "Space Barbarian", 2);
            
            Debug.Log("Mob Created.");
            
            var commandShip = ShipFactory.GenerateCommandShip();
            
            Debug.Log("Command Ship Created.");
            
            _room = new Room("You enter into a {{ link }} with many asteroids.", "Giant Nebula", commandShip, new List<IRoomEntity>() { mob });
            
            Debug.Log("Room Created.");
            
            Debug.Log("Room look text: " + _room.GetLookText());
            
            ZoneText.SetText(_room.GetLookText().ToString());
            MobText.SetText(mob.GetLookText().ToString());
        }

        public void OnPlayerChoseAction(IRoomAction action)
        {
            //Debug.Log("Player chose action.");
            
            //_room.ResolveNext(action);
            
            //Debug.Log("Room resolved.");
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var result = TMP_TextUtilities.FindIntersectingLink(ZoneText, eventData.position, UIManager.Instance.UICamera);
            if (result >= 0)
            {
                var objectId = ZoneText.textInfo.linkInfo[result].GetLinkID();
                Debug.Log("Clicked link for entity with Id: " + objectId);
                return;
            }
            
            result = TMP_TextUtilities.FindIntersectingLink(MobText, eventData.position, UIManager.Instance.UICamera);
            if (result >= 0)
            {
                var objectId = MobText.textInfo.linkInfo[result].GetLinkID();
                Debug.Log("Clicked link for entity with Id: " + objectId);
                return;
            }
        }
    }
}