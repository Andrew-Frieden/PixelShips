using UnityEngine;
using System.Collections;
using Models.Actions;

namespace TextSpace.Events
{
    public static class EventTagBroadcaster
    {
        public delegate void EventTagTriggered(EventTag tag);
        public static event EventTagTriggered EventTagTrigger;

        public static void Broadcast(EventTag tag)
        {
            EventTagTrigger?.Invoke(tag);
        }
    }
}
