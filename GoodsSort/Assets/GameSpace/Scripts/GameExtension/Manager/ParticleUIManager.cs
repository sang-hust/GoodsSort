// using Coffee.UIExtensions;
using MoreMountains.Tools;
using UnityEngine;

public class ParticleUIManager : MMSingleton<ParticleUIManager>
{
    // [SerializeField] private UIParticle fxTouchPrefab;
    [SerializeField] private ParticleSystem fxPaperFireworks; 
    
    public void PlayPaperFireworks() => fxPaperFireworks.Play();
    public void StopPaperFireworks() => fxPaperFireworks.Stop();
}
