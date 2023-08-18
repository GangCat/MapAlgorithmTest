using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public void Init(VoidVoidDelegate _callback)
    {
        canvasGenMap.Init(_callback);
    }

    [SerializeField]
    private CanvasGenMap canvasGenMap = null;
}
