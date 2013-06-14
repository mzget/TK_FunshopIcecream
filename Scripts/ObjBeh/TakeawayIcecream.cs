using UnityEngine;
using System.Collections;

public class TakeawayIcecream : ProductAssemble {

    public tk2dAnimatedSprite step1_icecream;
    public tk2dAnimatedSprite step2_jam;
    public tk2dAnimatedSprite step3_whipcream;
    public tk2dAnimatedSprite step4_topping;
    public tk2dAnimatedSprite step5_dressing;
    public tk2dAnimatedSprite banana;
	
	private int icecream_id;
	private	string jam_clipname = "";
	private string topping_clipname = "";
	private string dressing_clipname = "";
	
	private readonly string[] icecreams_animationData = new string[IcecreamTankBeh.AMOUNT_OF_ICECREAM_PRODUCT] {
		"strawberry", "chocolate", "vanilla", "mint", "greentea",
		"lemon", "chocolatechip", "orange", "coffee", "bringcherry",
	} ;

	// Use this for initialization
	protected override void Start () {
		base.Start();

        step1_icecream.gameObject.SetActive(false);
		step2_jam.gameObject.SetActive(false);
		step3_whipcream.gameObject.SetActive(false);
		step4_topping.gameObject.SetActive(false);
		step5_dressing.gameObject.SetActive(false);
		banana.gameObject.SetActive(false);
	    base.productPos = this.transform.position;
		
		foreach (IngredientBeh item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event += Handle_BlockIcecream_active_event;
		}
	}

	void Handle_BlockIcecream_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		foreach (IngredientBeh item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event -= Handle_BlockIcecream_active_event;
		}
		
		IngredientBeh icecream = sender as IngredientBeh;
		icecream_id = IcecreamTankBeh.Instance.dict_nameOfIcecreamBlock[icecream.gameObject.name];
		
