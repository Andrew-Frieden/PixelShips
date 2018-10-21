using System.Collections.Generic;
using Models.Factories;
using Controller;
using Models.Dtos;

namespace Models.Actions
{
    public class WarpAction : SimpleAction
    {
        //  bring on the hacks
        //  instead of figuring out how to serialize a RoomTemplate as part of a SimpleAction
        //  or build a non-SimpleAction,
        //  just stuff all the RoomTemplate properties 
        private const string ActorFlavorKey = "ActorFlavor";
        private const string DifficultyKey = "RoomDifficulty";
        private const string RoomFlavorKey = "RoomFlavor";

        private RoomTemplate _template;

        private void StoreTemplateStats()
        {
            Stats[RoomFlavorKey] = (int)_template.Flavor;
            Stats[DifficultyKey] = (int)_template.Difficulty;

            int flavorCount = 0;
            foreach (var actorFlavor in _template.ActorFlavors)
            {
                var key = $"{ActorFlavorKey}{flavorCount}";
                Stats[key] = (int)actorFlavor;
                flavorCount++;
            }
        }

        private void SetupTemplateFromStats()
        {
            var actorFlavors = new List<RoomActorFlavor>();
            var flavorCount = 0;
            var key = $"{ActorFlavorKey}{flavorCount}";

            while (Stats.ContainsKey(key))
            {
                actorFlavors.Add((RoomActorFlavor)Stats[key]);
                flavorCount++;
                key = $"{ActorFlavorKey}{flavorCount}";
            }
        }

        public WarpAction(RoomTemplate template) : base()
        {
            _template = template;
            StoreTemplateStats();
        }

        public WarpAction(SimpleActionDto dto, IRoom room) : base(dto, room)
        {
            SetupTemplateFromStats();
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            room.PlayerShip.WarpDriveReady = false;
            room.PlayerShip.WarpTarget = _template;
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "You begin to jump into hyperspace!",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "3...",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "2...",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "1...",
                    Tags = new List<EventTag> { }
                }
            };
        }
    }
}