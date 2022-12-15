using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

[Serializable]
public class VfxData
{
    public VfxType vfxType;
    public ParticleSystem particle;
}
public enum VfxType
{
    THUNDERSTORM,
    WINDSTORM,
    RAID
}


public class VFXManager : MonoBehaviour
{
    [SerializeField] private VfxData[] _vfxData;
    [SerializeField] private Transform _vfxParent;
    private Dictionary<VfxType, ParticleSystem> _vfxDict = new Dictionary<VfxType, ParticleSystem>();

    private const float VFX_DURATION = 3f;

    public static Action<string> VfxToggled = null;
    private ParticleSystem _currentVFX = null;

    public void PlayVFX(VfxType type) 
    {
        if(_vfxDict.ContainsKey(type))
        {
            _currentVFX = _vfxDict[type];
        }
        else
        {
            var data = _vfxData.FirstOrDefault(i => i.vfxType == type);
            if (data != null)
            {
                ParticleSystem particles = Instantiate(data.particle, _vfxParent);
                _vfxDict.Add(type, particles);
                _currentVFX = particles;
            }
        }
        if (_currentVFX != null)
        {
            _currentVFX.gameObject.SetActive(true);
            _currentVFX.Play();
            VfxToggled?.Invoke(type.ToString());

            StopVfx(type, VFX_DURATION);
        }
    }

    public void StopVfx(VfxType type, float delay)
    {
        StartCoroutine(StopVfxCoroutine(type, delay));
    }

    IEnumerator StopVfxCoroutine(VfxType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        _currentVFX.Stop();
        _currentVFX.gameObject.SetActive(false);
        VfxToggled?.Invoke(string.Empty);
    }
}
