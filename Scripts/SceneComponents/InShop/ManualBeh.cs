using UnityEngine;
using System.Collections;

public class ManualBeh : ObjectsBeh {
	private readonly string[] arr_titleImgName = new string[] {
		GoodDataStore.FoodMenuList.IcecreamFloat.ToString(),
		GoodDataStore.FoodMenuList.ChocolateChip_ChocolateSundae.ToString(),
		GoodDataStore.FoodMenuList.BringCherry_StrawberrySundae.ToString(),
		GoodDataStore.FoodMenuList.BananaSplitSundae_strawberry.ToString(),
		GoodDataStore.FoodMenuList.TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit_banana.ToString(),
		GoodDataStore.FoodMenuList.FreshyFreeze_C_strawberry.ToString(),
	};

    private readonly string[] arr_foodOrderName = new string[] {
        "icecreamFloat_form", "chocolatesundae_form", "strawberrysundae_form",
		"bananasplitsundae_form","takeawayicecream_form","freshyfreezeicecream_form",
    };

    public GameObject manualCookbook;
	private tk2dAnimatedSprite cookbook_animatedSprite;
	public tk2dSprite[] titles_sprite = new tk2dSprite[3];
    public tk2dSprite form_0;
    public tk2dSprite form_1;
    public tk2dSprite form_2;

    public tk2dTextMesh cookbookPage_textmesh;

    private int currentPage_id = 0;
    private const int MaxPageNumber = 2;


	// Use this for initialization
    private new void Start()
    {
        cookbook_animatedSprite = manualCookbook.GetComponent<tk2dAnimatedSprite>();
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    internal void OnActiveCookbook()
	{
        this.ActiveManualAnimation();
        StartCoroutine_Auto(CheckingUnityAnimationComplete.ICheckAnimationComplete(this.animation, "ManualAnim", this.gameObject, "ActiveCookbookObjectGroup"));
    }

    internal void ActiveManualAnimation()
    {
        baseScene.audioEffect.PlayOnecSound(baseScene.audioEffect.calc_clip);
        this.animation.Play("ManualAnim");
    }

    private void ActiveCookbookObjectGroup() {
        manualCookbook.SetActive(true);
        baseScene.plane_darkShadow.SetActive(true);
		this.Setting_CookbookOrder ();
    }

    private void UnActiveCookbookObjectGroup()
    {
        manualCookbook.SetActive(false);
        baseScene.plane_darkShadow.SetActive(false);
    }

    internal void Setting_CookbookOrder() {
		for (int i = 0; i < titles_sprite.Length; i++) {
			titles_sprite [i].spriteId = titles_sprite [i].GetSpriteIdByName (arr_titleImgName [(currentPage_id * 3) + i]);
		}

        form_0.spriteId = form_0.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 0]);
        form_1.spriteId = form_1.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 1]);
        form_2.spriteId = form_2.GetSpriteIdByName(arr_foodOrderName[(currentPage_id * 3) + 2]);

        int displayId = currentPage_id + 1;
        cookbookPage_textmesh.text = displayId + "/" + MaxPageNumber;
        cookbookPage_textmesh.Commit();
    }
	
	private void ActivateCookbookFormOrder(bool p_active) {
		foreach (var item in titles_sprite) {
			item.gameObject.active = p_active;
		}

		form_0.gameObject.active = p_active;
		form_1.gameObject.active = p_active;
		form_2.gameObject.active = p_active;
	}

    internal void Handle_onInput(ref string nameInput)
    {
        if (nameInput == "Previous_button") {
            cookbook_animatedSprite.Play("Playback");
            this.ActivateCookbookFormOrder(false);
			baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[8]);
            cookbook_animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite sprite, int clipId) {
                this.ActivateCookbookFormOrder(true);
                if (currentPage_id > 0)
                {
                    currentPage_id--;
                    Setting_CookbookOrder();
                }
                else
                {
                    currentPage_id = MaxPageNumber - 1;
                    Setting_CookbookOrder();
                }
            };
        }
        else if (nameInput == "Next_button") {
			cookbook_animatedSprite.Play("Play");
			this.ActivateCookbookFormOrder(false);
			baseScene.audioEffect.PlayOnecWithOutStop(baseScene.soundEffect_clips[8]);
            cookbook_animatedSprite.animationCompleteDelegate = (sprite, clipId) => {
                this.ActivateCookbookFormOrder(true);
	            if (currentPage_id < MaxPageNumber - 1) {
	                currentPage_id++;
	                Setting_CookbookOrder();
	            }
	            else {
	                currentPage_id = 0;
	                Setting_CookbookOrder();
	            }
			};
        }
        else if (nameInput == "Close_button") {
            Shop shop = baseScene.GetComponent<Shop>();
            shop.currentGamePlayState = Shop.GamePlayState.Ordering;

            this.UnActiveCookbookObjectGroup();
        }
    }
}
