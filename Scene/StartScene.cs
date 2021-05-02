using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        sceneType = Defines.SceneType.StartScene;
        Managers.UI.ShowSceneUI<FixedStartSceneUI>(Defines.SceneType.StartScene);
    }

    public override void Clear()
    {
    }
}
