using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;
using Links.Colors;

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

        public override IRoomAction GetNextAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override ABDialogueContent CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcState.Full:
                    return DialogueBuilder.Init()
                        .AddMainText("The most ubiquitous and least valuable item in the galaxy.  Worth 1 space bux per unit.")
                        .SetMode(ABDialogueMode.ACancel)
                        .AddTextA("Pick it up")
                            .AddActionA(new LootAction(room.PlayerShip, this))
                        .Build();
            }

            throw new NotSupportedException();
        }

        public override string GetLookText()
        {
            return LookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.Gatherable);;
        }

        public override void AfterAction(IRoom room)
        {
            if (Stats[Gathered] == 0)
            {
                room.Entities.Remove(this);
            }
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

            public override IEnumerable<string> Execute(IRoom room)
            {
                var scrap = Source.Stats["captainship"] + (int)UnityEngine.Random.Range(0, 10);
                Source.Stats["scrap"] += scrap;
                Target.Stats["gathered"] = 0;
                return new string[] { "You gathered " + scrap + " scrap." };

            }
        }
        
    }
}