using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateTextController : MonoBehaviour
{

    public string rotationToStoring;                   // ��]�ŕ\�����镶�����
    private string template = "��]�F";                // ����̃e���v���[�g��

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