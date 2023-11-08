using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    //программный скролл списка (к актуальной точке)
    public class ScrollMover : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }
        public void SetScrollValueX(float scrollValue) // 1-start, 0-end
        {
            _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, scrollValue);
        }
    
        public void SetScrollValueY(float scrollValue) // 1-start, 0-end
        {
            _scrollRect.normalizedPosition = new Vector2(scrollValue, _scrollRect.normalizedPosition.y);
        }
    }
}
