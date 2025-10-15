using System.Collections.Generic;
using UnityEngine;

public class WaveCollaps : MonoBehaviour
{
    [SerializeField] Vector2Int grid;
    [SerializeField] List<WCTile> tiles;

    private float[,,] probs;
    private float[,] score;
    private Vector2Int[] directions = {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    void Awake()
    {
        GenerateWaveCollaps();
    }

    void GenerateWaveCollaps()
    {
        probs = new float[grid.x, grid.y, tiles.Count];
        score = new float[grid.x, grid.y];


        // Словарь которые мапит имя тайла и канал на который он влияет
        Dictionary<string, int> name2ind = new Dictionary<string, int>();
        for (int ind = 0; ind < tiles.Count; ind++)
        {
            name2ind.Add(tiles[ind].Name, ind);
        }

        // Инициализация вероятностей тайлов
        for (int x = 0; x < grid.x; x++)
        {
            for (int y = 0; y < grid.y; y++)
            {
                for (int chanel = 0; chanel < tiles.Count; chanel++)
                {
                    probs[x, y, chanel] = 1f / tiles.Count;
                }
                score[x, y] = 1;
            }
        }


        // Ставим тайлики
        for (int placeTile = 0; placeTile < grid.x * grid.y; placeTile++)
        {
            int sx = 0, sy = 0;
            float ss = score[0, 0];

            // Выбираем тайлики у которого вероятности меньше - стоит больше соседей
            for (int x = 0; x < grid.x; x++)
            {
                for (int y = 0; y < grid.y; y++)
                {
                    if (score[x, y] < ss)
                    {
                        ss = score[x, y];
                        sx = x;
                        sy = y;
                    }
                }
            }

            // Нормализуем вероятности
            float total = 0;
            for (int ch = 0; ch < tiles.Count; ch++)
            {
                total += probs[sx, sy, ch];
            }
            float rnd = Random.value;
            float cumsum = 0;
            int selected_ind = tiles.Count - 1;
            // Выбираем случайный канал используя веса
            for (int ch = 0; ch < tiles.Count; ch++)
            {
                probs[sx, sy, ch] /= total;
                cumsum += probs[sx, sy, ch];
                if (rnd <= cumsum)
                {
                    selected_ind = ch;
                    break;
                }
            }

            // Ставим тайлик
            Instantiate(tiles[selected_ind].Tile, transform.position + new Vector3(sx, tiles[selected_ind].Tile.transform.position.y, sy), transform.rotation).transform.parent = transform;
            score[sx, sy] = 20;

            // Обновляем вероятности соседей
            foreach (Vector2Int dir in directions)
            {
                int nx = sx + dir.x;
                int ny = sy + dir.y;

                if (nx < 0 || ny < 0 || nx >= grid.x || ny >= grid.y || score[nx, ny] == 20)
                {
                    continue;
                }
                
                // Для каждого канала обновляем вероятности и скор
                foreach (WCTilePair pair in tiles[selected_ind].Prob)
                {
                    score[nx, ny] -= probs[nx, ny, name2ind[pair.tile.name]];
                    probs[nx, ny, name2ind[pair.tile.name]] *= pair.prob;
                    score[nx, ny] += probs[nx, ny, name2ind[pair.tile.name]];
                }
            }

        }
    }

}
