using EloBuddy.SDK;

namespace myAddon.Modes
{
    public abstract class ModeBase
    {
        // Change the spell type to whatever type you used in the SpellManager
        // here to have full features of that spells, if you don't need that,
        // just change it to Spell.SpellBase, this way it's dynamic with still
        // the most needed functions
        protected Spell.Targeted Q => SpellManager.Q;

        protected Spell.Skillshot W => SpellManager.W;

        protected Spell.Active E => SpellManager.E;
        protected Spell.Skillshot R => SpellManager.R;

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
