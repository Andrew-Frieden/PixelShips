using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using PixelShips.Helpers;

namespace PixelShips.Widgets
{
    public class ActiveTextWidget : BaseWidget
    {
        public TextMeshProUGUI DisplayText;

        private List<DateTimeGuid> ObservedNoteIds = new List<DateTimeGuid>();
        private bool _hasUpdated;

        private void Start()
        {
            if (DisplayText == null)
                DisplayText = GetComponentInChildren<TextMeshProUGUI>();

            DisplayText.text = string.Empty;

            AddActiveText("System initializing...");
        }

        public void AddActiveText(string text)
        {
            DisplayText.text += text + Environment.NewLine;
        }

        private IEnumerator ConnectedSequence()
        {
            AddActiveText("Connected.");
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f));
            AddActiveText("Welcome CMDR");
        }

        protected override void OnVerseUpdate(IGameState state)
        {
            if (!_hasUpdated)
            {
                _hasUpdated = true;
                StartCoroutine(ConnectedSequence());
            }

            var notifications = state.Notifications;

            //  look at all the notifications in the room and show any that haven't been seen before
            foreach (var note in notifications)
            {
                if (!ObservedNoteIds.Any(n => n.Id == note.Id))
                {
                    ObservedNoteIds.Add(new DateTimeGuid(note.Id, DateTime.UtcNow));

                    if (note.Text.Contains("["))
                    {

                    }
                    else
                    {
                        AddActiveText(note.Text);
                    }
                }
            }

            //  figure out which notes that you've already seen are old enough that we don't care anymore
            var notesToDrop = new List<DateTimeGuid>();
            foreach (var dtg in ObservedNoteIds)
            {
                if ((DateTime.UtcNow - dtg.Time).TotalMilliseconds > (30 * 1000))
                {
                    notesToDrop.Add(dtg);
                }
            }

            //  go through all the old notes we don't want and drop them
            foreach (var dtg in notesToDrop)
            {
                ObservedNoteIds.Remove(dtg);
            }
        }
    }

   
}