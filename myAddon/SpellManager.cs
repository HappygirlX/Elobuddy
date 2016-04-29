#region

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

#endregion

namespace myAddon
{
    public static class SpellManager
    {
        static SpellManager()
        {
            // Initialize spells
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, W.Range, SkillShotType.Cone, W.CastDelay, W.Speed, W.Width);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, R.Range, SkillShotType.Circular, R.CastDelay, R.Speed, R.Width);
        }

        // You will need to edit the types of spells you have for each champ as they
        // don't have the same type for each champ, for example Xerath Q is chargeable,
        // right now it's  set to Active.
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Skillshot W { get; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot R { get; }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }
    }
}