#region

using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = myAddon.Config.Modes.LastHit;

#endregion

namespace myAddon.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on lasthit mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            if (Q.IsReady() && Settings.UseQ)
            {
                var target =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.Position, Q.Range)
                        .Where(minion => minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q));
                if (!Player.HasBuff("pyromania_particle"))
                {
                    Orbwalker.DisableAttacking = true;
                    Q.Cast(target.First());
                }
            }
        }
    }
}