using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCredit : MonoBehaviour
{
    bool isOpen;
    public GameObject CreditList;
    public void Open()
    {
        isOpen = CreditList.activeSelf;
        isOpen = !isOpen;
        CreditList.SetActive(isOpen);
    }
}
