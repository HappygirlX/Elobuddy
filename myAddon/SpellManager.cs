#region

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

#endregion

namespace myAddon
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; set; }
        public static Spell.Skillshot W { get; set; }

        public static Spell.Active E { get; set; }
        public static Spell.Skillshot R { get; set; }

        public static void Spells()
        {
            // Initialize spells
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, W.Range, SkillShotType.Cone, W.CastDelay, W.Speed, W.Width);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, R.Range, SkillShotType.Circular, R.CastDelay, R.Speed, R.Width);
        }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }
    }
}