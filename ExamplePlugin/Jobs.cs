using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Data;
using System.Reflection;

namespace ExamplePlugin
{

    public class Jobs
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        [Hook("Game.Data.Abilities.CEFKCKGJKKM")] // RegisterJob
        private void CEFKCKGJKKM(Abilities myAbility, ref string ClassName, ref string Name, ref string Description, ref string AbilityName, ref WeaponsType WeaponsAllowed, ref ArmorType ArmorAllowed, ref int BonusEvasion, ref int BonusCrit, ref int BaseMove, ref float BaseJump, ref bool CanSwim, ref float AttackGrowth, ref float MAttackGrowth, ref float DefenseGrowth, ref float MDefenseGrowth, ref float SpeedGrowth, ref float HealthMinGrowth, ref float HealthMaxGrowth, ref int AttackBase, ref int MAttackBase, ref int DefenseBase, ref int MDefenseBase, ref int SpeedBase, ref int HealthMinBase, ref int HealthMaxBase)
        {
            StreamWriter w = File.AppendText("fellmod_log.txt");
            Log("I'm here!", w);
            Log(ClassName, w);
            Abilities.Job currentJob = (Abilities.Job)typeof(Abilities).GetField("currentJob", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myAbility);
            int nextAbility = (int)typeof(Abilities).GetField("nextAbility", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myAbility);
            int nextJob = (int)typeof(Abilities).GetField("nextJob", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myAbility);
            List<Abilities.Job> jobs = (List<Abilities.Job>)typeof(Abilities).GetField("jobs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myAbility);
            Dictionary<string, Abilities.Job> jobsByClassName = (Dictionary<string, Abilities.Job>)typeof(Abilities).GetField("jobsByClassName", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(myAbility);

            currentJob = new Abilities.Job();
            currentJob.className = ClassName;
            currentJob.name = Name;
            currentJob.abilityName = AbilityName;
            currentJob.description = "WHATEVER DESCRIPTION I WANT";
            currentJob.abilityDescription = string.Empty;
            currentJob.weaponsAllowed = WeaponsAllowed;
            currentJob.armorsAllowed = ArmorAllowed;
            currentJob.bonusEvasion = BonusEvasion;
            currentJob.bonusCrit = BonusCrit;
            currentJob.baseMovement = BaseMove;
            currentJob.baseJump = BaseJump;
            currentJob.canSwim = CanSwim;
            currentJob.monsterJob = false;
            currentJob.playerAvailable = true;
            currentJob.statsGrowth[0] = AttackGrowth * 0.5f;
            currentJob.statsGrowth[2] = MAttackGrowth * 0.5f;
            currentJob.statsGrowth[1] = DefenseGrowth * 0.5f;
            currentJob.statsGrowth[3] = MDefenseGrowth * 0.5f;
            currentJob.speedGrowth = SpeedGrowth * 250f / 150f;
            currentJob.healthGrowth[0] = HealthMinGrowth;
            currentJob.healthGrowth[1] = HealthMaxGrowth;
            currentJob.statsBase[0] = (float)AttackBase * 0.5f;
            currentJob.statsBase[2] = (float)MAttackBase * 0.5f;
            currentJob.statsBase[1] = (float)DefenseBase * 0.5f;
            currentJob.statsBase[3] = (float)MDefenseBase * 0.5f;
            currentJob.speedBase = (float)SpeedBase * 1.66666663f;
            currentJob.healthBase[0] = (float)HealthMinBase;
            currentJob.healthBase[1] = (float)HealthMaxBase;
            nextAbility = 0;
            nextJob++;
            jobs.Add(currentJob);
            jobsByClassName.Add(ClassName, currentJob);
        }
    }
}
