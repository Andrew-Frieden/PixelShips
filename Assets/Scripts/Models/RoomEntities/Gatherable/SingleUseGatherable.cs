using System;
using System.Collections.Generic;
using Helpers;
using Items;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
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
        
        public SingleUseGatherable(FlexEntityDto dto) : base(dto)
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

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcState.Full:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText("Your mining scanners detect some serious resource in this bad boy.")
                        .AddTextA("Attempt to extract resources")
                            .AddActionA(new MiningLootAction(room.PlayerShip, this))
                        .Build(room);
                    break;
                case (int)NpcState.Empty:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"The {Name} is depleted, it may take eons to grow back.")
                        .Build(room);
                    break;
                default:
                    throw new NotSupportedException();
            }
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
                var results = new List<TagString>();
                    
                Target.ChangeState((int)NpcState.Empty);
                
                var loot = Source.LootGatherable();
                var text = $"You gathered {loot.Amount} {loot.Type}.";

                results.Add(new TagString()
                {
                    Text = text,
                    Tags = new List<UIResponseTag> { }
                });

                return results;
            }
        }
    }
}