using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CustomerOrderRequire {
	public Food food {get; set;}
	//    public int number {get; set;}
};

public class CustomerBeh : MonoBehaviour {
		
    private tk2dAnimatedSprite animatedSprite;

	internal bool _isPlayingAnimation = false;

//	public tk2dAnimatedSprite AnimatedSprite { get {return animatedSprite;}}
	private int currentPlayAnimatedID = 0;

    public string[] animationClip_name = new string[] {
        "boy_001", "boy_002", "boy_003", "boy_004",
        "boy_005", "boy_006", "boy_007", "boy_008",
        "boy_009", "boy_010", "boy_011",
    };
	public string[] arr_mutterAnimationClip_name = new string[] {
		"boy001_mutter", "boy002_mutter", "boy003_mutter", "boy004_mutter", 
		"boy005_mutter", "boy006_mutter", "boy007_mutter", "boy008_mutter", 
		"boy009_mutter", "boy010_mutter", "boy011_mutter", 
	};
	
	public List<Food> list_goodsBag;		// Use for shuffle bag goods obj.
    public GameObject customerSprite_Obj;
	public GameObject customerOrderingIcon_Obj;
    public List<CustomerOrderRequire> customerOrderRequire = new List<CustomerOrderRequire>();
    public int amount = 0;
	public int payMoney = 0;

	
	// Use this for initialization
	void Start ()
	{
        StartCoroutine(RandomCustomerFace());

		list_goodsBag = new List<Food>(Shop.Instance.CanSellGoodLists);
//		this.GenerateGoodOrder ();
	}
	
	private IEnumerator RandomCustomerFace() {
		if(customerSprite_Obj) {
			animatedSprite = customerSprite_Obj.GetComponent<tk2dAnimatedSprite>();
			
        	int r = Random.Range(0, animationClip_name.Length);
			currentPlayAnimatedID = animatedSprite.GetClipIdByName(animationClip_name[r]);
        	animatedSprite.Play(currentPlayAnimatedID);
		}

        yield return 0;
	}
	
	public void PlayRampage_animation ()
	{
		if (animatedSprite != null) {
			animatedSprite.Play(arr_mutterAnimationClip_name[currentPlayAnimatedID]);	
            Shop.Instance.audioEffect.PlayOnecWithOutStop(Shop.Instance.audioEffect.mutter_clip);
			_isPlayingAnimation = true;

            animatedSprite.animationCompleteDelegate += new tk2dAnimatedSprite.AnimationCompleteDelegate(delegate(tk2dAnimatedSprite sprite, int id) {
                animatedSprite.Play(currentPlayAnimatedID);
				_isPlayingAnimation = false;
            });
		}
	}

	internal void GenerateTutorGoodOrderEvent() {
		customerOrderRequire.Add(new CustomerOrderRequire() { 
			food = new Food(GoodDataStore.FoodMenuList.BananaCoveredWithChocolate.ToString(), 15) });   // number = 1,	// Random.Range(1, 4),
		amount = 15;
		Shop.Instance.GenerateOrderGUI();
	}

    internal void GenerateGoodOrder()
    {
        int maxGoodsType = 3;

        int r = Random.Range(1, maxGoodsType + 1);
        for (int i = 0; i < r; i++) {
			customerOrderRequire.Add(new CustomerOrderRequire() { food = new Food(), });   // number = 1,	// Random.Range(1, 4),
        }

        Debug.Log("GenerateGoodOrder complete! " + "customerOrderRequire.Count : " + customerOrderRequire.Count);

        this.CalculationPrice();
        Shop.Instance.GenerateOrderGUI();
	}

    private void CalculationPrice()
    {
        int[] prices = new int[3];
//        int[] number = new int[3];
        for (int i = 0; i < customerOrderRequire.Count; i++)
        {
            prices[i] = customerOrderRequire[i].food.price;
//            number[i] = customerOrderRequire[i].number;
			
			Debug.Log(customerOrderRequire[i].food.name);
        }

        for(int j = 0; j < customerOrderRequire.Count; j++) {
            amount += prices[j]; 
//			 * number[j];
        }

        Debug.Log("CalculationPrice => amount : " + amount);
    }

	// Update is called once per frame
	void Update () { }

    public void Dispose() {
		list_goodsBag.Clear();
        Destroy(customerSprite_Obj);
		Destroy(customerOrderingIcon_Obj);
        Destroy(this.gameObject);
    }
}
