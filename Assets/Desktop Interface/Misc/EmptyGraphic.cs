using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]

public class EmptyGraphic : Graphic
{
    // Empty Graphic for no visual raycast targets.

    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }
}