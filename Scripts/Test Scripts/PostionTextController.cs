using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostionTextController : MonoBehaviour
{

    public string positionToString;         // �\������|�W�V�����̕������
    private string template = "�ʒu�F";     // ����̃e���v���[�g��

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