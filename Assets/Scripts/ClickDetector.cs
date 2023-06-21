using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDetector : MonoBehaviour,IPointerDownHandler
{
    public bool BClick = false;
    public Button Button;
    private bool isSpawning = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (BClick)
        {
            // ���� ������ ��� �������, ������� ���������
            BClick = false;
            Debug.Log("off");
        }
        else
        {
            // ���� ������ �� �������, �������� ���
            BClick = true;
            Debug.Log("on");
        }
    }

    private void Start()
    {
        Button.onClick.AddListener(ToggleSpawn);

    }
    public void ToggleSpawn()
    {
        isSpawning = !isSpawning;
    }


    void Update()
    {
        if (BClick == true && isSpawning == true)
        {
            Destroy(this.gameObject);
            isSpawning = false;
        }
    }
}
