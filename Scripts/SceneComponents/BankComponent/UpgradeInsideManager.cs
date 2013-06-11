using System;
using UnityEngine;
using System.Collections;

public class UpgradeInsideManager : MonoBehaviour {
	//<@-- Upgrade button.
	public GameObject[,] upgradeButton_Objs = new GameObject[2,4];
	private tk2dSprite[,] upgradeButton_Sprites = new tk2dSprite[2,4];
	//<@-- Upgrade item objs.
	internal GameObject[,] upgradeInsideObj2D = new GameObject[2,4];
	private tk2dSprite[,] upgradeInsideSprite2D = new tk2dSprite[2, 4];
    private tk2dTextMesh[,] upgradeInside_PriceTextmesh = new tk2dTextMesh[2, 4];
    //<@-- Confirmation window obj.
    public GameObject confirmationWindow;

    internal static readonly string[,] page1_spriteNames = new string[2, 4] {
       {
           "strawberry_icecream_block",
           "chocolate_icecream_block",
           "vanilla_icecream_block",
           "mint_icecream_block",
       },
       {
           "greentea_icecream_block",
           "lemon_icecream_block",
           "chocolatechip_icecream_block",
           "orange_icecream_block",
		},
    };
    internal static readonly string[,] page2_spriteNames = new string[2, 4] {
		{
           "coffee_icecream_block",
           "bringcherry_icecream_block",
           "ColaTank",
           "StrawberryMilkShakeTank",
		},
        {
           "FruitPunchTank",
           "BananaTopping",
           "FreshyFreezeIcecream",
           "",
        },
    };
    internal static readonly string[,] thirdPage_spriteNames = new string[2, 4] {		
		{"", "", "", ""},
		{"", "", "", ""},
	};
    private int[,] firstPage_prices = new int[,] {
        {300, 300, 300, 300},
        {300, 300, 300, 300},
    };
    private int[,] secondPage_prices = new int[,] {
        {300, 300, 500, 500},
        {500, 150, 1200, 100},
    };
    private int[,] thirdPage_prices = new int[,] {
        {150, 200, 1600, 700},
        {500, 1300,0,0},
    };

	public const string Message_Warning_NotEnoughMoney = "Connot upgrade this item because available money is not enough.";
	private int currentPageIndex = 0;
    private const int MAX_PAGE_NUMBER = 2;
	private const int ActiveUpgradeButtonID = 25;
	private int UnActiveUpgradeButtonID ;
    private int none_sprite_id;
    public tk2dTextMesh displayCurrentPageID_Textmesh;
	internal bool _isInitialize = false;

    private event EventHandler<OnUpdateEvenArgs> OnUpgrade_Event;
    private void OnUpgradeEvent_checkingDelegation(OnUpdateEvenArgs e) {
        if (OnUpgrade_Event != null)
            OnUpgrade_Event(this, e);
    }
    private class OnUpdateEvenArgs : EventArgs {
        public int I = 0;
        public int J = 0;
        public string Item_name;
		public string AdditionalParams = string.Empty;
		
        public OnUpdateEvenArgs() { }
    };
    private OnUpdateEvenArgs currentOnUpdateTarget;

	/// <summary>
	/// Calss references.
	/// </summary>
	private SheepBank sceneController;
	
	// Use this for initialization
	void Start () {
		currentPageIndex = 0;
        confirmationWindow.gameObject.SetActive(false);

		var controller = GameObject.FindGameObjectWithTag("GameController");
		sceneController = controller.GetComponent<SheepBank>();

        OnUpgrade_Event += Handle_OnUpgrade_Event;
	}
	
	internal void ReInitializeData() {
		currentPageIndex = 0;

		CalculateObjectsToDisplay();
	}
	
	// Update is called once per frame
	void Update () {
		if(_isInitialize == false)
			InitailizeDataFields();
	}

