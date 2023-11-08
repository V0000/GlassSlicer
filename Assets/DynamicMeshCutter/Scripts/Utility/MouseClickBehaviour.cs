using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMeshCutter
{
    public class MouseClickBehaviour : CutterBehaviour
    {
        private Vector3 _from;
        private Vector3 _to;
        private float randRange = 300;


        protected override void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.nearClipPlane + 0.05f);

                _from = Camera.main.ScreenToWorldPoint(mousePos);
                _to = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mousePos.z);

                Cut();
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
                //objects.gameObject.AddComponent<Pinnable>();
            }
        }
    }
}