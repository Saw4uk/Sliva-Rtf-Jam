using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace SlivaRtfJam.Scripts
{
    public class BorderGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap borderTilemap;
        [SerializeField] private Tilemap maskTilemap;
        [SerializeField] private TileBase borderTile;
        [SerializeField] private Vector2Int size = new Vector2Int(32, 32);
        
        [SerializeField] private float scale = 20;
        [SerializeField] [Range(0, 1)] private float threshold = 0.5f;
        [SerializeField] private Vector2Int offsetRange = new Vector2Int(-100, 100);

        private void Awake()
        {
            maskTilemap.gameObject.SetActive(false);
            RandomGenerateNoiseBorder();
        }

        // private void Update()
        // {
        //     GenerateNoiseBorder();
        // }

        public void RandomGenerateNoiseBorder()
        {
            var randomOffset = new Vector2Int(
                Random.Range(offsetRange.x, offsetRange.y),
                Random.Range(offsetRange.x, offsetRange.y));

            GenerateNoiseBorder(scale, randomOffset, threshold);
        }

        private void GenerateNoiseBorder(float scaleValue, Vector2Int offsetValue, float thresholdValue)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var positionOnGrid = new Vector3Int(x - size.x / 2, y - size.y / 2);

                    if (!maskTilemap.GetTile<TileBase>(positionOnGrid))
                    {
                        continue;
                    }

                    borderTilemap.SetTile(positionOnGrid, null);

                    var sample = GetPerlinSample(x, y, scaleValue, offsetValue);

                    if (sample < thresholdValue)
                    {
                        borderTilemap.SetTile(positionOnGrid, borderTile);
                    }
                }
            }
        }

        private float GetPerlinSample(int x, int y, float scale, Vector2Int offset)
        {
            var xCoord = (float)x / size.x * scale + offset.x;
            var yCoord = (float)y / size.y * scale + offset.y;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}