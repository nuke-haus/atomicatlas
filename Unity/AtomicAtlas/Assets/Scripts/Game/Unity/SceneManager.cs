using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager: MonoBehaviour
{
    private IGameStateManager gameStateManager;

    public static SceneManager GlobalInstance
    {
        get;
        private set;
    }

    void Start()
    {
        GlobalInstance = this;
        gameStateManager = DependencyInjector.Resolve<IGameStateManager>();
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
