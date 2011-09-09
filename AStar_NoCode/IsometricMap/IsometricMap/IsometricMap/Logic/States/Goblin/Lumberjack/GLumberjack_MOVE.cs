using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Logic;
using IsometricMap.Logic.Base;

using IsometricMap.Entities;

namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    class GLumberjack_MOVE:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_MOVE m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_MOVE()
        {
            m_sStateName = "MOVE";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_MOVE GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_MOVE();

            return m_Instance;
            
        }

        public override void OnEnter(GoblinLumberjack owner)
        {
            // On state activation activate the "MOVE" animation.
            owner.SetCurrentAnimation("MOVE");
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {
            // If the goblin has reached its destination. 
            if (owner.IsAtDestination())
            {
                
                // Make the goblin go idle. 
                owner.GetStateMachine().ChangeState(GLumberjack_IDLE.GetInstance());
                
            }
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            // We do no clean up in this state.
        }
    }
}
