using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OvenBeh : Base_ObjectBeh {

    private static OvenBeh instance;
    public static OvenBeh Instance {
        get {
            if (instance == null) {
				if(Shop.Instance.overBeh == null) 
					Debug.LogError("Shop.Instance.overBeh == null");
				
                instance = Shop.Instance.overBeh;
            }
            return instance;
        }
    }
	
	private tk2dAnimatedSprite oven_spriteAnimate;
	private enum OvenAnimationState { open = 0, close = 1, };
	private OvenAnimationState currentAnimationState = OvenAnimationState.close;
	
	private string oven_open_clipName = "oven_open";
	private string oven_close_clipName = "oven_close";

	public Mz_GuiButtonBeh banana_covered_chocolate_button;
	private GoodsBeh banana_covered_choco_product;

	public Transform[] arr_of_cake_transforms;
    private GoodsBeh[] cake_products = new GoodsBeh[3];
	
	void Awake() {
		oven_spriteAnimate = this.gameObject.GetComponent<tk2dAnimatedSprite>();
	}
	
	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		StartCoroutine_Auto(this.IE_Initialize());
    }

	protected IEnumerator IE_Initialize ()
	{		
		banana_covered_chocolate_button.click_event += Handle_bananacoverChocolate_buttonDown_event; 
        yield return StartCoroutine_Auto(this.CreateIcecreamCake());
		
		base.implementUserTouchOther = new ImplementUserTouchOther();
		base.implementUserTouchOther.arr_exceptionObjectsName.Add(this.name);
		base.implementUserTouchOther.arr_exceptionObjectsName.Add(banana_covered_chocolate_button.gameObject.name);
		foreach (var item in cake_products) {
			base.implementUserTouchOther.arr_exceptionObjectsName.Add(item.gameObject.name);
		}
	}

    private IEnumerator CreateIcecreamCake()
    {
        yield return new WaitForFixedUpdate();

        for (int i = 0; i < 3; i++)
        {
            if (cake_products[i] == null)
            {
                string goodsName = GoodDataStore.FoodMenuList.IcecreamCake.ToString();
                cake_products[i] = GoodsFactory.Instance.GetGoods(goodsName);
                cake_products[i].gameObject.name = goodsName;
                cake_products[i].transform.position = arr_of_cake_transforms[i].position;
                cake_products[i]._canDragaable = true;
                cake_products[i].costs = Shop.Instance.goodDataStore.dict_FoodDatabase[goodsName].costs;
                cake_products[i].destroyObj_Event += this.Handle_DestroyProduct_Event;
                cake_products[i].putObjectOnTray_Event += this.Handle_putObjectOnTray_Event;
                cake_products[i].index_of_instance = i;
            }
        }
    }

    void Handle_bananacoverChocolate_buttonDown_event (object sender, System.EventArgs e)
    {		
		if(banana_covered_choco_product == null) {
			Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
			Shop.Instance.audioEffect.PlayOnecSound(Shop.Instance.audioEffect.longBring_clip);

			string goodname = GoodDataStore.FoodMenuList.BananaCoveredWithChocolate.ToString();
			banana_covered_choco_product = GoodsFactory.Instance.GetGoods(goodname);
			banana_covered_choco_product.gameObject.name = goodname;
			banana_covered_choco_product.transform.position = banana_covered_chocolate_button.transform.position + new Vector3(0, 8, -5);
            banana_covered_choco_product.transform.localScale = new Vector3(1.5f, 1.5f, 1);
			banana_covered_choco_product._canDragaable = true;
			banana_covered_choco_product.costs = Shop.Instance.goodDataStore.dict_FoodDatabase[goodname].costs;
			banana_covered_choco_product.destroyObj_Event += banana_covered_choco_product.Handle_DestroyProduct_Event;
			banana_covered_choco_product.putObjectOnTray_Event += Handle_bananaCoveredChoco_putObjectOnTray_Event;
    	}
		
		if(Mz_StorageManage._HasNewGameEvent == true) {
			Shop.Instance.SetActivateTotorObject(false);
			Shop.Instance.CreateDragGoodsToTrayTutorEvent();
			this.transform.position += Vector3.forward * 28f;
		}
	}

    void Handle_bananaCoveredChoco_putObjectOnTray_Event (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (Shop.Instance.foodTrayBeh.goodsOnTray_List.Contains(obj) == false &&
		    Shop.Instance.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
		{
			Shop.Instance.foodTrayBeh.goodsOnTray_List.Add(obj);
			Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;

			banana_covered_choco_product = null;
		}
		else
		{
			Debug.LogWarning("Goods on tray have to max capacity.");

			obj.transform.position = obj.originalPosition;
		}
    }

    void Handle_putObjectOnTray_Event (object sender, GoodsBeh.PutGoodsToTrayEventArgs e)
	{
		GoodsBeh obj = sender as GoodsBeh;
		if (Shop.Instance.foodTrayBeh.goodsOnTray_List.Contains(obj) == false &&
		    Shop.Instance.foodTrayBeh.goodsOnTray_List.Count < FoodTrayBeh.MaxGoodsCapacity)
		{
			Shop.Instance.foodTrayBeh.goodsOnTray_List.Add(obj);
			Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();

			//<!-- Setting original position.
			obj.originalPosition = obj.transform.position;
            obj.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            obj.SetOriginTransform(obj.originalPosition, obj.transform.localScale);

            cake_products[obj.index_of_instance] = null;
            StartCoroutine_Auto(this.CreateIcecreamCake());
		}
		else
		{
			Debug.LogWarning("Goods on tray have to max capacity.");

			obj.transform.position = obj.originalPosition;
		}
    }

    void Handle_DestroyProduct_Event(object sender, System.EventArgs e)
    {
        Debug.Log("Handle_DestroyProduct_Event : " + sender.ToString());

        GoodsBeh goods = sender as GoodsBeh;
        Mz_StorageManage.AvailableMoney -= goods.costs;
        Shop.Instance.CreateDeductionsCoin(goods.costs);
        Shop.Instance.ReFreshAvailableMoney();
        Shop.Instance.foodTrayBeh.goodsOnTray_List.Remove(goods);
        Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();

        StartCoroutine_Auto(this.CreateIcecreamCake());
    }
	
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();
		
		if(currentAnimationState == OvenAnimationState.close) {
			oven_spriteAnimate.Play(oven_open_clipName);
			currentAnimationState = OvenAnimationState.open;
			
			if(Mz_StorageManage._HasNewGameEvent == true) {
				Shop.Instance.TapToCreateGoods_TutorEvent();
			}
		}
 		else if(currentAnimationState == OvenAnimationState.open) { 
			oven_spriteAnimate.Play(oven_close_clipName);
			currentAnimationState = OvenAnimationState.close;
		}
	}
	
	protected override void OnTouchOther ()
	{
		base.OnTouchOther ();
		//<!-- Close oven when user touch other.
		if(currentAnimationState == OvenAnimationState.open) { 
			oven_spriteAnimate.Play(oven_close_clipName);
			currentAnimationState = OvenAnimationState.close;
		}
	}
}
