using UnityEngine;
using System.Collections;

public class IngredientController : MonoBehaviour {

	private static IngredientController instance;
	public static IngredientController Instance {
		get{
			if(instance == null) {
				GameObject main = GameObject.FindGameObjectWithTag("GameController");
				instance = main.GetComponent<IngredientController>();
			}
			return instance;
		}
	}

	public IngredientBeh chocolateJam;
	public IngredientBeh strawberryJam;

	public IngredientBeh blueberry_fruit; 
	public IngredientBeh cherry_fruit;
	public IngredientBeh strawberry_fruit;
	public IngredientBeh kiwi_fruit;
	public IngredientBeh dragon_fruit;
	public IngredientBeh mango_fruit;

	public IngredientBeh whipCream;
	public IngredientBeh almondPowder;
	public IngredientBeh fancy_sugar;
	public IngredientBeh banana;

	public IngredientBeh cola;
	public IngredientBeh fruitPunch;
	public IngredientBeh strawberryMillShake;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
