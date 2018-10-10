using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using GameData;
using Models.Dtos;
using Models.RoomEntities.Hazards;
using Models.RoomEntities.Mobs.Kelp;

namespace Models.Factories
{
    public class RoomFactory : IRoomFactory
    {
        //  these could get adjusted based on the mission or jump distance
        private float CHANCE_ROOM_DANGEROUS = 0.75f;
        private float CHANCE_FOR_HAZARD = 0.66f;    //  given the room is dangerous, what is the chance it contains a hazard
        private float CHANCE_FOR_MOB = 0.66f;       // given the room is dangerous, what is the chance it contains a mob
        private float CHANCE_FOR_TOWN = 0.25f;
        private float CHANCE_FOR_GATHERABLE = 0.25f;
        private float CHANCE_FOR_NPC = 0.25f;

        private int ROOM_ACTOR_CAPACITY = 4;

        private float CHANCE_TRANSITION_FROM_EMPTY = 0.33f; //  the chance that at least one of the exits of a room in empty space will lead to a non-empty flavor room
        private float CHANCE_TRANSITION_TO_EMPTY = 0.2f;    //  the chance that at least one of the exits in a non-empty flavored room will lead to empty space
        
        public static IEnumerable<FlexData> Hazards { get; private set; }
        public static IEnumerable<FlexData> Mobs { get; private set; }
        public static IEnumerable<FlexData> Gatherables { get; private set; }
        
        public RoomFactory(GameContentDto gameContent)
        {
            Hazards = gameContent.Hazards;
            Mobs = gameContent.Mobs;
            Gatherables = gameContent.Gatherables;
        }
        
        public IRoom GenerateRoom(RoomTemplate template)
        {
            //  first get the injectable flavor for the room
            var lookText = InjectableGameData.InjectableRoomLookTexts[template.Flavor].GetRandom();
            var descriptionText = InjectableGameData.InjectableRoomDescriptions[template.Flavor].GetRandom();
            var roomInject = new RoomInjectable(template.Flavor, lookText, descriptionText);

            //  add exits
            var exits = CalculateExits(template);

            //  add actors
            var actors = CalculateActors(template);

            return new Room(roomInject, exits, actors);
        }

        private IEnumerable<RoomTemplate> CalculateExits(RoomTemplate template)
        {
            var exits = new List<RoomTemplate>();

            if (template.Flavor == RoomFlavor.Empty)
            {
                exits.Add(BuildRoomExit(template, false));

                if (CHANCE_TRANSITION_FROM_EMPTY.Rng())
                    exits.Add(BuildRoomExit(template, true));
                else
                    exits.Add(BuildRoomExit(template, false));
            }
            else
            {
                exits.Add(BuildRoomExit(template, false));

                if (CHANCE_TRANSITION_TO_EMPTY.Rng())
                    exits.Add(BuildRoomExit(template, true));
                else
                    exits.Add(BuildRoomExit(template, false));
            }

            return exits;
        }

        private RoomTemplate BuildRoomExit(RoomTemplate template, bool IsZoneTransition)
        {
            //  figure out what kind of actors will be in adjacent rooms
            var entityFlavors = new List<RoomActorFlavor>();

            if (CHANCE_ROOM_DANGEROUS.Rng())
            {
                var dangerous = new List<RoomActorFlavor>();

                if (CHANCE_FOR_HAZARD.Rng())
                    dangerous.Add(RoomActorFlavor.Hazard);

                if (CHANCE_FOR_MOB.Rng())
                    dangerous.Add(RoomActorFlavor.Mob);

                if (!dangerous.Any())
                {
                    if (CHANCE_FOR_HAZARD.Rng())
                        dangerous.Add(RoomActorFlavor.Hazard);
                    else
                        dangerous.Add(RoomActorFlavor.Mob);
                }

                entityFlavors.AddRange(dangerous);
            }

            if (CHANCE_FOR_TOWN.Rng())
                entityFlavors.Add(RoomActorFlavor.Town);

            if (CHANCE_FOR_NPC.Rng())
                entityFlavors.Add(RoomActorFlavor.Npc);

            if (CHANCE_FOR_GATHERABLE.Rng())
                entityFlavors.Add(RoomActorFlavor.Gatherable);

            //  keep the room flavor the same unless its a zone transition
            var nextRoomFlavor = template.Flavor;
            if (IsZoneTransition)
            {
                if (nextRoomFlavor == RoomFlavor.Empty)
                {
                    nextRoomFlavor = RoomFlavor.Kelp;
                }
                else
                {
                    nextRoomFlavor = RoomFlavor.Empty;
                }
            }

            //  increase the difficulty of the exit by 1
            return new RoomTemplate(template.Difficulty + 1, nextRoomFlavor, entityFlavors);
        }

        private List<IRoomActor> CalculateActors(RoomTemplate template)
        {
            var actors = new List<IRoomActor>();

            if (template.ActorFlavors.Contains(RoomActorFlavor.Mob))
            {
                //  do something hacky for now
                if (template.Flavor == RoomFlavor.Kelp)
                {
                    if (template.Difficulty <= 5)
                    {
                        actors.Add(new VerdantObserverMob());
                    }
                    else if (template.Difficulty <= 10)
                    {
                        if (7 <= UnityEngine.Random.Range(0, 11))
                        {
                            actors.Add(new VerdantInformantMob());
                        } else
                        {
                            actors.Add(new VerdantObserverMob());
                            actors.Add(new VerdantObserverMob());
                        }
                    }
                    else if (template.Difficulty <= 15)
                    {

                        if (7 <= UnityEngine.Random.Range(0, 11))
                        {
                            actors.Add(new VerdantInterrogatorMob());
                        }
                        else
                        {
                            actors.Add(new VerdantInformantMob());
                            actors.Add(new VerdantObserverMob());
                        }
                    }
                    else if (template.Difficulty <= 20)
                    {

                        if (7 <= UnityEngine.Random.Range(0, 11))
                        {
                            actors.Add(new VerdantInterrogatorMob());
                            actors.Add(new VerdantInformantMob());
                        }
                        else
                        {
                            actors.Add(new VerdantInformantMob());
                            actors.Add(new VerdantObserverMob());
                            actors.Add(new VerdantObserverMob());
                        }
                    }
                }
                else
                {
                    actors.Add(new PirateMob());
                }
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Town))
            {
                actors.Add(new SpaceStationNpc());
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Hazard))
            {
                var data = Hazards.Where(h => h.RoomFlavors.Contains(template.Flavor) && h.DifficultyRating < 5).GetRandom();
                actors.Add(data.FromFlexData());
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Npc))
            {
                if (template.Flavor == RoomFlavor.Kelp)
                {
                    actors.Add(new NeedsHelpNpc());
                }
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Gatherable))
            {
                //  do something hacky for now
                if (template.Flavor == RoomFlavor.Empty)
                {
                    if (0.5f.Rng())
                    {
                        actors.Add(new SingleUseGatherable("Shiny Comet"));
                    }
                    else
                    {
                        actors.Add(new ScrapGatherable());
                    }
                }
                else if (template.Flavor == RoomFlavor.Kelp)
                {
                    actors.Add(new SingleUseGatherable("Kelp Fiber"));
                }
            }

            return actors;
        }
    }
}
