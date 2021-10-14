using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public enum ItemType
	{
		Item,
		Weapon,
		Bullet,
	}

	public ItemType itemType;
	public bool isDisapear;
	public bool isOwned;
}