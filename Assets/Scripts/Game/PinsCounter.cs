using UnityEngine;

namespace Game
{
    //вычисляет количество прикрепленных пинов на сцене
    public class PinsCounter : MonoBehaviour
    {
        public int GetCountKinematicChilds()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();

            int kinematicRigidbodies = 0;
        
            foreach (Transform childTransform in childTransforms)
            {
                Rigidbody rb = childTransform.GetComponent<Rigidbody>();
                if (rb != null && rb.isKinematic)
                {
                    kinematicRigidbodies++;
                }
            }

            return kinematicRigidbodies;
        }


    }
}
