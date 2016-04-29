#region

using EloBuddy;
using EloBuddy.SDK;
using Settings = myAddon.Config.Misc;

#endregion

namespace myAddon.Modes
{
    /// <summary>
    ///     Description of misc.
    /// </summary>
    public sealed class Misc : ModeBase
    {
        private readonly float _myMana = Player.Instance.Mana;
        private readonly float _qMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SData.Mana;
        private readonly float _rMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.Mana;
        private readonly float _wMana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.Mana;

        public override bool ShouldBeExecuted()
        {
            return !Player.HasBuff("pyromania_particle");
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(10000, DamageType.Magical);
            if (Settings.UseE && E.IsReady() && Mana())
            {
                E.Cast();
            }
            if (Settings.UseE && E.IsReady() && Player.Instance.IsInShopRange())
            {
                E.Cast();
            }
            if (Settings.AutoStacks && Player.Instance.IsInShopRange() && W.IsReady())
            {
                W.Cast();
            }
            if (Settings.AutoStacks && target == null && W.IsReady() && Player.Instance.ManaPercent > 90 &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                W.Cast();
            }
        }

        private bool Mana()
        {
            if (Q.IsReady() && !W.IsReady() && !R.IsReady() && _qMana + 20 > _myMana)
            {
                return true;
            }
            if (Q.IsReady() && W.IsReady() && !R.IsReady() && _qMana + _wMana + 20 > _myMana)
            {
                return true;
            }
            if (Q.IsReady() && W.IsReady() && R.IsReady() && _qMana + _wMana + _rMana + 20 > _myMana)
            {
                return true;
            }
            return false;
        }
    }
}