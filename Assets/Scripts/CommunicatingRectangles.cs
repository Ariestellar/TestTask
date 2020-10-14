using System.Collections.Generic;
using UnityEngine;
/*
 *Компонент отвечает за соединения прямоугольника
 *На объекте обязаны находиться SpriteRenderer
 *Задаем ссылки:
 *_prefabShear - на префаб "Ножниц"
 *_viewLineRenderer - на материал линий
 */
[RequireComponent(typeof(SpriteRenderer))]
public class CommunicatingRectangles : MonoBehaviour
{
    [SerializeField] private GameObject _prefabShear;
    [SerializeField] private Material _viewLineRenderer;
    [SerializeField] private List<CommunicatingRectangles> _hosts;
    [SerializeField] private List<int> _hostsId;    
    [SerializeField] private List<LineRenderer> _lineRenderers;


    /*
     *Создаем связь у прямоугольника по отношению к клиенту
     *Передаем в параметры:
     *- ссылку на клиента
     */
    public void CreateBond(CommunicatingRectangles client)
    {        
        _lineRenderers.Add(CreateLineRenderer(client.gameObject.transform.position, this.gameObject.GetComponent<SpriteRenderer>().color));
        client.BecomeAttachedToHost(this, _lineRenderers.Count - 1);
    }
    /*
     *Создаем связь у прямоугольника по отношению к хозяину
     *Передаем в параметры:
     *- ссылку на хост и id линии хоста
     */
    public void BecomeAttachedToHost(CommunicatingRectangles host, int id)
    {
        _hosts.Add(host);
        _hostsId.Add(id);
    }

    /*
     *Изменяем позиции точек линии при перемещении прямоугольника     
     */
    public void ChangePosition()
    {
        foreach (var line in _lineRenderers)
        {
            line.SetPosition(0, this.gameObject.transform.position);
        }

        for (int i = 0; i < _hosts.Count; i++)
        {
            _hosts[i].CangePositionClient(_hostsId[i], this.gameObject.transform.position);            
        }
    }
    /*
     *Удаляем связи при удалении прямоугольника     
     */
    public void BreakСonnectionsWithHost()
    {
        for (int i = 0; i < _hosts.Count; i++)
        {
            if (_hosts[i])
            {
                _hosts[i].DeleteLineRenderer(_hostsId[i]);
            }                   
        }

        foreach (var lineRender in _lineRenderers)
        {
            Destroy(lineRender.gameObject);
        }
    }
    private void CangePositionClient(int id, Vector3 newPositionClient)
    {
        if (_lineRenderers[id])
        {
            _lineRenderers[id].SetPosition(1, newPositionClient);
        }
    }

    private void DeleteLineRenderer(int id)
    {
        _lineRenderers[id].gameObject.SetActive(false);               
    }

    private LineRenderer CreateLineRenderer(Vector3 positionClient, Color color)
    {
        GameObject connectionLine = new GameObject("ConnectionLine");
        
        LineRenderer lineRenderer;
        lineRenderer = connectionLine.AddComponent<LineRenderer>();        
        lineRenderer.SetPosition(0, this.gameObject.transform.position);
        lineRenderer.SetPosition(1, positionClient);
        lineRenderer.material = _viewLineRenderer;
        lineRenderer.numCapVertices = 90;
        lineRenderer.startColor = color;
        lineRenderer.widthMultiplier = 0.2f;

        GameObject shear = Instantiate(_prefabShear, lineRenderer.transform);
        shear.GetComponent<ShearMovement>().Init(lineRenderer);
        return lineRenderer;
    }
}
