#region

using EloBuddy;
using EloBuddy.SDK;
// Using the config like this makes your life easier, trust me
using Settings = myAddon.Config;

#endregion

namespace myAddon.Modes
{
    public sealed class Combo : ModeBase
    {
        private readonly float _stacks = Player.GetBuff("pyromania").Count;

        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            // TODO: Add combo logic here
            // See how I used the Settings.UseQ here, this is why I love my way of using
            // the menu in the Config class!
            Orbwalker.DisableAttacking = false;
            if (Settings.UseQCombo && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (AutoAa(target))
                {
                    Orbwalker.DisableAttacking = false;
                }
                var predW = W.GetPrediction(target).CastPosition;
                var predR = R.GetPrediction(target).CastPosition;
                if (target != null && Player.Instance.Distance(target) < 600 && W.IsReady() && Settings.UseWCombo &&
                    _stacks > 2)
                {
                    Orbwalker.DisableAttacking = true;
                    W.Cast(predW);
                }
                else if (target != null)
                {
                    if (Player.Instance.Distance(target) < 600 && R.IsReady() && ComboDmg(target) > target.Health &&
                        _stacks > 2 && Settings.UseRCombo)
                    {
                        Orbwalker.DisableAttacking = true;
                        R.Cast(predR);
                    }
                    if (_stacks <= 2 && !W.IsReady())
                    {
                        Orbwalker.DisableAttacking = true;
                        Q.Cast(target);
                    }
                    if (_stacks >= 2 && W.IsReady())
                    {
                        Orbwalker.DisableAttacking = true;
                        Q.Cast(target);
                    }
                    if (Player.HasBuff("pyromania_particle") && !W.IsReady() && !R.IsReady())
                    {
                        Orbwalker.DisableAttacking = true;
                        Q.Cast(target);
                    }
                }
            }
            if (Settings.UseRCombo && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                if (AutoAa(target))
                {
                    Orbwalker.DisableAttacking = false;
                }
                var predW = W.GetPrediction(target).CastPosition;
                var predR = R.GetPrediction(target).CastPosition;
                if (target != null && target.CountEnemiesInRange(R.Width) >= 2 && Player.HasBuff("pyromania_particle") &&
                    !W.IsReady() && ComboDmg(target) >= target.Health && !target.HasBuff("bansheesveil"))
                {
                    Orbwalker.DisableAttacking = true;
                    R.Cast(predR);
                }
                else if (target != null)
                {
                    if (target.CountEnemiesInRange(W.Width) >= 2 && Player.HasBuff("pyromania_particle") && W.IsReady() &&
                        Settings.UseWCombo)
                    {
                        Orbwalker.DisableAttacking = true;
                        W.Cast(predW);
                    }
                    if (ComboDmg(target) >= target.Health && _stacks > 2 && Q.IsReady() ||
                        W.IsReady() && !target.HasBuff("bansheesveil"))
                    {
                        Orbwalker.DisableAttacking = true;
                        R.Cast(predR);
                    }
                    if (Player.Instance.GetSpellDamage(target, SpellSlot.R)*RReady() >= target.Health && Q.IsReady() &&
                        !W.IsReady() && target.Health > Player.Instance.GetSpellDamage(target, SpellSlot.Q)*QReady() &&
                        !target.HasBuff("bansheesveil"))
                    {
                        Orbwalker.DisableAttacking = true;
                        R.Cast(predR);
                    }
                    if (Player.Instance.GetSpellDamage(target, SpellSlot.R)*RReady() >= target.Health && !Q.IsReady() &&
                        W.IsReady() && target.Health > Player.Instance.GetSpellDamage(target, SpellSlot.W)*WReady() &&
                        !Player.HasBuff("pyromania_particle") && !target.HasBuff("bansheesveil"))
                    {
                        Orbwalker.DisableAttacking = true;
                        R.Cast(predR);
                    }
                }
            }
            if (Settings.UseWCombo && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                if (AutoAa(target))
                {
                    Orbwalker.DisableAttacking = false;
                }
                var predW = W.GetPrediction(target).CastPosition;
                if (target != null && Player.HasBuff("pyromania_particle") && ComboDmg(target) < target.Health)
                {
                    Orbwalker.DisableAttacking = true;
                    W.Cast(predW);
                }
                else if (target != null)
                {
                    if (target.Health <= Player.Instance.GetSpellDamage(target, SpellSlot.W)*WReady())
                    {
                        Orbwalker.DisableAttacking = true;
                        W.Cast(predW);
                    }

                    if (!Q.IsReady() && !R.IsReady() &&
                        (target.Health > Player.Instance.GetSpellDamage(target, SpellSlot.W)*WReady() ||
                         (_stacks <= 3 && _stacks > 1)))
                    {
                        Orbwalker.DisableAttacking = true;
                        W.Cast(predW);
                    }
                }
            }
        }

        private float ComboDmg(Obj_AI_Base target)
        {
            if (target != null)
            {
                return Player.Instance.GetSpellDamage(target, SpellSlot.Q)*QReady() +
                       Player.Instance.GetSpellDamage(target, SpellSlot.W)*WReady() +
                       Player.Instance.GetSpellDamage(target, SpellSlot.R)*RReady();
            }
            return 0;
        }

        private bool AutoAa(Obj_AI_Base target)
        {
            if (target != null && Player.Instance.Distance(target) < 600 &&
                Player.Instance.GetAutoAttackDamage(target)*2 <= target.Health)
            {
                return true;
            }
            return false;
        }

        private int QReady()
        {
            if (Q.IsReady())
            {
                return 1;
            }
            return 0;
        }

        private int WReady()
        {
            if (W.IsReady())
            {
                return 1;
            }
            return 0;
        }

        private int RReady()
        {
            if (R.IsReady())
            {
                return 1;
            }
            return 0;
        }
    }
}