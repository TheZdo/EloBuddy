using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace TrundleBuddy
{
    class Program
    {
        public const string Champion = "Trundle";

        public static Menu Trundle, ComboMenu, LaneClearMenu; // Ajouter les noms des menus

        // Declaration des spells
        private static Spell.Active Q;
        private static Spell.Skillshot W;
        private static Spell.Skillshot E;
        private static Spell.Targeted R;


        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingOnLoadingComplete;
        }

        private static void LoadingOnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.BaseSkinName != Champion)
            {
                return; 
            }
            Bootstrap.Init(null);

            Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange());
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 0, int.MaxValue, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 250, int.MaxValue, 225);
            R = new Spell.Targeted(SpellSlot.R, 700);

            Trundle = MainMenu.AddMenu("TrundleBuddy", "TrundleBuddy");

            // Combo Menu
            ComboMenu = Trundle.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("useQ", new CheckBox("Use Q"));
            ComboMenu.Add("useW", new CheckBox("Use W"));
            ComboMenu.Add("useE", new CheckBox("Use E"));
            ComboMenu.Add("useR", new CheckBox("Use R"));

            // Lane Clear Menu
            LaneClearMenu = Trundle.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.AddGroupLabel("LaneClear Settings");
            LaneClearMenu.Add("useQ", new CheckBox("Use Q"));
            LaneClearMenu.Add("useW", new CheckBox("Use W"));
            LaneClearMenu.Add("useE", new CheckBox("Use E"));
            LaneClearMenu.Add("useR", new CheckBox("Use R"));

            Chat.Print("TrundleBuddy | Loaded By TheZdo");

            Game.OnTick += GameOnTick;
        }

        private static void GameOnTick(EventArgs args)
        {
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var target = TargetSelector.GetTarget(Player.Instance.AttackRange, DamageType.Physical, Player.Instance.ServerPosition);

                if (target != null)
                {
                    Q.Cast();
                    
                    R.Cast();
                }

                var eTarget = TargetSelector.GetTarget(E.Range, DamageType.Physical, Player.Instance.ServerPosition);
                var epred = E.GetPrediction(eTarget);

                if (epred.HitChance == HitChance.High)
                {
                    E.Cast(epred.CastPosition);
                }

                var wTarget = TargetSelector.GetTarget(W.Range, DamageType.Physical, Player.Instance.ServerPosition);
                var wpred = W.GetPrediction(wTarget);

                if (wpred.HitChance == HitChance.High)
                {
                    W.Cast(wpred.CastPosition);
                }

            }
            




        }
    }
}
