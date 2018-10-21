using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using System;
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
            ChangeState((int)NpcDealerState.HasDeals);
        }

        public ScrapDealerNpc(FlexEntityDto dto) : base(dto) { }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcDealerState.HasDeals:
                    var sellAction = new SellScrapAction(this, room.PlayerShip, ScrapToResourcium);
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText(DialogueText.Encode(this, LinkColors.NPC))
                        .AddOption(sellAction.OptionText(room), sellAction)
                        .Build(room);
                    break;
                case (int)NpcDealerState.FinishedDealing:
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
}