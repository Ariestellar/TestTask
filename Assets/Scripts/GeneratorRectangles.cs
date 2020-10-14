using System.Collections.Generic;
using UnityEngine;
/*
 *Компонент отвечает за непосредственноесоздание создание и удаление прямоугольников на сцене
 *Задаем ссылки:
 *_prefabRectangle - на префаб "Прямоугольника"
 */
public class GeneratorRectangles : MonoBehaviour
{
    [SerializeField] private GameObject _prefabRectangle;
    [SerializeField] private List<BoxCollider2D> _rectangles;

    private int rectangelPositionOnZ = 0;
    /*
     *Создаем прямоугольник
     *Передаем в параметры:
     *-позицию клика
     */
    public void Create(Vector3 clickPosition)
    {        
        GameObject rectangle = Instantiate(_prefabRectangle, SetPositionRectangle(clickPosition), Quaternion.identity);
        if (CheckForIntersections(rectangle.GetComponent<BoxCollider2D>(), _rectangles))
        {
            Destroy(rectangle);
        }
        else
        {            
            rectangle.GetComponent<SpriteRenderer>().color = SetColor();
            _rectangles.Add(rectangle.GetComponent<BoxCollider2D>());            
        }
    }
    /*
     *Удаляем прямоугольник на сцене
     *Передаем в параметры:
     *-ссылку на прямоугольник
     */
    public void DeleteExisting(GameObject existingRectangle)
    {
        _rectangles.Remove(existingRectangle.GetComponent<BoxCollider2D>());
        existingRectangle.GetComponent<CommunicatingRectangles>().BreakСonnectionsWithHost();
       
        Destroy(existingRectangle);
    }
    
    private Vector3 SetPositionRectangle(Vector3 clickPosition)
    {
        return new Vector3(clickPosition.x, clickPosition.y, rectangelPositionOnZ);
    }

    private Color SetColor()
    {
        return Random.ColorHSV();
    }

    private bool CheckForIntersections(BoxCollider2D newRectangle, List<BoxCollider2D> currentListRectangle)
    {
        bool intersect = false;
        if (currentListRectangle.Count != 0)
        {
            foreach (var rectangl in currentListRectangle)
            {
                if (newRectangle.GetComponent<BoxCollider2D>().bounds.Intersects(rectangl.bounds))
                {
                    intersect = true;
                    break;
                }
            }
        }
        return intersect;
    }
}
