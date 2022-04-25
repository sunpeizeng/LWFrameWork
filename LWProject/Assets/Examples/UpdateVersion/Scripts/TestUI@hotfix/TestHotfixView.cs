using System.Collections.Generic;
using UnityEngine;
using LWFramework.UI;
using UnityEngine.UI;
using LWFramework.Core;
using LWFramework;

[UIViewData("Assets/@Resources/Prefabs/TestHotfixView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class TestHotfixView : LWFramework.UI.BaseUIView 
{
    [UIElement("Button1")]
    private Button button1 = null;
    [UIElement("Button2", "Assets/@Resources/Sprites/log2.png")]
    private Button button2 = null;
    [UIElement("Text1")]
    private Text text1;
    [UIElement("Parent")]
    private Transform parent;
    [UIElement("Parent2")]
    private Transform parent2;
    /// <summary>
    /// 节点处理  
    /// </summary>
    [UIElement("Parent/TestChildTemplate")]
    private Transform testChildTemplate=null;
    private GameObjectPool<TestChildNode> _pool;
    private List<TestChildNode> _nodes;

    public override void CreateView(GameObject gameObject)
    {
        base.CreateView(gameObject);
        //节点处理
        _pool = new GameObjectPool<TestChildNode>(5,testChildTemplate.gameObject);
        _nodes = new List<TestChildNode>();
        button1.onClick.AddListener(() =>
        {
            CreateNode();
        });

        button2.onClick.AddListener(() =>
        {
            _nodes.ForEach(item => _pool.Unspawn(item));
            _nodes.Clear();
        });


        //可用于拆分逻辑代码
        //TestChildItem2 uIViewBase2 = (TestChildItem2)MainManager.Instance.GetManager<IUIManager>().CreateView<TestChildItem2>(parent2);
        // uIViewBase2._HeadImg.sprite = UIUtility.Instance.GetSprite("Assets/@Resources/Sprites/log3.png");
        // var asset2 = await MainManager.Instance.GetManager<AssetsManager>().LoadAsyncTask<Texture2D>("http://192.168.2.109:8089/Windows/%E9%A6%96%E9%A1%B5.png");
        // Texture2D texture = (Texture2D)asset2.asset;
        //button1.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void CreateNode() {
        for (int i = 0; i < 3; i++)
        {
            TestChildNode testChildNode = _pool.Spawn();
            testChildNode.NameText = "aaaa" + Random.Range(0, 10);
            testChildNode.HeadImgName = "Assets/@Resources/Sprites/log3.png";
            _nodes.Add(testChildNode);
        }
    }
}
