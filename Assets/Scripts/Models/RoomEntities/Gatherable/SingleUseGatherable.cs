using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

namespace Models
{
    public partial class SingleUseGatherable : FlexEntity
    {
        private enum NpcState
        {
            Full = 0,
            Empty = 1,
        }
        
        private readonly Dictionary<NpcState, string> _lookText = new Dictionary<NpcState, string>
        {
            { NpcState.Full, "Your scanners detect a nearby <>. Might be worth a look." },
            { NpcState.Empty, "Your scanners detect a nearby <> but it is depleted." }
        };
        
        public SingleUseGatherable(FlexEntityDto dto, IRoom room) : base(dto, room)
        {
        }

        public SingleUseGatherable(string name = "Resource-Rich Asteroid") : base()
        {
            Name = name;
            Stats = new Dictionary<string, int>();
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override ABDialogueContent CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcState.Full:
                    return DialogueBuilder.Init()
                        .AddMainText("Your mining scanners detect some serious resource in this bad boy.")
                        .AddTextA("Attempt to extract resources")
                            .AddActionA(new MiningLootAction(room.PlayerShip, this))
                        .Build();
                case (int)NpcState.Empty:
                    return DialogueBuilder.Init()
                        .AddMainText($"The {Name} is depleted, it may take eons to grow back.")
                        .Build();
            }

            throw new NotSupportedException();
        }

        public override TagString GetLookText()
        {
            return new TagString()
            {
                Text = _lookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.Gatherable)
            };
        }
    }

    public partial class SingleUseGatherable
    {
        private class MiningLootAction : SimpleAction
        {
            public MiningLootAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }
            
            public MiningLootAction(IRoomActor src, IRoomActor target)
            {
                Source = src;
                Target = target;
            }

            public override IEnumerable<TagString> Execute(IRoom room)
            {
                var gatheredText = "";
                
                var lootDrop = UnityEngine.Random.Range(0, 10);
                var results = new List<TagString>();
                    
                Target.ChangeState((int)NpcState.Empty);

                switch (lootDrop)
                {
                    case 0:
                        Source.Stats["resourcium"] += 15;
                        gatheredText = "You gathered 15 resourcium.";
                        break;
                    case 1:
                        Source.Stats["resourcium"] += 5;
                        gatheredText = "You gathered 5 resourcium.";
                        break;
                    case 2:
                        Source.Stats["scrap"] += 15;
                        gatheredText = "You gathered 15 scrap.";
                        break;
                    case 3:
                        Source.Stats["resourcium"] += 25;
                        gatheredText = "You gathered 25 resourcium.";
                        break;
                    case 4:
                        Source.Stats["scrap"] += 25;
                        gatheredText = "You gathered 25 scrap.";
                        break;
                    case 5:
                        Source.Stats["scrap"] += 75;
                        gatheredText = "You gathered 75 scrap.";
                        break;
                    case 6:
                        Source.Stats["scrap"] += 75;
                        gatheredText = "You gathered 75 scrap.";
                        break;
                    case 7:
                        Source.Stats["scrap"] += 150;
                        gatheredText = "You gathered 150 scrap.";
                        break;
                    case 8:
                        Source.Stats["resourcium"] += 1;
                        gatheredText = "You gathered 1 resourcium.";
                        break;        
                    case 9:
                        Source.Stats["Hull"] -= 5;
                        gatheredText = "The minerals exploded dealing 5 damage";
                        break;
                    default:
                        Source.Stats["scrap"] += 5;
                        gatheredText = "You gathered 5 scrap.";
                        break;        
                }

                if (gatheredText != "")
                {
                    results.Add(new TagString()
                    {
                        Text = gatheredText,
                        Tags = new List<EventTag> { }
                    });
                }

                return results;
            }
        }
    }
}