		step1_icecream.gameObject.SetActive(true);
		step1_icecream.Play(icecreams_animationData[icecream_id]);
        step1_icecream.animationCompleteDelegate += Handle_step1_icecream_playcomplete;
	}

    private void Handle_step1_icecream_playcomplete(tk2dAnimatedSprite sprite, int clipId) {
        step1_icecream.animationCompleteDelegate -= Handle_step1_icecream_playcomplete;

        IngredientController.Instance.chocolateJam.active_event += Handle_Jam_active;
        IngredientController.Instance.strawberryJam.active_event += Handle_Jam_active;
    }

	void Handle_Jam_active (object sender, IngredientBeh.HandleNameArgs e)
	{
        IngredientController.Instance.chocolateJam.active_event -= Handle_Jam_active;
        IngredientController.Instance.strawberryJam.active_event -= Handle_Jam_active;
		
		IngredientBeh obj = sender as IngredientBeh;
		if(obj.name == "ChocolateJam") {
			jam_clipname = "chocolateJam";
		}
		else if(obj.name == "StrawberryJam") {
			jam_clipname = "strawberryJam";
		}
		
		step2_jam.gameObject.SetActive(true);
		step2_jam.Play(jam_clipname);
		step2_jam.animationCompleteDelegate += Handle_step3;
	}

	void Handle_step3 (tk2dAnimatedSprite sprite, int clipId)
	{
		step2_jam.animationCompleteDelegate -= Handle_step3;
		
		IngredientController.Instance.whipCream.active_event += Handle_WaitForWhipCream;
	}

	void Handle_WaitForWhipCream (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCream;
		
		step3_whipcream.gameObject.SetActive(true);
		step3_whipcream.Play();
		
		if(jam_clipname == "chocolateJam") 
			IngredientController.Instance.almondPowder.active_event += Handle_Topping_active_event;
		else if(jam_clipname == "strawberryJam")
			IngredientController.Instance.fancy_sugar.active_event += Handle_Topping_active_event;
	}

	void Handle_Topping_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.almondPowder.active_event -= Handle_Topping_active_event;
		IngredientController.Instance.fancy_sugar.active_event -= Handle_Topping_active_event;
		
		if(e.eventName == IngredientController.Instance.fancy_sugar.name) {
			topping_clipname = "sugar";
		}
		else if(e.eventName == IngredientController.Instance.almondPowder.name) {
			topping_clipname = "almond";
		}
		
		step4_topping.gameObject.SetActive(true);
		step4_topping.Play(topping_clipname);
		
		if(jam_clipname == "chocolateJam")
			IngredientController.Instance.cherry_fruit.active_event += Handle_dressing_active_event;
		else if(jam_clipname == "strawberryJam")
		IngredientController.Instance.strawberry_fruit.active_event += Handle_dressing_active_event;
	}

	void Handle_dressing_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.cherry_fruit.active_event -= Handle_dressing_active_event;
		IngredientController.Instance.strawberry_fruit.active_event -= Handle_dressing_active_event;
		
		if(e.eventName == IngredientController.Instance.cherry_fruit.name) {
			dressing_clipname = "cherryFruit";
		}
		else if(e.eventName == IngredientController.Instance.strawberry_fruit.name) {
			dressing_clipname = "strawberryFruit";
		}
		
		step5_dressing.gameObject.SetActive(true);
		step5_dressing.Play(dressing_clipname);
        step5_dressing.animationCompleteDelegate += Handle_step5_dressing_complete;
	}

    private void Handle_step5_dressing_complete(tk2dAnimatedSprite sprite, int clipid)
    {
        step5_dressing.animationCompleteDelegate -= Handle_step5_dressing_complete;

        StartCoroutine_Auto(CreateProductAndDestroySelf());
    }

    IEnumerator CreateProductAndDestroySelf()
    {
        Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);

        string productName = this.GetGoodsName();
        product = GoodsFactory.Instance.GetGoods(productName);
        product.gameObject.name = productName;
        product.transform.position = productPos;
		product.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        product._canDragaable = true;
        product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[productName].costs;
        product.destroyObj_Event += product.Handle_DestroyProduct_Event;
        product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;
		
		product.gameObject.AddComponent<TakeawayIcecreamWithBanana>();
		
        yield return new WaitForFixedUpdate();

        OnDestroyObject_event(System.EventArgs.Empty);
    }

 	protected override string GetGoodsName ()
	{
		string s = "";
		s = "TakeawayIcecream_" + icecreams_animationData[icecream_id] + "_" + jam_clipname + "_" + topping_clipname + "_" + dressing_clipname;
		
		return s;
	}

    void Handle_PutProductOnTray_Event(object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
    {
        GoodsBeh obj = sender as GoodsBeh;
        if (Shop.Instance.foodTrayBeh.goodsOnTray_List.Contains(obj) == false &&
            Shop.Instance.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
        {
            Shop.Instance.foodTrayBeh.goodsOnTray_List.Add(obj);
            Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();

            //<!-- Setting original position.
            obj.originalPosition = obj.transform.position;
			
			TakeawayIcecreamWithBanana get_Type = obj.gameObject.GetComponent<TakeawayIcecreamWithBanana>();
			if(get_Type != null) {
				get_Type.enabled = false;
                IngredientController.Instance.banana.active_event -= get_Type.Handle_banana_active_event;
			}

            product = null;
        }
        else {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
    }
	
	public override void OnDispose ()
	{
		base.OnDispose ();
		
		foreach (IngredientBeh item in IcecreamTankBeh.Instance.block_icecreams) {
			item.active_event -= Handle_BlockIcecream_active_event;
		}
        step1_icecream.animationCompleteDelegate -= Handle_step1_icecream_playcomplete;
        IngredientController.Instance.chocolateJam.active_event -= Handle_Jam_active;
        IngredientController.Instance.strawberryJam.active_event -= Handle_Jam_active;
		step2_jam.animationCompleteDelegate -= Handle_step3;
		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCream;
		IngredientController.Instance.almondPowder.active_event -= Handle_Topping_active_event;
		IngredientController.Instance.fancy_sugar.active_event -= Handle_Topping_active_event;
		IngredientController.Instance.cherry_fruit.active_event -= Handle_dressing_active_event;
        IngredientController.Instance.strawberry_fruit.active_event -= Handle_dressing_active_event;
        step5_dressing.animationCompleteDelegate -= Handle_step5_dressing_complete;
	}
}
