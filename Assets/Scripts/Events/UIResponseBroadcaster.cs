using UnityEngine;
using System.Collections;
using TextSpace.Models.Actions;

namespace TextSpace.Events
{
    public static class UIResponseBroadcaster
    {
        public delegate void UIResponseTagTriggered(UIResponseTag tag);
        public static event UIResponseTagTriggered UIResponseTagTrigger;

        public static void Broadcast(UIResponseTag tag)
        {
            UIResponseTagTrigger?.Invoke(tag);
        }
    }
}
