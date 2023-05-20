using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUpResponse : MonoBehaviour
{
    [SerializeField] private WeaponResponse weaponPrefab;
    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            PlayerMechanicResponse player = target.GetComponent<PlayerMechanicResponse>();
            WeaponResponse weapon = Instantiate(weaponPrefab);
            player.Equip(weapon);
        }
    }
}
