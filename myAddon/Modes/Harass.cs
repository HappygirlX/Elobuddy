#region

using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
// Using the config like this makes your life easier, trust me
using Settings = myAddon.Config;

#endregion

namespace myAddon.Modes
{
    public sealed class Harass : ModeBase
    {
        private readonly float _stacks = Player.GetBuff("pyromania").Count;

        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on harass mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            if (Settings.UseQHarass && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                var mob =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.Position, Q.Range)
                        .Where(minion => minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q));
                if (target != null && _stacks >= 2 && Settings.UseQHarass && Player.Instance.ManaPercent > Settings.ManaHarass)
                {
                    Orbwalker.DisableAttacking = true;
                    Q.Cast(target);
                }
                else if (Settings.FarmingHarass)
                {
                    Orbwalker.DisableAttacking = true;
                    Q.Cast(mob.First());
                }
            }
            if (Settings.UseWHarass && Settings.UseWHarass && this.W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                var predW = W.GetPrediction(target).CastPosition;
                if (target != null && !Q.IsReady())
                {
                    W.Cast(predW);
                }
            }
        }
    }
}