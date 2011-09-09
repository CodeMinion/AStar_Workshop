using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
using IsometricMap.Logic.Base;

namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    public class GLumberjack_ChopWood:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_ChopWood m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_ChopWood()
        {
            m_sStateName = "CHOP_WOOD";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_ChopWood GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_ChopWood();

            return m_Instance;
        }

        public override void OnEnter(GoblinLumberjack owner)
        {
            // Set the chopping animation.
            owner.SetCurrentAnimation("CHOP");
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {
            // Chop wood, what else could this be...
            owner.ChopWood();
            if (owner.IsDoneChoppingWood())
            {
                // Transition to wood mill travel state.
                owner.GetStateMachine().ChangeState(GLumberjack_MillTravel.GetInstance());
            }
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            // Make the goblin stay IDLE
            owner.SetCurrentAnimation("IDLE");
            // Stop Chopping Wood.
            owner.StopChoppingWood();
        }
    }
}
