using System.Collections;
using UnityEngine;

/*
 * Обработчик кликов
 * Этот компонент проверяет и вызывает события по нажатию на кнопки мыши 
 * На объекте обязан находиться: GeneratorRectangles
 */

[RequireComponent(typeof(GeneratorRectangles))]
public class ClickHandler : MonoBehaviour
{
    private GeneratorRectangles _generatorRectangles;
    private CommunicatingRectangles _communicatingRectangles;
    private Camera _camera;
    private int _countClick;   

    private void Awake()
    {
        _camera = Camera.main;
        _generatorRectangles = GetComponent<GeneratorRectangles>();
    }

    private void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = CreateRay();
            /*
             * Условия: 
             * Нажата левая кнопка мыши.
             * Луч никуда не попал(попал на пустое место)
             * Событие:
             * Генератор прямоугольников создает новый прямоугольник
             */
            if (hit == false)
            {
                _generatorRectangles.Create(_camera.ScreenToWorldPoint(Input.mousePosition));
            }
            /*
             * Условия: 
             * Нажата левая кнопка мыши.
             * Луч попал по прямоугольнику
             * Событие:
             * Подсчитываем нажатие
             */
            else if (hit.collider.gameObject.GetComponent<RectangleMovement>())
            {
                _countClick += 1;
                StartCoroutine(TimerClick());
                /*
                 * Условие:
                 * Нажата левая кнопка мыши.
                 * Луч попал по прямоугольнику 2 раза
                 * Событие:
                 * Удаляем прямоугольник
                 */
                if (_countClick == 2)
                {
                    _generatorRectangles.DeleteExisting(hit.collider.gameObject);
                }                                
            }
            /*
             * Условия: 
             * Нажата левая кнопка мыши.
             * Луч попал по "ножницам"
             * Событие:
             * Обрезаем связь
             */
            else if (hit.collider.gameObject.GetComponent<ShearMovement>())
            {                
                hit.collider.gameObject.GetComponent<ShearMovement>().CutTies();                
            }
        }         
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = CreateRay();
            
            if (hit != false)
            {                 
                if (hit.collider.gameObject.GetComponent<RectangleMovement>())
                {
                    /*
                     * Условия: 
                     * Нажата правая кнопка мыши.
                     * Луч попал по прямоугольнику.
                     * Прямоугольника Хоста не выбранно.
                     * Событие:
                     * "Выделяем" прямоугольник в качестве хоста 
                     * назанчаем его в поле _communicatingRectangles
                     */
                    if (_communicatingRectangles == null)
                    {
                        hit.collider.gameObject.GetComponent<HighlightRectangle>().enabled = true;
                        _communicatingRectangles = hit.collider.gameObject.GetComponent<CommunicatingRectangles>();
                    }
                    /*
                     * Условия: 
                     * Нажата правая кнопка мыши.
                     * Луч попал по прямоугольнику.
                     * Прямоугольник Хост выбран.
                     * Событие:
                     * Создаем связь между Хостом и клиентом 
                     * поле _communicatingRectangles очищаем
                     */
                    else
                    {
                        _communicatingRectangles.gameObject.GetComponent<HighlightRectangle>().enabled = false;
                        _communicatingRectangles.CreateBond(hit.collider.gameObject.GetComponent<CommunicatingRectangles>());
                        _communicatingRectangles = null;
                    }
                }
            }                     
        }
    }

    /*
     * Таймер кликов
     * необходим для проверки двойного нажатия, 
     * если между кликами меньше половины секунды
     * - значит нажатие двойное 
     */
    private IEnumerator TimerClick()
    {
        yield return new WaitForSeconds(0.5f);
        _countClick = 0;
    }

    /*
     * Создание луча
     * return: возвращает созданный луч
    */
    private RaycastHit2D CreateRay()
    {
        Vector3 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.Raycast(clickPosition, Vector3.back);
    }
}
