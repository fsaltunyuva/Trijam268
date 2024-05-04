using UnityEngine;

public class Cloud : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public bool allowDrag = true;
    [SerializeField] private Player playerScript;
    
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void OnMouseDown()
    {
        if (playerScript.tutorialActive) return;
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    
    private void OnMouseDrag()
    {
        if (!allowDrag) return;
        if (playerScript.tutorialActive) return;
        Vector3 point = GetMouseWorldPosition() + mousePositionOffset;
        point.y = Mathf.Clamp(point.y, 0.5f, 4.5f);
        transform.position = point; 
    }
}
