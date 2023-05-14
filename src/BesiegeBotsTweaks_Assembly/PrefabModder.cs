/*
 * PrefabModder.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 */

using BesiegeBotsTweaks;
using UnityEngine;
using Modding.Blocks;
using System.Collections.Generic;

namespace BotFix
{
    public class PrefabModder : MonoBehaviour
    {
        /*
        The very first change BBM made, and most common change BBM makes currently is to change the durability of certain blocks.
        This is done by changing the breakForce and breakTorque thresholds of Joints.
        */
        private static void TweakBreakForces(BlockBehaviour BB)
        {
            Joint joint = BB.gameObject.GetComponent<Joint>();
            TriggerSetJoint2 secondJoint = BB.gameObject.GetComponentInChildren<TriggerSetJoint2>();
            StrengthenTreads ST = BB.gameObject.GetComponent<StrengthenTreads>();
            if (joint)
            {
                switch (BB.Prefab.Type)
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
                        BuildSurface BS = ((BuildSurface)BB);
                        BuildSurface.SurfaceMaterialType[] buildSurfaceMats = { BS.wood, BS.glass, BS.wing, BS.metal };
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
        internal static void TweakFriction(BlockBehaviour BB)
        {
            //Gets the colliders of each block, and if there is at least one then friction is modified based on a switch statement of the block type.
            Collider[] colliders = BB.gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                switch (BB.Prefab.Type)
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
        private static void TweakMass(BlockBehaviour BB)
        {
            //Gets the Rigidbody of each block, and if it isn't null then the mass is modified based on a switch statement of the block type.
            Rigidbody RB = BB.gameObject.GetComponent<Rigidbody>();
            switch (BB.Prefab.Type)
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
        Thus, the blocks in this collection have their prefabs' myDamageType changed to Blunt,
        thereby rendering those blocks unable to cut ropes.
        */
        private static readonly List<BlockType> toBlunten = new List<BlockType>(){BlockType.MetalBlade, BlockType.CircularSaw, BlockType.Spike, BlockType.Flamethrower, BlockType.Wing, BlockType.Propeller, BlockType.SmallPropeller, BlockType.Drill, BlockType.MetalBall};
        private static void ApplyComponents(BlockBehaviour BB)
        {
            /*** Block-Specific modifications ***/
            switch (BB.Prefab.Type)
            {

                case BlockType.DoubleWoodenBlock:
                case BlockType.WoodenPole:
                    BB.gameObject.AddComponent<WoodDustToggle>();
                    break;
                case BlockType.Wheel:
                case BlockType.LargeWheel:
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                    BB.gameObject.AddComponent<RealisticMotorTorque>();
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    BB.gameObject.AddComponent<FrictionSlider>();
                    break;
                case BlockType.Hinge:
                case BlockType.Piston:
                case BlockType.Ballast:
                case BlockType.Boulder:
                    BB.gameObject.AddComponent<FrictionSlider>();
                    break;
                case BlockType.MetalBlade:
                case BlockType.Spike:
                    BB.gameObject.AddComponent<InvincibleJointToggle>();
                    break;
                case BlockType.Spring:
                    BB.gameObject.AddComponent<WinchFix>();
                    break;
                case BlockType.Cannon:
                case BlockType.ShrapnelCannon:
                    ((CanonBlock)BB).randomDelay = 0f;
                    break;
                case BlockType.SteeringBlock:
                    //This has to be done here, instead of Awake(). Glad I got it first try.
                    ((SteeringWheel)BB).allowLimits = true;
                    //The other parts have to be done after SteeringWheel's Awake though.
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    break;
                case BlockType.FlyingBlock:
                case BlockType.SteeringHinge:
                case BlockType.WaterCannon:
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    break;
                case BlockType.Suspension:
                    BB.gameObject.AddComponent<Suspensionzcript>();
                    break;
                case BlockType.CircularSaw:
                    //1.25 update compat removed these
                    //BB.transform.GetChild(9).localPosition = new Vector3(0f, 0f, -0.1f);
                    BB.transform.GetChild(10).localPosition = new Vector3(0f, 0f, -0.1f);
                    BB.gameObject.AddComponent<RealisticMotorTorque>();
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    break;
                case BlockType.Flamethrower:
                    BB.gameObject.AddComponent<FlameTFix>();
                    break;
                case BlockType.SpinningBlock:
                    BB.gameObject.AddComponent<InvincibleJointToggle>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    BB.gameObject.AddComponent<RealisticMotorTorque>();
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    break;
                case BlockType.Wing:
                case BlockType.WingPanel:
                    BB.gameObject.AddComponent<AxialDragToggle>();
                    break;
                case BlockType.Grabber:
                    BB.gameObject.AddComponent<Grabberfix>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    BB.gameObject.AddComponent<AntiPhysXExplosion>();
                    break;
                case BlockType.CogMediumPowered:
                    BB.gameObject.AddComponent<Cogfix>();
                    BB.gameObject.AddComponent<RealisticMotorTorque>();
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    break;
                case BlockType.WheelUnpowered:
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                    BB.gameObject.AddComponent<FrictionSlider>();
                    break;
                case BlockType.RopeWinch:
                    BB.gameObject.AddComponent<WinchFix>();
                    BB.gameObject.AddComponent<BlockMapperLimits>();
                    break;
                case BlockType.CogLargeUnpowered:
                    BB.gameObject.AddComponent<FrictionSlider>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    break;
                case BlockType.LargeWheelUnpowered:
                    BB.gameObject.AddComponent<Wheelfix3_round>();
                    BB.gameObject.AddComponent<FrictionSlider>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    break;
                case BlockType.Log:
                    BB.gameObject.AddComponent<WoodDustToggle>();
                    BB.gameObject.AddComponent<SpinnerSound>();
                    break;
                case BlockType.Sensor:
                    BB.gameObject.AddComponent<SensorDisjoint>();
                    break;
            }
        }

        internal static void ModAllPrefab()
        {
            //Modding.ModConsole.Log("ModAllPrefab called");
            Modding.Events.OnBlockInit += delegate (Block toInit)
            {
                BlockBehaviour BB = toInit.InternalObject;
#if DEBUG
                Modding.ModConsole.Log("Initializing " + toInit.ToString());
#endif

                //maxAngularVelocity is set sometime after simulation starts,
                //so we need to apply a component to EVERY BLOCK WE TWEAK
                BB.gameObject.AddComponent<DragTweaks>();

                //Prevent sharp blocks from cutting winches
                if (toBlunten.Contains(BB.Prefab.Type))
                    BB.Prefab.myDamageType = DamageType.Blunt;

                //General Static Block Prefab modifications
                TweakBreakForces(BB);
                TweakFriction(BB);
                TweakMass(BB);

                //Apply components onto blocks that need
                ApplyComponents(BB);

                //Buildsurface statics tweaks
                BuildSurface.AllowThicknessChange = true;
                BuildSurface.ShowMassSlider = true;
                BuildSurface.ShowCollisionToggle = true;
            };
        }
    }
}