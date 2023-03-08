/*
 * PrefabModder.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 */

using BesiegeBotsTweaks;
using UnityEngine;
using System.Collections.Generic;

namespace BotFix
{
    public class PrefabModder : MonoBehaviour
    {
        /*
        The very first change BBM made, and most common change BBM makes currently is to change the durability of certain blocks.
        This is done by changing the breakForce and breakTorque thresholds of Joints.
        */
        private static void TweakBreakForces(BlockType type)
        {
            Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
            Joint joint = BPI.GameObject.GetComponent<Joint>();
            TriggerSetJoint2 secondJoint = BPI.GameObject.GetComponentInChildren<TriggerSetJoint2>();
            StrengthenTreads ST = BPI.GameObject.GetComponent<StrengthenTreads>();
            if (joint)
            {
                switch (type)
                {
                    case BlockType.FlyingBlock:
                        joint.breakForce = 20000;
                        joint.breakTorque = 20000;
                        break;
                    case BlockType.Ballast:
                    case BlockType.Decoupler:
                    case BlockType.SteeringHinge:
                    case BlockType.Hinge:
                    case BlockType.BallJoint:
                    case BlockType.Swivel:
                        joint.breakForce = 30000;
                        joint.breakTorque = 30000;
                        //Hinges have a specialized component made to strengthen hinge-based treads.
                        //This changes the break threshold values of that component to strengthen treads further.
                        if (ST != null)
                        {
                            ST.breakForce = 50000;
                            ST.breakTorque = 50000;
                        }
                        break;
                    case BlockType.Suspension:
                    case BlockType.Piston:
                        joint.breakForce = 35000;
                        joint.breakTorque = 35000;
                        break;
                    case BlockType.Flamethrower:
                    case BlockType.Wing:
                        joint.breakForce = 60000;
                        joint.breakTorque = 60000;
                        break;
                    case BlockType.WingPanel:
                        joint.breakForce = 40000;
                        joint.breakTorque = 40000;
                        break;
                    case BlockType.SteeringBlock:
                    case BlockType.SmallWheel:
                    case BlockType.Wheel:
                    case BlockType.WheelUnpowered:
                    case BlockType.LargeWheelUnpowered:
                        joint.breakForce = 60000;
                        joint.breakTorque = 60000;
                        break;
                    case BlockType.LargeWheel:
                        joint.breakForce = 70000;
                        joint.breakTorque = 70000;
                        break;
                    case BlockType.MetalBlade:
                    case BlockType.Spike:
                        joint.breakForce = 80000;
                        joint.breakTorque = 80000;
                        break;
                    case BlockType.SpinningBlock:
                        joint.breakForce = 100000;
                        joint.breakTorque = 100000;
                        break;
                    case BlockType.CogMediumPowered:
                    case BlockType.CircularSaw:
                        joint.breakForce = 90000;
                        joint.breakTorque = 90000;
                        break;
                    case BlockType.WoodenPanel:
                        joint.breakForce = Mathf.Infinity;
                        joint.breakTorque = Mathf.Infinity;
                        break;
                    //The following blocks have 2 joints, all of which are modified.
                    case BlockType.SingleWoodenBlock:
                        joint.breakForce = 55000;
                        joint.breakTorque = 55000;
                        if (secondJoint)
                        {
                            secondJoint.jointToCopy.breakForce = 55000;
                            secondJoint.jointToCopy.breakTorque = 55000;
                        }
                        break;
                    case BlockType.DoubleWoodenBlock:
                        joint.breakForce = 50000;
                        joint.breakTorque = 50000;
                        if (secondJoint)
                        {
                            secondJoint.jointToCopy.breakForce = 50000;
                            secondJoint.jointToCopy.breakTorque = 50000;
                        }
                        break;
                    case BlockType.WoodenPole:
                        joint.breakForce = 40000;
                        joint.breakTorque = 40000;
                        if (secondJoint)
                        {
                            secondJoint.jointToCopy.breakForce = 40000;
                            secondJoint.jointToCopy.breakTorque = 40000;
                        }
                        break;
                    case BlockType.BuildSurface:
                        //Change breakforce of each material.
                        BuildSurface BB = ((BuildSurface)BPI.InternalObject.blockBehaviour);
                        BuildSurface.SurfaceMaterialType[] buildSurfaceMats = { BB.wood, BB.glass, BB.wing, BB.metal };
                        foreach (BuildSurface.SurfaceMaterialType surfaceType in buildSurfaceMats)
                        {
                            surfaceType.breakImpactThreshold = 500000f;
                            surfaceType.jointBaseBreakForce = 50000f;
                            surfaceType.jointBreakForceScaler = Mathf.Infinity;
                            surfaceType.jointBreakTorqueScaler = Mathf.Infinity;
                        }
                        break;
                }
            }
        }

