using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    public Image mask;
    float originalSize;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
    }

    void Start()
    {
 
        originalSize = mask.rectTransform.rect.width;
        SetValue(LoadBuffer.currentHealthRuby / (float)5f);
    }

 public void SetValue (float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
