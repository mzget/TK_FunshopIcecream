using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoodDataStore {
	
	private static GoodDataStore instance;
	public static GoodDataStore Instance {
		get { 
			if(instance == null) {
				instance = Shop.Instance.goodDataStore;
			}
			return instance;
		}
	}
	
    public enum FoodMenuList
    {
        //<!-- Chocolate Sundae.
        Chocolate_ChocolateSundae = 0,
        Strawberry_ChocolateSundae = 1,
        ChocolateChip_ChocolateSundae = 2,
        Greentea_ChocolateSundae = 3,
        Lemon_ChocolateSundae = 4,
        Vanilla_ChocolateSundae = 5,
        Mint_ChocolateSundae = 6,
        Orange_ChocolateSundae = 7,
        BringCherry_ChocolateSundae = 8, 
        Coffee_ChocolateSundae = 9,
        //<@-- Strawberry Sundae.
        Chocolate_StrawberrySundae = 10,
        Strawberry_StrawberrySundae = 11,
        ChocolateChip_StrawberrySundae = 12,
        Greentea_StrawberrySundae = 13,
        Lemon_StrawberrySundae = 14,
        Vanilla_StrawberrySundae = 15,
		Mint_StrawberrySundae = 16,
        Orange_StrawberrySundae = 17,        
        BringCherry_StrawberrySundae = 18,
        Coffee_StrawberrySundae = 19,
        //<@-- beverage.
        IcecreamFloat = 20,
        Cola = 21,
        FruitPunch = 22,
        StrawberryMilkShake = 23,
        // <@-- Baked.
        IcecreamCake = 24,
        BananaCoveredWithChocolate = 25,
		//<@-- FreshyFreezeIcecream 12 units.
		FreshyFreeze_C_blueberry,
		FreshyFreeze_C_cherry,
		FreshyFreeze_C_dragonfruit,
		FreshyFreeze_C_kiwi,
		FreshyFreeze_C_mango,
		FreshyFreeze_C_strawberry,
		FreshyFreeze_S_blueberry,
		FreshyFreeze_S_cherry,
		FreshyFreeze_S_dragonfruit,
		FreshyFreeze_S_kiwi,
		FreshyFreeze_S_mango,
		FreshyFreeze_S_strawberry,
		// BananaSplitSundae.
		BananaSplitSundae_bringcherry,
		BananaSplitSundae_chocolate,
		BananaSplitSundae_chocolatechip,
		BananaSplitSundae_coffee,
		BananaSplitSundae_greentea,
		BananaSplitSundae_lemon,
		BananaSplitSundae_mint,
		BananaSplitSundae_orange,
		BananaSplitSundae_strawberry,
		BananaSplitSundae_vanilla,
        // Take away icecream. chocolate jam.
        TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_lemon_chocolateJam_almond_cherryFriut,
        TakeawayIcecream_mint_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_orange_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit,
        TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit,
		// with banana.
        TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_lemon_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_mint_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_orange_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit_banana,
        TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit_banana,
        // Take away icecream. strawberry jam.
        TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit,
        TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit,
		// with banana.
        TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit_banana,
        TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit_banana,
    };
	
   	public Dictionary<string,Food> dict_FoodDatabase = new Dictionary<string, Food> {                               
		{ FoodMenuList.Chocolate_ChocolateSundae.ToString(), new Food(FoodMenuList.Chocolate_ChocolateSundae.ToString(), 10, 2) },
        { FoodMenuList.Strawberry_ChocolateSundae.ToString(), new Food(FoodMenuList.Strawberry_ChocolateSundae.ToString(), 10, 2) },
        { FoodMenuList.ChocolateChip_ChocolateSundae.ToString(), new Food(FoodMenuList.ChocolateChip_ChocolateSundae.ToString(), 10, 2) },
        { FoodMenuList.Greentea_ChocolateSundae.ToString(), new Food(FoodMenuList.Greentea_ChocolateSundae.ToString(), 10, 2) },
        { FoodMenuList.Lemon_ChocolateSundae.ToString(), new Food(FoodMenuList.Lemon_ChocolateSundae.ToString(), 10, 2) },
	    { FoodMenuList.Vanilla_ChocolateSundae.ToString(), new Food(FoodMenuList.Vanilla_ChocolateSundae.ToString(), 10, 2)},
        { FoodMenuList.Mint_ChocolateSundae.ToString(), new Food(FoodMenuList.Mint_ChocolateSundae.ToString(), 10, 2)},
        { FoodMenuList.Orange_ChocolateSundae.ToString(), new Food(FoodMenuList.Orange_ChocolateSundae.ToString(), 10, 2)},
        { FoodMenuList.BringCherry_ChocolateSundae.ToString(), new Food(FoodMenuList.BringCherry_ChocolateSundae.ToString(), 10, 2)}, 
	    { FoodMenuList.Coffee_ChocolateSundae.ToString(), new Food(FoodMenuList.Coffee_ChocolateSundae.ToString(), 12, 3)}, 
		
        { FoodMenuList.Chocolate_StrawberrySundae.ToString(),new Food(FoodMenuList.Chocolate_StrawberrySundae.ToString(), 12, 3)},
        { FoodMenuList.Strawberry_StrawberrySundae.ToString(),new Food(FoodMenuList.Strawberry_StrawberrySundae.ToString(), 12, 3)},
        { FoodMenuList.ChocolateChip_StrawberrySundae.ToString(),new Food(FoodMenuList.ChocolateChip_StrawberrySundae.ToString(), 15, 4)},        
	    { FoodMenuList.Greentea_StrawberrySundae.ToString(),new Food(FoodMenuList.Greentea_StrawberrySundae.ToString(), 30, 8)},
        { FoodMenuList.Lemon_StrawberrySundae.ToString(),new Food(FoodMenuList.Lemon_StrawberrySundae.ToString(), 25, 7)},
        { FoodMenuList.Vanilla_StrawberrySundae.ToString(),new Food(FoodMenuList.Vanilla_StrawberrySundae.ToString(), 25, 7)},
        { FoodMenuList.Mint_StrawberrySundae.ToString(),new Food(FoodMenuList.Mint_StrawberrySundae.ToString(), 30, 10)},
        { FoodMenuList.Orange_StrawberrySundae.ToString(),new Food(FoodMenuList.Orange_StrawberrySundae.ToString(), 25, 6)},
        { FoodMenuList.BringCherry_StrawberrySundae.ToString(),new Food(FoodMenuList.BringCherry_StrawberrySundae.ToString(), 5, 1)},		
        { FoodMenuList.Coffee_StrawberrySundae.ToString(),new Food(FoodMenuList.Coffee_StrawberrySundae.ToString(), 10, 3)},

        { FoodMenuList.IcecreamFloat.ToString(), new Food(FoodMenuList.IcecreamFloat.ToString(), 50, 10) },
        { FoodMenuList.Cola.ToString(), new Food(FoodMenuList.Cola.ToString(), 35, 8) },
        { FoodMenuList.FruitPunch.ToString(), new Food(FoodMenuList.FruitPunch.ToString(), 35, 8) },
        { FoodMenuList.StrawberryMilkShake.ToString(), new Food(FoodMenuList.StrawberryMilkShake.ToString(), 35, 8) },
        { FoodMenuList.IcecreamCake.ToString(), new Food(FoodMenuList.IcecreamCake.ToString(), 20, 10) },
        { FoodMenuList.BananaCoveredWithChocolate.ToString(), new Food(FoodMenuList.BananaCoveredWithChocolate.ToString(), 15, 6) },
		
		{ FoodMenuList.FreshyFreeze_C_blueberry.ToString(),new Food(FoodMenuList.FreshyFreeze_C_blueberry.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_C_cherry.ToString(),new Food(FoodMenuList.FreshyFreeze_C_cherry.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_C_dragonfruit.ToString(),new Food(FoodMenuList.FreshyFreeze_C_dragonfruit.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_C_kiwi.ToString(),new Food(FoodMenuList.FreshyFreeze_C_kiwi.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_C_mango.ToString(),new Food(FoodMenuList.FreshyFreeze_C_mango.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_C_strawberry.ToString(),new Food(FoodMenuList.FreshyFreeze_C_strawberry.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_blueberry.ToString(),new Food(FoodMenuList.FreshyFreeze_S_blueberry.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_cherry.ToString(),new Food(FoodMenuList.FreshyFreeze_S_cherry.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_dragonfruit.ToString(),new Food(FoodMenuList.FreshyFreeze_S_dragonfruit.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_kiwi.ToString(),new Food(FoodMenuList.FreshyFreeze_S_kiwi.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_mango.ToString(),new Food(FoodMenuList.FreshyFreeze_S_mango.ToString(), 70, 20) },
		{ FoodMenuList.FreshyFreeze_S_strawberry.ToString(),new Food(FoodMenuList.FreshyFreeze_S_strawberry.ToString(), 70, 20) },

        { FoodMenuList.BananaSplitSundae_bringcherry.ToString(),new Food(FoodMenuList.BananaSplitSundae_bringcherry.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_chocolate.ToString(),new Food(FoodMenuList.BananaSplitSundae_chocolate.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_chocolatechip.ToString(),new Food(FoodMenuList.BananaSplitSundae_chocolatechip.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_coffee.ToString(),new Food(FoodMenuList.BananaSplitSundae_coffee.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_greentea.ToString(),new Food(FoodMenuList.BananaSplitSundae_greentea.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_lemon.ToString(),new Food(FoodMenuList.BananaSplitSundae_lemon.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_mint.ToString(),new Food(FoodMenuList.BananaSplitSundae_mint.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_orange.ToString(),new Food(FoodMenuList.BananaSplitSundae_orange.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_strawberry.ToString(),new Food(FoodMenuList.BananaSplitSundae_strawberry.ToString(), 30, 8) },
        { FoodMenuList.BananaSplitSundae_vanilla.ToString(),new Food(FoodMenuList.BananaSplitSundae_vanilla.ToString(), 30, 8) },
        // Take away icecream.
        { FoodMenuList.TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit.ToString(),50, 10)},
        {FoodMenuList.TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_bringcherry_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_bringcherry_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolate_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolate_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolatechip_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_chocolatechip_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_coffee_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit.ToString(), 50, 15)},
        {FoodMenuList.TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_coffee_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_greentea_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit.ToString(), 50, 15)},
        {FoodMenuList.TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_greentea_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_lemon_chocolateJam_almond_cherryFriut.ToString(), new Food(FoodMenuList.TakeawayIcecream_lemon_chocolateJam_almond_cherryFriut.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_lemon_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_lemon_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_lemon_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)}, 
        {FoodMenuList.TakeawayIcecream_mint_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_mint_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_mint_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_mint_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_mint_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_orange_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_orange_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_orange_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_orange_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit.ToString(), 50, 15)},
        {FoodMenuList.TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_orange_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_strawberry_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_strawberry_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_vanilla_chocolateJam_almond_cherryFruit_banana.ToString(), 65, 15)},
        {FoodMenuList.TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit.ToString(), new Food(FoodMenuList.TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit.ToString(), 50, 10)},
        {FoodMenuList.TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit_banana.ToString(), new Food(FoodMenuList.TakeawayIcecream_vanilla_strawberryJam_sugar_strawberryFruit_banana.ToString(), 65, 15)},
    };
	
	public void OnDestroy() { 
		instance = null;
	}
}
