using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using Opsive.ThirdPersonController.Abilities;

namespace RTSPrototype
{
    public class RTSHeightChange : HeightChange
    {
        RTSGameMode gamemode
        {
            get { return RTSGameMode.thisInstance; }
        }

        public override bool CanStopAbility()
        {
            return !Physics.Raycast(
                m_Transform.position, 
                m_Transform.up, 
                m_Controller.CapsuleCollider.height - 
                m_ColliderHeightAdjustment, 
                gamemode.IgnoreInvisibleLayersAndAllies, 
                QueryTriggerInteraction.Ignore);
        }
    }
}