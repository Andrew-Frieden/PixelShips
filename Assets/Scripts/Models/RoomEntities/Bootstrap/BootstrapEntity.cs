using System;
using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using TextEncoding;
using UnityEngine;

namespace Models
{
    public partial class BootstrapEntity : FlexEntity
    {
        private bool FTUE { get; set; }

        public BootstrapEntity(bool ftue) : base()
        {
            IsHidden = true;
            FTUE = ftue;
        }

        public override string GetLinkText()
        {
            switch (CurrentState)
            {
                case 0:
                case 1:
                    return "You";
                case 2:
                    return "Homeworld";
                default:
                    break;
            }
            throw new Exception("Invalid Bootstrap State");
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (FTUE)
            {
                if (CurrentState > 3)
                    return new DoNothingAction(this);
                //  this FTUE action should setup homeworld
                return new FTUEAction(this, CurrentState);
            }
            else
            {
                //  setup rng homeworld then do this action
                return new StartBaseViewAction(this);
            }
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case 0:
                case 1:
                    var remember = DialogueBuilder.Init()
                        .AddMainText("You feel pretty empty.")
                        .AddOption("Try to remember your ancestry...", new DoNothingAction(this));
                    DialogueContent = remember.Build(room);
                    break;
                case 2:
                    var worldA = HomeworldGenerator.Rng();
                    var worldB = HomeworldGenerator.Rng();

                    var pickHomeworld = DialogueBuilder.Init()
                        .AddMainText("Consider the planet from which your people descend.")
                        .AddOption(GetOptionText(worldA), new SetWorldAction(worldA))
                        .AddOption(GetOptionText(worldB), new SetWorldAction(worldB));
                    DialogueContent = pickHomeworld.Build(room);
                    break;
                default:
                    break;
            }
        }

        private string GetOptionText(Homeworld world)
        {
            return $"<b>{world.PlanetName}</b>{Env.ll}{world.Description} World";
        }

        public override TagString GetLookText()
        {
            return string.Empty.Tag();
        }
    }

    public class SetWorldAction : SimpleAction
    {
        private Homeworld World;

        public SetWorldAction(Homeworld world)
        {
            World = world;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            GameManager.Instance.SetHomeworld(World);
            return "".ToTagSet();
        }
    }

    public static class HomeworldGenerator
    {
        private static List<string> PlanetNames = new List<string>();
        private static readonly List<string> _names = new List<string>
        {
            "Bacchus",
            "Thanaton",
            "Tunkuna",
            "Yaevis",
            "Tiagawa",
            "Nilia",
            "Tenobos",
            "Utov",
            "Kegawa",
            "Astea",
            "Thibenus",
            "Hotov",
            "Jegia",
            "Lobovo",
            "Hox"
        };

        public static readonly List<string> Descriptions = new List<string>
        {
            "Water",
            "Volcanic",
            "Ice",
            "Desert",
            "Jungle"
        };

        public static Homeworld Rng()
        {
            if (PlanetNames.Count == 0)
                PlanetNames = new List<string>(_names);
            var name = PlanetNames.OrderBy(n => Guid.NewGuid()).First();
            PlanetNames.Remove(name);

            return new Homeworld
            {
                PlanetName = name,
                Description = Descriptions.OrderBy(d => Guid.NewGuid()).First()
            };
        }
    }

    public class FTUEAction : SimpleAction
    {
        private int Step;

        public FTUEAction(IRoomActor src, int step)
        {
            Source = src;
            Step = step;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            Step++;
            Source.ChangeState(Step);

            switch (Step)
            {
                case 1:
                    return new TagString[]
                    {
                        "".Tag(new [] { UIResponseTag.HideHUD, UIResponseTag.HideNavBar }),
                        "You are lost in the void".Tag(),
                        "You have no identity".Tag(),
                        "Maybe <> can change that...".Encode(Source, LinkColors.Player).Tag()
                    };
                case 2:
                    return new TagString[]
                    {
                        "Ah yes.".Tag(),
                        "You recall a starfaring tribe of <b>Space Barbarians</b>!".Tag(),
                        "What was your <>?".Encode(Source, LinkColors.Player).Tag()
                    };
                case 3:
                    var home = GameManager.Instance.GameState.Home;
                    return new TagString[]
                    {
                        $"You hail from the {home.Description.ToLower()} world of {home.PlanetName}.".Tag(),
                        $"For centuries your people have ventured deep into space to sustain themselves and prove their worth.".Tag(),
                        $"Your clan continues their ancient impetus.".Tag(),
                        ".".Tag(),
                        ".".Tag(),
                        "<b>Visit your homeworld to start a new expedition.</b>".Tag(new[] { UIResponseTag.ShowNavBar, UIResponseTag.DisableCmdView })
                    };
                default:
                    break;
            }
            Debug.Log("Invalid Bootstrapper State");
            return "".ToTagSet();
        }
    }

    public class StartBaseViewAction : SimpleAction
    {
        public StartBaseViewAction(IRoomActor src)
        {
            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            return new TagString[]
            {
                "".Tag(new [] { UIResponseTag.ViewBase, UIResponseTag.HideHUD, UIResponseTag.HideNavBar }),
            };
        }
    }
}