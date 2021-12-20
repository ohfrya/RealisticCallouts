using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;

namespace FryaCallouts.Callouts
{
    [CalloutInfo("Disturbance", CalloutProbability.Medium)]

    public class Disturbance : Callout
    {

        private string[] Suspects = new string[] { "ig_andreas", "g_m_m_armlieut_01", "a_m_m_bevhills_01", "a_m_y_business_02", "s_m_m_gaffer_01", "a_f_y_golfer_01", "a_f_y_bevhills_01", "a_f_y_bevhills_04", "a_f_y_fitness_02" };
        private Ped Suspect;
        private Ped Player;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private Vector3 Location1;
        private Vector3 Location2;
        private Vector3 Location3;
        private Vector3 Location4;
        private int counter;
        private string maleFemale;
        private bool Scene1 = false;
        private bool Scene2 = false;
        private bool Scene3 = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            this.Location1 = new Vector3(917.1311f, -651.3591f, 57.86318f);
            this.Location2 = new Vector3(-1905.715f, 365.4793f, 93.58082f);
            this.Location3 = new Vector3(1661.571f, 4767.511f, 42.00745f);
            this.Location4 = new Vector3(1878.274f, 3922.46f, 33.06999f);

            Random random = new Random();
            List<string> list = new List<string>
            {
                "Location1",
                "Location2",
                "Location3",
                "Location4",
            };
            int num = random.Next(0, 4);
            if (list[num] == "Location1")
            {
                this.Spawnpoint = this.Location1;
            }
            if (list[num] == "Location2")
            {
                this.Spawnpoint = this.Location2;
            }
            if (list[num] == "Location3")
            {
                this.Spawnpoint = this.Location3;
            }
            if (list[num] == "Location4")
            {
                this.Spawnpoint = this.Location4;
            }
           
            Suspect = new Ped(Suspects[new Random().Next((int)Suspects.Length)], Spawnpoint, 0f);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            CalloutMessage = "Disturbance at City Hall";
            CalloutAdvisory = "Caller states that the individual is refusing to comply with City Hall Officials";
            CalloutPosition = Spawnpoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE_01 IN_OR_ON_POSITION", Spawnpoint);

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {


            Player = Game.LocalPlayer.Character;

            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~r~RealisticCallouts", "~y~Disturbance", "~g~Control: ~w~Approach the suspect & handle them accordingly");
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Yellow;
            SuspectBlip.IsRouteEnabled = true;


            if (Suspect.IsMale)
                maleFemale = "Sir";
            else
                maleFemale = "Madam";
            counter = 0;


            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
     
                    base.Process();

                    if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
                    {
                        Game.DisplayNotification("Press ~r~'Y' ~w~to interaction with the suspect");

                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            counter++;

                            if (counter == 1)
                            {
                                Game.DisplaySubtitle("~b~You: ~w~Excuse me " + maleFemale + ", what are you doing?", 5000);
                            }
                            if (counter == 2)
                            {
                                Game.DisplaySubtitle("~r~Suspect: ~w~What do you want?", 2000);
                            }
                            if (counter == 3)
                            {
                                Game.DisplaySubtitle("~b~You: ~w~We recieved a call about you shouting on the property.", 5000);
                            }
                            if (counter == 4)
                            {
                                Game.DisplaySubtitle("~r~Suspect: ~w~I'm yelling because they cant tell me to not record.", 5000);
                            }
                            if (counter == 5)
                            {
                                Game.DisplaySubtitle("~b~You: ~w~Alright, well you're being disorderly at the moment in front of City Hall.", 5000);
                            }
                            if (counter == 6)
                            {
                                Game.DisplaySubtitle("~r~Suspect: ~w~I am just exercising my right to freedom of speech.", 5000);
                            }
                            if (counter == 7)
                            {
                                Game.DisplaySubtitle("~b~You: ~w~That is fine but please stop yelling, especially in front of City Hall.", 5000);
                            }
                            if (counter == 8)
                            {
                                Game.DisplaySubtitle("~r~Suspect: ~w~ Whatever, go away.", 5000);
                            }

                            if (counter >= 9)
                            {
                                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~FryaCallouts", "~r~Disturbance at City Hall", "~w~Tend to the suspect ~y~accordingly");
                            }
                        }
                    }

                    if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
                    {
                        End();
                    }
            }

    public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            Game.LogTrivial("FryaCallouts - Disturbance has been Code 4'd.");

        }
    }
}