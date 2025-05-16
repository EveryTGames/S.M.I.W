using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _dragOffset;
    private bool _canDrag = false;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canDrag = false;



        // Only drag if clicking directly on this object (not a child)
        Debug.Log(eventData.pointerEnter );
        if (eventData.pointerEnter.transform.parent.gameObject == gameObject)
        {
            _canDrag = true;

            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localMousePosition))
            {
                _dragOffset = _rectTransform.anchoredPosition - localMousePosition;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_canDrag || _canvas == null)
            return;

        Vector2 localMousePosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localMousePosition))
        {
            _rectTransform.anchoredPosition = localMousePosition + _dragOffset;
        }
    }
}
