using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public bool allowDrag = true;
    
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    
    private void OnMouseDrag()
    {
        if (!allowDrag) return;
        Vector3 point = GetMouseWorldPosition() + mousePositionOffset;
        point.y = Mathf.Clamp(point.y, 0.5f, 4.5f);
        transform.position = point; 
    }
}