	private void InitailizeDataFields() {
		if(upgradeInsideSprite2D[0,0] == null) 
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					upgradeInsideSprite2D[i, j] = upgradeInsideObj2D[i, j].GetComponent<tk2dSprite>();
					upgradeInside_PriceTextmesh[i, j] = upgradeInsideObj2D[i, j].transform.GetComponentInChildren<tk2dTextMesh>();
					
					upgradeButton_Sprites[i,j] = upgradeButton_Objs[i,j].GetComponent<tk2dSprite>();
				}
			}
		}

        UnActiveUpgradeButtonID = upgradeButton_Sprites[0, 0].GetSpriteIdByName("upgrade_button_unActive");
		none_sprite_id = upgradeInsideSprite2D[0,0].GetSpriteIdByName("none_button_up");
		_isInitialize  = true;

        Debug.Log("UpgradeInsideManager._isInitialize == " + _isInitialize);
	}
	
	public void GotoNextPage() {
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }
		
		if(currentPageIndex < MAX_PAGE_NUMBER - 1)	
            currentPageIndex += 1;
		else 
            currentPageIndex = 0;
		
        CalculateObjectsToDisplay();
	}
	
	public void BackToPreviousPage() {		
	    foreach(GameObject obj in upgradeInsideObj2D) {
            obj.animation.Play();
        }

        if(currentPageIndex > 0)             
			currentPageIndex -= 1;
		else
			currentPageIndex = MAX_PAGE_NUMBER - 1;
		
		CalculateObjectsToDisplay();
	}

    private void CalculateObjectsToDisplay()
    {
        if(currentPageIndex == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = page1_spriteNames[i, j];
                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                    upgradeInsideSprite2D[i, j].color = Color.white;

                    upgradeInside_PriceTextmesh[i, j].text = firstPage_prices[i, j].ToString();
                    upgradeInside_PriceTextmesh[i, j].Commit();
					
					upgradeButton_Objs[i,j].SetActive(true);
					upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
                }
            }			
			
			#region <!-- Page 1, row 0
		
			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,0]) || Mz_StorageManage.AccountBalance < firstPage_prices[0,0]) {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0, 0]))
                {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,1]) || Mz_StorageManage.AccountBalance < firstPage_prices[0, 1])
            {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,1]))
                {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
                    upgradeButton_Objs[0, 1].SetActive(false);
				}
			}
			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,2]) || Mz_StorageManage.AccountBalance < firstPage_prices[0,2]) {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,2]))
                {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0, 3]) || Mz_StorageManage.AccountBalance < firstPage_prices[0, 3])
            {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,3]))
                {
                    upgradeInsideSprite2D[0, 3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActive(false);
				}
			}

			#endregion

			#region <!-- page 1, row 1.

			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,0]) || Mz_StorageManage.AccountBalance < firstPage_prices[1,0]) {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,0]))
                {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,1]) || Mz_StorageManage.AccountBalance < firstPage_prices[1, 1])
            {
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,1]))
                {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,2]) || Mz_StorageManage.AccountBalance < firstPage_prices[1, 2])
            {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,2]))
                {
                    upgradeInsideSprite2D[1,2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,3]) || Mz_StorageManage.AccountBalance < firstPage_prices[1, 3])
            {
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,3]))
                {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActive(false);
				}
            }

			#endregion
        }
        else if(currentPageIndex == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string nameSpecify = page2_spriteNames[i, j];
					if(nameSpecify != "") {
	                    upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
	                    upgradeInsideSprite2D[i, j].color = Color.white;

	                    upgradeInside_PriceTextmesh[i, j].text = secondPage_prices[i, j].ToString();
	                    upgradeInside_PriceTextmesh[i, j].Commit();
						
						upgradeButton_Objs[i,j].SetActive(true);
						upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
					}
					else {						
						tk2dSprite sprite = upgradeInsideSprite2D[i,j];
						tk2dTextMesh textmesh = upgradeInside_PriceTextmesh[i, j];
						sprite.spriteId = none_sprite_id;
						textmesh.text = "none";
						textmesh.transform.localPosition = new Vector3(1, textmesh.transform.localPosition.y, textmesh.transform.localPosition.z);
						textmesh.Commit();

						upgradeButton_Objs[i,j].SetActive(false);
					}
                }
            }

            #region <!-- page 2, row 0.

            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,0]) || Mz_StorageManage.AccountBalance < secondPage_prices[0, 0])
            {
				upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,0]))
                {
                    upgradeInsideSprite2D[0, 0].color = Color.grey;
					upgradeButton_Objs[0,0].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,1]) || Mz_StorageManage.AccountBalance < secondPage_prices[0, 1])
            {
				upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,1]))
                {
                    upgradeInsideSprite2D[0, 1].color = Color.grey;
					upgradeButton_Objs[0,1].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,2]) || Mz_StorageManage.AccountBalance < secondPage_prices[0, 2])
            {
				upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,2])) {
                    upgradeInsideSprite2D[0, 2].color = Color.grey;
					upgradeButton_Objs[0,2].SetActive(false);
				}
			}
			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,3]) || Mz_StorageManage.AccountBalance < secondPage_prices[0,3]) {
				upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;
				
				if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,3])) {
					upgradeInsideSprite2D[0,3].color = Color.grey;
					upgradeButton_Objs[0,3].SetActive(false);
				}
			}
            #endregion

            #region <!-- page 2, row 1.

            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,0]) || Mz_StorageManage.AccountBalance < secondPage_prices[1, 0])
            {
				upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,0])) {
                    upgradeInsideSprite2D[1, 0].color = Color.grey;
					upgradeButton_Objs[1,0].SetActive(false);
				}
			}
			if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,1]) || Mz_StorageManage.AccountBalance < secondPage_prices[1,1])	{
				upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,1]))
                {
                    upgradeInsideSprite2D[1, 1].color = Color.grey;
					upgradeButton_Objs[1,1].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,2]) || Mz_StorageManage.AccountBalance < secondPage_prices[1, 2])
            {
				upgradeButton_Sprites[1,2].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,2]))
                {
                    upgradeInsideSprite2D[1, 2].color = Color.grey;
					upgradeButton_Objs[1,2].SetActive(false);
				}
			}
            if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,3]) || Mz_StorageManage.AccountBalance < secondPage_prices[1, 3])
            {
				upgradeButton_Sprites[1,3].spriteId = UnActiveUpgradeButtonID;

                if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,3]))
                {
                    upgradeInsideSprite2D[1, 3].color = Color.grey;
					upgradeButton_Objs[1,3].SetActive(false);
				}
            }

            #endregion
        }
        else if(currentPageIndex == 2)
        {
            #region <@-- Page index == 2

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    string nameSpecify = thirdPage_spriteNames[i, j];
					if(nameSpecify != "") {
                    	upgradeInsideSprite2D[i, j].spriteId = upgradeInsideSprite2D[i, j].GetSpriteIdByName(nameSpecify);
                        upgradeInsideSprite2D[i, j].color = Color.white;

                    	upgradeInside_PriceTextmesh[i, j].text = thirdPage_prices[i, j].ToString();
                    	upgradeInside_PriceTextmesh[i, j].Commit();
						
						upgradeButton_Objs[i,j].SetActive(true);
						upgradeButton_Sprites[i,j].spriteId = ActiveUpgradeButtonID;
					}
					else {
						tk2dSprite sprite = upgradeInsideSprite2D[i,j];
						tk2dTextMesh textmesh = upgradeInside_PriceTextmesh[i, j];
						sprite.spriteId = none_sprite_id;
						textmesh.text = "none";
						textmesh.transform.localPosition = new Vector3(1, textmesh.transform.localPosition.y, textmesh.transform.localPosition.z);
						textmesh.Commit();
						
						upgradeButton_Objs[i,j].SetActive(false);
					}
                }
            }
			
            //if(Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Mint_StrawberrySundae.ToString()) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,0]) {
            //    upgradeButton_Sprites[0,0].spriteId = UnActiveUpgradeButtonID;

            //    if (Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Mint_StrawberrySundae.ToString())) {
            //        upgradeInsideSprite2D[0, 0].color = Color.grey;
            //        upgradeButton_Objs[0,0].SetActive(false);
            //    }
            //}
            //if(Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Orange_StrawberrySundae.ToString()) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,1]) {
            //    upgradeButton_Sprites[0,1].spriteId = UnActiveUpgradeButtonID;

            //    if (Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Orange_StrawberrySundae.ToString())) {
            //        upgradeInsideSprite2D[0, 1].color = Color.grey;
            //        upgradeButton_Objs[0,1].SetActive(false);
            //    }
            //}
            //if(Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.BringCherry_StrawberrySundae.ToString()) || Mz_StorageManage.AccountBalance < thirdPage_prices[0,2]) {
            //    upgradeButton_Sprites[0,2].spriteId = UnActiveUpgradeButtonID;

            //    if (Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.BringCherry_StrawberrySundae.ToString())) {
            //        upgradeInsideSprite2D[0, 2].color = Color.grey;
            //        upgradeButton_Objs[0,2].SetActive(false);
            //    }
            //}
            //if (Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Coffee_StrawberrySundae.ToString()) || Mz_StorageManage.AccountBalance < thirdPage_prices[0, 3])
            //{
            //    upgradeButton_Sprites[0,3].spriteId = UnActiveUpgradeButtonID;

            //    if (Shop.Name_Of_CanSellItem.Contains(GoodDataStore.FoodMenuList.Coffee_StrawberrySundae.ToString())) {
            //        upgradeInsideSprite2D[0, 3].color = Color.grey;
            //        upgradeButton_Objs[0,3].SetActive(false);
            //    }
            //}
            //if(SushiShop.NumberOfCansellItem.Contains(27) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,0]) {
            //    upgradeButton_Sprites[1,0].spriteId = UnActiveUpgradeButtonID;

            //    if (SushiShop.NumberOfCansellItem.Contains(27)) {
            //        upgradeInsideSprite2D[1, 0].color = Color.grey;
            //        upgradeButton_Objs[1,0].SetActiveRecursively(false);
            //    }
            //}
            //if(SushiShop.NumberOfCansellItem.Contains(29) || Mz_StorageManage.AccountBalance < thirdPage_prices[1,1]) {
            //    upgradeButton_Sprites[1,1].spriteId = UnActiveUpgradeButtonID;

            //    if (SushiShop.NumberOfCansellItem.Contains(29)) {
            //        upgradeInsideSprite2D[1, 1].color = Color.grey;
            //        upgradeButton_Objs[1,1].SetActiveRecursively(false);
            //    }
            //}

            #endregion
        }
		
        int temp_pageID = currentPageIndex + 1;
        displayCurrentPageID_Textmesh.text = temp_pageID + "/" + MAX_PAGE_NUMBER;
        displayCurrentPageID_Textmesh.Commit();
    }

    internal void BuyingUpgradeMechanism(string upgradeName) 
	{	
        if(currentPageIndex == 0) 
		{
            #region <!-- page 0, Low 0.

            if(upgradeName == "upgrade_00")
            {
                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,0]) 
				{
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,0]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_name = page1_spriteNames[0,0] };
                        this.ActiveComfirmationWindow();
						
						//<@-- Handle buying upgrade tutor.
                        if (Mz_StorageManage._HasNewGameEvent)
                        {
                            sceneController.SetActivateTotorObject(false);
                        }
                    }
					else{
						this.PlaySoundWarning();
					}
                }
				else {
                    this.PLayWarningCannotBuyItem();
                }
            }
            else if(upgradeName == "upgrade_01")
            {
                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,1])
                {								
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0,1]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_name = page1_spriteNames[0,1] };
                        this.ActiveComfirmationWindow();
                    }
					else{
						this.PlaySoundWarning();
					}
                }
				else {
                    this.PLayWarningCannotBuyItem();
                }
            }
			else if(upgradeName == "upgrade_02")
            {
                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,2])
				{
                    if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0, 2]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_name = page1_spriteNames[0, 2], };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
                    this.PLayWarningCannotBuyItem();
                }
            }
			else if(upgradeName == "upgrade_03")
            {
                if (Mz_StorageManage.AccountBalance >= firstPage_prices[0,3]) 
                {
                    if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[0, 3]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_name = page1_spriteNames[0, 3], };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
                    this.PLayWarningCannotBuyItem();
                }
            }
	
			#endregion 

			#region <!-- page 0, Low 1.
			
			else if(upgradeName == "upgrade_10") 
			{	
                if (Mz_StorageManage.AccountBalance >= firstPage_prices[1,0]) {					
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,0]) == false) {
						currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_name = page1_spriteNames[1,0] };
						this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
                }
                else {
					this.PLayWarningCannotBuyItem();
                }
			}
			else if(upgradeName == "upgrade_11") 
			{						
				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 1]) 
                {
                    if (ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,1]) == false)
                    {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_name = page1_spriteNames[1,1], };
                        this.ActiveComfirmationWindow();
					}
					else{
						this.PlaySoundWarning();
					}
				}
				else {
					this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_12")
			{				
				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1, 2])
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,2]) == false) {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_name = page1_spriteNames[1,2] };
                        this.ActiveComfirmationWindow();
					}
					else {
						this.PlaySoundWarning();
					}
				}
				else {
					this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_13") 
			{
				if(Mz_StorageManage.AccountBalance >= firstPage_prices[1,3]) 
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page1_spriteNames[1,3]) == false) {
						Debug.Log("buying : Sweetened_egg_sushi");
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_name = page1_spriteNames[1,3] };
                        this.ActiveComfirmationWindow();
					}
					else {
						this.PlaySoundWarning();
					}
				}
				else {
					this.PLayWarningCannotBuyItem();
				}
			}
			
			#endregion
        }
		else if(currentPageIndex == 1)
		{
			#region <!-- Page 2 low 0.
			
			if(upgradeName == "upgrade_00")
			{	
                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,0]) 
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,0]) == false) {
						this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_name = page2_spriteNames[0,0] };
						this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
					this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_01") 
			{
                if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,1])
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,1]) == false) {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_name = page2_spriteNames[0,1] };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
                    this.PLayWarningCannotBuyItem();
				}
			}
            else if(upgradeName == "upgrade_02") 
			{
				if(Mz_StorageManage.AccountBalance >= secondPage_prices[0,2]) 
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,2]) == false) {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_name = page2_spriteNames[0,2] };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
                    this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_03") 
			{
                if (Mz_StorageManage.AccountBalance >= secondPage_prices[0, 3]) {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[0,3]) == false) {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_name = page2_spriteNames[0,3] };
                        this.ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
                else {
                    this.PLayWarningCannotBuyItem();
                }
			}
			
			#endregion

			#region <!-- page2 , row 1.

			if(upgradeName == "upgrade_10")
			{				
				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,0]) 
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,0]) == false) {
                        this.currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0,  Item_name = page2_spriteNames[1,0] };
                        this.ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_11")
			{				
				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,1])
                {		
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,1]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_name = page2_spriteNames[1,1] };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    this.PLayWarningCannotBuyItem();
				}
			}
			else if(upgradeName == "upgrade_12")
			{		
				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,2]) 
                {
					if(ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Contains(page2_spriteNames[1,2]) == false) {
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 2, Item_name = page2_spriteNames[1,2] };
                        ActiveComfirmationWindow();
                    }
					else
						PlaySoundWarning();
				}
				else {
                    this.PLayWarningCannotBuyItem();
				}
			}
