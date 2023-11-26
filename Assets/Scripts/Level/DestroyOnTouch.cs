using UnityEngine;

// Уничтожаем осколки, выпавшие за экран и попавшие в коллайдер
namespace Game
{
    public class DestroyOnTouch : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            Destroy(collider.gameObject);
        }
    }
}
