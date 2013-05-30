using UnityEngine;
using System.Collections;

public class FreshyFreezeBeh : ProductAssemble {
	
	private enum StateBeh { idle = 0, step1, waitForStep2, step2, waitForStep3, step3, waitForStep4, step4, complete, };  
	private StateBeh currentStateBeh = StateBeh.idle;
	
	public Mz_GuiButtonBeh popup;
	public tk2dAnimatedSprite layer1;
	public tk2dAnimatedSprite layer2;
	public tk2dAnimatedSprite layer3;
	public tk2dAnimatedSprite layer4;
	public tk2dAnimatedSprite layer5;
	public tk2dAnimatedSprite layer6;
	public tk2dAnimatedSprite layer7;
	internal readonly string[] nameOfAnimationClip_Step3 = new string[] {
		"Step3_blueberry", "Step3_cherry", "Step3_dragonfruit", "Step3_kiwi", "Step3_mango", "Step3_strawberry",
	};
	internal readonly string[] namesOfAnimationClip_Step4_2 = new string[] {
		"Step4_2_blueberry", "Step4_2_cherry", "Step4_2_dragonfruit", "Step4_2_kiwi", "Step4_2_mango", "Step4_2_strawberry",
	};
	internal string[] nameOfAnimationClip_Step4_3_S = new string[] {
		"Step4_3_S_blueberry", "Step4_3_S_cherry", "Step4_3_S_dragonfruit", "Step4_3_S_kiwi", "Step4_3_S_mango", "Step4_3_S_strawberry",
	};
	internal string[] nameOfAnimationClip_Step4_3_C = new string[] {
		"Step4_3_C_blueberry", "Step4_3_C_cherry", "Step4_3_C_dragonfruit", "Step4_3_C_kiwi", "Step4_3_C_mango", "Step4_3_C_strawberry",
	};
	private int currentJam_id = 0;
	private int currentFruit_id = 0;
	internal string[] nameOfFruits = new string[] {
		"blueberry", "cherry", "dragonfruit", "kiwi", "mango", "strawberry",
	};

	// Use this for initialization
	private new void Start () // this product assemble is no handle destroy event.
    {
        this.Initialize();
		
        base.arr_exceptionObjectsName.Add(this.name);
        base.arr_exceptionObjectsName.Add(popup.name);
        popup.click_event += Handle_popup_click_event;
	}

	private void Handle_popup_click_event (object sender, System.EventArgs e)
	{
        this.RemoveEventsHandle();
		this.Initialize();
		this.SetActiveClearPopup(false);
	}

    private void Initialize()
    {
        layer1.gameObject.SetActive(true);
        layer2.gameObject.SetActive(false);
        layer3.gameObject.SetActive(false);
        layer4.gameObject.SetActive(false);
        layer5.gameObject.SetActive(false);
        layer6.gameObject.SetActive(false);
        layer7.gameObject.SetActive(false);
        layer1.clipId = layer1.GetClipIdByName("Step1");
        layer1.Play();
        layer1.StopAndResetFrame();

        currentStateBeh = StateBeh.idle;
        productPos = new Vector3(76, -22, -1);
    }
	
	internal void PlayLayer1() {
		layer1.Play();
		layer1.animationCompleteDelegate += Handle_Layer1AnimatedComplete;
	}

	public void Handle_Layer1AnimatedComplete (tk2dAnimatedSprite sprite, int clipId)
	{
		this.currentStateBeh = StateBeh.waitForStep2;
		
		layer1.animationCompleteDelegate -= Handle_Layer1AnimatedComplete;
		IngredientController.Instance.chocolateJam.active_event += Handle_jam_active_event;
		IngredientController.Instance.strawberryJam.active_event += Handle_jam_active_event;
	}	
	
