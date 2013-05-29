using UnityEngine;
using System.Collections;

public class IcecreamFloatBeh : ProductAssemble {

	public tk2dAnimatedSprite glass_anim;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		IcecreamTankBeh.Instance.block_icecreams[2].active_event += Handle_WaitForVanillaIcecream;

		base.productPos = this.transform.position;
	}

	void Handle_WaitForVanillaIcecream (object sender, IngredientBeh.HandleNameArgs e)
	{
		IcecreamTankBeh.Instance.block_icecreams[2].active_event -= Handle_WaitForVanillaIcecream;
		this.sprite.enabled = false;
		this.gameObject.AddComponent<tk2dAnimatedSprite>();
		glass_anim = this.gameObject.GetComponent<tk2dAnimatedSprite>();
		glass_anim.anim = Resources.Load(Const_info.DYNAMIC_ANIMATED_SPRITE_DATA + "Beverage_SpriteAnimation", typeof(tk2dSpriteAnimation)) as tk2dSpriteAnimation;
		glass_anim.Play("IcecreamFloat");
		glass_anim.animationCompleteDelegate += Handle_PlayComplete;
	}

	void Handle_PlayComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		glass_anim.animationCompleteDelegate -= Handle_PlayComplete;
		
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
		return "IcecreamFloat";
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	public override void OnDispose ()
	{
		base.OnDispose ();
		
		IcecreamTankBeh.Instance.block_icecreams[2].active_event -= Handle_WaitForVanillaIcecream;
		glass_anim.animationCompleteDelegate -= Handle_PlayComplete;
	}
}
