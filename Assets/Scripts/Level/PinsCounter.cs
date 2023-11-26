using UnityEngine;

namespace Game
{
    //вычисляет количество прикрепленных пинов на сцене
    public class PinsCounter : MonoBehaviour
    {
        private readonly string _pinTag = "Pin";

        public int GetCount()
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();

            int kinematicRigidbodies = 0;

            foreach (Transform childTransform in childTransforms)
            {
                Rigidbody rb = childTransform.GetComponent<Rigidbody>();
                if (rb != null && rb.isKinematic && rb.CompareTag(_pinTag))
                {
                    kinematicRigidbodies++;
                }
            }

            return kinematicRigidbodies;
        }
    }
}