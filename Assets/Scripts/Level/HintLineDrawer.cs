using UnityEngine;


namespace Game
{
    // Отображение линиЙ-подсказок
    [RequireComponent(typeof(LineRenderer))]
    public class HintLineDrawer : MonoBehaviour
    {
        #region Private variables

        private Vector3[] _points;
        private GameObject _lineObject;
        private GameObject[] _linesObjects;

        private const int PositionCount = 2;
        private const float StartWidth = 0.03f;
        private const float EndWidth = 0.03f;
        private const string MaterialPath = "Sprites/Default";
        private readonly Color _startColor = Color.blue;
        private readonly Color _endColor = Color.red;

        private LineRenderer lineRenderer => GetComponent<LineRenderer>();
        private int _currentPointIndex = 0;
        private bool _linesIsActive = false;

        #endregion

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _linesIsActive)
            {
                DestroyLines();
            }
        }

        public void ShowLines(Vector3[] pointsForLines)
        {
            _points = pointsForLines;
            lineRenderer.positionCount = _points.Length;
            _linesObjects = new GameObject[_points.Length];

            for (int i = 0; i < _points.Length / 2; i++)
            {
                Vector3 startVector = _points[i * 2];
                Vector3 endVector = _points[i * 2 + 1];
                _linesObjects[i] = RenderLine(startVector, endVector);
            }

            _linesIsActive = true;
        }

        private GameObject RenderLine(Vector3 startVector, Vector3 endVector)
        {
            _lineObject = new GameObject("LineObject");
            LineRenderer lineObjRenderer = _lineObject.AddComponent<LineRenderer>();
            lineObjRenderer.positionCount = PositionCount;
            lineObjRenderer.startWidth = StartWidth;
            lineObjRenderer.endWidth = EndWidth;
            lineObjRenderer.material = new Material(Shader.Find(MaterialPath));
            lineObjRenderer.startColor = _startColor;
            lineObjRenderer.endColor = _endColor;

            _lineObject.transform.parent = transform;

            lineObjRenderer.SetPosition(0, startVector);
            lineObjRenderer.SetPosition(1, endVector);
            lineObjRenderer.enabled = true;

            return _lineObject;
        }

        private void DestroyLines()
        {
            foreach (var line in _linesObjects)
            {
                Destroy(line);
            }

            _linesIsActive = false;
        }
    }
}