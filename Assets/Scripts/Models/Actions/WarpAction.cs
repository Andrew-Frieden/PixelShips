using System.Collections.Generic;
using System.Linq;
using TextSpace.Framework.IoC;
using TextSpace.Models.Dtos;
using TextSpace.Services;
using UnityEngine;

namespace TextSpace.Models.Actions
{
    public class WarpAction : SimpleAction
    {
        private RoomTemplate _template;
        private Mission _mission;

        public WarpAction(RoomTemplate template, Mission mission = null) : base()
        {
            _template = template;
            _mission = mission;
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            ServiceContainer.Resolve<NavigationService>()
                .OnPlayerNavigate(room, _template, _mission);

            room.PlayerShip.WarpDriveReady = false;
            room.PlayerShip.WarpTarget = _template;

            // if _mission != null return some Mission Accepted text

            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "You begin to jump into hyperspace!",
                    Tags = new List<UIResponseTag> { }
                },
                new TagString()
                {
                    Text = "3...",
                    Tags = new List<UIResponseTag> { }
                },
                new TagString()
                {
                    Text = "2...",
                    Tags = new List<UIResponseTag> { }
                },
                new TagString()
                {
                    Text = "1...",
                    Tags = new List<UIResponseTag> { }
                }
            };
        }
    }
}