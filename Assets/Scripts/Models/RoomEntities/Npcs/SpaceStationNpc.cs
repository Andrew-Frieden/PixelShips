using System.Collections.Generic;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using TextEncoding;

public class SpaceStationNpc : FlexEntity
{
    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        var repairPrice = RepairAction.CalculateRepairCost(room.PlayerShip);
        var tradeValue = TradeCommoditiesAction.CalculateCommodityValue(room.PlayerShip);

        return DialogueBuilder.Init()
            .AddMainText("The busy <> is blinking with activity. Trade vessels constantly come and go. Comms light up with public chatter.".Encode(Name, Id, LinkColors.NPC))
            .AddOption($"Repair your ship.{Env.ll}Costs {repairPrice} credits." , new RepairAction(room.PlayerShip, repairPrice))
            .AddOption($"Trade commodities for {tradeValue} credits.", new TradeCommoditiesAction(room.PlayerShip, tradeValue))
            .Build();
    }

    public override StringTagContainer GetLookText()
    {
        return new StringTagContainer()
        {
            Text = "A slowly rotating <> orbits a nearby system.".Encode(Name, Id, LinkColors.NPC),
        };
    }

    public override IRoomAction GetNextAction(IRoom room)
    {
        //  TODO this could instead sometimes return flavor text for things happening around at the space station
        return new DoNothingAction(this);
    }

    public SpaceStationNpc(string name = "Starport")
    {
        Name = name;
    }

    private class RepairAction : SimpleAction
    {
        public static int CalculateRepairCost(CommandShip ship)
        {
            var hullToRepair = ship.MaxHull - ship.Hull;
            var pricePerHull = 5;
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

        public override IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            var ship = (CommandShip)Source;

            var repairAmount = ship.MaxHull - ship.Hull;

            if (ship.Stats[StatKeys.Credits] >= Cost)
            {
                ship.Stats[StatKeys.Credits] -= Cost;
                ship.Hull = ship.MaxHull;
                
                return new List<StringTagContainer>()
                {
                    new StringTagContainer()
                    {
                        Text = $"Your ship is repaired {repairAmount} hull."
                    }
                };
            }
            
            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text = "You try to make a deal but turn up empty handed."
                }
            };
        }
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

        public override IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            var ship = (CommandShip)Source;

            ship.Stats[StatKeys.Scrap] = 0;
            ship.Stats[StatKeys.Resourcium] = 0;
            ship.Stats[StatKeys.Techanite] = 0;
            ship.Stats[StatKeys.MachineParts] = 0;
            ship.Stats[StatKeys.PulsarCoreFragments] = 0;

            ship.Stats[StatKeys.Credits] += Profit;

            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text =  $"<> make a deal for {Profit} resourcium".Encode(Source.GetLinkText(), Source.Id, LinkColors.Player) 
                }
            };
        }
    }
}
