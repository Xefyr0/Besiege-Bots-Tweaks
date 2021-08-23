using UnityEngine;
using Modding;

namespace BotFix
{
    public class BBMUI : MonoBehaviour
    {
        private ModKey keykey = ModKeys.GetKey("keykey");
        private readonly int windowID = ModUtility.GetWindowId();
        private Rect windowRect = new Rect(15f, 100f, 190f, 300f); //260
        public bool mute = true;
        private string mutetext;
        private string MBtext;
        private string Betatext;
        private string gtext;
        public float oload;
        public float temp;

        private readonly GUIStyle style = new GUIStyle()        
        {
            normal = { textColor = Color.white},
            alignment = TextAnchor.MiddleLeft,
            active = { background = Texture2D.whiteTexture,
            textColor = Color.black},
            margin = { top = 5},
            fontSize = 15
        };

        private readonly GUIStyle style1 = new GUIStyle()
        {
            normal = { textColor = Color.white},
            alignment = TextAnchor.MiddleRight,
            active = { background = Texture2D.whiteTexture,
            textColor = Color.black},
            margin = { top = 5},
            fontSize = 15
        };

        private bool ShowGUI;
        private BBinfo BBinfo;
        private event Click VelocityButtonClickEvent;
        private event Click MuteEvent;
        private event Click BMuteEvent;
        private event Click BetaEvent;
        private event Click MBEvent;

        private void Start()
        {
            ShowGUI = true;
            InitEvent();
            GameObject gameObject2 = new GameObject("BBinfo Yeet");
            gameObject2.transform.SetParent(transform);
            BBinfo = gameObject2.AddComponent<BBinfo>();          
        }

        private void InitEvent()
        {
            VelocityButtonClickEvent += (Click)(() => { });
            MuteEvent += (Click)(() => { });
            BMuteEvent += (Click)(() => { });
            BetaEvent += (Click)(() => { });
            MBEvent += (Click)(() => { });
        }

        private void Update()
        { 
            if (!keykey.IsPressed)
                return;
            ShowGUI = !ShowGUI;
        }

        private void OnGUI()
        {
            
			if (StatMaster.hudHidden)
				return;
            windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(TimerWindow), "BBMod Tools");
        }

        private void TimerWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Block:");
            GUILayout.Label(BBinfo.yeet);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Height:");
            GUILayout.Label(string.Format("{0} {1}", BBinfo.Position.y.ToString("#0.00"), "m"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Velocity:"))
            {
                BBinfo.ChangedVelocityUnit();
                VelocityButtonClickEvent();
            }
            GUILayout.Label(string.Format("{0} {1}", (object)BBinfo.Velocity.magnitude.ToString("#0.00"), (object)BBinfo.velocityUnit));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Distance:");
            GUILayout.Label(string.Format("{0:N2}", (object)(float)((double)BBinfo.Distance / 1000.0)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            float num = BBinfo.Acceleration;
            GUILayout.Label("G-Force:");
            string format = "{0}(g)";
            num = BBinfo.Overload;
            string str = num.ToString("#0.00");
            GUILayout.Label(string.Format(format, (object)str));
            GUILayout.EndHorizontal();

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Rot-Velocity:");
                GUILayout.Label(string.Format("{0} {1}", BBinfo.angval.ToString("#0.00"), "RPM"));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (Mod.UseModdedBlocks == true)
                {
                    gtext = "Modblocks ON";
                }
                else
                {
                    gtext = "Modblocks OFF";
                }

                if (GUILayout.Button(gtext))
                {
                    BMute();
                    BMuteEvent();
                    PrefabModder.ModAllPrefab();
                    //PrefabModder.DestroyAllModPrefab();
                    
                }
                GUILayout.EndHorizontal();

            }

            GUILayout.BeginHorizontal();

            if (mute == true)
            {
                mutetext = "Disable Higher G";
            }
            else
            {
                mutetext = "Enable Higher G";
            }

            if (GUILayout.Button(mutetext))
            {
                Mute();
                MuteEvent();
                
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (Mod.BetaMode == true)
            {
                Betatext = "Beta ON";
            }
            else
            {
                Betatext = "Beta OFF";
            }

            if (GUILayout.Button(Betatext))
            {
                Betamode();
                BetaEvent();
                PrefabModder.ModAllPrefab();
            }
            GUILayout.EndHorizontal();
            ////////////////////////////////////////////////////////////////////////
            GUILayout.BeginHorizontal();
            if (Mod.ModbotMode == true)
            {
                MBtext = "Modbot ON";
            }
            else
            {
                MBtext = "Modbot OFF";
            }

            if (GUILayout.Button(MBtext))
            {
                Modbotmode();
                MBEvent();
                PrefabModder.ModAllPrefab();
            }
            GUILayout.EndHorizontal();
            /*
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            GUILayout.BeginHorizontal();
            float num2 = oload;
            GUILayout.Label("G-Force:");
            string format2 = "{0}(G2)";
            string st2 = num2.ToString("#0.00");
            GUILayout.Label(string.Format(format2, st2));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            float num3 = temp;
            GUILayout.Label("Temp.");
            string format3 = "{0}(C)";
            string st3 = num3.ToString("#0.00");
            GUILayout.Label(string.Format(format3, st3));
            GUILayout.EndHorizontal();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///*/
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        public void Mute()
        {
            if (mute)
            {
                mute = false;
                Physics.gravity = new Vector3(Physics.gravity.x, -32.81f, Physics.gravity.z);
            }
            else
            {
                mute = true;
                Physics.gravity = new Vector3(Physics.gravity.x, -50f, Physics.gravity.z);               
            }
        }

        public void BMute()
        {
            if (Mod.UseModdedBlocks) Mod.UseModdedBlocks = false;
            else Mod.UseModdedBlocks = true;
        }

        public void Betamode()
        {
            if (Mod.BetaMode) Mod.BetaMode = false;
            else Mod.BetaMode = true;
        }

        public void Modbotmode()
        {
            if (Mod.ModbotMode) Mod.ModbotMode = false;
            else Mod.ModbotMode = true;
        }
    }
}
