using Models.Actions;
using Models.Dialogue;
using Models.Stats;

namespace Models.Actors
{
    public class DelayedAttackActor : TemporaryEntity
    {
        private readonly IRoomActor _source;
        private readonly IRoomActor _target;
        private readonly int _damage;
        private readonly string _name;

        public DelayedAttackActor(IRoomActor source, IRoomActor target, int timeToLive, int damage, string name) : base()
        {
            Stats[StatKeys.TimeToLive] = timeToLive;
            _source = source;
            _target = target;
            _damage = damage;
            _name = name;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.Init()
                 .AddMainText("The " + _name + " will strike in " + Stats[StatKeys.TimeToLive] + " turns.")
                  .Build();
        }

        public override TagString GetLookText()
        {
            return new TagString();
        }

        public override IRoomAction MainAction(IRoom s)
        {
            if (Stats[StatKeys.TimeToLive] == 1)
            {
                Stats[StatKeys.TimeToLive]--;
                return new AttackAction(_source, _target, _damage, _name, 0);
            }
            
            return new DelayedAction($"A {_name} will hit {_target.GetLinkText()} ", Id);
        }

        public override IRoomAction CleanupStep(IRoom room)
        {
            base.CleanupStep(room);

            if (_target.IsDestroyed)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
    }
}