using UnityEngine;

/*
 *Компонент отвечает за выделение прямоугольника
 *На объекте обязаны находиться SpriteRenderer
 *Задаем ссылку на спрайт в поле _highlight,
 *который отображается при выделении объекта 
 */

[RequireComponent(typeof(SpriteRenderer))]
public class HighlightRectangle : MonoBehaviour
{
    [SerializeField] private Sprite _highlight;

    private Sprite _default;
    private SpriteRenderer _spriteRenderer;
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _default = _spriteRenderer.sprite;
        _spriteRenderer.sprite = _highlight;
    }

    private void OnDisable()
    {
        _spriteRenderer.sprite = _default;
    }
}
