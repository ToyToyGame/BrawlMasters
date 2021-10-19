using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBulletGun : MonoBehaviour
{
	[Header("Weapon")]
	public float reloadTime = 0.5f;
	public uint magazine = 3;
	private uint restMagazine = 3;

	private float lastShootingTime = 0.0f;
	public float shootingDelay = 0.25f;
	public float currShootingDelay;

	public Animation anim;
	public Transform muzzlePosition;

	[SerializeField]
	private GameObject ownedBulletObject;

	public Bullet GetBullet()
	{
		return ownedBulletObject.GetComponent<Bullet>();
	}

	void Start()
	{
		lastShootingTime = Time.realtimeSinceStartup;
		InvokeRepeating(nameof(CheckReloading), 0.0f, reloadTime);
	}

	void CheckReloading()
	{
		if (restMagazine < magazine)
		{
			restMagazine += 1;
			UIManager.Instance.weaponStatText.text = restMagazine + " / " + magazine;
		}
	}

	public bool Fire(BrawlerController player)
	{
		if (restMagazine > 0)
		{
			//float delay = (player.itemStat.isSpeedUp ? shootingDelay * (2 - player.itemStat.speedUpAmount) : shootingDelay);
			float delay = (player.isSpeedUp ? shootingDelay * (2 - player.speedUpAmount) : shootingDelay);
			if ((lastShootingTime + delay) <= Time.realtimeSinceStartup)
			{
				if(anim.GetClip("FireGun"))
				{
					anim.CrossFade("FireGun", 0.1f);
				}

				restMagazine -= 1;
				UIManager.Instance.weaponStatText.text = restMagazine + " / " + magazine;
				lastShootingTime = Time.realtimeSinceStartup;

				GameObject bulletObject = ObjectPoolingManager.SharedInstance.GetPooledObject(ownedBulletObject.tag);
				if (bulletObject != null)
				{
					bulletObject.GetComponent<Bullet>().SetBulletInfo(player, muzzlePosition.position, transform.forward * 1.5f);
					bulletObject.SetActive(true);
					Debug.Log($"BULLET INFO damage: {bulletObject.GetComponent<Bullet>().damage} range:{bulletObject.GetComponent<Bullet>().distance}");
					return true;
				}
			}
		}

		return false;
	}
}
