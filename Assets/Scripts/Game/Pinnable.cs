using UnityEngine;


namespace Game
{
    // Все объекты, которые могут быть закреплены "пинами"
    public class Pinnable : MonoBehaviour
    {
        [SerializeField] private int delaqFrames = 10;
        private int _lastTouching = 0;
        private int _numberOfSpheresTouching = 0;
        private Collider _collider = new Collider();
        private Rigidbody _rigidbody = new Rigidbody();
        private PhysicMaterial _physicMaterial;
        private GameObject _pin;
        private MeshCollider _meshCollider;
        private bool _disconnected = false;
        private int _counter;

        private void Start()
        {
        
            _collider = GetComponent<Collider>();
        
            _meshCollider = gameObject.GetComponent<MeshCollider>();
            if (_meshCollider == null)
            {
                _meshCollider = gameObject.AddComponent<MeshCollider>();
            }
        
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        
            _meshCollider.convex = true;
            _meshCollider.isTrigger = false;

            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        
            _physicMaterial = new PhysicMaterial();
            _physicMaterial.dynamicFriction = 0.01f;
            _physicMaterial.staticFriction = 0f;
            _physicMaterial.bounciness = 0.18f;
        
            _collider.material = _physicMaterial;
        
            int instanceID = gameObject.GetInstanceID();
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Pin")) 
            {
                _numberOfSpheresTouching++;
                _pin = collider.gameObject;
            }
            //Debug.Log(numberOfSpheresTouching);
        }
    
  
        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("Pin"))
            {
                _numberOfSpheresTouching--;
            }
            //Debug.Log(numberOfSpheresTouching);
        }


        private void Update()
        {
            if (_counter++ < delaqFrames)
            {
                return;
            }
            _counter = 0;
        
            if (_lastTouching != _numberOfSpheresTouching) // Проверяем, что все объекты вошли в триггер
            {
                _lastTouching = _numberOfSpheresTouching;
                return;
            }
    
            if (_numberOfSpheresTouching <= 1 && !_disconnected) // когда объекта касается менее одного пина, пин и объект открепляются и падают
            {
                _disconnected = true;
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;

                if (_pin)
                {
                    Rigidbody pinRigidbody = _pin.GetComponent<Rigidbody>();
                    pinRigidbody.useGravity = true;
                    pinRigidbody.isKinematic = false;
                    _pin.tag = "Untagged";
                }
            }

        }
  
    }
}
