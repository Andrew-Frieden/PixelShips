using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using PixelSpace.Models.SharedModels;
using PixelShips.Helpers;

namespace PixelShips.Widgets
{
    public class ScanButton : BaseWidget
    {
        private IGameState _state;
        private IGameState scanState;
        public ActiveTextWidget text;

        public int ScanCooldownMs = 2000;   // this should be a server side thing
        private DateTime LastScan = DateTime.MinValue;

        protected override void OnVerseUpdate(IGameState state)
        {
            _state = state;
        }

        public void Scan()
        {
            if (_state == null)
                return;

            if (DateTime.UtcNow > LastScan.AddMilliseconds(ScanCooldownMs))
            {
                LastScan = DateTime.UtcNow;
                scanState = _state;
                StartCoroutine(RunScan());
            }
            else
            {
                text.AddActiveText("Scan system on cooldown!");
            }
        }

        private IEnumerator RunScan()
        {
            text.AddActiveText("Scanning...");
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
            text.AddActiveText(scanState.Room.Description);
            text.AddActiveText(string.Format("Location: [{0},{1}]", scanState.Room.X, scanState.Room.Y));

            if (scanState.Ships.Count() > 1)
            {
                text.AddActiveText("Other ships detected.");
                foreach (var otherShip in scanState.Ships)
                {
                    if (otherShip.Id != scanState.UserShip.Id)
                    {
                        text.AddActiveText(otherShip.GetActiveTextTag());
                    }
                }
            }

           
        }
    }
}
