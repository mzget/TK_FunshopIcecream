using UnityEngine;
using System;
using System.Collections;

public class TK_news : MonoBehaviour {
	private const int AMOUNT_OF_NEWS_TAG = 3;
    private readonly int AmountOfPage = 2; 
    private int currentPage = 0;
	
	public Mz_GuiButtonBeh facebook_button;
	public Mz_GuiButtonBeh close_button;
	public Mz_GuiButtonBeh newsTag_button0;
	public Mz_GuiButtonBeh newsTag_button1;
	public Mz_GuiButtonBeh newsTag_button2;
	public tk2dSprite[] news_tags = new tk2dSprite[AMOUNT_OF_NEWS_TAG];
	private readonly string[] arr_NameOfNewsSprite = new string[6] {
		"Bakery Shop", "Sushi Shop", "Icecream Shop",
        "Pizza Shop", "None", "None",
	};

	public const string FACEBOOK_FANPAGE_URL = "https://www.facebook.com/Taokaenoi.game";
	public const string ITUNES_STORE_LINK = "http://itunes.apple.com/app/id";
	public const string ITUNES_STORE_USER_REVIEW_LINK = "http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=APP_ID";
	public const string TK_BAKERY_SHOP_APP_ID = "626645567";
	public const string TK_SUSHI_SHOP_APP_ID = "638173688";
	public static void GotoiTunes_ViewSoftware(string app_id) {
		string openURL = string.Format("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id={0}&mt=8", app_id);
		Application.OpenURL(openURL);
	}
	
	public static void GoToiTunes_UserReview(string app_id) {
		string openURL = string.Format("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}", app_id);
		Application.OpenURL(openURL);
	}

	// Use this for initialization
    void Start()
    {
        currentPage = 0;
        this.SynchronizeNewsTag();
		
		close_button.click_event += HandleClose_button_click_event;
		facebook_button.click_event += HandleFacebook_button_click_event;
		newsTag_button0.click_event += HandleNewsTag_button_click_event;
		newsTag_button1.click_event += HandleNewsTag_button_click_event;
		newsTag_button2.click_event += HandleNewsTag_button_click_event;
	}

    void HandleNewsTag_button_click_event (object sender, EventArgs e)
    {
		Mz_GuiButtonBeh temp = sender as Mz_GuiButtonBeh;
		
		switch (temp.name) {
		case "Bakery Shop":
            TK_news.GotoiTunes_ViewSoftware(TK_news.TK_BAKERY_SHOP_APP_ID);
            break;
		case "Sushi Shop":
            TK_news.GotoiTunes_ViewSoftware(TK_news.TK_SUSHI_SHOP_APP_ID);
            break;
		case "Icecream Shop":
//			TK_news.GoToiTunes_UserReview();
			break;
		default:
		break;
		}
    }

    void HandleFacebook_button_click_event (object sender, EventArgs e)
    {
		Application.OpenURL(TK_news.FACEBOOK_FANPAGE_URL);
    }

	void HandleClose_button_click_event (object sender, System.EventArgs e)
	{
		MainMenu.GetInstance.SetActivateTKNews(false);
	}

    private void SynchronizeNewsTag()
    {
        for (int i = 0; i < AMOUNT_OF_NEWS_TAG; i++) {
			news_tags[i].spriteId = news_tags[i].GetSpriteIdByName(arr_NameOfNewsSprite[i + (currentPage * AMOUNT_OF_NEWS_TAG)]);
			news_tags[i].gameObject.name = arr_NameOfNewsSprite[i + (currentPage * AMOUNT_OF_NEWS_TAG)];
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    internal void ShakeFacebookButton() {
        iTween.ShakePosition(facebook_button.gameObject, iTween.Hash("name", "ShakePosFacebook","amount", new Vector3(1f, 1f, 0f), "islocal", false, "time", 1f, "looptype", iTween.LoopType.pingPong));
    }

    internal void StopShakeFacebookButton() {
        iTween.StopByName(facebook_button.gameObject, "ShakePosFacebook");
    }

    internal void MoveUpPage()
    {
        if (currentPage > 0)
            currentPage--;
        else
            currentPage = AmountOfPage - 1;

        SynchronizeNewsTag();
    }

    internal void MoveDownPage()
    {
        if (currentPage < AmountOfPage-1)
            currentPage++;
        else
            currentPage = 0;

        SynchronizeNewsTag();
    }
}