        /*
        Before the existence of BBMod, Spinners and other high-speed weapons had low speed limits
        and needed many braces to achieve passable speeds.
        Thus, Doyle decreased the drag of blocks' Rigidbodies and increased their maxAngularVelocitys.
        The Component added by this function does the same thing, but consolidated into one location.
        */
        private static void TweakDrags(BlockType type)
        {
            Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
            //Gets the Rigidbody of each block, and if it isn't null then the drags & maxAngularVelocity are modified based on a switch statement of the block type.
            Rigidbody RB = BPI.GameObject.GetComponent<Rigidbody>();
            if (RB != null)
            {
                switch (type)
                {
                    case BlockType.CogMediumUnpowered:
                    case BlockType.GripPad:
                    case BlockType.Log:
                    case BlockType.StartingBlock:
                    case BlockType.BuildSurface:
                    case BlockType.Ballast:
                    case BlockType.Swivel:
                    case BlockType.BallJoint:
                    case BlockType.Spike:
                    case BlockType.MetalBlade:
                    case BlockType.Cannon:
                    case BlockType.Torch:
                    case BlockType.MetalBall:
                    case BlockType.ShrapnelCannon:
                    case BlockType.Decoupler:
                    case BlockType.DoubleWoodenBlock:
                    case BlockType.Hinge:
                    case BlockType.Plow:
                    case BlockType.HalfPipe:
                    case BlockType.SteeringBlock:
                    case BlockType.SteeringHinge:
                    case BlockType.Suspension:
                    case BlockType.WheelUnpowered:
                    case BlockType.LargeWheelUnpowered:
                    case BlockType.CogLargeUnpowered:
                    case BlockType.WoodenPole:
                    case BlockType.SingleWoodenBlock:
                    case BlockType.WoodenPanel:
                    case BlockType.Wheel:
                    case BlockType.LargeWheel:
                    case BlockType.WaterCannon:
                    case BlockType.ArmorPlateSmall:
                    case BlockType.ArmorPlateLarge:
                    case BlockType.ArmorPlateRound:
                        RB.drag = 0f;
                        RB.angularDrag = 0f;
                        RB.maxAngularVelocity = 100;
                        break;
                    case BlockType.FlyingBlock:
                    case BlockType.SpinningBlock:
                    case BlockType.CircularSaw:
                    case BlockType.Drill:
                        RB.drag = 0f;
                        RB.angularDrag = 0f;
                        break;
                    //Grabbers have nonzero drag because they're used so often in spinners.
                    case BlockType.Grabber:
                        RB.drag = 0.01f;
                        RB.angularDrag = 0f;
                        RB.maxAngularVelocity = 100;
                        break;
                }
            }
        }

        /*
        In vanilla Besiege, Plows are the only viable wedging component, and there isn't really anything useful as a skidplate.
        Additionally, the high friction of blocks with an invincible connection render them more vulnerable to explosions.
        This method exists to modify the friction of some blocks' prefabs, solving the above problems.
        */
        internal static void TweakFriction(BlockType type)
        {
            //Gets the colliders of each block, and if there is at least one then friction is modified based on a switch statement of the block type.
            Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
            GameObject GO = BPI.InternalObject.gameObject;
            Collider[] colliders = GO.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                switch (type)
                {
                    case BlockType.MetalBlade:
                    case BlockType.Spike:
                    case BlockType.ArmorPlateLarge:
                    //case BlockType.ArmorPlateSmall:
                    case BlockType.ArmorPlateRound:
                    case BlockType.WoodenPanel:
                    case BlockType.BuildSurface:
                        collider.material.dynamicFriction = 0.1f;
                        collider.material.staticFriction = 0.1f;
                        collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                        break;
                    case BlockType.Grabber: //Grabbers and spinblocks have even lower friction because of their inherent explosion risk
                    case BlockType.SpinningBlock:
                        collider.material.dynamicFriction = 0.05f;
                        collider.material.staticFriction = 0.05f;
                        collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                        break;
                }
            }
        }

