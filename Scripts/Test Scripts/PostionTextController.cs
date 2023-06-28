using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostionTextController : MonoBehaviour
{

    public string positionToString;         // 表示するポジションの文字情報
    private string template = "位置：";     // 操作のテンプレート名

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = template + positionToString;
    }

}