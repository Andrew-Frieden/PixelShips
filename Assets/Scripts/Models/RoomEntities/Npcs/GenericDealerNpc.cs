using TextSpace.Items;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using System;
using System.Collections.Generic;
using TextEncoding;
using System.Linq;

namespace TextSpace.RoomEntities
{
    public interface IHaveDependents
    {
        IEnumerable<FlexData> FindHardwareDependents(IEnumerable<FlexData> data);
        IEnumerable<FlexData> FindWeaponDependents(IEnumerable<FlexData> data);
    }

    public enum NpcDeal
    {
        None = 0,
        BuyHardware = 1,
        BuyWeapon = 2,
        SellScrap = 3,
        Repair = 4,
        BuyMaxHull = 5
    }

    public class GenericDealerNpc : FlexEntity, IHaveDependents
    {
        private const int DEFAULT_SCRAP_TO_RESOURCIUM = 10;
        private const int DEFAULT_ITEM_COST = 1;

        public NpcDeal DealA
        {
            get
            {
                if (!Values.ContainsKey(ValueKeys.NpcDealA))
                    DealA = NpcDeal.None;
                return (NpcDeal)Enum.Parse(typeof(NpcDeal), Values[ValueKeys.NpcDealA]);
            }
            protected set
            {
                Values[ValueKeys.NpcDealA] = value.ToString();
            }
        }

        public NpcDeal DealB
        {
            get
            {
                if (!Values.ContainsKey(ValueKeys.NpcDealB))
                    DealB = NpcDeal.None;
                return (NpcDeal)Enum.Parse(typeof(NpcDeal), Values[ValueKeys.NpcDealB]);
            }
            protected set
            {
                Values[ValueKeys.NpcDealB] = value.ToString();
            }
        }

        private int ScrapToResourcium
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.ScrapToResourcium))
                    ScrapToResourcium = DEFAULT_SCRAP_TO_RESOURCIUM;
                return Stats[StatKeys.ScrapToResourcium];
            }
            set { Stats[StatKeys.ScrapToResourcium] = value; }
        }

        public GenericDealerNpc(FlexData data) : base(data)
        {
            IsAttackable = false;
            IsHostile = false;
        }

        public GenericDealerNpc(FlexEntityDto dto) : base(dto) { }

        private readonly List<IRoomActor> inventory = new List<IRoomActor>();

        private bool SetupDealOption(NpcDeal deal, IRoom room, IDialogueBuilder builder)
        {
            switch (deal)
            {
                case NpcDeal.None:
                    break;
                case NpcDeal.BuyHardware:
                    var hardwareToSell = room.FindDependentActors(this).Where(a => a is Hardware && !inventory.Contains(a)).FirstOrDefault();
                    if (hardwareToSell == null)
                        return false;
                    var sellHardwareAction = new SellItemAction(this, hardwareToSell, DEFAULT_ITEM_COST);
                    builder.AddOption(sellHardwareAction.OptionText(room), sellHardwareAction);
                    inventory.Add(hardwareToSell);
                    break;
                case NpcDeal.BuyWeapon:
                    var weaponToSell = room.FindDependentActors(this).Where(a => a is Weapon && !inventory.Contains(a)).FirstOrDefault();
                    if (weaponToSell == null)
                        return false;
                    var sellWeaponAction = new SellItemAction(this, weaponToSell, DEFAULT_ITEM_COST);
                    builder.AddOption(sellWeaponAction.OptionText(room), sellWeaponAction);
                    inventory.Add(weaponToSell);
                    break;
                case NpcDeal.SellScrap:
                    var sellAction = new SellScrapAction(this, room.PlayerShip, ScrapToResourcium);
                    builder.AddOption(sellAction.OptionText(room), sellAction);
                    break;
                case NpcDeal.Repair:
                    var repairPrice = RepairAction.CalculateRepairCost(room.PlayerShip);
                    var repairAction = new RepairAction(room.PlayerShip, repairPrice);
                    builder.AddOption($"Repair your ship.{Env.ll}Costs {repairPrice} resourcium.", repairAction);
                    break;
                case NpcDeal.BuyMaxHull:
                    return false;
                default:
                    break;
            }
            return true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            if (DealA == NpcDeal.None && DealB == NpcDeal.None)
            {
                var doneBuilder = DialogueBuilder.Init().AddMainText($"<> has nothing else to sell.".Encode(this, LinkColors.NPC));
                DialogueContent = doneBuilder.Build(room);
                return;
            }

            var builder = DialogueBuilder.Init().AddMainText(DialogueText.Encode(this, LinkColors.NPC));

            inventory.Clear();
            if (!SetupDealOption(DealA, room, builder))
                DealA = NpcDeal.None;
            if (!SetupDealOption(DealB, room, builder))
                DealB = NpcDeal.None;

            DialogueContent = builder.Build(room);
        }

        public override TagString GetLookText()
        {
            return LookText.Encode(this, LinkColors.NPC).Tag();
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public IEnumerable<FlexData> FindHardwareDependents(IEnumerable<FlexData> data)
        {
            var items = 0;

            if (DealA == NpcDeal.BuyHardware)
                items++;

            if (DealB == NpcDeal.BuyHardware)
                items++;

            return data.OrderBy(d => Guid.NewGuid()).Take(items);
        }

        public IEnumerable<FlexData> FindWeaponDependents(IEnumerable<FlexData> data)
        {
            var items = 0;

            if (DealA == NpcDeal.BuyWeapon)
                items++;

            if (DealB == NpcDeal.BuyWeapon)
                items++;

            return data.OrderBy(d => Guid.NewGuid()).Take(items);
        }
    }
}