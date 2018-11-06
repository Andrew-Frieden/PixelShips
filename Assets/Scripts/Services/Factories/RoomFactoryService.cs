using System;
using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using GameData;
using TextSpace.Framework;
using TextSpace.Items;
using TextSpace.Models;
using TextSpace.Models.Dtos;
using TextSpace.Models.RoomEntities.Mobs;
using TextSpace.Models.Stats;
using TextSpace.RoomEntities;
using UnityEngine;

namespace TextSpace.Services.Factories
{
    public class RoomFactoryService : IResolvableService
    {
        //  these could get adjusted based on the mission or jump distance
        private float CHANCE_ROOM_DANGEROUS = 0.75f;
        private float CHANCE_FOR_HAZARD = 0.66f;    //  given the room is dangerous, what is the chance it contains a hazard
        private float CHANCE_FOR_MOB = 0.66f;       // given the room is dangerous, what is the chance it contains a mob
        private float CHANCE_FOR_TOWN = 0.25f;
        private float CHANCE_FOR_GATHERABLE = 0.25f;
        private float CHANCE_FOR_MINEABLE = 0.1f;
        private float CHANCE_FOR_NPC = 0.4f;
        private float CHANCE_FOR_LIGHT_WEAPON = 0.01f;
        private float CHANCE_FOR_HEAVY_WEAPON = 0.01f;

        private int ROOM_ACTOR_CAPACITY = 4;

        private float CHANCE_TRANSITION_FROM_EMPTY = 0.7f; //  the chance that at least one of the exits of a room in empty space will lead to a non-empty flavor room
        private float CHANCE_TRANSITION_TO_EMPTY = 0.2f;    //  the chance that at least one of the exits in a non-empty flavored room will lead to empty space
        
        public static IEnumerable<FlexData> Hazards { get; private set; }
        public static IEnumerable<FlexData> Mobs { get; private set; }
        public static IEnumerable<FlexData> Gatherables { get; private set; }
        public static IEnumerable<FlexData> HardwareContent { get; private set; }
        public static IEnumerable<FlexData> NpcContent { get; private set; }
        public static IEnumerable<FlexData> Mineables { get; private set; }

        private readonly ContentLoadService contentLoadSvc;
        private readonly WeaponFactoryService weaponFactorySvc;
        private readonly MobFactoryService mobFactorySvc;

        public RoomFactoryService(ContentLoadService contentService, WeaponFactoryService weaponFactory, MobFactoryService mobFactory)
        {
            contentLoadSvc = contentService;
            weaponFactorySvc = weaponFactory;
            mobFactorySvc = mobFactory;

            var gameContent = contentLoadSvc.Content;
            Hazards = gameContent.Hazards;
            Mobs = gameContent.Mobs;
            Gatherables = gameContent.Gatherables;
            HardwareContent = gameContent.Hardware;
            NpcContent = gameContent.Npcs;
            Mineables =  gameContent.Mineables;
        }
        
        public IRoom GenerateBootstrapRoom(bool FTUE = true)
        {
            var room = GenerateRoom(new RoomTemplate(0, RoomFlavor.Empty));
            room.AddEntity(new BootstrapEntity(FTUE));
            return room;
        }

        public IRoom GenerateHomeworldRoom(Homeworld world)
        {
            var room = GenerateRoom(new RoomTemplate(0, RoomFlavor.Empty), true);

            var homeworld = new HomeworldNpc(world);
            room.AddEntity(homeworld);

            //  developer hack to add hardware to starting room
            var devHardware = HardwareContent.Where(d => d.EntityType.ToLower().Contains("superdetector")).Single();
            room.AddEntity(devHardware.FromFlexData());

            return room;
        }

        public IRoom GenerateRoom(RoomTemplate template)
        {
            return GenerateRoom(template, false);
        }

        public IRoom GenerateRoom(RoomInjectable injectable, List<IRoomActor> actors, IEnumerable<RoomTemplate> exits)
        {
            return new Room(injectable, exits, actors);
        }

        public IRoom GenerateRoom(RoomTemplate template, bool forceEmpty)
        {
            //  first get the injectable flavor for the room
            var lookText = ExampleGameData.InjectableRoomLookTexts[template.Flavor].GetRandom();
            var descriptionText = ExampleGameData.InjectableRoomDescriptions[template.Flavor].GetRandom();

            // For debug purposes
            descriptionText += "\n PowerLevel: " + template.PowerLevel;

            var roomInject = new RoomInjectable(template.Flavor, lookText, descriptionText);

            //  add exits
            var exits = CalculateExits(template);

            //  add actors
            var actors = new List<IRoomActor>();
            if (!forceEmpty)
                actors = CalculateActors(template);

            return new Room(roomInject, exits, actors);
        }

