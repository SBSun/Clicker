using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResolution : MonoBehaviour
{
    public RectTransform canvas;
    private RectTransform rectTransform;
    private Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2( canvas.rect.width, canvas.rect.height );
    }
}
