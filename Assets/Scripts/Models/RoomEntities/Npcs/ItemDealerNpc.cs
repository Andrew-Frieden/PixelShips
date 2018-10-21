using Items;
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
                        var sellAction = new SellItemAction(this, item, 1);
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
        public SellItemAction(IRoomActor src, IRoomActor target, int price) : base()
        {
            //  source is npc, target is item being sold
            Source = src;
            Target = target;
            Resourcium = price;
        }

        public SellItemAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            if (Target is Hardware)
            {
                var item = (Hardware)Target;
                room.PlayerShip.EquipHardware(item);
                item.DependentActorId = string.Empty;
                var entitySource = (FlexEntity)Source;

                return $"{entitySource.Name} trades you <> for {Resourcium} resourcium.".Encode(Target, LinkColors.Gatherable).ToTagSet();
            }

            throw new NotSupportedException();
        }

        public string OptionText(IRoom room)
        {
            var item = (FlexEntity)Target;
            var baseText = $"Buy {item.Name} for {Resourcium} resourcium.";

            CalculateValid(room);

            //  TODO include item display stats text here
            if (IsValid)
            {
                return baseText;
            }
            else
            {
                if (Target.DependentActorId != Source.Id)
                    return $"{baseText}{Env.ll}Deal already accepted.";

                if (room.PlayerShip.Resourcium < Resourcium)
                    return $"{baseText}{Env.ll}Not enough resourcium.";

                if (room.PlayerShip.OpenHardwareSlots == 0)
                    return $"{baseText}{Env.ll}Hardware at capacity.";
            }

            throw new Exception("ItemDealerNpc OptionText hit an unexpected case");
        }

        public override void CalculateValid(IRoom room)
        {
            var dealerHasItem = Target.DependentActorId == Source.Id;
            var playerHasOpenSlots = room.PlayerShip.OpenHardwareSlots > 0;
            var enoughMoney = room.PlayerShip.Resourcium >= Resourcium;

            if (dealerHasItem && playerHasOpenSlots && enoughMoney)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }
    }
}