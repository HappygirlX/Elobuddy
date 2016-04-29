using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

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
        private const string MenuName = "Annie";

        private static readonly Menu Menu = MainMenu.AddMenu(displayName: MenuName, uniqueMenuId: MenuName.ToLower());

        static Config()
        {
            // Initialize the menu

            // Initialize the modes
            Modes.Initialize();
            Misc.Initialize();
            Draws.Initialize();
        }

        public static void Initialize()
        {
        }
        
        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                // Initialize the menu
                Menu = Config.Menu.AddSubMenu("Modes", "modes");

                // Initialize all modes
                Combo.Initialize();
                Harass.Initialize();
                LaneClear.Initialize();
                LastHit.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useR;

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool UseR => _useR.CurrentValue;

                static Combo()
                {
                    // Initialize the menu values
                    Menu.AddGroupLabel("Combo");
                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Use R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
            	private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _farming;
                private static readonly Slider _mana;
                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool Farming => _farming.CurrentValue;

                public static int Mana => _mana.CurrentValue;

                static Harass()
                {
                    // Here is another option on how to use the menu, but I prefer the
                    // way that I used in the combo class
                    Menu.AddGroupLabel("Harass");
                    
                   _useQ = Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("harassUseW", new CheckBox("Use W"));
                   	_farming = Menu.Add("farming", new CheckBox("Farm(op)"));
                    _mana = Menu.Add("harassMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {
                }
            }
           public static class LaneClear
            {
           	    private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                public static bool UseQ => _useQ.CurrentValue;

               public static bool UseW => _useW.CurrentValue;

               static LaneClear()
                {
                    Menu.AddGroupLabel("LaneClear");
                   _useQ = Menu.Add("clearUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("clearUseW", new CheckBox("Use W"));
                }

                public static void Initialize()
                {
                }
           }
          	public static class LastHit
            {
          		private static readonly CheckBox _useQ;
                public static bool UseQ => _useQ.CurrentValue;

          	    static LastHit()
                {
                    Menu.AddGroupLabel("LastHit");
                   _useQ = Menu.Add("lastUseQ", new CheckBox("Use Q"));
                }

                public static void Initialize()
                {
                }
           }
        }
          	
 		public static class Misc
            {
              private static readonly CheckBox _useE;
              private static readonly CheckBox _stacks;

 		    public static bool UseE => _useE.CurrentValue;

 		    public static bool AutoStacks => _stacks.CurrentValue;

 		    static Misc()
                {
                    var menu = Menu.AddSubMenu("Misc","misc");
                    _useE = menu.Add("autoE", new CheckBox("Auto E"));
                    _stacks = menu.Add("autoStacks", new CheckBox("Auto Stacks"));
                }
 
                public static void Initialize()
                {
                }
           }

        private static class Draws
            {
             	private const int BarWidth = 106;
                private static readonly CheckBox _drawdmg;
                private const int LineThickness = 10;
                private static Vector2 BarOffset = new Vector2(0, 16);

            private static Color DrawingColor
                {
                set { Color.FromArgb(170, value); }
                }
                private static Color _drawQ;

            private static Color  DrawQ
                {
                    get { return _drawQ; }
                    set { _drawQ = Color.FromArgb(170, value); }
                }
                private static Color _drawW;

            private static Color  DrawW
                {
                    get { return _drawW; }
                    set { _drawW = Color.FromArgb(170, value); }
                }
                private static Color _drawR;

            private static Color  DrawR
                {
                    get { return _drawR; }
                    set { _drawR = Color.FromArgb(170, value); }
                }

            private static bool DrawDmg => _drawdmg.CurrentValue;

             static Draws()
                {
                    var menu = Menu.AddSubMenu("Draws","draw");
                    _drawdmg = menu.Add("drawDmg", new CheckBox("Draw Dmg"));
                }

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
                        if (damageQ +  damageW + damageR <= 0)
                        {
                        continue;
                        }
                        var damagePercentageQ = ((unit.TotalShieldHealth() - damageQ) > 0 ? (unit.TotalShieldHealth() - damageQ) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var damagePercentageW = ((unit.TotalShieldHealth() - damageW) > 0 ? (unit.TotalShieldHealth() - damageW) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var damagePercentageR = ((unit.TotalShieldHealth() - damageR) > 0 ? (unit.TotalShieldHealth() - damageR) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var currentHealthPercentage = unit.TotalShieldHealth() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var startPointQ = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageQ * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var startPointW = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageW * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var startPointR = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentageR * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var endPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
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