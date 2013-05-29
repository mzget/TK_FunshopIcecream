using UnityEngine;
using System;
using System.Collections;

public class GoodsBeh : ObjectsBeh {    
    public const string ClassName = "GoodsBeh";
    internal int index_of_instance;

    public Vector3 offsetPos;	
	private string animationName_001 = string.Empty;
	private string animationName_002 = string.Empty;
	internal int costs;

    #region <@-- Events data.

    //<!-- WaitForIngredientEvent.
	protected bool _isWaitFotIngredient = false;	
	protected event EventHandler waitForIngredientEvent;
    protected void CheckingDelegationOfWaitFotIngredientEvent(object sender, EventArgs e) {
        if (waitForIngredientEvent != null)
            waitForIngredientEvent(sender, System.EventArgs.Empty);
    }
	protected virtual void Handle_waitForIngredientEvent (object sender, System.EventArgs e)
	{
		_isWaitFotIngredient = true;
	}
	public virtual void WaitForIngredient(string ingredientName) {			
		Debug.Log("WaitForIngredient :: " + ingredientName);
	}
	
	//<!-- Put goods objects intance on food tray.
    public class PutGoodsToTrayEventArgs : EventArgs
    {
        public GameObject foodInstance;
    };
	internal event EventHandler<PutGoodsToTrayEventArgs> putObjectOnTray_Event;
	protected void OnPutOnTray_event (PutGoodsToTrayEventArgs e) {
		if (putObjectOnTray_Event != null) 
        {
			putObjectOnTray_Event (this, e);
			Shop.Instance.audioEffect.PlayOnecWithOutStop(Shop.Instance.soundEffect_clips[5]);
			
            Debug.Log(putObjectOnTray_Event + ":: OnPutOnTray_event : " + this.name);

            if(Mz_StorageManage._HasNewGameEvent)
				Shop.Instance.CheckingGoodsObjInTray("newgame_event");
		}
	}

    #endregion

    protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		Ray cursorRay;
		RaycastHit hit;		
	    cursorRay = new Ray(this.transform.position, Vector3.forward);		
		Debug.DrawRay(cursorRay.origin, Vector3.forward, Color.red);
		
		if(Physics.Raycast(cursorRay, out hit, 1000f)) 
        {
			if(hit.collider.name == Shop.Instance.binBeh.name) {			
				if(this._isDropObject == true) {
					Shop.Instance.binBeh.PlayOpenAnimation();
                    this.OnDispose();
                    OnDestroyObject_event(System.EventArgs.Empty);
				}
			}
			else if(hit.collider.name == Shop.Instance.foodsTray_obj.name) {
                if(this._isDropObject) {
					this._isDropObject = false;
	                base._isDraggable = false;
					base._canActive = false;
					this._isWaitFotIngredient = false;
					this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;

                    OnPutOnTray_event(new PutGoodsToTrayEventArgs() { foodInstance = this.gameObject });
                }
            }
        	else {
	            if(this._isDropObject) {
	                this.transform.position = originalPosition;
	                this._isDropObject = false;
	                base._isDraggable = false;
	            }
        	}
		}
		else {
            if(this._isDropObject) {
                this.transform.position = originalPosition;
                this._isDropObject = false;
                base._isDraggable = false;
            }
		}
	}

    private void StopActiveAnimation() {
		//<!--- On object active.
        if(animation) {
			this.animation.Stop();
		}
		else if(animatedSprite) {
            animatedSprite.Stop();
		}
    }

	public void animationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId) {
		if(animationName_002 != "") {
			animatedSprite.Play(animationName_002);
			animatedSprite.animationCompleteDelegate -= animationCompleteDelegate;
		}
	}

    protected override void OnTouchBegan()
    {
        base.OnTouchBegan();

        this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        baseScene.audioEffect.PlayOnecWithOutStop(baseScene.audioEffect.pop_clip);
    }

	protected override void OnTouchDown ()
	{
		if(_canActive && _isWaitFotIngredient) {
			//<!--- On object active.
			if(animatedSprite && animationName_001 != string.Empty) {
				animatedSprite.Play(animationName_001);				
				animatedSprite.animationCompleteDelegate = animationCompleteDelegate;
			}
			else { 
				iTween.PunchPosition(this.gameObject, iTween.Hash("y", 0.2f, "time", 1f, "looptype", iTween.LoopType.loop));
			}
		}

		base.OnTouchDown();
	}

    protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

        this.transform.localScale = new Vector3(1f, 1f, 1);
    }

    public override void OnDispose()
    {
        base.OnDispose();

        Destroy(this.gameObject);
        this.waitForIngredientEvent -= this.Handle_waitForIngredientEvent;
    }	
	
	internal void Handle_DestroyProduct_Event(object sender, System.EventArgs e)
	{
		Debug.Log("Handle_DestroyProduct_Event : " + sender.ToString());
		
		GoodsBeh goods = sender as GoodsBeh;
		Mz_StorageManage.AvailableMoney -= goods.costs;
		Shop.Instance.CreateDeductionsCoin (goods.costs);
		Shop.Instance.ReFreshAvailableMoney();		
		Shop.Instance.foodTrayBeh.goodsOnTray_List.Remove(goods);
		Shop.Instance.foodTrayBeh.ReCalculatatePositionOfGoods();
	}
}