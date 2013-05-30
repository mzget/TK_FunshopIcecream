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
	protected virtual void Onactive_event (HandleNameArgs e)
	{
		var handler = active_event;
		if (handler != null)
			handler (this, e);
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
}
