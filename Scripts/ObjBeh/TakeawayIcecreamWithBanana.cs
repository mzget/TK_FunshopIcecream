using UnityEngine;
using System.Collections;

public class TakeawayIcecreamWithBanana : ProductAssemble {

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        IngredientController.Instance.banana.active_event += Handle_banana_active_event;
    }

	void Handle_banana_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
        IngredientController.Instance.banana.active_event -= Handle_banana_active_event;
		
		this.sprite.enabled = false;
		this.gameObject.AddComponent<tk2dAnimatedSprite>();
		this.animatedSprite = this.gameObject.GetComponent<tk2dAnimatedSprite>();
		this.animatedSprite.anim = Resources.Load(Const_info.DYNAMIC_ANIMATED_SPRITE_DATA + "TakeawayIcecream_SpriteAnimation", typeof(tk2dSpriteAnimation)) as tk2dSpriteAnimation;
		this.animatedSprite.Play(this.gameObject.name + "_banana");
		this.animatedSprite.animationCompleteDelegate += Delegate_animationComplete;
	}

	private void Delegate_animationComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		animatedSprite.animationCompleteDelegate -= Delegate_animationComplete;
		
        Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);

        string productName = this.gameObject.name + "_banana";
        product = GoodsFactory.Instance.GetGoods(productName);
        product.gameObject.name = productName;
        product.transform.position = this.transform.position;
        product._canDragaable = true;
        product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[productName].costs;
        product.destroyObj_Event += product.Handle_DestroyProduct_Event;
        product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;

        OnDestroyObject_event(System.EventArgs.Empty);
	}

	private void Handle_PutProductOnTray_Event (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
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
        else {
            Debug.LogWarning("Goods on tray have to max capacity.");

            obj.transform.position = obj.originalPosition;
        }
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
	
	public override void OnDispose ()
	{
		base.OnDispose ();
        IngredientController.Instance.banana.active_event -= Handle_banana_active_event;
		animatedSprite.animationCompleteDelegate -= Delegate_animationComplete;
	}
}
