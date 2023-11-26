using UnityEngine;


namespace Game
{
    // Все объекты, которые могут быть закреплены "пинами"
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Pinnable : MonoBehaviour
    {
        #region Private variables

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

        #endregion

        #region Public methods

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _meshCollider = gameObject.GetComponent<MeshCollider>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();

            SetStartMaterialStates();
            AddLowFrictionPhisicalMaterial();
        }

        private void Update()
        {
            CheckObjectState();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Pin"))
            {
                _numberOfSpheresTouching++;
                _pin = collider.gameObject;
            }
        }


        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag("Pin"))
            {
                _numberOfSpheresTouching--;
            }
        }

        private void CheckObjectState()
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

            if (_numberOfSpheresTouching <= 1 &&
                !_disconnected) // когда объекта касается менее одного пина, пин и объект открепляются и падают
            {
                DisconnectObject();
                DisconnectPin();
            }
        }

        private void DisconnectObject()
        {
            _disconnected = true;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
        }

        private void DisconnectPin()
        {
            if (_pin)
            {
                Rigidbody pinRigidbody = _pin.GetComponent<Rigidbody>();
                pinRigidbody.useGravity = true;
                pinRigidbody.isKinematic = false;
                _pin.tag = "Untagged";
            }
        }

        private void SetStartMaterialStates()
        {
            _meshCollider.convex = true;
            _meshCollider.isTrigger = false;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }

        private void AddLowFrictionPhisicalMaterial()
        {
            _physicMaterial = new PhysicMaterial();
            _physicMaterial.dynamicFriction = 0.01f;
            _physicMaterial.staticFriction = 0f;
            _physicMaterial.bounciness = 0.18f;

            _collider.material = _physicMaterial;
        }

        #endregion
    }
}