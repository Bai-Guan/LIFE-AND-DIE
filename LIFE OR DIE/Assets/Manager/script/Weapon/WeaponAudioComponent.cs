using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioComponent : WeaponComponent
{
    private WeaponAudioData _Audiodata;
    private InitWeaponSystem Weapon;
    public override void InitData(ComponentData data)
    {
        if(data is WeaponAudioData WAdata)
        {
            _Audiodata = WAdata;
        }
    }

    private void Awake()
    {
        Weapon=this.transform.GetComponent<InitWeaponSystem>();

    }
    private void Start()
    {
        Weapon.EventHandler.OneAttack += OnAttackAudio;
    }

    private void OnAttackAudio()
    {
        AudioManager.Instance.PlaySFX(_Audiodata.AudioName[Weapon.CurrentNum]);
    }
    private void OnDisable()
    {
        Weapon.EventHandler.OneAttack -= OnAttackAudio;
    }
}
