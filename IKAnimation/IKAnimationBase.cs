using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RootMotion.FinalIK;
using NaughtyAttributes;
namespace IKAnimation
{
    [RequireComponent(typeof(BipedIK))]
    [System.Serializable]
    public class IKAnimationBase : MonoBehaviour
    {
        [ReorderableList]
        public List<IKTarget> ikTargets;
        private BipedIK _biped;

        //TODO: BipedIK must be required, moving IK targets, maybe later you can save your animation process to scriptable object and just call it by base
        //TODO: Even better to do is saving as animationClip.
        //TODO: Maybe 

        private void Awake()
        {
            _biped = GetComponent<BipedIK>();

        }

        [Button("Start Animation Process")]
        public void StartAnimationProcess()
        {
            StartCoroutine(nameof(StartAnimationProcess_Helper));
        }


        private IEnumerator StartAnimationProcess_Helper()
        {
            for(int i = 0; i < ikTargets.Count; i++)
            {
                if(_biped.solvers.ikSolvers[ikTargets[i].partToMove.SolverID].IKPositionWeight != ikTargets[i].targetWeight)
                {
                    _biped.solvers.ikSolvers[ikTargets[i].partToMove.SolverID].IKPositionWeight.To(ikTargets[i].targetWeight, 0.3f, 0);
                    _biped.solvers.ikSolvers[ikTargets[i].partToMove.SolverID].IKPositionWeight = ikTargets[i].targetWeight;
                }
                bool completed = false;
                
                ikTargets[i].partToMove.transform.MoveTo(ikTargets[i].position, ikTargets[i].transitionTime, ikTargets[i].startDelay, ()=>
                {
                    completed = true;
                });

                yield return new WaitWhile(()=>!completed);
                yield return new WaitForSeconds(ikTargets[i].waitTime);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(ikTargets.Count <= 0)
                return;

            for(int i = 0; i < ikTargets.Count; i++)
            {
                ikTargets[i].OnDrawGizmos();
            }
        }


        [Button("Setup")]
        public void SetupIKSolvers()//!This function is a mess.
        {
            _biped = GetComponent<BipedIK>();
            SystemBase.solversReady = true;
            List<Transform> list = new List<Transform>();
            GameObject parent = new GameObject();
            parent.name = "Setup Parent";
            parent.transform.parent = transform;
            for(int i = 0; i < 7; i++)
            {
                GameObject go = new GameObject();
                go.name = "Setup GO: " + i.ToString();
                go.transform.parent = parent.transform;
                list.Add(go.transform);
                go.AddComponent<PartToMove>();
            }
            //look at target creation-->
            _biped.solvers.lookAt.target = list[0];
            list[0].name = "LookAtTarget";
            list[0].transform.position = _biped.solvers.lookAt.head.transform.position;
            list[0].GetComponent<PartToMove>().SolverID = 6;
            _biped.solvers.lookAt.IKPositionWeight = 0;
            //right hand target creation-->
            _biped.solvers.rightHand.target = list[1];
            list[1].name = "RightHandTarget";
            list[1].transform.position = _biped.solvers.rightHand.bone3.transform.position;
            list[1].GetComponent<PartToMove>().SolverID = 3;
            _biped.solvers.rightHand.IKPositionWeight = 0;
            //left hand
            _biped.solvers.leftHand.target = list[2];
            list[2].name = "LeftHandTarget";
            list[2].transform.position = _biped.solvers.leftHand.bone3.transform.position;
            list[2].GetComponent<PartToMove>().SolverID = 2;
            _biped.solvers.leftHand.IKPositionWeight = 0;
            //right foot target creation -->
            _biped.solvers.rightFoot.target = list[3];
            list[3].name = "RightFootTarget";
            list[3].transform.position = _biped.solvers.rightFoot.bone3.transform.position;
            list[3].GetComponent<PartToMove>().SolverID = 1;
            _biped.solvers.rightFoot.IKPositionWeight = 0;
            //left foot target creation -->
            _biped.solvers.leftFoot.target = list[4];
            list[4].name = "LeftFootTarget";
            list[4].transform.position = _biped.solvers.leftFoot.bone3.transform.position;
            list[4].GetComponent<PartToMove>().SolverID = 0;
            _biped.solvers.leftFoot.IKPositionWeight = 0;
            //spine target creation -->
            _biped.solvers.spine.target = list[5];
            list[5].name = "SpineTarget";
            list[5].transform.position = _biped.solvers.spine.bones[_biped.solvers.spine.bones.Length-1].transform.position;
            list[5].GetComponent<PartToMove>().SolverID = 4;
            _biped.solvers.spine.IKPositionWeight = 0;
            //aim target creation --> 
            _biped.solvers.aim.target = list[6];
            list[6].name = "AimTarget";
            list[6].transform.position = _biped.solvers.aim.bones[_biped.solvers.aim.bones.Length - 1].transform.position;
            list[6].GetComponent<PartToMove>().SolverID = 5;
            _biped.solvers.aim.IKPositionWeight = 0;
            Debug.Log($"IK Solvers Set.");
        }
        #endif
    }

   /* #if UNITY_EDITOR
    [CustomEditor(typeof(IKAnimationBase))]
    public class IKAnimationBase_Editor: NaughtyInspector 
    {
        public override void OnInspectorGUI()
        {
            IKAnimationBase _script = (IKAnimationBase)target;
            DrawDefaultInspector();
           
           
            GUI.backgroundColor = Color.magenta;
            if(GUILayout.Button("Start Animation Process"))
            {
                _script.StartAnimationProcess();
            }

            if(!SystemBase.solversReady)
            {
                GUI.backgroundColor = Color.green;
                if(GUILayout.Button("SETUP IK SOLVERS"))
                {
                    _script.SetupIKSolvers();
                }
            }
        }
        
    }

    #endif*/

}
