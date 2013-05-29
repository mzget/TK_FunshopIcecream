using UnityEngine;
using System.Collections;

public class ObjectsBeh : Base_ObjectBeh {

	protected Mz_BaseScene baseScene;    
	internal tk2dSprite sprite;
    internal tk2dAnimatedSprite animatedSprite;

    public bool _canDragaable = false;
	protected bool _isDraggable = false;
    protected bool _isDropObject = false;
	protected bool _canActive = false;	
    internal Vector3 originalPosition;
    internal Vector3 originalScale;

    #region <!-- Events data.

    /// <summary>
    /// destroyObj_Event.
    /// </summary>	
	internal event System.EventHandler destroyObj_Event;
    protected void OnDestroyObject_event(System.EventArgs e) {
        if (destroyObj_Event != null)
        {
            destroyObj_Event(this, e);
            Debug.Log(destroyObj_Event + ": destroyObj_Event : " + this.name);
        }
    }

    #endregion

	internal void SetOriginTransform(Vector3 newOriginPos, Vector3 newOriginalScale) {
		this.originalPosition = newOriginPos;
        this.originalScale = newOriginalScale;
	}

    protected virtual void Awake()
    {
        GameObject sceneObj = GameObject.FindGameObjectWithTag("GameController");
        baseScene = sceneObj.GetComponent<Mz_BaseScene>();

        try
        {
            sprite = this.gameObject.GetComponent<tk2dSprite>();
            animatedSprite = this.gameObject.GetComponent<tk2dAnimatedSprite>();
        }
        catch { }
        finally { }
	}

    protected override void Start()
    {
        base.Start();
        this.SetOriginTransform(this.transform.localPosition, this.transform.localScale);
    }
	
	protected virtual void ImplementDraggableObject() {
        Vector3 screenPoint;
		
        if (Input.touchCount >= 1) {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else {
            screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        this.transform.position = new Vector3(screenPoint.x, screenPoint.y, -20f);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		
		if(_canDragaable) {
			if(_isDraggable) {
				this.ImplementDraggableObject();
			}
			
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            if (baseScene.touch.phase == TouchPhase.Ended || baseScene.touch.phase == TouchPhase.Canceled) {			
				if(this._isDraggable)
					_isDropObject = true;
			}
#elif UNITY_EDITOR
			if(Input.GetMouseButtonUp(0)) {
				if(this._isDraggable)
					_isDropObject = true;
			}
#endif
		}
	}
	
    protected override void OnTouchDrag()
    {
        base.OnTouchDrag();
        
        if(this._canDragaable && base._OnTouchBegin) {
			this._isDraggable = true;
        }
    }
	
	protected override void OnTouchEnded ()
	{
		base.OnTouchEnded();
		
		if(this._isDraggable)
			_isDropObject = true;
	}
}
