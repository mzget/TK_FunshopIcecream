using UnityEngine;
using System.Collections;

public class BananaSplitSundae : ProductAssemble {

	public tk2dAnimatedSprite step1;
	public tk2dAnimatedSprite step2;
	public tk2dAnimatedSprite step3;
	public tk2dAnimatedSprite step4;
	public tk2dAnimatedSprite step5;
	public tk2dAnimatedSprite step6;
//	public tk2dAnimatedSprite step7;
	
	private string currentSelectedIcecream = string.Empty;
	private string[] icecream_clipNames = new string[] {
		"strawberry", "chocolate", "vanilla", "mint", "greentea",
		"lemon", "chocolatechip", "orange", "coffee", "bringcherry",
	};
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
			
		step1.gameObject.SetActive(true);
		step1.SetSprite("banana_layer7_0001");
		step2.gameObject.SetActive(false);
		step3.gameObject.SetActive(false);
		step4.gameObject.SetActive(false);
		step5.gameObject.SetActive(false);
		step6.gameObject.SetActive(false);
		IngredientController.Instance.banana.active_event += Handle_BananaActive_event;
		
		base.productPos = this.transform.position;
	}

	void Handle_BananaActive_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.banana.active_event -= Handle_BananaActive_event;
		step1.Play();
		step1.animationCompleteDelegate += Handle_Step1PalyComplete;
	}

	void Handle_Step1PalyComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step1.animationCompleteDelegate -= Handle_Step1PalyComplete;
		
		foreach (var item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event += Handle_BlockIcecream_active_event;
		}
	}

	void Handle_BlockIcecream_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		foreach (var item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event -= Handle_BlockIcecream_active_event;
		}
		
		IngredientBeh block = sender as IngredientBeh;
		
		int i = IcecreamTankBeh.Instance.dict_nameOfIcecreamBlock[block.name];
		currentSelectedIcecream = icecream_clipNames[i];
		step2.gameObject.SetActive(true);
		step2.Play(currentSelectedIcecream);
        step2.animationCompleteDelegate += Handle_Step2PlayComplete;
	}

	void Handle_Step2PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step2.animationCompleteDelegate -= Handle_Step2PlayComplete;
		IngredientController.Instance.chocolateJam.active_event += Handle_WaitForChocolateJam;
	}

	void Handle_WaitForChocolateJam (object sender, IngredientBeh.HandleNameArgs e)
	{		
		IngredientController.Instance.chocolateJam.active_event -= Handle_WaitForChocolateJam;
		
		step3.gameObject.SetActive(true);
		step3.Play();
		step3.animationCompleteDelegate += Handle_Step3PlayComplete;
	}

	void Handle_Step3PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step3.animationCompleteDelegate -= Handle_Step3PlayComplete;
		
		IngredientController.Instance.whipCream.active_event += Handle_WaitForWhipCream;
	}

	void Handle_WaitForWhipCream (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCream;
		
		step4.gameObject.SetActive(true);
		step4.Play();
		step4.animationCompleteDelegate += Handle_Step4PlayComplete;
	}

	void Handle_Step4PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step4.animationCompleteDelegate -= Handle_Step4PlayComplete;
		
		IngredientController.Instance.almondPowder.active_event += Handle_WaitForAlmondPowder; 
	}

	void Handle_WaitForAlmondPowder (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.almondPowder.active_event -= Handle_WaitForAlmondPowder;
		
		step5.gameObject.SetActive(true);
		step5.Play();
		step5.animationCompleteDelegate += Handle_Step5PlayComplete;
	}

	void Handle_Step5PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		step5.animationCompleteDelegate -= Handle_Step5PlayComplete;
		IngredientController.Instance.cherry_fruit.active_event += Handle_WaitForCherryFruit;
	}

	void Handle_WaitForCherryFruit (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.cherry_fruit.active_event -= Handle_WaitForCherryFruit;
		step6.gameObject.SetActive(true);
		step6.Play();
		
		StartCoroutine_Auto(this.CreateProductAndDestroySelf());
	}	
			

	IEnumerator CreateProductAndDestroySelf ()
	{
        Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);
	
		string goodname = this.GetGoodsName();
		product = GoodsFactory.Instance.GetGoods(goodname);
		product.gameObject.name = goodname;
		product.SetOriginTransform(productPos, Vector3.zero);
		product.transform.position = productPos;
		product._canDragaable = true;
		product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[goodname].costs;
		product.destroyObj_Event += product.Handle_DestroyProduct_Event;
		product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;

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
	
	protected override string GetGoodsName ()
	{
		string s = string.Empty;
		s = "BananaSplitSundae_" + currentSelectedIcecream;
		
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
		IngredientController.Instance.banana.active_event -= Handle_BananaActive_event;
		step1.animationCompleteDelegate -= Handle_Step1PalyComplete;
		foreach (var item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event -= Handle_BlockIcecream_active_event;
		}
		step2.animationCompleteDelegate -= Handle_Step2PlayComplete;
		IngredientController.Instance.chocolateJam.active_event -= Handle_WaitForChocolateJam;
		step3.animationCompleteDelegate -= Handle_Step3PlayComplete;
		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCream;
		step4.animationCompleteDelegate -= Handle_Step4PlayComplete;
		IngredientController.Instance.almondPowder.active_event -= Handle_WaitForAlmondPowder;
		step5.animationCompleteDelegate -= Handle_Step5PlayComplete;
		IngredientController.Instance.cherry_fruit.active_event -= Handle_WaitForCherryFruit;
	}
}
