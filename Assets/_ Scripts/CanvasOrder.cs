using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOrder : MonoBehaviour
{

    public Canvas objectCanvas;

    public void MoveDown()
    {
        objectCanvas.sortingOrder = 1;
    }
    public void MoveUp()
    {
        objectCanvas.sortingOrder = 3;
    }
}


