using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
using IsometricMap.Logic.Base;

namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    public class GLumberjack_DepositWood:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_DepositWood m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_DepositWood()
        {
            m_sStateName = "DEPOSIT_WOOD";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_DepositWood GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_DepositWood();

            return m_Instance;
        }

        public override void OnEnter(GoblinLumberjack owner)
        {
            // Activate animation for putting down logs.
            owner.SetCurrentAnimation("PUT_DOWN");
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {

            owner.DepositWood();
            if (owner.IsDoneDropingWood())
            {
                // If we are done depositing then switch back to the 
                // Tree Travel state to go back to our tree and continue
                // chopping wood. 
                owner.GetStateMachine().ChangeState(GLumberjack_TreeTravel.GetInstance());
            }
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            // Stop Depositing Wood.
            owner.StopDepositingWood();
        }
    }
}
