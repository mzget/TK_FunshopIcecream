using UnityEngine;
using System;
using System.Collections;

public class DragableGUI : ObjectsBeh
{

    #region <!-- Event data.

    public event EventHandler dragable_event;
	protected void OnDragableEvent(EventArgs e) {
		if(dragable_event != null) {
			dragable_event(this, e);
		}
	}

    #endregion

    protected override void OnTouchBegan()
    {
        base.OnTouchBegan();
        this.transform.localScale = this.transform.localScale * 1.3f;
    }

    protected override void OnTouchEnded()
    {
        base.OnTouchEnded();

        this.transform.localScale = this.originalScale;
    }

    protected override void ImplementDraggableObject ()
	{
		base.ImplementDraggableObject ();
		
		this.OnDragableEvent(EventArgs.Empty);
		
		Ray cursorRay;
		RaycastHit hit;
		cursorRay = new Ray(this.transform.position, Vector3.forward);		
		Debug.DrawRay(cursorRay.origin, Vector3.forward * 100f, Color.red);
		
		if (Physics.Raycast(cursorRay, out hit, 1000f))
		{
            if (hit.collider.name == "Napery")
            {
                if (this._isDropObject)
                {
               		bool canDropObjToNapery = Shop.Instance.napery.CheckingStatus();
                    if (canDropObjToNapery)
                    {
                        Shop.Instance.CreateObjOnNaperyArea(this.gameObject.name);
                        GlassLockerBeh.Instance.CreateIcon(this.gameObject.name);
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        GlassLockerBeh.Instance.CreateIcon(this.gameObject.name);
                        Destroy(this.gameObject);
                    }

                    this._isDropObject = false;
                    base._isDraggable = false;
                }
            }
            //			else if(hit.collider.tag == "Building" || hit.collider.tag == "TerrainElement") {
            //				print("Tag == " + hit.collider.tag + " : Name == " + hit.collider.name);
            //				
            //				TilebaseObjBeh hit_obj = hit.collider.GetComponent<TilebaseObjBeh>();
            //				hit_obj.ShowConstructionAreaStatus();
            //				
            //				if(_isDropObject) {
            //					Debug.LogWarning("Building and Terrain element cannot construction");
            //					
            //					this.transform.position = this.originalPosition;
            //					constructionArea = temp_originalArea;
            //					Destroy(this.gameObject);
            //					sceneController.taskManager.CreateBuildingIconInRightSidebar(this.name);
            //					
            //					this._isDropObject = false;
            //					base._isDraggable = false;
            //					TaskManager.IsShowInteruptGUI = false;
            //				}
            //			}
            else
            {
                if (this._isDropObject)
                {
                    GlassLockerBeh.Instance.CreateIcon(this.gameObject.name);
                    Destroy(this.gameObject);
                }
            }
		}
		else
		{
            print("Out of ray direction");
            if (this._isDropObject)
            {
                GlassLockerBeh.Instance.CreateIcon(this.gameObject.name);
                Destroy(this.gameObject);
            }
		}
	}
}
