using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour,IPointerDownHandler
{
    bool BClick = false;
    bool BButtonClick = false;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (BClick)
        {
            // Если объект уже выделен, снимаем выделение
            BClick = false;
            Debug.Log("off");
        }
        else
        {
            // Если объект не выделен, выделяем его
            BClick = true;
            Debug.Log("on");
        }
    }


    public void ButtonClick()
    {
        if (BButtonClick)
        {
            BButtonClick = false;
            Debug.Log("button off");
        }
        else
        {
            BButtonClick = true;
            Debug.Log("button on");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (BClick == true && BButtonClick == true)
        {
            Destroy(this.gameObject);
        }
    }
}
