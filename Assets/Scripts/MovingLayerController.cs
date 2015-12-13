using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class MovingLayerController : MonoBehaviour {
	
	public MovingLayer[] layers;
	
	// Use this for initialization
	void Start () {
		foreach(MovingLayer layer in layers) {
			layer.Start();
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(MovingLayer layer in layers) {
			layer.Update();
		}
	}
}


[System.Serializable]
public class MovingLayer {
	
	public Transform background;
	//	public List<Transform> backgroundsMirrors;
	[Tooltip("If set 0 will be used sprite width/height")]
	public Vector2 size = new Vector2(0, 0);
	public Vector2 speed = new Vector2(0, 0);
	
	private Vector3 __movingSpeed3;
	private List<MovingLayerPart> __parts = new List<MovingLayerPart>();
	private Vector2 __sign = new Vector2(1, 1);
	
	public void Start() {
		
		if (size.x == 0) {
			size.x = background.GetComponent<SpriteRenderer>().bounds.size.x;
		}
		if (size.y == 0) {
			size.y = background.GetComponent<SpriteRenderer>().bounds.size.y;
		}
		
		MovingLayerPart mainSprite = new MovingLayerPart();
		mainSprite.SetSprite(background);
		mainSprite.SetSize(size);
		__parts.Add(mainSprite);

		__sign.x = speed.x < 0 ? 1 : -1;
		__sign.y = speed.y < 0 ? 1 : -1;
		
		if (speed.y != 0) {
			__SetClone(0, size.y, "vertical");
		}
		
		if (speed.x != 0) {
			__SetClone(size.x, 0, "horisontal");
		}
		
		if (speed.x != 0 && speed.y != 0) {
			__SetClone(size.x, size.y, "diagonal");
		}
		
		foreach (MovingLayerPart part in __parts) {
			part.Start();
		}
	}
	
	public void Update() {
		foreach (MovingLayerPart part in __parts) {
			part.Update(speed);
		}
	}
	
	private void __SetClone(float sizeX = 0, float sizeY = 0, string name = "") {
		MovingLayerPart verticalSprite = new MovingLayerPart();
		Transform verticalSpriteTransform = Transform.Instantiate(background) as Transform;
		verticalSpriteTransform.transform.parent = background.transform.parent;
		verticalSpriteTransform.transform.name = background.transform.name + " " + name + " clone";
		verticalSprite.SetSprite(verticalSpriteTransform);
		verticalSprite.SetDelta(new Vector2(sizeX * __sign.x, sizeY * __sign.y));
		verticalSprite.SetSize(size);
		__parts.Add(verticalSprite);
	}
	
}


[System.Serializable]
public class MovingLayerPart {
	private Transform __transform;
	private Vector2 __size = new Vector2(0, 0);
	private Vector2 __delta = new Vector2(0, 0);
	private Vector3 __startPositin;
	
	public void SetSprite(Transform newTransform) {
		__transform = newTransform;
	}
	public void SetDelta(Vector2 newDelta) {
		__delta = newDelta;
	}
	public void SetSize(Vector2 newSize) {
		__size = newSize;
	}
	
	public void Start() {
		__transform.position = new Vector3(
			__transform.position.x + __delta.x,
			__transform.position.y + __delta.y,
			__transform.position.z
			);
		__startPositin = __transform.position;
	}
	public void Update(Vector2 speed) {
		Vector3 newPos = new Vector3(
			__transform.position.x + speed.x,
			__transform.position.y + speed.y,
			__transform.position.z
		);

		if (newPos.x - __startPositin.x > __size.x) {
			newPos.x -= __size.x;
		} else if (newPos.x - __startPositin.x < -__size.x) {
			newPos.x += __size.x;
		}

		if (newPos.y - __startPositin.y > __size.y) {
			newPos.y -= __size.y;
		} else if (newPos.y - __startPositin.y < -__size.y) {
			newPos.y += __size.y;
		}

		__transform.position = newPos;
	}
}
