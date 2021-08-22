using UnityEngine;
using Modding.Blocks;
using System.Collections;

namespace BotFix
{
    public class WPFix : MonoBehaviour
    {
        private Collider[] colliders;
        private ConfigurableJoint CJ;
        private Rigidbody rigg;
        private BlockBehaviour BB;
        private BlockHealthBar BH;

        private int fcounter = 0;

        void Awake()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                BB = GetComponent<BlockBehaviour>();

                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = Mathf.Infinity;
                CJ.breakTorque = Mathf.Infinity;

                //rigg = GetComponent<Rigidbody>();
                //rigg.drag = 0f;
                //rigg.angularDrag = 0f;
                //rigg.mass = 0.5f;
                //rigg.maxAngularVelocity = 100;

                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = 0.1f;
                    collider.material.staticFriction = 0.1f;
                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }
            }
        }
       
        void Update()
        {
            if (!StatMaster.isClient && BB.SimPhysics)
            {
                fcounter++;
                if (fcounter < 8 && fcounter > 6)
                {
                    if (!CJ)
                    {
                        //Debug.Log("No CJ");
                        return;
                    }
                    Rigidbody AB = CJ.connectedBody;

                    ConfigurableJoint configurableJoint = BB.gameObject.AddComponent<ConfigurableJoint>();
                    //Debug.Log("MADE JOINT");
                    configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.xMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.yMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.zMotion = ConfigurableJointMotion.Locked;
                    configurableJoint.breakForce = Mathf.Infinity;
                    configurableJoint.breakTorque = Mathf.Infinity;
                    configurableJoint.connectedBody = AB;
                    
                }
            }

            /*
            IEnumerator ExampleCoroutine()
            {
                //Print the time of when the function is first called.
                Debug.Log("Started Coroutine at timestamp : " + Time.time);

                BH = GetComponent<BlockHealthBar>();
                Debug.Log(BH.health);
                //yield on a new YieldInstruction that waits for 5 seconds.
                yield return new WaitForSeconds(5);

                //After we have waited 5 seconds print the time again.
                Debug.Log("Finished Coroutine at timestamp : " + Time.time);
                StartCoroutine(ExampleCoroutine());
            }
            */
        }
    }
}

