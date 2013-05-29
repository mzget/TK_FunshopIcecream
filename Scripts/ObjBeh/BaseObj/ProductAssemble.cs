using UnityEngine;
using System;
using System.Collections;

public class ProductAssemble : ObjectsBeh {

	protected GoodsBeh product;
	protected Vector3 productPos;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		this.destroyObj_Event += Handle_destroyObj_Event;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected virtual string GetGoodsName() {
	 	return string.Empty;	
	}

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
					OnDestroyObject_event(System.EventArgs.Empty);
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
	
	void Handle_destroyObj_Event (object sender, EventArgs e)
	{
		this.destroyObj_Event -= Handle_destroyObj_Event;
		
		this.OnDispose();
		Destroy (this.gameObject);
	}
}
