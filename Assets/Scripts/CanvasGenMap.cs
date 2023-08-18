using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGenMap : MonoBehaviour
{

    private void Start()
    {
        buttonGenMap.onClick.AddListener(
            () =>
            {
                genMapCallback?.Invoke();
            }
            );
    }

    public void Init(VoidVoidDelegate _callback)
    {
        genMapCallback = _callback;
    }

    [SerializeField]
    private Button buttonGenMap = null;

    private VoidVoidDelegate genMapCallback = null;
}