/*			else if(upgradeName == "upgrade_13")
			{				
				if(Mz_StorageManage.AccountBalance >= secondPage_prices[1,3]) 
				{
					if(Shop.NumberOfCansellItem.Contains(curry_with_rice_id) == false) {
						Debug.Log("buying : Curry_with_rice");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 3, Item_id = curry_with_rice_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
                }
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}
			}
*/
			#endregion
		}
		else if(currentPageIndex == 2) 
		{
			#region <!-- page2 low 0.
/*			
			if(upgradeName == "upgrade_00")
			{				
				#region <@-- "buying : Miso_soup".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,0]) {
					if(Shop.NumberOfCansellItem.Contains(miso_soup_id) == false) {
						Debug.Log("buying : Miso_soup");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 0, Item_id = miso_soup_id };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_01")
			{		
				#region <@-- "buying : Kimji".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,1]) {
					if(Shop.NumberOfCansellItem.Contains(kimji_id) == false) {
						Debug.Log("buying : Kimji");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 1, Item_id = kimji_id };
                        ActiveComfirmationWindow();
					}
					else 
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_02") 
			{
				#region <@-- "buying : Bean_ice_jam_on_crunching".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,2]) {
					if(Shop.NumberOfCansellItem.Contains(Bean_ice_jam_on_crunching_id) == false) {
						Debug.Log("buying : Bean_ice_jam_on_crunching");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 2, Item_id = Bean_ice_jam_on_crunching_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
			else if(upgradeName == "upgrade_03") 
			{
				#region <@-- "buying : GreenTea_icecream".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[0,3]) 
				{
					if(Shop.NumberOfCansellItem.Contains(GreenTea_icecream_id) == false)  {
						Debug.Log("buying : GreenTea_icecream");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 0, J = 3, Item_id = GreenTea_icecream_id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
                    print(Message_Warning_NotEnoughMoney);
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
*/
			#endregion

			#region <!-- page2 low 1.
/*
			if(upgradeName == "upgrade_10")
			{	
				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,0]) {
					if(Shop.Name_Of_CanSellItem.Contains(thirdPage_spriteNames[1,0]) == false) {
						Debug.Log("buying : butter_cookie");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 0, Item_id = id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}
			}
			else if(upgradeName == "upgrade_11") 
			{		
				#region <@-- "buying : hotdog_cheese".

				if(Mz_StorageManage.AccountBalance >= thirdPage_prices[1,1]) 
				{
                    int id = (int)GoodDataStore.FoodMenuList.HotdogWithCheese;
					if(BakeryShop.NumberOfCansellItem.Contains(id) == false) {
						Debug.Log("buying : hotdog_cheese");
                        currentOnUpdateTarget = new OnUpdateEvenArgs() { I = 1, J = 1, Item_id = id };
                        ActiveComfirmationWindow();
					}
					else
						PlaySoundWarning();
				}
				else {
					sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
				}

				#endregion
			}
*/			
			#endregion
		}
    }

	void BuyingUpgradeComplete (ref OnUpdateEvenArgs e) 
    {
        if (currentPageIndex == 0)
            Mz_StorageManage.AccountBalance -= firstPage_prices[e.I, e.J];
        else if (currentPageIndex == 1)
            Mz_StorageManage.AccountBalance -= secondPage_prices[e.I, e.J];
        else if (currentPageIndex == 2)
            Mz_StorageManage.AccountBalance -= thirdPage_prices[e.I, e.J];
		
        //if (e.AdditionalParams != string.Empty)
        //{
        //    switch (e.AdditionalParams)
        //    {
        //        default:
        //            break;
        //    }
        //}

        upgradeButton_Sprites[e.I, e.J].spriteId = UnActiveUpgradeButtonID;
        ExtendsSaveManager.UpgradeInsideSaveData.List_of_purchased_item.Add(e.Item_name);
        Shop.NewItem_name.Add(e.Item_name);
        
		sceneController.ManageAvailabelMoneyBillBoard();
        sceneController.gameEffectManager.Create2DSpriteAnimationEffect(GameEffectManager.BLOOMSTAR_EFFECT_PATH, upgradeButton_Objs[e.I, e.J].transform);
        sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.longBring_clip);

        CalculateObjectsToDisplay();
	}

	void PlaySoundWarning ()
	{
		sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);
		Debug.LogWarning("This Item has be upgraded");
	}

    private void PLayWarningCannotBuyItem()
    {
        sceneController.audioEffect.PlayOnecWithOutStop(sceneController.audioEffect.wrong_Clip);

        Debug.Log(UpgradeInsideManager.Message_Warning_NotEnoughMoney);
        sceneController.audioDescribe.PlayOnecSound(sceneController.description_clips[11]);
        sceneController.CreateDepositIcon();
    }

    private void ActiveComfirmationWindow()
    {
        confirmationWindow.SetActive(true);
    }

	internal void UnActiveComfirmationWindow ()
	{
    	confirmationWindow.SetActive(false);
		currentOnUpdateTarget = null;
	}

    internal void UserComfirm()
    {
        this.OnUpgradeEvent_checkingDelegation(currentOnUpdateTarget);
    }

    private void Handle_OnUpgrade_Event(object sender, OnUpdateEvenArgs e)
    {
        this.BuyingUpgradeComplete(ref e);
        this.UnActiveComfirmationWindow();
    }
}
