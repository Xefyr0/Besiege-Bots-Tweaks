using UnityEngine;

namespace BotFix
{
    public class PrefabModder : MonoBehaviour
    {
        public static BlockBehaviour BB;

        public static void ModAllPrefab()
        {
            if (Mod.UseModdedBlocks)
            {
                BesiegeBotsTweaks.DragFix.FixDrags();
                BesiegeBotsTweaks.MassFix.FixMasses();
                BesiegeBotsTweaks.BreakForceFix.FixBreakForces();
                //0
                PrefabMaster.GetBlock(BlockType.StartingBlock, out BB);

                //1
                PrefabMaster.GetBlock(BlockType.DoubleWoodenBlock, out BB);

                //2              
/*                PrefabMaster.GetBlock(BlockType.Wheel, out BB);
               if (BB.gameObject.GetComponent<Wheelfix>() == null)
                    BB.gameObject.AddComponent<Wheelfix>();
                PrefabMaster.GetStrippedBlock(BlockType.Wheel, out BB);
                if (BB.gameObject.GetComponent<Wheelfix>() == null)
                    BB.gameObject.AddComponent<Wheelfix>();
*/
                PrefabMaster.GetBlock(BlockType.Wheel, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                PrefabMaster.GetStrippedBlock(BlockType.Wheel, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();


                //3
                PrefabMaster.GetBlock(BlockType.MetalBlade, out BB);
                if (BB.gameObject.GetComponent<Bladefix>() == null)
                    BB.gameObject.AddComponent<Bladefix>();

                //4
                PrefabMaster.GetBlock(BlockType.Decoupler, out BB);

                //5
                PrefabMaster.GetBlock(BlockType.Hinge, out BB);
                if (BB.gameObject.GetComponent<Hfix>() == null)
                    BB.gameObject.AddComponent<Hfix>();
                if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                    BB.gameObject.AddComponent<FrictionSlider>();

                //6
                PrefabMaster.GetBlock(BlockType.MetalBall, out BB);
                if (BB.gameObject.GetComponent<CanonFix>() == null)
                    BB.gameObject.AddComponent<CanonFix>();


                //7
                //PrefabMaster.GetBlock(BlockType.Brace, out BB);


                //8
                //PrefabMaster.GetBlock(BlockType.Unused, out BB);


                //9
                PrefabMaster.GetBlock(BlockType.Spring, out BB);
                if (BB.gameObject.GetComponent<WinchFix>() == null)
                    BB.gameObject.AddComponent<WinchFix>();
                PrefabMaster.GetStrippedBlock(BlockType.Spring, out BB);
                if (BB.gameObject.GetComponent<WinchFix>() == null)
                    BB.gameObject.AddComponent<WinchFix>();


                //10
                PrefabMaster.GetBlock(BlockType.WoodenPanel, out BB);
                if (BB.gameObject.GetComponent<WPFix>() == null)
                    BB.gameObject.AddComponent<WPFix>();

                //11
                PrefabMaster.GetBlock(BlockType.Cannon, out BB);
                if (BB.gameObject.GetComponent<CanonFix>() == null)
                    BB.gameObject.AddComponent<CanonFix>();

                //12
                //PrefabMaster.GetBlock(BlockType.ScalingBlock, out BB);
               
                //13
                PrefabMaster.GetBlock(BlockType.SteeringBlock, out BB);

                //14
                PrefabMaster.GetBlock(BlockType.FlyingBlock, out BB);

                //15
                PrefabMaster.GetBlock(BlockType.SingleWoodenBlock, out BB);

                //16
                PrefabMaster.GetBlock(BlockType.Suspension, out BB);
                if (BB.gameObject.GetComponent<Suspensionzcript>() == null)
                    BB.gameObject.AddComponent<Suspensionzcript>();
                PrefabMaster.GetStrippedBlock(BlockType.Suspension, out BB);
                if (BB.gameObject.GetComponent<Suspensionzcript>() == null)
                    BB.gameObject.AddComponent<Suspensionzcript>();


                //17
                PrefabMaster.GetBlock(BlockType.CircularSaw, out BB);
                if (BB.gameObject.GetComponent<Winchfixer>() == null)
                    BB.gameObject.AddComponent<Winchfixer>();
                //BB.transform.Find("Joint").localScale = new Vector3(0.9f,0.2f,0.9f);
                BB.transform.GetChild(9).localPosition = new Vector3(0f, 0f, -0.1f);
                BB.transform.GetChild(10).localPosition = new Vector3(0f, 0f, -0.1f);

                //18
                PrefabMaster.GetBlock(BlockType.Piston, out BB);
                if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                    BB.gameObject.AddComponent<FrictionSlider>();

                //19
                PrefabMaster.GetBlock(BlockType.Swivel, out BB);
                if (BB.gameObject.GetComponent<Hfix>() == null)
                    BB.gameObject.AddComponent<Hfix>();

                //20
                PrefabMaster.GetBlock(BlockType.Spike, out BB);
                if (BB.gameObject.GetComponent<Bladefix>() == null)
                    BB.gameObject.AddComponent<Bladefix>();

                //21
                PrefabMaster.GetBlock(BlockType.Flamethrower, out BB);
                if (BB.gameObject.GetComponent<FlameTFix>() == null)
                    BB.gameObject.AddComponent<FlameTFix>();
                PrefabMaster.GetStrippedBlock(BlockType.Flamethrower, out BB);
                if (BB.gameObject.GetComponent<FlameTFix>() == null)
                    BB.gameObject.AddComponent<FlameTFix>();

                //22
                PrefabMaster.GetBlock(BlockType.SpinningBlock, out BB);
                if (BB.gameObject.GetComponent<SpinBfix>() == null)
                    BB.gameObject.AddComponent<SpinBfix>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();
                PrefabMaster.GetStrippedBlock(BlockType.SpinningBlock, out BB);
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();

                //23
                //PrefabMaster.GetBlock(BlockType.Bomb, out BB);

                //24
                PrefabMaster.GetBlock(BlockType.ArmorPlateSmall, out BB);

                //25
                PrefabMaster.GetBlock(BlockType.Wing, out BB);
                if (BB.gameObject.GetComponent<WFix>() == null)
                    BB.gameObject.AddComponent<WFix>();
                if (BB.gameObject.GetComponent<Winchfixer>() == null)
                    BB.gameObject.AddComponent<Winchfixer>();

                //26
                PrefabMaster.GetBlock(BlockType.Propeller, out BB);
                if (BB.gameObject.GetComponent<Winchfixer>() == null)
                    BB.gameObject.AddComponent<Winchfixer>();

                //27
                PrefabMaster.GetBlock(BlockType.Grabber, out BB);
                if (BB.gameObject.GetComponent<Grabberfix>() == null)
                    BB.gameObject.AddComponent<Grabberfix>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();
                 PrefabMaster.GetStrippedBlock(BlockType.Grabber, out BB);
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();

                //28
                PrefabMaster.GetBlock(BlockType.SteeringHinge, out BB);

                //29
                PrefabMaster.GetBlock(BlockType.ArmorPlateRound, out BB);
                if (BB.gameObject.GetComponent<Platefix>() == null)
                    BB.gameObject.AddComponent<Platefix>();

                //30
                //PrefabMaster.GetBlock(BlockType.BombHolder, out BB);
                
                //31
                //PrefabMaster.GetBlock(BlockType.FlameBall, out BB);
                
                //32
                PrefabMaster.GetBlock(BlockType.ArmorPlateLarge, out BB);
                if (BB.gameObject.GetComponent<Platefix>() == null)
                    BB.gameObject.AddComponent<Platefix>();
                //BB.gameObject.GetComponent<Rigidbody>().mass = 0.5f;

                //33
                PrefabMaster.GetBlock(BlockType.Plow, out BB);
                if (BB.gameObject.GetComponent<Plowfix>() == null)
                    BB.gameObject.AddComponent<Plowfix>();

                //34
                PrefabMaster.GetBlock(BlockType.WingPanel, out BB);
                if (BB.gameObject.GetComponent<WFix>() == null)
                    BB.gameObject.AddComponent<WFix>();

                //35
                PrefabMaster.GetBlock(BlockType.Ballast, out BB);
                if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                    BB.gameObject.AddComponent<FrictionSlider>();

                //36
                //PrefabMaster.GetBlock(BlockType.Boulder, out BB);

                //37
                PrefabMaster.GetBlock(BlockType.HalfPipe, out BB);
                if (BB.gameObject.GetComponent<Plowfix>() == null)
                    BB.gameObject.AddComponent<Plowfix>();

                //38
                PrefabMaster.GetBlock(BlockType.CogMediumUnpowered, out BB);
               
                //39
                PrefabMaster.GetBlock(BlockType.CogMediumPowered, out BB);
                if (BB.gameObject.GetComponent<Cogfix>() == null)
                    BB.gameObject.AddComponent<Cogfix>();

                //40
                PrefabMaster.GetBlock(BlockType.WheelUnpowered, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                PrefabMaster.GetStrippedBlock(BlockType.WheelUnpowered, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();

                //41
                PrefabMaster.GetBlock(BlockType.WoodenPole, out BB);

                //42
                //PrefabMaster.GetBlock(BlockType.Slider, out BB);

                //43
                //PrefabMaster.GetBlock(BlockType.Balloon, out BB);
 
                //44
                PrefabMaster.GetBlock(BlockType.BallJoint, out BB);
                if (BB.gameObject.GetComponent<Hfix>() == null)
                    BB.gameObject.AddComponent<Hfix>();

                //45
                PrefabMaster.GetBlock(BlockType.RopeWinch, out BB);
                if (BB.gameObject.GetComponent<WinchFix>() == null)
                    BB.gameObject.AddComponent<WinchFix>();
                PrefabMaster.GetStrippedBlock(BlockType.RopeWinch, out BB);
                if (BB.gameObject.GetComponent<WinchFix>() == null)
                    BB.gameObject.AddComponent<WinchFix>();

                //46
                PrefabMaster.GetBlock(BlockType.LargeWheel, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                PrefabMaster.GetStrippedBlock(BlockType.LargeWheel, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();

                //47
                PrefabMaster.GetBlock(BlockType.Torch, out BB);
                if (BB.gameObject.GetComponent<CanonFix>() == null)
                    BB.gameObject.AddComponent<CanonFix>();
                if (BB.gameObject.GetComponent<TurchFix>() == null)
                    BB.gameObject.AddComponent<TurchFix>();

                //48
                PrefabMaster.GetBlock(BlockType.Drill, out BB);
                if (BB.gameObject.GetComponent<Winchfixer>() == null)
                    BB.gameObject.AddComponent<Winchfixer>();
                /*
                BoxCollider[] BCS = BB.gameObject.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider BC in BCS)
                {
                    BC.enabled = false;
                    //Debug.Log(BC);
                }
                GameObject NDC1 = new GameObject();
                NDC1.name = "NoDamageCollider1";
                BoxCollider NBC1 = NDC1.AddComponent<BoxCollider>();
                NDC1.transform.SetParent(BB.gameObject.transform);
                NDC1.transform.position = new Vector3(0f, 0f, 0.6f);
                NDC1.transform.eulerAngles = new Vector3(0f, 0f, 343.5f);
                NDC1.transform.localScale = new Vector3(1.3f, 1.1f, 0.5f);
                NDC1.transform.localPosition = new Vector3(0f, 0f, 0.6f);
                NBC1.size = new Vector3(0.9f, 0.9f, 0.9f);
                NBC1.isTrigger = false;

                GameObject NDC2 = new GameObject();
                NDC2.name = "NoDamageCollider2";
                BoxCollider NBC2 = NDC2.AddComponent<BoxCollider>();
                NDC2.transform.SetParent(BB.gameObject.transform);
                NDC2.transform.position = new Vector3(0f, 0f, 1.5f);
                NDC2.transform.eulerAngles = new Vector3(0f, 0f, 45f);
                NDC2.transform.localScale = new Vector3(0.7f, 0.7f, 1.7f);
                NDC2.transform.localPosition = new Vector3(0f, 0f, 1.5f);
                NBC2.size = new Vector3(0.9f, 0.9f, 0.9f);
                NBC2.isTrigger = false;

                GameObject NDC3 = new GameObject();
                NDC3.name = "NoDamageCollider3";
                BoxCollider NBC3 = NDC3.AddComponent<BoxCollider>();
                NDC3.transform.SetParent(BB.gameObject.transform);
                NDC3.transform.position = new Vector3(0f, 0f, 2.5f);
                NDC3.transform.eulerAngles = new Vector3(45.1f, 352.6f, 17.7f);
                NDC3.transform.localScale = new Vector3(0.3f, 0.3f, 0.6f);
                NDC3.transform.localPosition = new Vector3(0f, 0f, 2.5f);
                NBC3.size = new Vector3(0.9f, 0.9f, 0.9f);
                NBC3.isTrigger = false;
                */



                //49
                PrefabMaster.GetBlock(BlockType.GripPad, out BB);
                if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                    BB.gameObject.AddComponent<FrictionSlider>();

                //50
                PrefabMaster.GetBlock(BlockType.SmallWheel, out BB);

                //51
                PrefabMaster.GetBlock(BlockType.CogLargeUnpowered, out BB);
                if (BB.gameObject.GetComponent<Wheelfix>() == null)
                    BB.gameObject.AddComponent<Wheelfix>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();
                PrefabMaster.GetStrippedBlock(BlockType.CogLargeUnpowered, out BB);
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();

                //52
                //PrefabMaster.GetBlock(BlockType.Unused3, out BB);

                //53
                PrefabMaster.GetBlock(BlockType.ShrapnelCannon, out BB);
                if (BB.gameObject.GetComponent<CanonFix>() == null)
                    BB.gameObject.AddComponent<CanonFix>();

                //54
                //PrefabMaster.GetBlock(BlockType.Grenade, out BB);

                //55
                PrefabMaster.GetBlock(BlockType.SmallPropeller, out BB);
                if (BB.gameObject.GetComponent<Winchfixer>() == null)
                    BB.gameObject.AddComponent<Winchfixer>();

                //56
                PrefabMaster.GetBlock(BlockType.WaterCannon, out BB);
                if (BB.gameObject.GetComponent<WCfix>() == null)
                    BB.gameObject.AddComponent<WCfix>();

                //57
                //PrefabMaster.GetBlock(BlockType.Pin, out BB);
                

                //58
                //PrefabMaster.GetBlock(BlockType.CameraBlock, out BB);
                

                //59
                //PrefabMaster.GetBlock(BlockType.Rocket, out BB);
                

                //60
                PrefabMaster.GetBlock(BlockType.LargeWheelUnpowered, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();                
                PrefabMaster.GetStrippedBlock(BlockType.LargeWheelUnpowered, out BB);
                if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();

                //61
                //PrefabMaster.GetBlock(BlockType.Crossbow, out BB);
                

                //62
                //PrefabMaster.GetBlock(BlockType.Vacuum, out BB);
                

                //63
                PrefabMaster.GetBlock(BlockType.Log, out BB);
                if (BB.gameObject.GetComponent<Logfix>() == null)
                    BB.gameObject.AddComponent<Logfix>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                     BB.gameObject.AddComponent<SpinnerSound>();
                PrefabMaster.GetStrippedBlock(BlockType.Log, out BB);
                if (BB.gameObject.GetComponent<Logfix>() == null)
                    BB.gameObject.AddComponent<Logfix>();
                if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                    BB.gameObject.AddComponent<SpinnerSound>();

                //64
                //PrefabMaster.GetBlock(BlockType.Magnet, out BB);

                //65
                //PrefabMaster.GetBlock(BlockType.Sensor, out BB);

                //66
                //PrefabMaster.GetBlock(BlockType.Timer, out BB);

                //67
                //PrefabMaster.GetBlock(BlockType.Altimeter, out BB);

                //68
                //PrefabMaster.GetBlock(BlockType.LogicGate, out BB);

                //69
                //PrefabMaster.GetBlock(BlockType.Anglometer, out BB);

                //70
                //PrefabMaster.GetBlock(BlockType.Speedometer, out BB);

                //71 BuildNode
                //PrefabMaster.GetBlock(BlockType.Speedometer, out BB);

                //72 BuildEdge
                //PrefabMaster.GetBlock(BlockType.Speedometer, out BB);

                //73 BuildSurface
                PrefabMaster.GetBlock(BlockType.BuildSurface, out BB);
                if (BB.gameObject.GetComponent<BuildSurfaceFix>() == null)
                    BB.gameObject.AddComponent<BuildSurfaceFix>();
            }
        }
    }
}