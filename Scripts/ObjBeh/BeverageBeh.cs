using UnityEngine;
using System.Collections;

public class BeverageBeh : ProductAssemble {
	
	public tk2dAnimatedSprite glass;
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		base.productPos = this.transform.position;

        IngredientController.Instance.cola.active_event += Handle_cola_active_event;
        IngredientController.Instance.fruitPunch.active_event += Handle_fruitPunch_active_event;
        IngredientController.Instance.strawberryMillShake.active_event += Handle_strawberryMillShake_active_event;
	}

	void Handle_strawberryMillShake_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.strawberryMillShake.active_event -= Handle_strawberryMillShake_active_event;
		
		glass.Play("StrawberryMillShake");
		glass.animationCompleteDelegate += StrawberryMilkShakePlayComplete;
	}

	void StrawberryMilkShakePlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		glass.animationCompleteDelegate -= StrawberryMilkShakePlayComplete;
		
		StartCoroutine_Auto(this.CreateProductAndDestroySelf("StrawberryMilkShake"));
	}

	void Handle_fruitPunch_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
        IngredientController.Instance.fruitPunch.active_event -= Handle_fruitPunch_active_event;
		
        glass.Play("FruitPunch");
        glass.animationCompleteDelegate += FruitPunchPlayComplete;
	}

    private void FruitPunchPlayComplete(tk2dAnimatedSprite sprite, int clipid) {
        glass.animationCompleteDelegate -= FruitPunchPlayComplete;

        StartCoroutine_Auto(this.CreateProductAndDestroySelf("FruitPunch"));
    }

    void Handle_cola_active_event(object sender, IngredientBeh.HandleNameArgs e) {        
        IngredientController.Instance.cola.active_event -= Handle_cola_active_event;

        glass.Play("Cola");
		glass.animationCompleteDelegate += ColaPlayComplete;
//		glass.animationCompleteDelegate += Handle_IcecreamFloat;
    }

    private void ColaPlayComplete(tk2dAnimatedSprite sprite, int clipId) {    
        glass.animationCompleteDelegate -= ColaPlayComplete;

        StartCoroutine_Auto(this.CreateProductAndDestroySelf("Cola"));
    }

	void Handle_IcecreamFloat (tk2dAnimatedSprite sprite, int clipId)
	{ 
		glass.animationCompleteDelegate -= Handle_IcecreamFloat;
		IcecreamTankBeh.Instance.block_icecreams[2].active_event += Handle_VanillaIcecream_active_event;
	}

    void Handle_VanillaIcecream_active_event (object sender, IngredientBeh.HandleNameArgs e)
    {
		IcecreamTankBeh.Instance.block_icecreams[2].active_event -= Handle_VanillaIcecream_active_event;
		
		glass.Play("IcecreamFloat");
		glass.animationCompleteDelegate = (sprite, clipId) => { 
			StartCoroutine_Auto(this.CreateProductAndDestroySelf("IcecreamFloat"));
		};
    }

	IEnumerator CreateProductAndDestroySelf (string productName)
	{
        Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);

//		string goodsName = this.GetGoodsName();
		product = GoodsFactory.Instance.GetGoods(productName);
        product.gameObject.name = productName;
		product.SetOriginTransform(productPos, Vector3.zero);
		product.transform.position = productPos;
		product._canDragaable = true;
        product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[productName].costs;
		product.destroyObj_Event += product.Handle_DestroyProduct_Event;
		product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;
		
		if(productName == "Cola") {
			product.gameObject.AddComponent<IcecreamFloatBeh>();
		}

		yield return new WaitForFixedUpdate();

		OnDestroyObject_event(System.EventArgs.Empty);
	}
	
	void Handle_PutProductOnTray_Event (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (Shop.Instance.foodTrayBeh.goodsOnTray_List.Contains(obj) == false &&
		    Shop.Instance.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
		{
			Shop.Instance.foodTrayBeh.goodsOnTray_List.Add(obj);
			Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();
			
			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
			
			product = null;
		}
		else
		{
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

    public override void OnDispose()
    {
        base.OnDispose();

        IngredientController.Instance.cola.active_event -= Handle_cola_active_event;
        IngredientController.Instance.fruitPunch.active_event -= Handle_fruitPunch_active_event;
        IngredientController.Instance.strawberryMillShake.active_event -= Handle_strawberryMillShake_active_event;
		glass.animationCompleteDelegate -= Handle_IcecreamFloat;
        IcecreamTankBeh.Instance.block_icecreams[2].active_event -= Handle_VanillaIcecream_active_event;

        glass.animationCompleteDelegate -= ColaPlayComplete;
        glass.animationCompleteDelegate -= FruitPunchPlayComplete;
		glass.animationCompleteDelegate -= StrawberryMilkShakePlayComplete;
    }
}
