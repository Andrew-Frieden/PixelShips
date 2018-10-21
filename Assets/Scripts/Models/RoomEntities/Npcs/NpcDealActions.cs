using Items;
using Models;
using Models.Actions;
using Models.Dtos;
using Models.Stats;
using System;
using System.Collections.Generic;
using TextEncoding;

namespace TextSpace.RoomEntities
{
    public enum NpcDealerState
    {
        HasDeals = 0,
        FinishedDealing = 1
    }

    public class RepairAction : SimpleAction
    {
        public static int CalculateRepairCost(CommandShip ship)
        {
            var hullToRepair = ship.MaxHull - ship.Hull;
            var pricePerHull = 1;
            return hullToRepair * pricePerHull;
        }

        private const string CostKey = "cost";

        private int Cost
        {
            get { return Stats[CostKey]; }
            set { Stats[CostKey] = value; }
        }

        public RepairAction(IRoomActor source, int cost) : base()
        {
            Source = source;
            Cost = cost;
        }

        public RepairAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var ship = (CommandShip)Source;

            var repairAmount = ship.MaxHull - ship.Hull;

            if (ship.Resourcium >= Cost)
            {
                ship.Resourcium -= Cost;
                ship.Hull = ship.MaxHull;

                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = $"Your ship is repaired {repairAmount} hull."
                    }
                };
            }

            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "You try to make a deal but turn up empty handed."
                }
            };
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
            //  by convention, assume source is npc and target is the player
            Source = src;
            Target = target;
            ScrapToResourcium = scrapToResourcium;
        }

        public SellScrapAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var cmdShip = (CommandShip)Target;
            var scrap = cmdShip.Scrap;
            cmdShip.Scrap = 0;
            var resourcium = scrap / ScrapToResourcium;
            cmdShip.Resourcium += resourcium;
            return $"A <> trades you {resourcium} resourcium for your {scrap} scrap.".Encode(Source, LinkColors.NPC).ToTagSet();
        }

        public string OptionText(IRoom room)
        {
            CalculateValid(room);

            if (IsValid)
            {
                var scrap = ((CommandShip)Target).Scrap;
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
            if (Target is CommandShip)
            {
                var cmdShip = (CommandShip)Target;
                IsValid = cmdShip.Scrap >= ScrapToResourcium;
                return;
            }
            IsValid = false;
        }
    }
}