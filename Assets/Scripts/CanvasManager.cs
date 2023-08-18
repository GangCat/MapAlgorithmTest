using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public void Init(VoidVoidDelegate _callback, VoidVoidDelegate _resetCallback)
    {
        canvasGenMap.Init(_callback, _resetCallback);
    }

    [SerializeField]
    private CanvasGenMap canvasGenMap = null;
}
