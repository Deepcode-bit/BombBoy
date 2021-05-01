using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    class Cucumber : BaseEnemy
    {
        #region Animation Event
        public void TurnOffBomb() => targetPoint?.GetComponent<Bomb>()?.TurnOff();
        #endregion
    }
}
