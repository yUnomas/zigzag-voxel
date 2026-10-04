using UnityEngine;

public class PlayerAnimation : ModuleBase<PlayerController>
{
    private Transform modelTransform;

    public override void Initialize()
    {
        modelTransform = controller.Model.transform;
    }
    public void Turn(int direction)
    {
        modelTransform.rotation = Quaternion.Euler(0, 180f + -45f * direction, 0);
    }
}
