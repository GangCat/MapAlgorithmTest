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

        buttonReset.onClick.AddListener(
            () =>
            {
                resetCallback?.Invoke();
            }
            );
    }

    public void Init(VoidVoidDelegate _callback, VoidVoidDelegate _resetCallback)
    {
        genMapCallback = _callback;
        resetCallback = _resetCallback;
    }

    [SerializeField]
    private Button buttonGenMap = null;
    [SerializeField]
    private Button buttonReset = null;

    private VoidVoidDelegate genMapCallback = null;
    private VoidVoidDelegate resetCallback = null;
}
