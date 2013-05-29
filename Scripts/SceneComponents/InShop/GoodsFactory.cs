using UnityEngine;
using System.Collections;

public class GoodsFactory : ScriptableObject {
	
	private static GoodsFactory instance;
	public static GoodsFactory Instance {
		get {
			if(instance == null) {
				instance = ScriptableObject.CreateInstance<GoodsFactory>();
			}
			return instance;
		}
	}

	public enum FactoryState { 
		Idle = 0, Init, Create,
	};
	public FactoryState currentFactoryState = FactoryState.Idle;

	public GameObject goodsPrefab;

	private void OnEnable() {
		goodsPrefab = Resources.Load("Goods/Goods", typeof(GameObject)) as GameObject;
	}


	// Use this for initialization
	public GoodsBeh GetGoods(string goodsName) {
		GameObject g = null;
		GoodsBeh goods = null;
		tk2dSprite sprite = null;

		g = Instantiate(goodsPrefab) as GameObject;
		goods = g.GetComponent<GoodsBeh>();
		sprite = g.GetComponent<tk2dSprite>();

		sprite.spriteId = sprite.GetSpriteIdByName(goodsName);
		return goods;
	}
}
