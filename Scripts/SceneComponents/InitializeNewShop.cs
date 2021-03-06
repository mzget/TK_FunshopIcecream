using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeNewShop : MonoBehaviour {

	public tk2dSprite shopLogo_sprite;
	public string currentLogoColor = "Blue";
	public int currentLogoID = 0;
	private const int MaxLogoCount = 6;
	public static string[] shopLogo_NameSpecify = new string[MaxLogoCount] {
		"Logo_0001", "Logo_0002", "Logo_0003", "Logo_0004",
		"Logo_0005", "Logo_0006", 
//		"Logo_0007", "Logo_0008", 
//		"Logo_0009", "Logo_0010", "Logo_0011", "Logo_0012",
//		"Logo_0013", "Logo_0014", "Logo_0015", "Logo_0016",
	};
	public static Dictionary<string,Color> shopLogos_Color = new Dictionary<string, Color>() {
		{"Blue", new Color(0f, .5f, .75f, 1)},
		{"Green", new Color(0, .8f, 0, 1)},
		{"Pink", new Color(.9f, .3f, .9f, 1)},
		{"Red", new Color(.9f, 0, 0, 1)},
		{"Yellow", new Color(.9f, .8f, 0, 1)},
	};


	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HavePreviousCommand ()
	{
		if(currentLogoID > 0)
			currentLogoID -= 1;
		else 
			currentLogoID = (MaxLogoCount -1);

		this.ChangeLogoToID(currentLogoID);
	}

	public void HaveNextCommand() {
		if (currentLogoID < (MaxLogoCount - 1)) {
			currentLogoID += 1;		
		}
		else currentLogoID = 0;

		this.ChangeLogoToID(currentLogoID);
	}

	void ChangeLogoToID (int targetID)
	{
		shopLogo_sprite.spriteId = shopLogo_sprite.GetSpriteIdByName(shopLogo_NameSpecify[targetID]);
		this.currentLogoID = targetID;
	}

	public void HaveChangeLogoColor(string keyColor) {
		shopLogo_sprite.color = shopLogos_Color[keyColor];
		this.currentLogoColor = keyColor;
	}
}
