using System.Collections.Generic;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using TextSpace.RoomEntities;

public class SpaceStationNpc : FlexEntity
{
    public override void CalculateDialogue(IRoom room)
    {
        var repairPrice = RepairAction.CalculateRepairCost(room.PlayerShip);
        var tradeValue = TradeCommoditiesAction.CalculateCommodityValue(room.PlayerShip);

        DialogueContent = DialogueBuilder.Init()
            .AddMainText("The busy <> is blinking with activity. Trade vessels constantly come and go. Comms light up with public chatter.".Encode(Name, Id, LinkColors.NPC))
            .AddOption($"Repair your ship.{Env.ll}Costs {repairPrice} resourcium." , new RepairAction(room.PlayerShip, repairPrice))
            .AddOption($"Trade commodities for {tradeValue} credits.", new TradeCommoditiesAction(room.PlayerShip, tradeValue))
            .Build(room);
    }

    public override TagString GetLookText()
    {
        return new TagString()
        {
            Text = "A slowly rotating <> orbits a nearby system.".Encode(Name, Id, LinkColors.NPC),
        };
    }

    public override IRoomAction MainAction(IRoom room)
    {
        //  TODO this could instead sometimes return flavor text for things happening around at the space station
        return new DoNothingAction(this);
    }

    public SpaceStationNpc(FlexEntityDto dto) : base(dto) { }

    public SpaceStationNpc(string name = "Starport")
    {
        Name = name;
    }

    private class TradeCommoditiesAction : SimpleAction
    {
        public static int CalculateCommodityValue(CommandShip ship)
        {
            var Scrap = ship.Stats[StatKeys.Scrap] * 1;
            var Resourcium = ship.Stats[StatKeys.Resourcium] * 5;
            var Techanite = ship.Stats[StatKeys.Techanite] * 10;
            var MachineParts = ship.Stats[StatKeys.MachineParts] * 3;
            var PulsarCoreFragments = ship.Stats[StatKeys.PulsarCoreFragments] * 25;

            return Scrap + Resourcium + Techanite + MachineParts + PulsarCoreFragments;
        }

        private const string ProfitKey = "profit";

        private int Profit
        {
            get { return Stats[ProfitKey]; }
            set { Stats[ProfitKey] = value; }
        }

        public TradeCommoditiesAction(IRoomActor source, int profit) : base()
        {
            Source = source;
            Profit = profit;
        }

        public TradeCommoditiesAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var ship = (CommandShip)Source;

            ship.Stats[StatKeys.Scrap] = 0;
            ship.Stats[StatKeys.Resourcium] = 0;
            ship.Stats[StatKeys.Techanite] = 0;
            ship.Stats[StatKeys.MachineParts] = 0;
            ship.Stats[StatKeys.PulsarCoreFragments] = 0;

            ship.Stats[StatKeys.Credits] += Profit;

            return new List<TagString>()
            {
                new TagString()
                {
                    Text =  $"<> make a deal for {Profit} resourcium".Encode(Source.GetLinkText(), Source.Id, LinkColors.Player) 
                }
            };
        }
    }
}
