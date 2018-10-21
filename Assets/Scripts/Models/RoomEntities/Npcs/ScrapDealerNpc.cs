using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using System;
using System.Collections.Generic;
using TextEncoding;

namespace TextSpace.RoomEntities
{
    public class ScrapDealerNpc : FlexEntity
    {
        private int ScrapToResourcium
        {
            get { return Stats[StatKeys.ScrapToResourcium]; }
            set { Stats[StatKeys.ScrapToResourcium] = value; }
        }
    
        public ScrapDealerNpc(FlexData data) : base(data)
        {
            IsAttackable = false;
            IsHostile = false;
            ChangeState((int)ScrapDealerNpcState.HasDeals);
        }

        public ScrapDealerNpc(FlexEntityDto dto) : base(dto) { }

        private enum ScrapDealerNpcState
        {
            HasDeals = 0,
            FinishedDealing = 1
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)ScrapDealerNpcState.HasDeals:
                    var sellAction = new SellScrapAction(room.PlayerShip, this, ScrapToResourcium);
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText(DialogueText.Encode(this, LinkColors.NPC))
                        .AddOption(sellAction.OptionText(room), sellAction)
                        .Build(room);
                    break;
                case (int)ScrapDealerNpcState.FinishedDealing:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public override TagString GetLookText()
        {
            return LookText.Encode(this, LinkColors.NPC).Tag();
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }
    }

    public class SellScrapAction : SimpleAction
    {
        //  about 10:1
        private int ScrapToResourcium
        {
            get { return Stats[StatKeys.ScrapToResourcium]; }
            set { Stats[StatKeys.ScrapToResourcium] = value; }
        }

        public SellScrapAction(IRoomActor src, IRoomActor target, int scrapToResourcium) : base()
        {
            //  by convention, assume source is player and target is the npc providing the action
            Source = src;
            Target = target;
            ScrapToResourcium = scrapToResourcium;
        }

        public SellScrapAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var cmdShip = (CommandShip)Source;
            var scrap = cmdShip.Scrap;
            cmdShip.Scrap = 0;
            var resourcium = scrap / ScrapToResourcium;
            cmdShip.Resourcium += resourcium;
            return $"A <> trades you {resourcium} resourcium for your {scrap} scrap.".Encode(Target, LinkColors.NPC).ToTagSet();
        }

        public string OptionText(IRoom room)
        {
            CalculateValid(room);

            if (IsValid)
            {
                var scrap = ((CommandShip)Source).Scrap;
                var resourcium = scrap / ScrapToResourcium;
                return $"Trade {scrap} scrap{Env.l}for {resourcium} resourcium.";
            }
            else
            {
                return $"Trade scrap for resourcium.{Env.ll}(Not enough scrap)";
            }
        }

        public override void CalculateValid(IRoom room)
        {
            if (Source is CommandShip)
            {
                var cmdShip = (CommandShip)Source;
                IsValid = cmdShip.Scrap >= ScrapToResourcium;
                return;
            }
            IsValid = false;
        }
    }
}