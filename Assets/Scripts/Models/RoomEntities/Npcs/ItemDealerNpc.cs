using TextSpace.Items;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using System;
using System.Collections.Generic;
using TextEncoding;

namespace TextSpace.RoomEntities
{
    public class ItemDealerNpc : FlexEntity
    {
        public ItemDealerNpc(FlexData data) : base(data)
        {
            IsAttackable = false;
            IsHostile = false;
            ChangeState((int)NpcDealerState.HasDeals);
        }

        public ItemDealerNpc(FlexEntityDto dto) : base(dto) { }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcDealerState.HasDeals:
                    var itemsToSell = room.FindDependentActors(this);
                    var dealsBuilder = DialogueBuilder.Init().AddMainText(DialogueText.Encode(this, LinkColors.NPC));
                    foreach (var item in itemsToSell)
                    {
                        var sellAction = new SellItemAction(this, item, 1);
                        dealsBuilder.AddOption(sellAction.OptionText(room), sellAction);
                    }
                    DialogueContent = dealsBuilder.Build(room);
                    break;
                case (int)NpcDealerState.FinishedDealing:
                    var doneBuilder = DialogueBuilder.Init().AddMainText($"<> has nothing else to sell.".Encode(this, LinkColors.NPC));
                    DialogueContent = doneBuilder.Build(room);
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
}