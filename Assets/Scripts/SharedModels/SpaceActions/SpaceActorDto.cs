using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class SpaceActorDto
    {
        public int? Version { get; set; }
        public string Id { get; set; }
        public string ActorType { get; set; }   //  'ship', 'hazard'
        public List<SpaceActionDto> Actions { get; set; }
        public DateTime LastResolved { get; set; }
    }

    public static partial class SpaceActorHelpers
    {
        public static SpaceActorDto ToDto(this SpaceActor actor)
        {
            return new SpaceActorDto
            {
                Id = actor.Id,
                Version = actor.Version,
                LastResolved = actor.LastResolved,
                ActorType = actor.ActorType,
                Actions = actor.Actions.Select(a => a.ToDto()).ToList()
            };
        }
    }
}
