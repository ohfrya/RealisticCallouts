using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;
using System.Threading;
using FryaCallouts.Callouts;


namespace FryaCallouts
{
    public class Main: Plugin
    {

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin FryaCallouts" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by Frya has been initialized.");
            Game.LogTrivial("Go on duty to load FryaCallouts");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("FryaCallouts has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts();
                Game.Console.Print();
                Game.Console.Print("=============================================== RealisticCallouts by Frya ================================================");
                Game.Console.Print();
                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~RealisticCallouts", "by ~r~Frya", "~w~RealisticCallouts has been loaded! - ~r~v1.0.0.0");
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.Disturbance));
        }
            
        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
             
            }
            return null;
        }

        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
