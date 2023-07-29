using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager: MonoBehaviour
{
    private IGameStateManager gameStateManager = DependencyInjector.Resolve<IGameStateManager>();

    public static SceneManager GlobalInstance
    {
        get;
        private set;
    }

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
        gameStateManager.OnUpdate();
    }

    private void FixedUpdate()
    {
        gameStateManager.OnFixedUpdate();
    }
}
