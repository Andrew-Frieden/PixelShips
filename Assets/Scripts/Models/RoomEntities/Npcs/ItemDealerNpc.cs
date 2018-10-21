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
    public class ItemDealerNpc : FlexEntity
    {
        public ItemDealerNpc(FlexData data) : base(data)
        {
            IsAttackable = false;
            IsHostile = false;
            ChangeState((int)ScrapDealerNpcState.HasDeals);
        }

        public ItemDealerNpc(FlexEntityDto dto) : base(dto) { }

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

                    var itemsToSell = room.FindDependentActors(this);

                    var dealsBuilder = DialogueBuilder.Init().AddMainText(DialogueText.Encode(this, LinkColors.NPC));
                    foreach (var item in itemsToSell)
                    {
                        var sellAction = new SellItemAction(this, item);
                        dealsBuilder.AddOption(sellAction.OptionText(room), sellAction);
                    }
                    DialogueContent = dealsBuilder.Build(room);
                    break;
                case (int)ScrapDealerNpcState.FinishedDealing:
                    var doneBuilder = DialogueBuilder.Init().AddMainText("<> I'm done selling!".Encode(this, LinkColors.NPC));
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

    public class SellItemAction : SimpleAction
    {
        public SellItemAction(IRoomActor src, IRoomActor target) : base()
        {
            //  source is npc, target is item being sold
            Source = src;
            Target = target;
        }

        public SellItemAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {


            var cmdShip = (CommandShip)Source;
            var scrap = cmdShip.Scrap;
            cmdShip.Scrap = 0;
            var resourcium = 0;
            cmdShip.Resourcium += resourcium;
            return $"A <> trades you {resourcium} resourcium for your {scrap} scrap.".Encode(Target, LinkColors.NPC).ToTagSet();
        }

        public string OptionText(IRoom room)
        {
            //  TODO include item display stats text here
            if (IsValid)
            {
                return $"Buy this item.";
            }
            else
            {
                return $"Deal already accepted.";
            }
        }

        public override void CalculateValid(IRoom room)
        {
            IsValid = Target.DependentActorId == Source.Id;
        }
    }
}