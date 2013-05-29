using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Mz_ScriptLib/GUI/Mz_GuiButtonBeh")]
public class Mz_GuiButtonBeh : Base_ObjectBeh {
	
	public bool enablePlayAudio = true;
	public bool enableChangeScale = true;
	
	private Mz_BaseScene gameController;
    private Vector3 originalScale;

	public event EventHandler click_event; 	
	protected void OnButtonDown_event (EventArgs e)
	{
		var handler = click_event;
		if (handler != null)
			handler (this, e);
	}
		
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Mz_BaseScene>();		
        originalScale = this.transform.localScale;
	}

	void OnApplicationPause (bool pause) {
		collider.enabled = !pause;
	}
	
	protected override void OnTouchBegan ()
	{
		base.OnTouchBegan ();
		
		if(enableChangeScale)
			this.transform.localScale = this.transform.localScale * 1.1f;

		if(this.enablePlayAudio)
			gameController.audioEffect.PlayOnecSound(gameController.audioEffect.buttonDown_Clip);	
	}
	protected override void OnTouchDown ()
	{
        gameController.OnInput(this.gameObject.name);
		OnButtonDown_event (EventArgs.Empty);

		base.OnTouchDown ();
	}
	
	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded ();		
		
        this.transform.localScale = originalScale;
	}
}
