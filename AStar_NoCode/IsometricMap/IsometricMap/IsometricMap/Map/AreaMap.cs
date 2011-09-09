using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace IsometricMap.Map
{
    public class AreaMap
    {
        List<MapLayer> m_Map;
        Vector2 m_tileWidth;

        public AreaMap(int width, int height, int tileWidth, int tileHeight, int numberOfLayers)
        {
            m_tileWidth = new Vector2(width, height);
            m_Map = new List<MapLayer>(); 
            
            for (int i = 0; i < numberOfLayers; i++)
            {
                m_Map.Add(new MapLayer(new Vector2(width, height), new Vector2(tileWidth, tileHeight)));
            }
        }

        public void LoadLayer(List<List<int>> layerMap, int layerNumber)
        {
            MapLayer layer = m_Map[layerNumber];
            for (int i = 0; i < layerMap.Count; i++)
            {
                for(int j = 0; j < layerMap[i].Count; j ++)
                {
                    layer.AddTile( layerMap[i][j], new Vector2(i * m_tileWidth.X, j * m_tileWidth.Y));
                }
            }
        }

    }
}
