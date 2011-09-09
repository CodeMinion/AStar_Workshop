using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsometricMap.Logic.Base
{
    /// <summary>
    /// This class represents a single state. 
    /// It defines the besic structure of every
    /// state.
    /// </summary>
    /// <typeparam name="T">The type of the owner of this state.</typeparam>
    public abstract class BaseState<T>
    {
        protected String m_sStateName = "";
        /// <summary>
        /// This method is called everytime the 
        /// state is activated. Used for set up
        /// if needed.
        /// </summary>
        /// <param name="owner">Reference to the state owner.</param>
        public abstract void OnEnter(T owner);

        /// <summary>
        /// This method is called every time the
        /// state must be updated. Implement state 
        /// logic here.
        /// </summary>
        /// <param name="owner">Reference to the state owner.</param>
        public abstract void OnUpdate(T owner);

        /// <summary>
        /// This method is called every time that
        /// the state exits. Used for clean up
        /// if needed.
        /// </summary>
        /// <param name="owner"></param>
        public abstract void OnExit(T owner);

        public String GetStateName()
        {
            return m_sStateName;
        }
    }
}
