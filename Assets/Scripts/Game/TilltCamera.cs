using UnityEngine;

namespace Game
{
    public class TilltCamera : MonoBehaviour
    {
        [SerializeField] private float tiltSpeed = 5.0f; // Скорость наклона камеры
        [SerializeField] private float maxTiltAngle = 30.0f; 
        [SerializeField] private float maxPositionOffset = 0.4f; // Максимальное смещение позиции
   
        private Vector3 _initialPosition;
        void Start()
        {
            _initialPosition = transform.position;
        }
        void Update()
        {
            Vector3 acceleration = Input.acceleration;

            // Изменяем позицию камеры по горизонтали
            float xOffset = acceleration.x * maxPositionOffset;
            float yOffset = acceleration.y * maxPositionOffset;
            Vector3 newPosition = new Vector3(_initialPosition.x + xOffset, _initialPosition.y + yOffset, _initialPosition.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, tiltSpeed * Time.deltaTime);
        }
    }
}
