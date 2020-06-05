using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    private bool isRotating;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public bool isVisable()
    {
        if(this.GetComponent<CanvasGroup>().alpha > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool GetRotationSelected()
    {
        return isRotating;
    }

    public bool GetMoveSelected()
    {
        return isMoving;
    }

    public void Hide()
    {
        this.GetComponent<CanvasGroup>().alpha = 0f; //this makes everything transparent
        this.GetComponent<CanvasGroup>().blocksRaycasts = false; //this prevents the UI element to receive input events
    }
    public void Show()
    {
        this.GetComponent<CanvasGroup>().alpha = 1f;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
