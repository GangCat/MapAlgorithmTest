using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        canvasMng.Init(GenMap);
    }

    private void GenMap()
    {
        stageMng.GenerateMap();
    }

    [SerializeField]
    private StageManager stageMng = null;
    [SerializeField]
    private CanvasManager canvasMng = null;
}
