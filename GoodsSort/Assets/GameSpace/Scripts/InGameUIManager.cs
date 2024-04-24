using System;
using System.Collections;
using MoreMountains.Tools;

public class InGameUIManager : MMSingleton<InGameUIManager>
{
    public UIButtonSkill[] bBooster;
    private BoosterManager boosters;

    protected override void Awake()
    {
        InitializeButtonSkill();
    }
    
    private void InitializeButtonSkill()
    {
        bBooster[0].onClick = () => ActionClick(boosters.RevertBooster);
        bBooster[1].onClick = () => ActionClick(boosters.ShuffleBooster);
        bBooster[2].onClick = () => ActionClick(boosters.FrozenBooster);
        bBooster[3].onClick = () => ActionClick(boosters.HammerBooster);
    }
    
    public void ActionClick(Action action)
    {
        StartCoroutine(IEActionClick());
        return;

        IEnumerator IEActionClick()
        {
            yield return null;
            action?.Invoke();
        }
    }
}
