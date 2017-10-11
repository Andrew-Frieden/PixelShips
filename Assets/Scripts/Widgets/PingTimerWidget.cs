using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PixelShips.Widgets
{
    public class PingTimerWidget : BaseWidget
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
            }
        }
    }
}