        public List<FlexData> GetFlexData(GameContentFilterDto gameContentFilterDto)
        {
            switch (gameContentFilterDto.RoomActorFlavor)
            {
                case RoomActorFlavor.None:
                    return new List<FlexData>();
                case RoomActorFlavor.Mob:
                    return Mobs.Where(m => m.RoomFlavors.Contains(gameContentFilterDto.RoomFlavor)).ToList();
                case RoomActorFlavor.Hazard:
                    return Hazards.Where(h => h.RoomFlavors.Contains(gameContentFilterDto.RoomFlavor)).ToList();
                case RoomActorFlavor.Gatherable:
                    return Gatherables.Where(h => h.RoomFlavors.Contains(gameContentFilterDto.RoomFlavor)).ToList();
                case RoomActorFlavor.Npc:
                    return NpcContent.Where(h => h.RoomFlavors.Contains(gameContentFilterDto.RoomFlavor)).ToList();
                case RoomActorFlavor.Mineable:
                    return Mineables.Where(h => h.RoomFlavors.Contains(gameContentFilterDto.RoomFlavor)).ToList();
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            if (CHANCE_FOR_TOWN.Rng())
            {
                entityFlavors.Add(RoomActorFlavor.Town);
            }
            else
            {

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
            }
            

            if (CHANCE_FOR_NPC.Rng())
                entityFlavors.Add(RoomActorFlavor.Npc);

            if (CHANCE_FOR_GATHERABLE.Rng())
                entityFlavors.Add(RoomActorFlavor.Gatherable);
            
            if (CHANCE_FOR_MINEABLE.Rng())
                entityFlavors.Add(RoomActorFlavor.Mineable);

            //  keep the room flavor the same unless its a zone transition
            var nextRoomFlavor = template.Flavor;
            if (IsZoneTransition)
            {
                if (nextRoomFlavor == RoomFlavor.Empty)
                {
                    var roomFlavors = Enum.GetValues(typeof(RoomFlavor));

                    nextRoomFlavor = (RoomFlavor)roomFlavors.GetValue(UnityEngine.Random.Range(0,roomFlavors.Length));

                    // nextRoomFlavor = RoomFlavor.Kelp;

                }
                else
                {
                    nextRoomFlavor = RoomFlavor.Empty;
                }
            }

            return new RoomTemplate(Math.Max(20, template.PowerLevel + 2), nextRoomFlavor, entityFlavors);
        }

        private List<IRoomActor> CalculateActors(RoomTemplate template)
        {
            var actors = new List<IRoomActor>();

            if (CHANCE_FOR_LIGHT_WEAPON.Rng())
            {
                actors.Add(weaponFactorySvc.GetRandomWeapon(Weapon.WeaponTypes.Light, template.PowerLevel));
            }
            
            if (CHANCE_FOR_HEAVY_WEAPON.Rng())
            {
                actors.Add(weaponFactorySvc.GetRandomWeapon(Weapon.WeaponTypes.Heavy, template.PowerLevel));
            }
            
            if (template.ActorFlavors.Contains(RoomActorFlavor.Mob))
            {
                actors.AddRange(mobFactorySvc.BuildMob(template));
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Town))
            {
                var townData = NpcContent
                        .Where(c => c.ActorFlavors.Contains(RoomActorFlavor.Town))
                        .Where(t => t.RoomFlavors.Contains(template.Flavor) || t.RoomFlavors.Contains(RoomFlavor.Empty))
                        .GetRandom();
                actors.AddRange(CreateNpc(townData));

                var otherNpcs = NpcContent
                            .Where(c => !c.ActorFlavors.Contains(RoomActorFlavor.Town))
                            .Where(t => t.RoomFlavors.Contains(template.Flavor) || t.RoomFlavors.Contains(RoomFlavor.Empty))
                            .OrderBy(o => Guid.NewGuid())
                            .Take(2).ToList();

                for (int i = 0; i < otherNpcs.Count; i++)
                {
                    if (0.7f.Rng())
                        actors.AddRange(CreateNpc(otherNpcs[i]));
                }
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Hazard))
            {
                var data = Hazards.Where(h => h.RoomFlavors.Contains(template.Flavor) && h.Powerlevel <= template.PowerLevel).OrderByDescending(h => h.Powerlevel).First();
                actors.Add(data.FromFlexData());
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Npc))
            {
                var npc = NpcContent
                            .Where(c => !c.ActorFlavors.Contains(RoomActorFlavor.Town))
                            .Where(t => t.RoomFlavors.Contains(template.Flavor) || t.RoomFlavors.Contains(RoomFlavor.Empty))
                            .GetRandom();
                actors.AddRange(CreateNpc(npc));
            }

            if (template.ActorFlavors.Contains(RoomActorFlavor.Gatherable))
            {
                var gatherable = Gatherables.Where(h => h.RoomFlavors.Contains(template.Flavor) && h.Powerlevel <= template.PowerLevel).GetRandom();
                actors.Add(gatherable.FromFlexData());
            }
            
            if (template.ActorFlavors.Contains(RoomActorFlavor.Mineable))
            {
                var gatherable = Mineables.Where(h => h.RoomFlavors.Contains(template.Flavor) && h.Powerlevel <= template.PowerLevel).GetRandom();
                actors.Add(gatherable.FromFlexData());
            }

            return actors;
        }

        private IEnumerable<IRoomActor> CreateNpc(FlexData npcData)
        {
            var actors = new List<IRoomActor>();
            var npc = npcData.FromFlexData();
            actors.Add(npc);

            if (npc is IHaveDependents)
            {
                var parent = (IHaveDependents)npc;

                var dependents = new List<FlexData>();
                dependents.AddRange(parent.FindHardwareDependents(HardwareContent));

                foreach (var d in dependents)
                {
                    var dependent = d.FromFlexData();
                    dependent.IsHidden = true;
                    dependent.DependentActorId = npc.Id;
                    actors.Add(dependent);
                }

                actors.AddRange(weaponFactorySvc.GetWeaponDependents(parent));
            }

            return actors;
        }
    }
}
