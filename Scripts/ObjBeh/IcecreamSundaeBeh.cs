using UnityEngine;
using System.Collections;

public class IcecreamSundaeBeh : ProductAssemble {
	
	public tk2dSprite step1;
	public tk2dAnimatedSprite step2;
	public tk2dAnimatedSprite step3;
	public tk2dAnimatedSprite step4;
	public tk2dAnimatedSprite step5;
	public tk2dAnimatedSprite step6;
	
	private string currentSelectedIcecream = string.Empty;
	private string[] icecream_clipNames = new string[IcecreamTankBeh.AMOUNT_OF_ICECREAM_PRODUCT] {
		"Step2_strawberry", "Step2_chocolate", "Step2_vanilla", "Step2_mint", "Step2_greentea",
		"Step2_lemon", "Step2_chocolatechip", "Step2_orange", "Step2_coffee", "Step2_bringcherry",
	};
	private readonly string[] productName = new string[IcecreamTankBeh.AMOUNT_OF_ICECREAM_PRODUCT] {
		"Strawberry", "Chocolate", "Vanilla", "Mint", "Greentea",
		"Lemon", "ChocolateChip", "Orange", "Coffee", "BringCherry",
	} ;
	private	string jamName = string.Empty;
	private string icecreamName = string.Empty;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
				
		step1.gameObject.SetActive(true);
		step2.gameObject.SetActive(false);
		step3.gameObject.SetActive(false);
		step4.gameObject.SetActive(false);
		step5.gameObject.SetActive(false);
		step6.gameObject.SetActive(false);
		
		foreach (BlockIcecreamBeh item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event += Handle_WaitForScoopIcecreamEvent;
		}
		
		base.productPos = new Vector3(-40, 35, 1);
	}

	void Handle_WaitForScoopIcecreamEvent (object sender, IngredientBeh.HandleNameArgs e)
	{		
		foreach (BlockIcecreamBeh item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event -= Handle_WaitForScoopIcecreamEvent;
		}		
		if(this == null) return; 
		
		BlockIcecreamBeh senderBeh = sender as  BlockIcecreamBeh;
		
		int i = IcecreamTankBeh.Instance.dict_nameOfIcecreamBlock[senderBeh.name];
		currentSelectedIcecream = icecream_clipNames[i];
		icecreamName = productName[i];

		step2.gameObject.SetActive(true);
		step2.Play(currentSelectedIcecream);
        step2.animationCompleteDelegate += Handle_Step2PlayComplete;
	}

	void Handle_Step2PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{		
		step2.animationCompleteDelegate -= Handle_Step2PlayComplete;
		if(this == null) return;
		
		IngredientController.Instance.chocolateJam.active_event += Handle_JamActive_event;
		IngredientController.Instance.strawberryJam.active_event += Handle_JamActive_event;
	}

	void Handle_JamActive_event (object sender, IngredientBeh.HandleNameArgs e)
	{		
		IngredientController.Instance.chocolateJam.active_event -= Handle_JamActive_event;
		IngredientController.Instance.strawberryJam.active_event -= Handle_JamActive_event;
		if(this == null) return;

		string targetClipname = string.Empty;
		IngredientBeh jam = sender as IngredientBeh;
		if(jam.name == IngredientController.Instance.chocolateJam.name) {
			targetClipname = "Step3_chocolatejam";
			jamName = "Chocolate";
		}
		else if(jam.name == IngredientController.Instance.strawberryJam.name) {
			targetClipname = "Step3_strawberryjam";
			jamName = "Strawberry";
		}

		step3.gameObject.SetActive(true);
		step3.clipId = step3.GetClipIdByName(targetClipname);
		step3.Play();
		step3.animationCompleteDelegate  += Handle_Step3PlayComplete;
	}

	void Handle_Step3PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{		
		step3.animationCompleteDelegate -= Handle_Step3PlayComplete;
		if(this == null) return;
		
		IngredientController.Instance.whipCream.active_event += Handle_WaitForWhipcream;
	}

	void Handle_WaitForWhipcream (object sender, IngredientBeh.HandleNameArgs e)
	{		
		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipcream;
		if(this == null) return;

		step4.gameObject.SetActive(true);
		step4.Play();
		step4.animationCompleteDelegate += Handle_Step4PlayComplete;
	}

	void Handle_Step4PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{		
		step4.animationCompleteDelegate -= Handle_Step4PlayComplete;
		if(this == null) return;
		
		IngredientController.Instance.almondPowder.active_event += Handle_WaitForAlmondPowder;
	}

	void Handle_WaitForAlmondPowder (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.almondPowder.active_event -= Handle_WaitForAlmondPowder;
		if(this == null) return;

		step5.gameObject.SetActive(true);
		step5.Play();
		step5.animationCompleteDelegate += Handle_Step5PlayComplete;
	}

	void Handle_Step5PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step5.animationCompleteDelegate -= Handle_Step5PlayComplete;
		if(this == null) return;

        if (jamName == "Chocolate")
            IngredientController.Instance.cherry_fruit.active_event += Handle_FruitActive_event;
        else if(jamName == "Strawberry")
            IngredientController.Instance.strawberry_fruit.active_event += Handle_FruitActive_event;
	}

	void Handle_FruitActive_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.strawberry_fruit.active_event -= Handle_FruitActive_event;
		IngredientController.Instance.cherry_fruit.active_event -= Handle_FruitActive_event;
		if(this == null) return;

		step6.gameObject.SetActive(true);
		step6.Play();

		StartCoroutine_Auto(CreateProductAndDestroySelf());
	}

	IEnumerator CreateProductAndDestroySelf ()
	{		
		Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);

		string goodsName = this.GetGoodsName();
		product = GoodsFactory.Instance.GetGoods(goodsName);
        product.gameObject.name = goodsName;
		product.SetOriginTransform(productPos, Vector3.zero);
		product.transform.position = productPos;
		product._canDragaable = true;
        product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[goodsName].costs;
		product.destroyObj_Event += product.Handle_DestroyProduct_Event;
		product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;

		yield return new WaitForFixedUpdate();

		Destroy(this.gameObject, 1f);
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

	protected override string GetGoodsName ()
	{
		string s = "Sundae";
		s = icecreamName + "_" + jamName + s; 

		return s;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	public override void OnDispose ()
	{
		base.OnDispose ();
	}
}
