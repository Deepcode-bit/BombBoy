using Assets.Scripts.Enemies.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Captain:BaseEnemy
    {
        public async override void SkillAction()
        {
            if (!canSkill) return;
            if (Vector2.Distance(transform.position, targetPoint.position) > skillRange) return;
            canSkill = false;
            await Task.Delay(TimeSpan.FromSeconds(0.3));           
            animator.SetBool("isRun", true);
            TransToState(new RunState());
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            canSkill = true;
        }
    }
}
