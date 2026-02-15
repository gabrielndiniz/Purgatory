using Survivor.Trigger;
using UnityEngine;

public class DefeatTrigger : BaseTrigger
{
    [SerializeField] private Fader fader;

    private void OnEnable()
    {
        OnTriggered();
    }
    protected override void OnTriggered()
    {
        fader.FadeIn();
    }
}
