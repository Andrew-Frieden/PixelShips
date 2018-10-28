using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;

namespace Models.Actors
{
    public class DelayedAttackActor : TemporaryEntity
    {
        private IRoomActor _source;
        private IRoomActor _target;

        public string SourceId
        {
            get
            {
                return Values[ValueKeys.SourceId];
            }
            private set
            {
                Values[ValueKeys.SourceId] = value; 
            }
        }

        public string TargetId
        {
            get
            {
                return Values[ValueKeys.TargetId];
            }
            private set
            {
                Values[ValueKeys.TargetId] = value; 
            }
        }

        public int Damage
        {
            get
            {
                return Stats[StatKeys.BaseDamage];
            }
            private set
            {
                Stats[StatKeys.BaseDamage] = value;
            }
        }
        
        public string Name
        {
            get
            {
                return Values[ValueKeys.Name];
            }
            private set
            {
                Values[ValueKeys.Name] = value;
            }
        }

        public int TimeToLive
        {
            get
            {
                return Stats[StatKeys.TimeToLive];
            }
            private set
            {
                Stats[StatKeys.TimeToLive] = value;
            }
        }

        public DelayedAttackActor(FlexEntityDto dto) : base(dto) { }

        public DelayedAttackActor(IRoomActor source, IRoomActor target, int timeToLive, int baseDamage, string name)
        {
            SourceId = source.Id;
            TargetId = target.Id;
            
            TimeToLive = timeToLive;
            Damage = baseDamage;
            Name = name;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.Init()
                 .AddMainText("The " + Name + " will strike in " + Stats[StatKeys.TimeToLive] + " turns.")
                  .Build(room);
        }

        public override TagString GetLookText()
        {
            return new TagString();
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (_source == null || _target == null)
            {
                _source = room.FindEntity(SourceId) as IRoomActor;
                _target = room.FindEntity(TargetId) as IRoomActor;
            }
            
            // Changing TimeToLive is only Ok here because the actor is cleaned up on the same tick
            if (TimeToLive == 1)
            {
                TimeToLive--;
                
                return new AttackAction(_source, _target, Damage, Name, 0);
            }
            
            return new DelayedAction($"A {Name} will hit {_target.GetLinkText()} in {TimeToLive} ticks", Id);
        }

        public override IRoomAction CleanupStep(IRoom room)
        {
            base.CleanupStep(room);

            if (_target != null && _target.IsDestroyed)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
    }
}