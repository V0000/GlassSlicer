using UnityEngine;

namespace Game
{
    public class RandomMovement : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 1.0f; // Минимальная скорость движения
        [SerializeField] private float maxSpeed = 2.0f; // Максимальная скорость движения

        private Vector2 _targetPosition;
        private float _moveSpeed;

        private void Start()
        {
            _moveSpeed = Random.Range(minSpeed, maxSpeed);

            _targetPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }

        private void Update()
        {
            Vector2 direction = (_targetPosition - (Vector2)transform.position).normalized;

            // Двигаем объект в направлении цели с заданной скоростью
            transform.Translate(direction * _moveSpeed * Time.deltaTime);
        
            if (Vector2.Distance(transform.position, _targetPosition) < 0.2f)
            {
                _targetPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            }
        }
    }
}