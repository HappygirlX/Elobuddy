#region

using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace myAddon
{
    // I can't really help you with my layout of a good config class
    // since everyone does it the way they like it most, go checkout my
    // config classes I make on my GitHub if you wanna take over the
    // complex way that I use
    public static class Config
    {
        private static CheckBox _useQCombo;
        private static CheckBox _useWCombo;
        private static CheckBox _useRCombo;
        public static bool UseQCombo => _useQCombo.CurrentValue;
        public static bool UseWCombo => _useWCombo.CurrentValue;
        public static bool UseRCombo => _useRCombo.CurrentValue;
        private static CheckBox _useQHarass;
        private static CheckBox _useWHarass;
        private static CheckBox _farmingHarass;
        private static Slider _manaHarass;
        public static bool UseQHarass => _useQHarass.CurrentValue;
        public static bool UseWHarass => _useWHarass.CurrentValue;
        public static bool FarmingHarass => _farmingHarass.CurrentValue;
        public static int ManaHarass => _manaHarass.CurrentValue;
        private static CheckBox _useQLaneClear;
        private static CheckBox _useWLaneClear;
        public static bool UseQLaneClear => _useQLaneClear.CurrentValue;
        public static bool UseWLaneClear => _useWLaneClear.CurrentValue;
        private static CheckBox _useQLastHit;
        public static bool UseQLastHit => _useQLastHit.CurrentValue;
        private static CheckBox _useEMisc;
        private static CheckBox _stacksMisc;
        public static bool UseEMisc => _useEMisc.CurrentValue;
        public static bool AutoStacksMisc => _stacksMisc.CurrentValue;

        private static Menu Main, Combo, Harass, LaneClear, LastHit, Misc, Draw;

        public static void MyMenu()
        {
            // Initialize the menu
            Main = MainMenu.AddMenu("Annie", "Annie");
            Combo = Main.AddSubMenu("Combo");
            Harass = Main.AddSubMenu("Harass");
            LaneClear = Main.AddSubMenu("LaneClear");
            LastHit = Main.AddSubMenu("LastHit");
            Misc = Main.AddSubMenu("Misc");
            Draw = Main.AddSubMenu("Draw");

            Combo.AddGroupLabel("Combo");
            _useQCombo = Combo.Add("comboUseQ", new CheckBox("Use Q"));
            _useWCombo = Combo.Add("comboUseW", new CheckBox("Use W"));
            _useRCombo = Combo.Add("comboUseR", new CheckBox("Use R"));

            Harass.AddGroupLabel("Harass");
            _useQHarass = Harass.Add("harassUseQ", new CheckBox("Use Q"));
            _useWHarass = Harass.Add("harassUseW", new CheckBox("Use W"));
            _farmingHarass = Harass.Add("farming", new CheckBox("Farm(op)"));
            _manaHarass = Harass.Add("harassMana", new Slider("Maximum mana usage in percent ({0}%)", 40));

            LaneClear.AddGroupLabel("LaneClear");
            _useQLaneClear = LaneClear.Add("clearUseQ", new CheckBox("Use Q"));
            _useWLaneClear = LaneClear.Add("clearUseW", new CheckBox("Use W"));

            LastHit.AddGroupLabel("LastHit");
            _useQLastHit = LastHit.Add("lastUseQ", new CheckBox("Use Q"));

            _useEMisc = Misc.Add("autoE", new CheckBox("Auto E"));
            _stacksMisc = Misc.Add("autoStacks", new CheckBox("Auto Stacks"));

            // Initialize the modes
            Draws.Initialize();
        }

        public static void Initialize()
        {
        }

        private static class Draws
        {
            private const int BarWidth = 106;
            private const int LineThickness = 10;
            private static readonly CheckBox _drawdmg;
            private static Vector2 BarOffset = new Vector2(0, 16);
            private static Color _drawQ;
            private static Color _drawW;
            private static Color _drawR;

            static Draws()
            {
                _drawdmg = Draw.Add("drawDmg", new CheckBox("Draw Dmg"));
            }

            private static Color DrawingColor
            {
                set { Color.FromArgb(170, value); }
            }

            private static Color DrawQ
            {
                get { return _drawQ; }
                set { _drawQ = Color.FromArgb(170, value); }
            }

            private static Color DrawW
            {
                get { return _drawW; }
                set { _drawW = Color.FromArgb(170, value); }
            }

            private static Color DrawR
            {
                get { return _drawR; }
                set { _drawR = Color.FromArgb(170, value); }
            }

            private static bool DrawDmg => _drawdmg.CurrentValue;

            public static void Initialize()
            {
                DrawQ = Color.RoyalBlue;
                DrawW = Color.Teal;
                DrawR = Color.IndianRed;
                DrawingColor = Color.DarkBlue;
                Drawing.OnEndScene += OnEndScene;
            }

            private static void OnEndScene(EventArgs args)
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsHPBarRendered && u.IsValid))
                {
                    var damageQ = Player.Instance.GetSpellDamage(unit, SpellSlot.Q);
                    var damageW = Player.Instance.GetSpellDamage(unit, SpellSlot.W);
                    var damageR = Player.Instance.GetSpellDamage(unit, SpellSlot.R);
                    if (damageQ + damageW + damageR <= 0)
                    {
                        continue;
                    }
                    var damagePercentageQ = (unit.TotalShieldHealth() - damageQ > 0
                        ? unit.TotalShieldHealth() - damageQ
                        : 0)/
                                            (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    var damagePercentageW = (unit.TotalShieldHealth() - damageW > 0
                        ? unit.TotalShieldHealth() - damageW
                        : 0)/
                                            (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    var damagePercentageR = (unit.TotalShieldHealth() - damageR > 0
                        ? unit.TotalShieldHealth() - damageR
                        : 0)/
                                            (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    var currentHealthPercentage = unit.TotalShieldHealth()/
                                                  (unit.MaxHealth + unit.AllShield + unit.AttackShield +
                                                   unit.MagicShield);
                    var startPointQ =
                        new Vector2((int) (unit.HPBarPosition.X + BarOffset.X + damagePercentageQ*BarWidth),
                            (int) (unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    var startPointW =
                        new Vector2((int) (unit.HPBarPosition.X + BarOffset.X + damagePercentageW*BarWidth),
                            (int) (unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    var startPointR =
                        new Vector2((int) (unit.HPBarPosition.X + BarOffset.X + damagePercentageR*BarWidth),
                            (int) (unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    var endPoint =
                        new Vector2((int) (unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage*BarWidth) + 1,
                            (int) (unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    if (DrawDmg)
                    {
                        Drawing.DrawLine(startPointQ, endPoint, LineThickness, DrawQ);
                        Drawing.DrawLine(startPointW, endPoint, LineThickness, DrawW);
                        Drawing.DrawLine(startPointR, endPoint, LineThickness, DrawR);
                    }
                }
            }
        }
    }
}