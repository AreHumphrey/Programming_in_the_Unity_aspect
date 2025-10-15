using Unity.Collections;
using UnityEngine;
using System.Linq;

public class MazeGen : MonoBehaviour
{
    [SerializeField] private GameObject Tile;
    [SerializeField] private GameObject Wall;

    [SerializeField] private Vector2Int GridSize;

    private int[,] matrix;
    private Vector2 offsets;
    int nx, ny;
    Vector2Int dir;

    Vector2Int[] directions = {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };
    Quaternion[] rotations = {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 180, 0),
        Quaternion.Euler(0, 270, 0)
    };
    int[] index = {
        0, 1, 2, 3
    };
    int[] powers = {
        1, 2, 4, 8
    };

    bool borderBool;
    void Awake()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        matrix = new int[GridSize.x, GridSize.y];
        offsets = new Vector2(Tile.transform.localScale.x, Tile.transform.localScale.z);

        GenerateCell(0, 0);
    }

    private void GenerateCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= GridSize.x || y >= GridSize.y || matrix[x, y] > 0)
        {
            return;
        }


        matrix[x, y] = 16; // 0b1111 + 0b1 - 1 означает что в этом напарвлении стоит стена. 0b1 - обозначает что мы были в этой точке
        Instantiate(Tile, transform.position + new Vector3(x * offsets.x, 0, y * offsets.y), transform.rotation).transform.parent = transform;

        int[] shuffledIndex = index.OrderBy(x => Random.value).ToArray(); // Выбираем направления случайным способом
        foreach (int ind in shuffledIndex)
        {
            
            dir = directions[ind];
            nx = x + dir.x;
            ny = y + dir.y;


            // Смотрим - принадлежат ли координаты нашему лабиринту
            borderBool = nx < 0 || ny < 0 || nx >= GridSize.x || ny >= GridSize.y;
            /*
                matrix[nx, ny] > 0 - значит что в этой клетке мы уже были
                (ind + 2) % 4 - преобразует индексы и разворачивает направления
                0, 1, 2, 3 -> 2, 3, 0, 1

                0 - идти вниз, 2 - идти вверх и т.п.
            */
            if (borderBool || (matrix[nx, ny] > 0 && (((matrix[nx, ny] - 1) >> ((ind + 2) % 4)) % 2 == 1)))
            {
                // Ставим стену, сдвигаем на 0.95 чтоб стены стали толще
                Instantiate(Wall, transform.position + new Vector3(x * offsets.x + (offsets.x / 2f) * dir.x * 0.95f, Wall.transform.lossyScale.y / 2, y * offsets.y + (offsets.y / 2f) * dir.y * 0.95f), transform.rotation * rotations[ind]).transform.parent = transform;
            }
            else
            {
                // Если не были в клетке
                if (matrix[nx, ny] == 0)
                {
                    // Снимаем стену
                    matrix[x, y] -= powers[ind];
                    // Ставим новый тайл
                    GenerateCell(nx, ny);
                }
            }
        }
    }
}
