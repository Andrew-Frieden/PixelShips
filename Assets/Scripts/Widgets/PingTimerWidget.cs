using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.EventSystems;

namespace PixelShips.Widgets
{
    public class PingTimerWidget : BaseWidget, IPointerClickHandler
    {
        private float time;
        public TextMeshProUGUI text;

        private void Start()
        {
            time = 0;

            if (text == null)
                text = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void OnVerseUpdate(IGameState state)
        {
            time = 0;
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        private int updateCount = 0;
        private void FixedUpdate()
        {
            updateCount++;
            if (updateCount % 5 == 0)
            {
                updateCount = 0;
                text.text = string.Format("{0:0.0}s", time);

                if (_isShowingDebug)
                {
                    text.text += Environment.NewLine + "Debug" + Environment.NewLine;
                    text.text += eventDebug;
                }
            }
        }

        private bool _isShowingDebug = true;
        private void ToggleDebug()
        {
            _isShowingDebug = !_isShowingDebug;
        }

        private string eventDebug = string.Empty;
        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleDebug();

            eventDebug = string.Empty;

            if (_isShowingDebug)
            {
                eventDebug += Environment.NewLine + "press: " + eventData.pressPosition;
                eventDebug += Environment.NewLine + "pos: " + eventData.position;
                eventDebug += Environment.NewLine + "raw: " + eventData.rawPointerPress;
                eventDebug += Environment.NewLine + "press: " + eventData.ToString() + Environment.NewLine;
            }
        }
    }
}