using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

namespace Models
{
    public partial class ScrapGatherable : FlexEntity
    { 
        public const string Gathered = "gathered";

        private enum NpcState
        {
            Full = 0
        }
        
        private Dictionary<NpcState, string> LookText = new Dictionary<NpcState, string>
        {
            { NpcState.Full, "There is some floating <> nearby." }
        };
        
        public ScrapGatherable(FlexEntityDto dto, IRoom room) : base(dto, room)
        {
        }

        public ScrapGatherable(string name = "Scrap") : base()
        {
            Name = name;
            Stats = new Dictionary<string, int>();
            Stats.Add(Gathered, 1);
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
                        .AddMainText("The most ubiquitous and least valuable item in the galaxy.  Worth 1 credit per unit.")
                        .AddTextA("Pick it up")
                            .AddActionA(new LootAction(room.PlayerShip, this))
                        .Build();
            }

            throw new NotSupportedException();
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
            if (Stats[Gathered] == 0)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
    }

    public partial class ScrapGatherable
    {
        private class LootAction : SimpleAction
        {
            public LootAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }
            
            public LootAction(IRoomActor src, IRoomActor target)
            {
                Source = src;
                Target = target;
            }
            
            public override IEnumerable<TagString> Execute(IRoom room)
            {
                var scrap = Source.Stats["captainship"] + UnityEngine.Random.Range(0, 10);
                Source.Stats["scrap"] += scrap;
                Target.Stats["gathered"] = 0;
                
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = "You gathered " + scrap + " scrap."
                    }
                };
            }
        }
        
    }
}