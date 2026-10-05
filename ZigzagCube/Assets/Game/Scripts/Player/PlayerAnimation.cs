using UnityEngine;

public class PlayerAnimation : ModuleBase<PlayerController>
{
    private Transform modelTransform;

    public override void Initialize()
    {
        modelTransform = controller.Model.transform;
    }
}