using UnityEngine;
using System.Collections;

public class NewItemButtonBeh : Base_ObjectBeh {

	public string newItemName { get; set; }
    
	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();

        StartCoroutine_Auto(this.CreateEffectAndDestroy());
	}

	IEnumerator CreateEffectAndDestroy ()
	{
		Shop.Instance.gameEffectManager.Create2DSpriteAnimationEffect (GameEffectManager.BLOOMSTAR_EFFECT_PATH, this.transform);
        Shop.Instance.audioEffect.PlayOnecWithOutStop(Shop.Instance.audioEffect.correct_Clip);
        Shop.Instance.TK_animationManager.RandomPlayGoodAnimation();

		yield return new WaitForSeconds (0.5f);

        Shop.NewItem_name.Remove(newItemName);
        Shop.Instance.NoticeUserWhenHaveNewItem();
		Destroy (this.gameObject);
	}
}