        /*
        Some blocks are too heavy or light for their appearance or usage in a Besiege Bot.
        This function exists to remedy the mass of some blocks' prefabs given their BlockType.
        */
        private static void TweakMass(BlockType type)
        {
            //Gets the Rigidbody of each block, and if it isn't null then the mass is modified based on a switch statement of the block type.
            Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
            GameObject GO = BPI.InternalObject.gameObject;
            Rigidbody RB = GO.GetComponent<Rigidbody>();
            switch (type)
            {
                case BlockType.MetalBlade:
                case BlockType.Spike:
                case BlockType.Swivel:
                    RB.mass = 0.6f;
                    break;
                case BlockType.Grabber:
                    RB.mass = 0.7f;
                    break;
                case BlockType.SteeringHinge:
                    RB.mass = 0.4f;
                    break;
                case BlockType.WoodenPanel:
                    RB.mass = 0.5f;
                    break;
                case BlockType.ArmorPlateLarge:
                    RB.mass = 0.5f;
                    break;
            }
        }

        /*
        In vanilla Besiege, Blocks with certain DamageTypes cut Rope & Winches.
        This is undesirable, as ropes are sometimes used to power flippers in a manner similar to pneumatics.
        Thus, this method exists to modify the myDamageType of each of these blocks' prefabs to Blunt,
        thereby rendering those blocks unable to cut ropes.
        */
        private static readonly BlockType[] toBlunten = {BlockType.MetalBlade, BlockType.CircularSaw, BlockType.Spike, BlockType.Flamethrower, BlockType.Wing, BlockType.Propeller, BlockType.SmallPropeller, BlockType.Drill, BlockType.MetalBall};
        private static void BluntenBlocks()
        {
            //Gets the prefab of each block, and modifies the DamageType to not cut winches.
            foreach (BlockType type in toBlunten)
                Modding.Blocks.BlockPrefabInfo.GetOfficial(type).InternalObject.myDamageType = DamageType.Blunt;
        }

