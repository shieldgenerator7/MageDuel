using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetArrowDisplayer : MonoBehaviour
{
    public Image imgArrow;

    private RectTransform rectTransform;

    private TargetArrow arrow;
    
    // Start is called before the first frame update
    public void init(TargetArrow arrow)
    {
        rectTransform = GetComponent<RectTransform>();
        this.arrow = arrow;
        transform.position = arrow.startPos;
        imgArrow.color = arrow.color;
        showArrow(true);
    }

    public void showArrow(bool show)
    {
        if (show)
        {
            updatePosition();
        }
        this.gameObject.SetActive(show);
    }

    private void Update()
    {
        updatePosition();
    }

    private void updatePosition()
    {
        transform.up = Vector3.up;
        Vector2 pointer = arrow.Direction;
        Vector2 size = rectTransform.sizeDelta;
        size.y = pointer.magnitude * 2 * 1920 / Camera.main.pixelWidth;//TODO: get this "1920" from the canvas ref width
        rectTransform.sizeDelta = size;
        transform.up = pointer;
    }
}
