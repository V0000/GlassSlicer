using UnityEngine;


namespace Game
{
    // Отображение линии разреза
    public class LineDrawer : MonoBehaviour
    {
        private Vector3[] _points;
        private GameObject _lineObject;
        private GameObject[] _linesObjects;
        private LineRenderer lineRenderer => GetComponent<LineRenderer>();
        private int _currentPointIndex = 0;
        private bool _linesIsActive = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _linesIsActive)
            {
                foreach (var line in _linesObjects)
                {
                    Destroy(line);
                }

                _linesIsActive = false;
            }
        }

        public void ShowLines(Vector3[] pointsForLines)
        {
            _points = pointsForLines;
            lineRenderer.positionCount = _points.Length;
            _linesObjects = new GameObject[_points.Length];
            for (int i = 0; i < _points.Length / 2; i++)
            {
                _lineObject = new GameObject("LineObject");
                LineRenderer lineObjRenderer = _lineObject.AddComponent<LineRenderer>();
                lineObjRenderer.positionCount = 2;
                lineObjRenderer.startWidth = 0.03f;
                lineObjRenderer.endWidth = 0.03f;
                lineObjRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineObjRenderer.startColor = Color.blue;
                lineObjRenderer.endColor = Color.red;

                _lineObject.transform.parent = transform;
                _linesObjects[i] = _lineObject;

                lineObjRenderer.SetPosition(0, _points[i * 2]);
                lineObjRenderer.SetPosition(1, _points[i * 2 + 1]);
                lineObjRenderer.enabled = true;
            }

            _linesIsActive = true;
        }
    }
}