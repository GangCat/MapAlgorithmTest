using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        canvasMng.Init(GenMap, Resetmap);
    }

    private void GenMap()
    {
        stageMng.GenerateMap();
    }

    private void Resetmap()
    {
        stageMng.ResetMap();
    }

    [SerializeField]
    private StageManager stageMng = null;
    [SerializeField]
    private CanvasManager canvasMng = null;
}
