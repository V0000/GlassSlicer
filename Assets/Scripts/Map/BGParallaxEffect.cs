using UnityEngine;
using UnityEngine.UI;

// Параллакс эффект на карте уровней    
namespace Map
{
    public class BGParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float parallaxAmount = 1f; //сила параллакса
        [SerializeField] private ScrollRect scrollRect;
        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
        }

        private void Update()
        {
            float scrollValue = scrollRect.normalizedPosition.y;

            // смещение для параллакса
            float yOffset = (1f - scrollValue) * parallaxAmount;
        
            Vector3 newPosition = _initialPosition + Vector3.up * yOffset;
            transform.position = newPosition;
        }
    }
}
