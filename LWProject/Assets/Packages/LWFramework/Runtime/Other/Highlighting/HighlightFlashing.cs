using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HighlightFlashing : MonoBehaviour
{
    private HighlightEffect m_HighlightEffect;
    private Color[] m_ColorArray;
    public Color[] ColorArray {
        set => m_ColorArray = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
       m_HighlightEffect = gameObject.GetComponent<HighlightEffect>();
              
    }

    // Update is called once per frame
    void Update()
    {
        m_HighlightEffect.outlineColor = Color.Lerp(m_ColorArray[0], m_ColorArray[1], Mathf.PingPong(Time.time, 1));
    }
}
