using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Draggable : MonoBehaviour
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] private DragHandle dragHandle;
    [SerializeField] private RectTransform container;
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    [SerializeField] private int containerMarginLeft;
    [SerializeField] private int containerMarginRight;
    [SerializeField] private int containerMarginTop;
    [SerializeField] private int containerMarginBottom;
    
    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector3[] containerCorners = new Vector3[4];
    
    #endregion
    
    #region --- Events ---
    
    private void EventSubscription()
    {
        dragHandle.onDragStart.AddListener(DragStart);
        dragHandle.onDrag.AddListener(Drag);
    }
    
    private void EventUnsubscription()
    {
        dragHandle.onDragStart.RemoveListener(DragStart);
        dragHandle.onDrag.RemoveListener(Drag);
    }
    
    #endregion
    
    #region --- Initialization & Termination ---
    
    private void Start()
    {
        EventSubscription();

        rectTransform = (RectTransform)transform;
    }
    
    private void OnDestroy()
    {
        EventUnsubscription();
        
        
    }
    
    #endregion
    
    private void DragStart(DragHandle.StartDragData data)
    {
        startPos = rectTransform.position;
    }
    
    private void Drag(DragHandle.DragData data)
    {
        Vector2 newPos = startPos + data.posDifference;
        rectTransform.position = ClampToContainer(newPos);
    }
    
    private Vector2 ClampToContainer(Vector2 position)
    {
        Vector2 size = rectTransform.rect.size;
        Vector2 pivot = rectTransform.pivot;
        
        Vector2 containerMin;
        Vector2 containerMax;
        
        if (container != null)
        {
            container.GetWorldCorners(containerCorners);
            containerMin = containerCorners[0];
            containerMax = containerCorners[2];
        }
        
        else
        {
            containerMin = Vector2.zero;
            containerMax = new Vector2(Screen.width, Screen.height);
        }
        
        float minX = containerMin.x + size.x * pivot.x + containerMarginLeft;
        float maxX = containerMax.x - size.x * (1f - pivot.x) - containerMarginRight;
        float minY = containerMin.y + size.y * pivot.y + containerMarginBottom;
        float maxY = containerMax.y - size.y * (1f - pivot.y) - containerMarginTop;
        
        position.x = Mathf.Round(Mathf.Clamp(position.x, minX, maxX));
        position.y = Mathf.Round(Mathf.Clamp(position.y, minY, maxY));
        
        return position;
    }
}