using System;
using System.Collections.Generic;
using EnumerableExtensions;
using Helpers;
using TextSpace.Items;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using TextEncoding;

namespace TextSpace.Models
{
    public partial class SimpleMineable : FlexEntity
    {
        private enum NpcState
        {
            Full = 0,
            Empty = 1,
        }
        
        public string ResourceKey
        {
            get
            {
                return Values[ValueKeys.ResourceKey];
            }
            private set
            {
                Values[ValueKeys.ResourceKey] = value;
            }
        }
        
        public int SmallestAmount
        {
            get
            {
                return Stats[StatKeys.SmallestAmount];
            }
            private set
            {
                Stats[StatKeys.SmallestAmount] = value;
            }
        }
        
        public int LargestAmount
        {
            get
            {
                return Stats[StatKeys.LargestAmount];
            }
            private set
            {
                Stats[StatKeys.LargestAmount] = value;
            }
        }
        
        public int DepletionChance
        {
            get
            {
                return Stats[StatKeys.DepletionChance];
            }
            private set
            {
                Stats[StatKeys.DepletionChance] = value;
            }
        }
        
        private readonly Dictionary<NpcState, string> _lookText = new Dictionary<NpcState, string>
        {
            { NpcState.Full, "Your scanners detect a nearby <>. Might be worth a look." },
            { NpcState.Empty, "Your scanners detect a nearby <> but it is depleted." }
        };
        
        public SimpleMineable(FlexEntityDto dto) : base(dto)
        {
        }
        
        public SimpleMineable(FlexData dto) : base(dto)
        {
        }

        public SimpleMineable() : base()
        {
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
                        .AddMainText(DialogueText)
                        .AddTextA("Attempt to extract resources")
                            .AddActionA(new MiningLootAction(room.PlayerShip, this, ResourceKey.ResourceDisplayText(), ResourceKey, DepletionChance, SmallestAmount, LargestAmount))
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

    public partial class SimpleMineable
    {
        private class MiningLootAction : SimpleAction
        {
            private string _resourceKey;
            private int _depletionChance;
            private int _smallest;
            private int _largest;

            public MiningLootAction(IRoomActor src, IRoomActor target, string name, string resourceKey, int depletionChance, int smallest, int largest)
            {
                Source = src;
                Target = target;
                Name = name;

                _depletionChance = depletionChance;
                _resourceKey = resourceKey;
                _smallest = smallest;
                _largest = largest;
            }

            public override IEnumerable<TagString> Execute(IRoom room)
            {
                var amount = UnityEngine.Random.Range(_smallest, _largest);
                Source.Stats[_resourceKey] +=  amount;
                
                if ((_depletionChance / 100f).Rng())
                {
                    Target.ChangeState((int) NpcState.Empty);
                    
                    return new List<TagString>()
                    {
                        new TagString()
                        {
                            Text = "You mined " + amount + " " + Name + ". It looks depleted."
                        }
                    };
                }
                
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = "You mined " + amount + " " + Name + "."
                    }
                };
            }
        }
    }
}