	internal const string ChocolateJam_ClipName = "Step2_C";
	internal const string StrawberryJam_ClipName = "Step2_S";
	void Handle_jam_active_event (object sender, IngredientBeh.HandleNameArgs e)
	{
		if(e.eventName == IngredientController.Instance.chocolateJam.name) {
			currentJam_id = 0;
			this.PlayLayer2(ChocolateJam_ClipName);
		}
		else if(e.eventName == IngredientController.Instance.strawberryJam.name) {
			currentJam_id = 1;
			this.PlayLayer2(StrawberryJam_ClipName);
		}
		else if(e.eventName == IngredientController.Instance.blueberry_fruit.name) {
			currentFruit_id = 0;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);
		}
		else if(e.eventName == IngredientController.Instance.cherry_fruit.name) {
			currentFruit_id = 1;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);
		}
		else if(e.eventName == IngredientController.Instance.dragon_fruit.name) {
			currentFruit_id = 2;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);
		}
		else if(e.eventName == IngredientController.Instance.kiwi_fruit.name) {
			currentFruit_id = 3;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);
		}
		else if(e.eventName == IngredientController.Instance.mango_fruit.name) {
			currentFruit_id = 4;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);	
		}
		else if(e.eventName == IngredientController.Instance.strawberry_fruit.name) {
			currentFruit_id = 5;
			this.PlayLayer3(nameOfAnimationClip_Step3[currentFruit_id]);
		}
	}

    internal void PlayLayer2(string nameOfAnimation) {
        layer2.gameObject.SetActive(true);
		layer2.clipId = layer2.GetClipIdByName(nameOfAnimation);
        layer2.Play();

		layer2.animationCompleteDelegate += Handle_Layer2AnimationComplete;
    }

    void Handle_Layer2AnimationComplete (tk2dAnimatedSprite sprite, int clipId)
    {
		layer2.animationCompleteDelegate -= Handle_Layer2AnimationComplete;
		IngredientController.Instance.chocolateJam.active_event -= Handle_jam_active_event;
		IngredientController.Instance.strawberryJam.active_event -= Handle_jam_active_event;

		IngredientController.Instance.blueberry_fruit.active_event += Handle_jam_active_event;
		IngredientController.Instance.cherry_fruit.active_event += Handle_jam_active_event;
		IngredientController.Instance.dragon_fruit.active_event += Handle_jam_active_event;
		IngredientController.Instance.kiwi_fruit.active_event += Handle_jam_active_event;
		IngredientController.Instance.mango_fruit.active_event += Handle_jam_active_event;
		IngredientController.Instance.strawberry_fruit.active_event += Handle_jam_active_event;
    }
	
	//<@-- Requite clip name of step 3.
    internal void PlayLayer3(string message) {		
        layer3.gameObject.SetActive(true);
		layer3.clipId = layer3.GetClipIdByName(message);
        layer3.Play();
		layer3.animationCompleteDelegate += Handle_Step3AnimationComplete;
    }

	private void Handle_Step3AnimationComplete(tk2dAnimatedSprite sprite, int clipId) {
		layer3.animationCompleteDelegate -= Handle_Step3AnimationComplete;
		IngredientController.Instance.blueberry_fruit.active_event -= Handle_jam_active_event;
		IngredientController.Instance.cherry_fruit.active_event -= Handle_jam_active_event;
		IngredientController.Instance.dragon_fruit.active_event -= Handle_jam_active_event;
		IngredientController.Instance.kiwi_fruit.active_event -= Handle_jam_active_event;
		IngredientController.Instance.mango_fruit.active_event -= Handle_jam_active_event;
		IngredientController.Instance.strawberry_fruit.active_event -= Handle_jam_active_event;

		string animationClipName = "";
		if(currentJam_id == 0) 
			animationClipName = nameOfAnimationClip_Step4_3_C[currentFruit_id];
		else if(currentJam_id == 1) 
			animationClipName = nameOfAnimationClip_Step4_3_S[currentFruit_id];

		PlayStep4(namesOfAnimationClip_Step4_2[currentFruit_id], animationClipName);
	}
	
	//<@-- Requite clip name of step 4 layer 2.
	//<@-- Requite clip name of step 4 layer 3.
    internal void PlayStep4(string step4_2, string step4_3) {
        layer1.clipId = layer1.GetClipIdByName("Step4_1");
        layer1.Play();

		layer5.gameObject.SetActive(true);
		layer5.Play();

		layer3.clipId = layer3.GetClipIdByName(step4_2);
        layer3.Play();
		layer3.animationCompleteDelegate += Handle_Layer3AnimatedComplete;

		layer4.gameObject.SetActive(true);
		layer4.clipId = layer4.GetClipIdByName(step4_3);
		layer4.Play();
		layer4.animationCompleteDelegate += Handle_Step4AnimatedComplete;
    }
	
	private void Handle_Layer3AnimatedComplete(tk2dAnimatedSprite sprite, int clipId) {
		layer3.gameObject.SetActive(false);
		layer2.gameObject.SetActive(false);
		layer3.animationCompleteDelegate -= Handle_Layer3AnimatedComplete;
	}

	private void Handle_Step4AnimatedComplete(tk2dAnimatedSprite sprite, int clipId) {
		layer4.animationCompleteDelegate -= Handle_Step4AnimatedComplete;
		IngredientController.Instance.whipCream.active_event += Handle_WaitForWhipCreamActive;
		IngredientController.Instance.whipCream.animatedSpritePlayCompleteEvent += Handle_whipCreamanimatedSpritePlayCompleteEvent;
	}

	void Handle_whipCreamanimatedSpritePlayCompleteEvent (object sender, System.EventArgs e)
	{
		IngredientController.Instance.whipCream.animatedSpritePlayCompleteEvent -= Handle_whipCreamanimatedSpritePlayCompleteEvent;
		
        IngredientController.Instance.whipCream.transform.position = IngredientController.Instance.whipCream.originalPosition;
	}

	void Handle_WaitForWhipCreamActive (object sender, IngredientBeh.HandleNameArgs e) {
        IngredientController.Instance.whipCream.transform.position = this.transform.position + new Vector3(0, 20, -6);
		layer6.gameObject.SetActive(true);
		layer6.Play();

		IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCreamActive;
		IngredientController.Instance.almondPowder.active_event += Handle_WaitForAlmondPowderActive;
	}

	void Handle_WaitForAlmondPowderActive (object sender, IngredientBeh.HandleNameArgs e)
	{
		IngredientController.Instance.almondPowder.active_event -= Handle_WaitForAlmondPowderActive;

		layer7.gameObject.SetActive(true);
		layer7.Play();

        Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);
		
		string goodsName = this.GetGoodsName();
		product = GoodsFactory.Instance.GetGoods(goodsName);
		product.gameObject.name = goodsName;
		product.transform.position = productPos;
		product.transform.localScale = new Vector3(1.2f, 1.2f, 1);
		product._canDragaable = true;
		product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[goodsName].costs;
		product.destroyObj_Event += product.Handle_DestroyProduct_Event;
		product.putObjectOnTray_Event += Handle_PutProductOnTray_Event;

		this.Initialize();
	}
	
	protected override string GetGoodsName ()
	{
		string s = string.Empty;
		s = "FreshyFreeze_"; 
		if(currentJam_id == 0) s += "C_";
		else if(currentJam_id == 1) s += "S_";

		s += nameOfFruits[currentFruit_id];
		
		return s;
	}
	
	#region <!-- input handle.
	
	protected override void OnTouchDown()
	{
		base.OnTouchDown ();
		
		if(currentStateBeh == StateBeh.idle && product == null) {
			this.PlayLayer1();
			this.currentStateBeh = StateBeh.step1;
		}
	}
	
	protected override void OnTouchStationary ()
	{
		base.OnTouchStationary ();
		
		this.SetActiveClearPopup(true);
		Mz_BaseScene.GetInstance.audioEffect.PlayOnecSound(Mz_BaseScene.GetInstance.audioEffect.pop_clip);
	}
	
	protected override void OnTouchOther()
	{
		base.OnTouchOther();

		this.SetActiveClearPopup(false);
	}
	
	#endregion

	protected void SetActiveClearPopup(bool state)
	{
		popup.gameObject.SetActive(state);
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
		else
		{
			Debug.LogWarning("Goods on tray have to max capacity.");
			
			obj.transform.position = obj.originalPosition;
		}
	}

    private void RemoveEventsHandle() {
        layer1.animationCompleteDelegate -= Handle_Layer1AnimatedComplete;
        IngredientController.Instance.chocolateJam.active_event -= Handle_jam_active_event;
        IngredientController.Instance.strawberryJam.active_event -= Handle_jam_active_event;
        layer3.animationCompleteDelegate -= Handle_Step3AnimationComplete;
        IngredientController.Instance.blueberry_fruit.active_event -= Handle_jam_active_event;
        IngredientController.Instance.cherry_fruit.active_event -= Handle_jam_active_event;
        IngredientController.Instance.dragon_fruit.active_event -= Handle_jam_active_event;
        IngredientController.Instance.kiwi_fruit.active_event -= Handle_jam_active_event;
        IngredientController.Instance.mango_fruit.active_event -= Handle_jam_active_event;
        IngredientController.Instance.strawberry_fruit.active_event -= Handle_jam_active_event;
        layer3.animationCompleteDelegate -= Handle_Layer3AnimatedComplete;
        layer4.animationCompleteDelegate -= Handle_Step4AnimatedComplete;
        IngredientController.Instance.whipCream.active_event -= Handle_WaitForWhipCreamActive;
		IngredientController.Instance.whipCream.animatedSpritePlayCompleteEvent -= Handle_whipCreamanimatedSpritePlayCompleteEvent;
        IngredientController.Instance.almondPowder.active_event -= Handle_WaitForAlmondPowderActive;
    }
}
