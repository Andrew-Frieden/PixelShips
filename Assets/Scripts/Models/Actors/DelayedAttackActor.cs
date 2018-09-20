using Models.Actions;
using Models.Dialogue;

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
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damage = damage;
            _name = name;
        }

        public override ABDialogueContent CalculateDialogue(IRoom room)
        {
            return DialogueBuilder.Init()
                 .AddMainText("The " + _name + " will strike in " + Stats[TimeToLiveKey] + " turns.")
                  .Build();
        }

        public override StringTagContainer GetLookText()
        {
            return new StringTagContainer();
        }

        public override IRoomAction MainAction(IRoom s)
        {
            if (Stats[TimeToLiveKey] == 1)
            {
                Stats[TimeToLiveKey]--;
                return new AttackAction(_source, _target, _damage, _name);
            }
            else
            {
                return new DelayedAction($"A {_name} will hit {_target.GetLinkText()} ", Id);
            }
        }
    }
}