        internal static void ModAllPrefab()
        {
            BlockBehaviour BB;

            //General Static Block Prefab modifications
            BluntenBlocks();
            foreach (BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                TweakBreakForces(type);
                TweakFriction(type);
                TweakMass(type);

                //maxAngularVelocity is set sometime after simulation starts,
                //so we need to apply a component to EVERY BLOCK WE TWEAK
                PrefabMaster.GetBlock(type, out BB);
                if (BB.gameObject.GetComponent<DragTweaks>() == null)
                    BB.gameObject.AddComponent<DragTweaks>();
            }

            /*** Block-Specific modifications ***/

            //0
            //PrefabMaster.GetBlock(BlockType.StartingBlock, out BB);

            //1
            PrefabMaster.GetBlock(BlockType.DoubleWoodenBlock, out BB);
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();
            PrefabMaster.GetStrippedBlock(BlockType.DoubleWoodenBlock, out BB);
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();

            //2              
            PrefabMaster.GetBlock(BlockType.Wheel, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();
            if (BB.gameObject.GetComponent<RealisticMotorTorque>() == null)
                BB.gameObject.AddComponent<RealisticMotorTorque>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();
            /*
            PrefabMaster.GetStrippedBlock(BlockType.Wheel, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();
            */

            //3
            PrefabMaster.GetBlock(BlockType.MetalBlade, out BB);
            if (BB.gameObject.GetComponent<InvincibleJointToggle>() == null)
                BB.gameObject.AddComponent<InvincibleJointToggle>();

            //4
            //PrefabMaster.GetBlock(BlockType.Decoupler, out BB);

            //5
            PrefabMaster.GetBlock(BlockType.Hinge, out BB);
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();

            //6
            //PrefabMaster.GetBlock(BlockType.MetalBall, out BB);

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
            //PrefabMaster.GetBlock(BlockType.WoodenPanel, out BB);

            //11
            PrefabMaster.GetBlock(BlockType.Cannon, out BB);
            {
                ((CanonBlock)BB).randomDelay = 0f;
            }

            //12
            //PrefabMaster.GetBlock(BlockType.ScalingBlock, out BB);

            //13
            PrefabMaster.GetBlock(BlockType.SteeringBlock, out BB);
            //This has to be done here, instead of Awake(). Glad I got it first try.
            ((SteeringWheel)BB).allowLimits = true;
            //The other parts have to be done after SteeringWheel's Awake though.
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();


            //14
            PrefabMaster.GetBlock(BlockType.FlyingBlock, out BB);
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();


            //15
            //PrefabMaster.GetBlock(BlockType.SingleWoodenBlock, out BB);

            //16
            PrefabMaster.GetBlock(BlockType.Suspension, out BB);
            if (BB.gameObject.GetComponent<Suspensionzcript>() == null)
                BB.gameObject.AddComponent<Suspensionzcript>();
            PrefabMaster.GetStrippedBlock(BlockType.Suspension, out BB);
            if (BB.gameObject.GetComponent<Suspensionzcript>() == null)
                BB.gameObject.AddComponent<Suspensionzcript>();


            //17
            PrefabMaster.GetBlock(BlockType.CircularSaw, out BB);
            BB.transform.GetChild(9).localPosition = new Vector3(0f, 0f, -0.1f);
            BB.transform.GetChild(10).localPosition = new Vector3(0f, 0f, -0.1f);
            if (BB.gameObject.GetComponent<RealisticMotorTorque>() == null)
                BB.gameObject.AddComponent<RealisticMotorTorque>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();

            //18
            PrefabMaster.GetBlock(BlockType.Piston, out BB);
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();

            //19
            //PrefabMaster.GetBlock(BlockType.Swivel, out BB);

            //20
            PrefabMaster.GetBlock(BlockType.Spike, out BB);
            if (BB.gameObject.GetComponent<InvincibleJointToggle>() == null)
                BB.gameObject.AddComponent<InvincibleJointToggle>();

            //21
            PrefabMaster.GetBlock(BlockType.Flamethrower, out BB);
            if (BB.gameObject.GetComponent<FlameTFix>() == null)
                BB.gameObject.AddComponent<FlameTFix>();
            PrefabMaster.GetStrippedBlock(BlockType.Flamethrower, out BB);
            if (BB.gameObject.GetComponent<FlameTFix>() == null)
                BB.gameObject.AddComponent<FlameTFix>();

            //22
            PrefabMaster.GetBlock(BlockType.SpinningBlock, out BB);
            if (BB.gameObject.GetComponent<InvincibleJointToggle>() == null)
                BB.gameObject.AddComponent<InvincibleJointToggle>();
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();
            if (BB.gameObject.GetComponent<RealisticMotorTorque>() == null)
                BB.gameObject.AddComponent<RealisticMotorTorque>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();
            PrefabMaster.GetStrippedBlock(BlockType.SpinningBlock, out BB);
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();

            //23
            //PrefabMaster.GetBlock(BlockType.Bomb, out BB);

            //24
            //PrefabMaster.GetBlock(BlockType.ArmorPlateSmall, out BB);

            //25
            PrefabMaster.GetBlock(BlockType.Wing, out BB);
            if (BB.gameObject.GetComponent<AxialDragToggle>() == null)
                BB.gameObject.AddComponent<AxialDragToggle>();


            //26
            //PrefabMaster.GetBlock(BlockType.Propeller, out BB);

            //27
            PrefabMaster.GetBlock(BlockType.Grabber, out BB);
            if (BB.gameObject.GetComponent<Grabberfix>() == null)
                BB.gameObject.AddComponent<Grabberfix>();
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();
            if (BB.gameObject.GetComponent<AntiPhysXExplosion>() == null)
                BB.gameObject.AddComponent<AntiPhysXExplosion>();
            PrefabMaster.GetStrippedBlock(BlockType.Grabber, out BB);
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();

            //28
            PrefabMaster.GetBlock(BlockType.SteeringHinge, out BB);
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();

            //29
            //PrefabMaster.GetBlock(BlockType.ArmorPlateRound, out BB);

            //30
            //PrefabMaster.GetBlock(BlockType.BombHolder, out BB);

            //31
            //PrefabMaster.GetBlock(BlockType.FlameBall, out BB);

            //32
            //PrefabMaster.GetBlock(BlockType.ArmorPlateLarge, out BB);

            //33
            //PrefabMaster.GetBlock(BlockType.Plow, out BB);

            //34
            PrefabMaster.GetBlock(BlockType.WingPanel, out BB);
            if (BB.gameObject.GetComponent<AxialDragToggle>() == null)
                BB.gameObject.AddComponent<AxialDragToggle>();

            //35
            PrefabMaster.GetBlock(BlockType.Ballast, out BB);
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();

            //36
            PrefabMaster.GetBlock(BlockType.Boulder, out BB);
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();

            //37
            //PrefabMaster.GetBlock(BlockType.HalfPipe, out BB);

            //38
            //PrefabMaster.GetBlock(BlockType.CogMediumUnpowered, out BB);

            //39
            PrefabMaster.GetBlock(BlockType.CogMediumPowered, out BB);
            if (BB.gameObject.GetComponent<Cogfix>() == null)
                BB.gameObject.AddComponent<Cogfix>();
            if (BB.gameObject.GetComponent<RealisticMotorTorque>() == null)
                BB.gameObject.AddComponent<RealisticMotorTorque>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();

            //40
            PrefabMaster.GetBlock(BlockType.WheelUnpowered, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();
            PrefabMaster.GetStrippedBlock(BlockType.WheelUnpowered, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();

            //41
            PrefabMaster.GetBlock(BlockType.WoodenPole, out BB);
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();
            PrefabMaster.GetStrippedBlock(BlockType.WoodenPole, out BB);
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();

            //42
            //PrefabMaster.GetBlock(BlockType.Slider, out BB);

            //43
            //PrefabMaster.GetBlock(BlockType.Balloon, out BB);

            //44
            //PrefabMaster.GetBlock(BlockType.BallJoint, out BB);

            //45
            PrefabMaster.GetBlock(BlockType.RopeWinch, out BB);
            if (BB.gameObject.GetComponent<WinchFix>() == null)
                BB.gameObject.AddComponent<WinchFix>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();
            PrefabMaster.GetStrippedBlock(BlockType.RopeWinch, out BB);
            if (BB.gameObject.GetComponent<WinchFix>() == null)
                BB.gameObject.AddComponent<WinchFix>();

            //46
            PrefabMaster.GetBlock(BlockType.LargeWheel, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();
            if (BB.gameObject.GetComponent<RealisticMotorTorque>() == null)
                BB.gameObject.AddComponent<RealisticMotorTorque>();
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();
            PrefabMaster.GetStrippedBlock(BlockType.LargeWheel, out BB);
            if (BB.gameObject.GetComponent<Wheelfix3_round>() == null)
                BB.gameObject.AddComponent<Wheelfix3_round>();

            //47
            //PrefabMaster.GetBlock(BlockType.Torch, out BB);

            //48
            //PrefabMaster.GetBlock(BlockType.Drill, out BB);

            //49
            //PrefabMaster.GetBlock(BlockType.GripPad, out BB);

            //50
            //PrefabMaster.GetBlock(BlockType.SmallWheel, out BB);

            //51
            PrefabMaster.GetBlock(BlockType.CogLargeUnpowered, out BB);
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();
            PrefabMaster.GetStrippedBlock(BlockType.CogLargeUnpowered, out BB);
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();

            //52
            //PrefabMaster.GetBlock(BlockType.Unused3, out BB);

            //53
            PrefabMaster.GetBlock(BlockType.ShrapnelCannon, out BB);
            {
                ((CanonBlock)BB).randomDelay = 0f;
            }

            //54
            //PrefabMaster.GetBlock(BlockType.Grenade, out BB);

            //55
            //PrefabMaster.GetBlock(BlockType.SmallPropeller, out BB);

            //56
            PrefabMaster.GetBlock(BlockType.WaterCannon, out BB);
            if (BB.gameObject.GetComponent<BlockMapperLimits>() == null)
                BB.gameObject.AddComponent<BlockMapperLimits>();

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
            if (BB.gameObject.GetComponent<FrictionSlider>() == null)
                BB.gameObject.AddComponent<FrictionSlider>();
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
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();
            PrefabMaster.GetStrippedBlock(BlockType.Log, out BB);
            if (BB.gameObject.GetComponent<WoodDustToggle>() == null)
                BB.gameObject.AddComponent<WoodDustToggle>();
            if (BB.gameObject.GetComponent<SpinnerSound>() == null)
                BB.gameObject.AddComponent<SpinnerSound>();

            //64
            //PrefabMaster.GetBlock(BlockType.Magnet, out BB);

            //65
            PrefabMaster.GetBlock(BlockType.Sensor, out BB);
            if (BB.gameObject.GetComponent<SensorDisjoint>() == null)
                BB.gameObject.AddComponent<SensorDisjoint>();

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

            //71
            //PrefabMaster.GetBlock(BlockType.BuildNode, out BB);

            //72
            //PrefabMaster.GetBlock(BlockType.BuildEdge, out BB);

            //73 BuildSurface
            PrefabMaster.GetBlock(BlockType.BuildSurface, out BB);
            //Show thickness slider, mass slider, and collision toggle.
            BuildSurface.AllowThicknessChange = true;
            BuildSurface.ShowMassSlider = true;
            BuildSurface.ShowCollisionToggle = true;
        }
    }
}