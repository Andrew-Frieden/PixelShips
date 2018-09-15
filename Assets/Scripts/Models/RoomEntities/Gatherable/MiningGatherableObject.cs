using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;
using Links.Colors;

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
                        .AddMainText("Your mining scanners detect some seroius resource in this bad boy.")
                        .SetMode(ABDialogueMode.ACancel)
                        .AddTextA("Attempt to extract resources")
                            .AddActionA(new MiningLootAction(room.PlayerShip, this))
                        .Build();
                case (int)NpcState.Empty:
                    return DialogueBuilder.Init()
                        .AddMainText("The asteroid is depleted, it may take eons to grow back.")
                        .SetMode(ABDialogueMode.Cancel)
                        .Build();
            }

            throw new NotSupportedException();
        }

        public override string GetLookText()
        {
            return _lookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.Gatherable);;
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

            public override IEnumerable<string> Execute(IRoom room)
            {
                var lootDrop = (int)UnityEngine.Random.Range(0, 10);

                Target.ChangeState((int)NpcState.Empty);

                switch (lootDrop)
                {
                    case 0:
                        Source.Stats["resourcium"] += 15;
                        return new string[] { "You gathered 15 resourcium." };

                    case 1:
                        Source.Stats["resourcium"] += 5;
                        return new string[] { "You gathered 5 resourcium." };

                    case 2:
                        Source.Stats["scrap"] += 15;
                        return new string[] { "You gathered 15 scrap." };

                    case 3:
                        Source.Stats["resourcium"] += 25;
                        return new string[] { "You gathered 25 resourcium." };

                    case 4:
                        Source.Stats["scrap"] += 25;
                        return new string[] { "You gathered 25 scrap." };

                    case 5:
                        Source.Stats["scrap"] += 75;
                        return new string[] { "You gathered 75 scrap." };

                    case 6:
                        Source.Stats["scrap"] += 75;
                        return new string[] { "You gathered 75 scrap." };

                    case 7:
                        Source.Stats["scrap"] += 150;
                        return new string[] { "You gathered 150 scrap." };

                    case 8:
                        Source.Stats["resourcium"] += 1;
                        return new string[] { "You gathered 1 resourcium." };

                    case 9:
                        Source.Stats["Hull"] -= 5;
                        return new string[] { "The minerals exploded dealing 5 damage" };

                    default:
                        Source.Stats["scrap"] += 5;
                        return new string[] { "You gathered 5 scrap." };

                }
            }
        }
    }
}