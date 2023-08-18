using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestImage : MonoBehaviour
{
    public void SetText(string _str)
    {
        GetComponentInChildren<TMP_Text>().text = _str;
    }
}
