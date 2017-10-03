using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PixelShips.Widgets
{
    public class ExampleWidget : BaseWidget
    {
        public TextMeshProUGUI TextFeed;
        public List<DateTimeGuid> SeenNoteIds = new List<DateTimeGuid>();
    
        protected override void OnVerseUpdate(IGameState state)
        {
            if (TextFeed != null)
            {
                var notifications = state.shipState.Room.Notifications;

                //  look at all the notifications in the room and show any that haven't been seen before
                foreach (var note in notifications)
                {
                    if (!SeenNoteIds.Any(n => n.Id == note.Id))
                    {
                        SeenNoteIds.Add(new DateTimeGuid(note.Id, DateTime.UtcNow));
                        TextFeed.text += note.Text + Environment.NewLine;
                    }
                }

                //  figure out which notes that you've already seen are old enough that we don't care anymore
                var notesToDrop = new List<DateTimeGuid>();
                foreach (var dtg in SeenNoteIds)
                {
                    if ((DateTime.UtcNow - dtg.Time).TotalMilliseconds > (30 * 1000))
                    {
                        notesToDrop.Add(dtg);
                    }
                }
                
                //  go through all the old notes we don't want and drop them
                foreach (var dtg in notesToDrop)
                {
                    SeenNoteIds.Remove(dtg);
                }
            }
        }

        new void OnEnable()
        {
            base.OnEnable();
            //  any ExampleWidget OnEnable behavior needs to call BaseWidget's OnEnable
        }

        new void OnDisable()
        {
            base.OnDisable();
            //  any ExampleWidget OnDisable behavior needs to call BaseWidget's OnDisable
        }
    }

    public class DateTimeGuid
    {
        public DateTime Time { get; set; }
        public string Id { get; set; }

        public DateTimeGuid(string id, DateTime timeStamp)
        {
            Id = id;
            Time = timeStamp;
        }
    } 
}