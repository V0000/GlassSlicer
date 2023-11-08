using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace DynamicMeshCutter
{
    [RequireComponent(typeof(LineRenderer))]
    public class MouseBehaviour : CutterBehaviour
    {
        public delegate void CutAction();
        public event CutAction OnObjectCutOff;
        public bool cutterIsLocked;
        public LineRenderer LR => GetComponent<LineRenderer>();

        private Vector3 _from;
        private Vector3 _to;
        private bool _isDragging;
        private float _minCut  = 0.05f;


        protected override void Update()
        {
            base.Update();

            if (cutterIsLocked)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;

                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.nearClipPlane + 0.05f);
                _from = Camera.main.ScreenToWorldPoint(mousePos);
            }

            if (_isDragging)
            {
                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.nearClipPlane + 0.05f);
                _to = Camera.main.ScreenToWorldPoint(mousePos);
                VisualizeLine(true);
            }
            else
            {
                VisualizeLine(false);
            }

            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                if (Vector3.Distance(_from, _to) > _minCut)
                {
                    Cut();
                    _isDragging = false;
                }
                else
                {
                    VisualizeLine(false);
                }
                
            }


        }

        private void Cut()
        {
            Plane plane = new Plane(_from, _to, Camera.main.transform.position);

            var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                if (!root.activeInHierarchy)
                    continue;
                var targets = root.GetComponentsInChildren<MeshTarget>();
                foreach (var target in targets)
                {
                    Cut(target, _to, plane.normal, null, OnCreated);
                }
            }
        }

        void OnCreated(Info info, MeshCreationData cData)
        {
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);

            foreach (var objects in cData.CreatedTargets)
            {
                objects.gameObject.AddComponent<Pinnable>();
            }

            if (OnObjectCutOff != null)
            {
                OnObjectCutOff();
            }

            Debug.Log("Is created");
        }

        private void VisualizeLine(bool value)
        {
            if (LR == null)
                return;

            LR.enabled = value;

            if (value)
            {
                LR.positionCount = 2;
                LR.SetPosition(0, _from);
                LR.SetPosition(1, _to);
            }
        }
    }
}