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

            //entities.Add(tabbyOfficer);

            var exampleNpc = new ExampleNpcFlexEntity();
            entities.Add(exampleNpc);

            return entities;
        }
    }
}