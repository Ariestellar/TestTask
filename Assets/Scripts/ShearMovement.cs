using UnityEngine;

/*
 *Компонент отвечает за перемещение "ножниц"
 *на связующей линии объектов
 */
public class ShearMovement : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    /*
     *При создании "ножниц", 
     *Инициализаруем их и передаем ссылку на линию,
     *которую контролируют ножницы
     */
    public void Init(LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
    }

    private void Update()
    {
        Vector3 startLine = _lineRenderer.GetPosition(0);
        Vector3 endLine = _lineRenderer.GetPosition(1);

        transform.position = GetPosition(startLine, endLine);
    }

    /*
     *"Отрезаем" линию которую курируют данные ножницы
     */
    public void CutTies()
    {
        _lineRenderer.gameObject.SetActive(false);
    } 
    
    /*
     *Задаем позицию ножницам посередине между двумя точками
     */
    private Vector3 GetPosition(Vector3 startLine, Vector3 endLine)
    { 
        return new Vector3((startLine.x + endLine.x) / 2, (startLine.y + endLine.y) / 2, 0);
    }

}
