using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GlassLockerBeh : Base_ObjectBeh {
	private static GlassLockerBeh instance;
	public static GlassLockerBeh Instance {
		get {
			if(instance == null) {
                if (Shop.Instance.glassLockerBeh == null)
                    Debug.LogError("Shop.Instance.glassLockerBeh == null");

				instance = Shop.Instance.glassLockerBeh;
			}

			return instance;
		}
	}
	
	
	public Transform glassLocker_UI;
	public Transform naperyHandIndex;
	private DragableGUI bigGlass;
	private DragableGUI glass;
	private DragableGUI cup;
	private DragableGUI disk;
    public Mz_GuiButtonBeh close_button;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		glassLocker_UI.position = new Vector3(-42, 150, -2);
		SetActiveNaperyHandIndex(false);
		close_button.click_event += (sender, e) => { CloseGlassLockerUI(); };
		
		this.CreateIcon("BigGlass");
        this.CreateIcon("Glass");
        this.CreateIcon("Cup");
        this.CreateIcon("Disk");
	}
	
	protected override void Update ()
	{
		base.Update ();
	}

    private void CloseGlassLockerUI()
    {
        HOTween.To(glassLocker_UI, 1f, new TweenParms().Prop("position", new Vector3(-42, 150, -2)).Ease(EaseType.EaseOutBounce));
		this.SetActiveNaperyHandIndex(false);
    }
	
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();
		
		HOTween.To(glassLocker_UI, 1f, new TweenParms().Prop("position", new Vector3(-42, 100, -2)).Ease(EaseType.EaseOutBounce));
		this.SetActiveNaperyHandIndex(true);
	}

	private void SetActiveNaperyHandIndex (bool state)
	{
		naperyHandIndex.gameObject.SetActive(state);
	}

    internal void CreateIcon(string p_name)
    {
        if (p_name == "Glass") {
            GameObject obj = Instantiate(Resources.Load(Const_info.GENERALS_OBJECT_PATH + "DragableUI", typeof(GameObject))) as GameObject;
			obj.transform.parent = glassLocker_UI;
            obj.transform.localPosition = new Vector3(-31f, -18f, -2);
            obj.name = "Glass";

            tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
            sprite.spriteId = sprite.GetSpriteIdByName("glass");

            glass = obj.GetComponent<DragableGUI>();
            glass.dragable_event += Handle_GlassOnDragable_event;
        }
        else if (p_name == "Cup") { 
            GameObject obj = Instantiate(Resources.Load(Const_info.GENERALS_OBJECT_PATH + "DragableUI", typeof(GameObject))) as GameObject;
			obj.transform.parent = glassLocker_UI;
            obj.transform.localPosition = new Vector3(-0.6f, -18f, -2);
            obj.name = "Cup";

            tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
            sprite.spriteId = sprite.GetSpriteIdByName("cup");

            System.EventHandler Handle_cup_event = null;
            Handle_cup_event = (sender, e) => {
				cup.dragable_event -= Handle_cup_event;
				this.CloseGlassLockerUI();
			};
			
            cup = obj.GetComponent<DragableGUI>();
            cup.dragable_event += Handle_cup_event;
		}
        else if (p_name == "Disk") {
            GameObject newdisk = Instantiate(Resources.Load(Const_info.GENERALS_OBJECT_PATH + "DragableUI", typeof(GameObject))) as GameObject;
            newdisk.transform.parent = glassLocker_UI;
            newdisk.transform.localPosition = new Vector3(30.5f, -18.85f, -2f);
            newdisk.name = "Disk";

            tk2dSprite sprite = newdisk.GetComponent<tk2dSprite>();
            sprite.spriteId = sprite.GetSpriteIdByName("disk");

            System.EventHandler myHandle = null;
            myHandle = (sender, e) => {
                disk.dragable_event -= myHandle;
                this.CloseGlassLockerUI();
            };
            disk = newdisk.GetComponent<DragableGUI>();
            disk.dragable_event += myHandle;
        }
		else if(p_name == "BigGlass") {
            GameObject obj = Instantiate(Resources.Load(Const_info.GENERALS_OBJECT_PATH + "DragableUI", typeof(GameObject))) as GameObject;
			obj.transform.parent = glassLocker_UI;
            obj.transform.localPosition = new Vector3(-62f, -18f, -2);
            obj.name = "BigGlass";

            tk2dSprite sprite = obj.GetComponent<tk2dSprite>();
            sprite.spriteId = sprite.GetSpriteIdByName("BigGlass");
			
            bigGlass = obj.GetComponent<DragableGUI>();
            bigGlass.dragable_event += Handle_BigGlassdragable_event;
		}
    }

	void Handle_GlassOnDragable_event (object sender, System.EventArgs e)
	{
		glass.dragable_event -= Handle_GlassOnDragable_event;
		
		this.CloseGlassLockerUI();
	}

    void Handle_BigGlassdragable_event (object sender, System.EventArgs e)
    {
		bigGlass.dragable_event -= Handle_BigGlassdragable_event;

        this.CloseGlassLockerUI();
    }
}
