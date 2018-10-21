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
            var entitySource = (FlexEntity)Source;

            if (Target is Hardware)
            {
                var item = (Hardware)Target;
                room.PlayerShip.EquipHardware(item);
                item.DependentActorId = string.Empty;

            }
            else if (Target is Weapon)
            {
                var item = (Weapon)Target;
                room.PlayerShip.SwapWeapon(item);
                item.DependentActorId = string.Empty;
            }

            if (room.FindDependentActors(Source).Count == 0)
            {
                Source.ChangeState((int)NpcDealerState.FinishedDealing);
            }

            return $"{entitySource.Name} trades you <> for {Resourcium} resourcium."
                .Encode(Target, (Target is Weapon) ? LinkColors.Weapon : LinkColors.Gatherable)
                .ToTagSet();
        }

        public string OptionText(IRoom room)
        {
            var item = (FlexEntity)Target;
            var baseText = $"Buy {item.Name}{Env.l}({Resourcium} resourcium)";

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

                if (Target is Hardware && room.PlayerShip.OpenHardwareSlots == 0)
                    return $"{baseText}{Env.ll}Hardware at capacity.";
            }

            throw new Exception("ItemDealerNpc OptionText hit an unexpected case");
        }

        public override void CalculateValid(IRoom room)
        {
            var dealerHasItem = Target.DependentActorId == Source.Id;
            var playerHasOpenSlots = room.PlayerShip.OpenHardwareSlots > 0;
            var enoughMoney = room.PlayerShip.Resourcium >= Resourcium;

            if (dealerHasItem && enoughMoney && (playerHasOpenSlots || Target is Weapon))
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