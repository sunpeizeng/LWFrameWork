using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
public class HighlightingPlusManager : IHighlightingManager,IManager
{
    HighlightProfile m_HighlightProfile;
    public void Init()
    {
        m_HighlightProfile = Resources.Load<HighlightProfile>("HighlightPlusFastProfile");
    }
    public void Update()
    {
    }
    public void AddFlashingHighlighting(GameObject p_Go, Color[] p_ColorArray)
    {
        HighlightEffect highlightEffect = p_Go.GetComponent<HighlightEffect>();
        if (!highlightEffect)
            highlightEffect = p_Go.AddComponent<HighlightEffect>();
        highlightEffect.ProfileLoad(m_HighlightProfile);
        highlightEffect.highlighted = true;
        HighlightFlashing highlightFlashing = p_Go.GetComponent<HighlightFlashing>();
        if (!highlightFlashing)
            highlightFlashing = p_Go.AddComponent<HighlightFlashing>();
        highlightFlashing.ColorArray = p_ColorArray;
    }

    public void AddHighlighting(GameObject p_Go, Color p_Color)
    {
        HighlightEffect highlightEffect = p_Go.GetComponent<HighlightEffect>();
        if (!highlightEffect)
            highlightEffect = p_Go.AddComponent<HighlightEffect>();
        highlightEffect.ProfileLoad(m_HighlightProfile);
        highlightEffect.outlineColor = p_Color;
        highlightEffect.highlighted = true;
       
    } 
    public void RemoveHighlighting(GameObject p_Go)
    {
        GameObject.DestroyImmediate(p_Go.GetComponent<HighlightEffect>());
        if (p_Go.GetComponent<HighlightFlashing>()) {
            GameObject.DestroyImmediate(p_Go.GetComponent<HighlightFlashing>());
        }
    }

   
}
