using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace IsometricMap.Sprites
{
    /// <summary>
    /// Animation Class for the SIG-Games workshops.
    /// This class represents a single animation. 
    /// For the purpose of this project animations 
    /// are represented as a consecutive set of frames 
    /// in a given image.
    /// </summary>
    public class Animation
    {
        // Start X , Y position in the source image of the animation.
        int m_sourceX;
        int m_sourceY;
        
        // Size of the frame in pixels.
        int m_frameWidth;
        int m_frameHeight;

        // Current frame index.
        int m_currFrame;
        // Number of frames in the animation.
        int m_animSize;

        // Time in milliseconds that the frame remains on the screen
        // before the next one is loaded.
        int m_frameDelay;

        // Should the animation loop once it is finished?
        bool m_Looping;

        // Time to switch the next frame.
        double m_nextUpdate;

        /// <summary>
        /// Animation constructor.
        /// </summary>
        /// <param name="sourceX">Start X Position of the animation in the source image.</param>
        /// <param name="sourceY">Start Y Position of the animation in the source image.</param>
        /// <param name="animSize">Number of frames in the animation.</param>
        /// <param name="frameWidth">Width in pixel of each frame.</param>
        /// <param name="frameHeight">Height in pixel of each frame.</param>
        /// <param name="framDelay">Amount of time in milliseconds to display the frame.</param>
        /// <param name="loop">Should the animation loop?</param>
        public Animation(int sourceX, int sourceY, int animSize, int frameWidth, int frameHeight, int framDelay, bool loop)
        {
            m_currFrame = 0;
            m_frameDelay = framDelay;
            m_frameHeight = frameHeight;
            m_frameWidth = frameWidth;
            m_Looping = loop;
            m_sourceX = sourceX;
            m_sourceY = sourceY;
            m_animSize = animSize;
        }
        /// <summary>
        /// Update function for the animation.
        /// During each update we check if the 
        /// current frame has expired, if so
        /// then we load the next frame and 
        /// update the next update counter.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public void Update(double gameTime)
        {
            if (m_nextUpdate <= gameTime)
            {
                m_currFrame = (m_currFrame + 1) % m_animSize;
                m_nextUpdate = gameTime + m_frameDelay;
            }
            
        }

        /// <summary>
        /// This method returns a rectangle for
        /// the current frame. In this example 
        /// animation are displayed in a single line
        /// in the source image.
        /// </summary>
        /// <returns>Rectangle for this frame.</returns>
        public Rectangle GetCurrentFrameSource()
        {
            Rectangle source = new Rectangle();
            source.X = m_sourceX + m_currFrame * m_frameWidth;
            source.Y = m_sourceY;
            source.Width = m_frameWidth;
            source.Height = m_frameHeight;

            return source;
        }
    }
}
