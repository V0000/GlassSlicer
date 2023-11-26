using UnityEngine;

namespace Game
{
    public class TiltCamera : MonoBehaviour
    {
        [SerializeField] private float tiltSpeed = 5.0f; // Скорость наклона камеры
        [SerializeField] private float maxPositionOffset = 0.4f; // Максимальное смещение позиции

        private Vector3 initialPosition;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            UpdateCameraPosition();
        }

        private void Initialize()
        {
            initialPosition = transform.position;
        }

        private void UpdateCameraPosition()
        {
            Vector3 acceleration = Input.acceleration;
            
            float xOffset = acceleration.x * maxPositionOffset;
            float yOffset = acceleration.y * maxPositionOffset;
            Vector3 newPosition = new Vector3(initialPosition.x + xOffset, initialPosition.y + yOffset, initialPosition.z);
            
            transform.position = Vector3.Lerp(transform.position, newPosition, tiltSpeed * Time.deltaTime);
        }
    }
}