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

        private void Start()
        {
            if (DisplayText == null)
                DisplayText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void AddActiveText(string text)
        {
            DisplayText.text += text + Environment.NewLine;
        }

        protected override void OnVerseUpdate(IGameState state)
        {
            var notifications = state.Notifications;

            //  look at all the notifications in the room and show any that haven't been seen before
            foreach (var note in notifications)
            {
                if (!ObservedNoteIds.Any(n => n.Id == note.Id))
                {
                    ObservedNoteIds.Add(new DateTimeGuid(note.Id, DateTime.UtcNow));
                    AddActiveText(note.Text);
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