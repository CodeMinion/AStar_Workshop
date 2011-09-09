using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Logic;
using IsometricMap.Logic.Base;

using IsometricMap.Entities;
namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    public class GLumberjack_IDLE:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_IDLE m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_IDLE()
        {
            m_sStateName = "IDLE";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_IDLE GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_IDLE();

            return m_Instance;
        }

        public override void OnEnter(GoblinLumberjack owner)
        {
            // There is no init we want to do.
            owner.SetCurrentAnimation("IDLE");
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {

            //If there is a target tree, then we wish to make the
            // gobling travel to it. 
            if (owner.GetTargetTree() != null && !owner.IsDoneChoppingWood())
            {
                // Transition to TreeTravel State.
                owner.GetStateMachine().ChangeState(GLumberjack_TreeTravel.GetInstance());
            }
            else if (owner.GetTargetLumberMill() != null && !owner.IsDoneDropingWood())
            {
                // Transition to Mill Travel State
                owner.GetStateMachine().ChangeState(GLumberjack_MillTravel.GetInstance());
            }

            
            // Otherwise we do nothing.
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            // There is no clean up we want to do.
        }
    }
}
