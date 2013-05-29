using UnityEngine;
using System.Collections;

public class BlockIcecreamBeh : IngredientBeh
{
	
	// Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void OnTouchDown ()
	{
		IcecreamTankBeh.Instance.Handle_input(this.gameObject.name);
		base.Onactive_event(new IngredientBeh.HandleNameArgs());
		
		base.OnTouchDown ();
	}
}

