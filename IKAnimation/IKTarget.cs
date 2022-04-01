using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace IKAnimation
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class IKTarget :  ITarget
    {
        public bool usePartToMovePosition;
        public Vector3 position;
        public PartToMove partToMove;
        [Range(0,10)]public float transitionTime;
        public Color color;
        public MeshType mesh;
        [Range(0.1f, 10)]public float size = 0.1f;

        public float startDelay = 0;

        public float waitTime;

        [Range(0,1)]public float targetWeight;

       

        public void OnDrawGizmos()
        {
            if(partToMove != null)
            {
                if(usePartToMovePosition)
                    position = partToMove.transform.position;
            }
                
            Gizmos.color = color;
            switch(mesh)
            {
                case MeshType.Sphere:
                    Gizmos.DrawSphere(position, size);
                    break;
                case MeshType.Cube:
                    Gizmos.DrawCube(position, Vector3.one * size);
                    break;
                default:
                    break;
            }
        }
    }

}

