using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //selected enable variables
    [SerializeField]
    GameObject crabSprite;

    private void OnEnable()
    {
        if (crabSprite != null) { crabSprite.SetActive(false); }
    }

    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("The cursor entered the selectable UI element.");
        if(crabSprite != null) { crabSprite.SetActive(true); }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (crabSprite != null) { crabSprite.SetActive(false); }
    }
}
