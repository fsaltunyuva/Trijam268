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
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Drag Limit")
        {
            Debug.Log("Entered Drag Limit");
            allowDrag = false;
            Vector3 temp = gameObject.GetComponent<RectTransform>().transform.position;
            GetComponent<RectTransform>().transform.position = new Vector3(temp.x, temp.y + 0.01f, 0);
        }
            
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Drag Limit")
        {
            Debug.Log("Exited Drag Limit");
            allowDrag = true;
        }
    }
    
}
