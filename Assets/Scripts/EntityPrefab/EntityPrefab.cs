using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityPrefab: MonoBehaviour
{
	[SerializeField]
	Transform entity = default;

	public void setLayer(string name)
	{
		entity.gameObject.layer = LayerMask.NameToLayer(name);
	}

	public void setSprite(string sprite, string alignment)
	{
		Sprite newSprite = Resources.Load($"Art/Sprites/Entity/{alignment}/{sprite}", typeof(Sprite)) as Sprite;
		entity.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
	}

	public void setXFlip()
	{
		Vector3 newScale = entity.gameObject.transform.localScale;
		newScale.x *= -1;
		entity.gameObject.transform.localScale = newScale;
	}
}