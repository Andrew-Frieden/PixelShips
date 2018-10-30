using System;
using System.Collections.Generic;
using Helpers;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using TextEncoding;
using UnityEngine;

namespace TextSpace.Models
{
    public class BasicGatherable : FlexEntity
    {
        private enum NpcState
        {
            Full = 0
            
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
        
        public bool IsGathered
        {
            get
            {
                return Stats[StatKeys.Gathered] != 0;
            }
            private set
            {
                Stats[StatKeys.Gathered] = Convert.ToInt32(value);
            }
        }

        private Dictionary<NpcState, string> LookText = new Dictionary<NpcState, string>
        {
            { NpcState.Full, "There is some floating <> nearby." }
        };

        public BasicGatherable(FlexData dto) : base(dto)
        {
            IsGathered = false;
        }

        public BasicGatherable(FlexEntityDto dto) : base(dto) { }
        
        public BasicGatherable(string name = "Scrap") : base()
        {
            Name = name;
            IsGathered = false;
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
                        .AddTextA("Pick it up")
                            .AddActionA(new LootAction(room.PlayerShip, this, ResourceKey.ResourceDisplayText(), ResourceKey, SmallestAmount, LargestAmount))
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
                Text = LookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.Gatherable)
            };
        }

        public override IRoomAction CleanupStep(IRoom room)
        {
            if (IsGathered)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
        
        private class LootAction : SimpleAction
        {
            private string _resourceKey;
            private int _smallest;
            private int _largest;
            
            public LootAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }
            
            public LootAction(IRoomActor src, IRoomActor target, string name, string resourceKey, int smallest, int largest)
            {
                Source = src;
                Target = target;
                Name = name;
                
                _resourceKey = resourceKey;
                _smallest = smallest;
                _largest = largest;
            }
            
            public override IEnumerable<TagString> Execute(IRoom room)
            {
                var amount = UnityEngine.Random.Range(_smallest, _largest);
                Source.Stats[_resourceKey] +=  amount;
                Target.Stats[StatKeys.Gathered] = 1;
                
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = "You gathered " + amount + " " + Name + "."
                    }
                };
            }
        }
    }
}