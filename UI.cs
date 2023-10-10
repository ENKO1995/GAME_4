using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;



public class UI : MonoBehaviour
{
    public static UI instance;
    public TextMeshProUGUI WinnerText;


    private void Awake()
    {
        instance = this;
    }
    public void SetWinnerText(string _winner)
    {
        WinnerText.gameObject.SetActive(true);
        WinnerText.text = _winner + " wins";
    }
}

[System.Serializable]
public class Container
{
    public GameObject Object;
    public TextMeshProUGUI Text;
}

