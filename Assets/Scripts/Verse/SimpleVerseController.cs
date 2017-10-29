using Newtonsoft.Json;
using PixelShips.Verse;
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Helpers;
using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Verse
{
    public class SimpleVerseController : MonoBehaviour, IVerseController
    {
        public string ShipId = "a9b0857b-7887-4ead-9664-a0c4f6973e6c";
        public SimpleGameState GameState;

        public const string GET_SHIPSTATE_URL = "https://h3l6swrjm3.execute-api.us-east-1.amazonaws.com/Prod/client/";
        private const float UPDATE_INTERVAL_MS = 200;

        private event VerseUpdate OnUpdate;

        public void StartUpdates()
        {
            try { StartCoroutine(UpdateForever()); }
            catch (UnityException ex) { Debug.Log("StartUpdates: " + ex.Message); }
        }

        public bool SubmitAction(SpaceAction spaceAction)
        {
            if (!spaceAction.Validate())
                return false;

            try { StartCoroutine(QueueAction(spaceAction.ToDbi())); }
            catch (UnityException ex) { Debug.Log("SubmitAction: " + ex.Message); }

            return true;
        }

        public void Subscribe(VerseUpdate updateDelegate)
        {
            OnUpdate += updateDelegate;
        }

        public void Unsubscribe(VerseUpdate updateDelegate)
        {
            OnUpdate -= updateDelegate;
        }

        private IEnumerator UpdateForever()
        {
            while (true)
            {
                //var time = DateTime.UtcNow;
                yield return WaitForLatestShipState();

                //var updateTimeMs = (DateTime.UtcNow - time).TotalMilliseconds;
                //var waitForSeconds = (float)(UPDATE_INTERVAL_MS - updateTimeMs) / 1000f;
                //yield return new WaitForSeconds(waitForSeconds);
                yield return new WaitForSeconds(UPDATE_INTERVAL_MS/1000f);
            }
        }

        IEnumerator WaitForLatestShipState()
        {
            //var before = DateTime.UtcNow;
            var getStateRequest = string.Format("{0}{1}", GET_SHIPSTATE_URL, ShipId);
            var getStateWww = new WWW(getStateRequest);
            //Debug.Log(getStateRequest);

            yield return getStateWww;
            //Debug.Log("yield time: " + (DateTime.UtcNow - before).TotalMilliseconds);

            string txt = "";
            if (string.IsNullOrEmpty(getStateWww.error))
                txt = getStateWww.text;  //text of success
            else
                txt = getStateWww.error;  //error

            //Debug.Log("ping -> " + txt);
            //try
            //{

                var latestShipState = JsonConvert.DeserializeObject<ShipState>(txt);
            //    Debug.Log("ss: " + latestShipState.ShipId);
            //latestShipState.Ships.ForEach(s => Debug.Log(string.Format("ship: {0} {1}", s.Name, s.Id)));
            //Debug.Log("ship: " + latestShipState.Ship);
            //Debug.Log("room: " + latestShipState.Room);



            //  should fix this on the server side...
            if (latestShipState.Room != null)
            {
                if (latestShipState.Room.Notifications == null)
                    latestShipState.Room.Notifications = new List<FeedUpdate>();
            }
            //Debug.Log("notes: " + latestShipState.Room.Notifications);

            var gameState = new SimpleGameState(latestShipState);
            //Debug.Log("after ");

            //RoomState rs = new RoomState(latestShipState.Room, latestShipState.Room.Ships);
            //var actionFactory = new SpaceActionFactory(gameState);

            //var firstAction = gameState.UserActions.FirstOrDefault();
            //if (firstAction != null)
            //    someAction = firstAction;


            //foreach (var pa in gameState.shipState.PossibleActions)
            //{
            //    var actionModel = actionFactory.GetModel(pa);
            //    //Debug.Log(string.Format("action: {0} [{1}]  ok: {2}", actionModel.Name, pa.TargetId, actionModel.Validate()));
            //}

            //GameState.Current = gameState;
            GameState = gameState;
            OnUpdate(gameState);
                //Debug.Log("verse ping complete");
            //}
            //catch(Exception e)
            //{
            //    Debug.Log("LatestShipState error: " + e.Message + Environment.NewLine + " with response: " + txt);
            //}
        }

        private SpaceAction someAction;
        public void TestFirstAction()
        {
            if (someAction != null)
            {
                Debug.Log("TestFirstAction HAS ACTION");
                SubmitAction(someAction);
            }
            else
            {
                Debug.Log("TestFirstAction NOTHING");
            }
        }

        private IEnumerator QueueAction(SpaceActionDbi dbi)
        {
            string postData = "";
            try
            {
                postData = JsonConvert.SerializeObject(dbi);
            }
            catch(Exception e)
            {
                Debug.Log("queue action: " + e.Message + " " + dbi.ToString());
            }

            Debug.Log(postData);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("X-HTTP-Method-Override", "PUT");
            byte[] pData = Encoding.ASCII.GetBytes(postData.ToCharArray());
            var putActionRequest = "https://h3l6swrjm3.execute-api.us-east-1.amazonaws.com/Prod/actions";

            var api = new WWW(putActionRequest, pData, headers);
            yield return api;

            string txt = "";
            if (string.IsNullOrEmpty(api.error))
                txt = api.text;  //text of success
            else
                txt = api.error;  //error

            Debug.Log("action completed with: " + txt);
        }
    }
}
