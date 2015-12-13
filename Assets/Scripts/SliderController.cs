using UnityEngine;
using System.Collections;

public class SliderController : MonoBehaviour
{
	public int nextScene = 1;

	private int currentSlide = 1;
	private float[] buttonsState = {0f, 0f};
	private Transform[] slides;

	// Use this for initialization
	void Start ()
	{
		slides = gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < slides.Length; i ++) {
			if (i > 1) {
				slides[i].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		float leftBtn = Input.GetAxis("Fire1");
		if (leftBtn != buttonsState[0]) {
			buttonsState[0] = leftBtn;
			if (leftBtn == 1 && currentSlide > 1) {
				currentSlide--;
				slides[currentSlide].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
				slides[currentSlide + 1].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
			}
		}
		
		float rightBtn = Input.GetAxis("Fire2");
		if (rightBtn != buttonsState[1]) {
			buttonsState[1] = rightBtn;
			if (rightBtn == 1) {
				if (currentSlide == slides.Length - 1) {
					Application.LoadLevel(nextScene);
				} else {
					slides[currentSlide].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
					currentSlide++;
					slides[currentSlide].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
				}
			}
		}
	}
}

