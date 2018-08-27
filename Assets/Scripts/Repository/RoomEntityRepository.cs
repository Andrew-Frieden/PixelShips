using System.Collections.Generic;
using Models;

namespace Repository
{
    public class RoomEntityRepository : IJsonRepository<IRoomActor>
    {
        public IEnumerable<IRoomActor> LoadData()
        {
            var entities = new List<IRoomActor>();
            
            var tabbyOfficerContent = new ABDialogueContent()
            {
                MainText = @"A tabby officer greets you over the voice comms:

Meow Citizen!

I was in pursuit of two renegade Verdants in this sectorrr but my ship got stuck in the kelp.Can you renderrr me some assistance ? ",
                OptionAText = "Fire basic laser",
                OptionBText = "Fire hellfire missle.",
            };
            
            var tabbyOfficer = new Mob("A < > floats here.", "Tabby Officer", 2, tabbyOfficerContent);
    
            //TODO: Set these post creation so there is no dependency in the repository on game state
            //tabbyOfficerContent.OptionAAction = new AttackAction(tabbyOfficer, commandShip, 3);
            //tabbyOfficerContent.OptionBAction = new AttackAction(tabbyOfficer, commandShip, 5);

            entities.Add(tabbyOfficer);

            return entities;
        }
    }
}