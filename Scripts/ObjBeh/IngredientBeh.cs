using UnityEngine;
using System;
using System.Collections;

public class IngredientBeh : ObjectsBeh {

	public class HandleNameArgs : EventArgs {
		public string eventName;
	};

	public event EventHandler<HandleNameArgs> active_event;
	protected virtual void Onactive_event (HandleNameArgs e)
	{
		var handler = active_event;
		if (handler != null)
			handler (this, e);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnTouchDown ()
	{
		this.Onactive_event(new HandleNameArgs() { eventName = this.name, });
		if(animatedSprite != null) 
			animatedSprite.Play();
		
		base.OnTouchDown ();
	}
}
