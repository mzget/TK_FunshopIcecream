using UnityEngine;
using System;
using System.Collections;

public class IngredientBeh : ObjectsBeh {

	public class HandleNameArgs : EventArgs {
		public string eventName;
	};
	
	public event EventHandler animatedSpritePlayCompleteEvent;
	private void OnanimtedSpritePlayComplete(EventArgs e) {
		if(animatedSpritePlayCompleteEvent != null)
			animatedSpritePlayCompleteEvent(this, e);
	} 
	
	public event EventHandler<HandleNameArgs> active_event;
	protected void Onactive_event (HandleNameArgs e)
	{
		if (active_event != null)
            active_event(this, e);
		else {
			Shop.Instance.WarningPlayerToSeeManual();
		}
	}
	
	protected override void OnTouchBegan ()
	{
		base.OnTouchBegan ();
		
		this.transform.localScale = originalScale * 1.5f;
	}

	protected override void OnTouchDown ()
	{
		this.Onactive_event(new HandleNameArgs() { eventName = this.name, });
		
		if(animatedSprite != null) { 
			animatedSprite.Play();
            animatedSprite.animationCompleteDelegate += (sprite, clipid) => { 
                OnanimtedSpritePlayComplete(EventArgs.Empty);
            };
        }
		
		base.OnTouchDown ();
	}
	
	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded ();
		
		this.transform.localScale = originalScale;
	}
}
