using UnityEngine;

/*
 *Компонент отвечает за перемещение прямоугольника
 *На объекте обязаны находиться Rigidbody2D, CommunicatingRectangles
 */

[RequireComponent(typeof(CommunicatingRectangles))]
[RequireComponent(typeof(Rigidbody2D))]
public class RectangleMovement : MonoBehaviour
{
    private CommunicatingRectangles _communicatingRectangles;
    private Rigidbody2D _ridgidbody;       

    private void Awake()
    {
        _communicatingRectangles = GetComponent<CommunicatingRectangles>();
        _ridgidbody = GetComponent<Rigidbody2D>();
        TakePosition(true);
    }

    private void OnMouseDown()
    {
        TakePosition(false);        
    }

    private void OnMouseDrag()
    {        
        _ridgidbody.MovePosition(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.gameObject.transform.position.z)));        
        _communicatingRectangles.ChangePosition();
    }

    private void OnMouseUp()
    {
        TakePosition(true);
    }

    private void TakePosition(bool state)
    {
        _ridgidbody.isKinematic = state;
    }
}
