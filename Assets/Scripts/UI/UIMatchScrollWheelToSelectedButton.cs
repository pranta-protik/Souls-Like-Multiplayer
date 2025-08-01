using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoulsLike
{
    public class UIMatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] private GameObject _currentSelected;
        [SerializeField] private GameObject _previouslySelected;
        [SerializeField] private RectTransform _currentSelectedTransform;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private ScrollRect _scrollRect;

        private void Update() {
            _currentSelected = EventSystem.current.currentSelectedGameObject;

            if (_currentSelected != null) {
                if (_currentSelected != _previouslySelected) {
                    _previouslySelected = _currentSelected;
                    _currentSelectedTransform = _currentSelected.GetComponent<RectTransform>();
                    SnapTo(_currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target) {
            Canvas.ForceUpdateCanvases();

            var newPositon = (Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) -
                             (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);

            // WE ONLY WANT TO LOCK THE POSITION ON THE Y AXIS (UP AND DOWN)
            newPositon.x = 0f;

            _contentPanel.anchoredPosition = newPositon;
        }
    }
}