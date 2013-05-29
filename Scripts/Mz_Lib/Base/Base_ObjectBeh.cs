using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mz_get.
/// last edit 2013-5-29
/// </summary>
public class Base_ObjectBeh : MonoBehaviour {
    
    protected bool _OnTouchBegin = false;
//	protected bool _OnTouchMove = false;
	protected bool _OnTouchRelease = false;
	private double time;
    protected List<string> arr_exceptionObjectsName = new List<string>();
	
	
	protected virtual void Start() { }
	
	protected virtual void Update() {      
		if(_OnTouchBegin) {
			if(time < 1) 
				time += Time.deltaTime;
			
			if(time >= 1) { 
				time = 0;
				this.OnTouchStationary();
			}
		}
		
        if (_OnTouchBegin && _OnTouchRelease) {
            OnTouchDown();
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
            if (Input.touchCount >= 1) {
                Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Began) {
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					RaycastHit rayHit ;
					if(Physics.Raycast(ray, out rayHit, Mathf.Infinity)) {
						if (arr_exceptionObjectsName.Contains(rayHit.collider.name) == false)
	                    {
	                        this.OnTouchOther();
	                    }
					}
					else {
						this.OnTouchOther();
					}
				}
				
                if (touch.phase == TouchPhase.Ended) {
                    this.OnTouchEnded();
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor) {
            if (Input.GetMouseButtonUp(0)) {
                this.OnTouchEnded();
            }
			
			if(Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit rayHit ;
                if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
                {
					if (arr_exceptionObjectsName.Contains(rayHit.collider.name) == false)
                    {
                        this.OnTouchOther();
                    }
				}
				else {
					this.OnTouchOther();
				}
			}
        }
	}

	#region <!-- On Mouse Events.

	protected virtual void OnTouchBegan() {
        if(_OnTouchBegin == false)
			_OnTouchBegin = true;
	}

    protected virtual void OnTouchDown() {
        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
//		_OnTouchMove = false;
    }

    protected virtual void OnTouchStationary() { }

    protected virtual void OnTouchDrag() {
        //		_OnTouchMove = true;
    }

    protected virtual void OnTouchEnded () {
		if(_OnTouchBegin)
			_OnTouchRelease = true;
		
		time = 0;
    }
	
	protected virtual void OnTouchOther() {	}

    #endregion

	public virtual void OnDispose() { }
}
