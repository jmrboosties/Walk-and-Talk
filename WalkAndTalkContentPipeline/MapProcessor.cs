using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using TiledLib;

namespace WalkAndTalkContentPipeline
{
    [ContentSerializerRuntimeType("WalkAndTalk.Engine.Tile, WalkAndTalk")]
    public class WorldMapTileContent
    {
        public ExternalReference<Texture2DContent> Texture;
        public Rectangle SourceRectange;
        public SpriteEffects SpriteEffects;
    }

    [ContentSerializerRuntimeType("WalkAndTalk.Engine.Layer, WalkAndTalk")]
    public class WorldMapLayerContent 
    {
        public int Width;
        public int Height;
        public WorldMapTileContent[] Tiles;
        public bool Collisions;
        public bool Foreground;
    }

    [ContentSerializerRuntimeType("WalkAndTalk.Engine.WorldMap, WalkAndTalk")]
    public class WorldMapContent
    {
        public int TileWidth;
        public int TileHeight;
        public int TilesCountX;
        public int TilesCountY;
        public List<WorldMapLayerContent> Layers = new List<WorldMapLayerContent>();
    }

    [ContentProcessor(DisplayName = "TMX Processor")]
    public class MapProcessor : ContentProcessor<MapContent, WorldMapContent>
    {
        public override WorldMapContent Process(MapContent input, ContentProcessorContext context)
        {

            //System.Diagnostics.Debugger.Launch();

            TiledHelpers.BuildTileSetTextures(input, context);
            TiledHelpers.GenerateTileSourceRectangles(input);

            WorldMapContent output = new WorldMapContent
            {
                TileWidth = input.TileWidth,
                TileHeight = input.TileHeight,
                TilesCountX = input.Width,
                TilesCountY = input.Height
            };

            foreach (LayerContent layer in input.Layers)
            {
                TileLayerContent tlc = layer as TileLayerContent;
                if (tlc != null)
                {
                    
                    WorldMapLayerContent layerContent = new WorldMapLayerContent
                    {
                        Width = tlc.Width,
                        Height = tlc.Height,
                        Collisions = tlc.Collisions,
                        Foreground = tlc.Foreground
                    };

                    layerContent.Tiles = new WorldMapTileContent[tlc.Data.Length];
                    for (int i = 0; i < tlc.Data.Length; i++)
                    {
                        uint tileID = tlc.Data[i];

                        int tileIndex;
                        SpriteEffects spriteEffects;
                        TiledHelpers.DecodeTileID(tileID, out tileIndex, out spriteEffects);

                        ExternalReference<Texture2DContent> textureContent = null;
                        Rectangle rectange = new Rectangle();

                        if (tileIndex != 0)
                        {
                            foreach (var tileSet in input.TileSets)
                            {
                                if (tileIndex - tileSet.FirstId < tileSet.Tiles.Count)
                                {
                                    textureContent = tileSet.Texture;
                                    rectange = tileSet.Tiles[(int)(tileIndex - tileSet.FirstId)].Source;
                                    break;
                                }
                            }
                        }                        

                        layerContent.Tiles[i] = new WorldMapTileContent
                        {
                            Texture = textureContent,
                            SourceRectange = rectange,
                            SpriteEffects = spriteEffects
                        };
                    }
                    output.Layers.Add(layerContent);
                }
            }
            return output;
        }
    }
}