using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IcecreamTankBeh : ObjectsBeh {
	
	private static IcecreamTankBeh instance;
	public static IcecreamTankBeh Instance {
		get {
			if(instance == null) {
				instance = Shop.Instance.icecreamTank;
			}
			return instance;
		}
	}
	public const int AMOUNT_OF_ICECREAM_PRODUCT = 10;
    
    public tk2dAnimatedSprite lidTankAnimation;
	private bool _isOpen = false;
	
	public tk2dAnimatedSprite[] scoop_icecreams;
	public BlockIcecreamBeh[] block_icecreams;
	internal Dictionary<string, int> dict_nameOfIcecreamBlock = new Dictionary<string, int>() {
		{"strawberry_icecream_block", 0}, 
		{"chocolate_icecream_block", 1},
		{"vanilla_icecream_block", 2},
		{"mint_icecream_block", 3},
		{"greentea_icecream_block", 4},
		{"lemon_icecream_block", 5},
		{"chocolatechip_icecream_block", 6},
		{"orange_icecream_block", 7},
		{"coffee_icecream_block", 8},
		{"bringcherry_icecream_block", 9},
	};

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
		
		base.implementUserTouchOther = new ImplementUserTouchOther();
		base.implementUserTouchOther.arr_exceptionObjectsName.Add(this.name);
		base.implementUserTouchOther.arr_exceptionObjectsName.Add(lidTankAnimation.gameObject.name);
		foreach (string block in dict_nameOfIcecreamBlock.Keys) {
			base.implementUserTouchOther.arr_exceptionObjectsName.Add(block);
		}
		
		SetActivateScoopIcecream(false);
		StartCoroutine_Auto(this.IE_SetActiveIcecreamBlock());
    }
	
	private void SetActivateScoopIcecream(bool p_active) {
		foreach (tk2dAnimatedSprite item in scoop_icecreams) {
			item.gameObject.SetActive(p_active);
		}
	}

	internal IEnumerator IE_SetActiveIcecreamBlock ()
	{
		foreach(BlockIcecreamBeh item in block_icecreams) {
			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(item.gameObject.name) == false)
				item.gameObject.SetActive(false);
			else 
				item.gameObject.SetActive(true);
		}
		
		yield return null;
	}
	
	private void SetActivateTank() {
		if(_isOpen == false) {
			lidTankAnimation.Play("OpenTank");
			_isOpen = true;
		}
		else if(_isOpen == true) {
			lidTankAnimation.Play("CloseTank");
			_isOpen = false;
		}
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

	public void Handle_input (string p_name)
	{
		Debug.Log("IcecreamTankBeh : Handle_input() : " + p_name);
		 
		if(p_name == "LidIcecreamTank") {
			this.SetActivateTank();
		}
		else {
			int i = dict_nameOfIcecreamBlock[p_name];
			scoop_icecreams[i].gameObject.SetActive(true);
			scoop_icecreams[i].Play();
			scoop_icecreams[i].animationCompleteDelegate = (sprite, clipId) => {
				SetActivateScoopIcecream(false);
			};
		}
	}

    protected override void OnTouchDown()
    {
		this.SetActivateTank();
        base.OnTouchDown();
    }
	
	protected override void OnTouchOther ()
	{
		base.OnTouchOther ();
		
		if(_isOpen == true) {
			lidTankAnimation.Play("CloseTank");
			_isOpen = false;
		}
	}
}
