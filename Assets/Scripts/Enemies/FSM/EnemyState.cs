using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState
{
    void EnterState(BaseEnemy enemy);
    void OnUpdate(BaseEnemy enemy);
}
