using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateTextController : MonoBehaviour
{

    public string rotationToStoring;                   // 回転で表示する文字情報
    private string template = "回転：";                // 操作のテンプレート名

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = template + rotationToStoring;
    }

}