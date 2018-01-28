using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using ORKFramework;

namespace RTSPrototype
{
    public class RTSUiMasterWrapper : RTSUiMaster
    {
        #region Overrides
        public override bool isPauseMenuOn
        {
            get
            {
                //Is RPG Menu Active
                return ORK.Control.InMenu;
            }
        }

        float MenuFadeValue
        {
            get {
                return Mathf.Max(
                    ORK.MainMenu.fadeIn.time
                    ,ORK.MainMenu.fadeOut.time);
            }
        }

        public override void CallEventMenuToggle()
        {
            //If Ui Item isn't being used or Pause Menu is turned on
            if(isUiAlreadyInUse == false || isPauseMenuOn)
                Invoke("WaitToCallEventMenuToggle", MenuFadeValue + 0.4f);
        }
        #endregion

    }
}