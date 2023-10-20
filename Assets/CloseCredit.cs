using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCredit : MonoBehaviour
{

    bool isOpen;
    public GameObject CreditList;
    public void CLose()
    {
        isOpen = CreditList.activeSelf;
        isOpen = !isOpen;
        CreditList.SetActive(isOpen);
    }

}
