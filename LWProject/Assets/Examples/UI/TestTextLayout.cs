using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTextLayout : MonoBehaviour
{
    public Text t;
    // Start is called before the first frame update
    void Start()
    {
        t.text = "\u3000\u30001、介绍\n\u3000\u3000a、幅度萨芬大师傅大法师发生发放大使 \n\u3000\u30002、说明" +
            "\n\u3000\u3000b、幅度萨芬大师傅大法师发生发放大使幅度萨芬大师傅大法师发生发放大使幅度萨芬大师傅大法师发生发放大使幅度萨芬大师傅大法师发生发放大使";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
