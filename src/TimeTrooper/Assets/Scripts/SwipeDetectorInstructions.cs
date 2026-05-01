using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetectorInstructions : MonoBehaviour, IEndDragHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    public float minSwipeDistance = 50f; // Pixels: Ignore short drags

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // Drag vector: end pos - start pos (normalized)
        Vector2 dragVector = (eventData.position - eventData.pressPosition).normalized;
        float dragDistance = eventData.position.magnitude - eventData.pressPosition.magnitude;

        // Only process significant swipes
        if (dragDistance > minSwipeDistance)
        {
            return;
        }

        // Determine direction (horizontal priority for left/right)
        float absX = Mathf.Abs(dragVector.x);
        float absY = Mathf.Abs(dragVector.y);

        if (absX > absY)
        {
            if (dragVector.x < 0) // Negative X: finger swiped LEFT
            {
                OnSwipeLeft();
            }
            else // Positive X: right
            {
                Debug.Log("Swipe Right");
            }
        }
        else
        {
            if (dragVector.y > 0) Debug.Log("Swipe Up");
            else Debug.Log("Swipe Down");
        }
    }

    private void OnSwipeLeft()
    {
        Debug.Log("LEFT SWIPE DETECTED!");
        GameStateManager.instance.ChangeToGetDificulty();
    }
}
