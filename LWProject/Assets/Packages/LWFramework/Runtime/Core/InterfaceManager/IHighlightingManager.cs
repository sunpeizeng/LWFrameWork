using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 边缘发光功能
/// </summary>
public interface IHighlightingManager{
    void AddHighlighting(GameObject go, Color color);
    void AddFlashingHighlighting(GameObject go, Color[] colorArray);
    void RemoveHighlighting(GameObject go);

}
