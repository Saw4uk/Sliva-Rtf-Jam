using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class EngineerBuildSystem : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Tilemap constructionTilemap;
    [SerializeField] private TileBase searchlightTile;
    [SerializeField] private float timeToBuild;
    [SerializeField] private GameObject Torch;

    private int searchlightCount;
    private bool isBuildTurnOn = false;

    public int SearchlightCount
    {
        get => searchlightCount;
        set
        {
            searchlightCount = value;
            OnSearchlightCountChanged?.Invoke(value);
        }
    }

    public event Action OnBuildStart;
    public event Action OnBuildEnd;
    public event Action<float> OnBuildProgress;
    public event Action<int> OnSearchlightCountChanged;

    private void Awake()
    {
        OnBuildStart += OnStartBuildTest;
        OnBuildEnd += OnEndBuildTest;
    }

    private void OnStartBuildTest()
    {
        Debug.Log("Start building");
    }
    
    private void OnEndBuildTest()
    {
        Debug.Log("End building");
    }

    public void AddSearchlight(int count)
    {
        SearchlightCount += count;
        Debug.Log(SearchlightCount);
    }

    // При первом нажатии, если возможно, начинается процесс стройки. При повторном нажатии во время процесса стройки, стройка отменяется.
    public void OnSearchlightBuild(InputAction.CallbackContext context)
    {
        if (searchlightCount > 0 && context.performed)
        {
            var playerPositionOnGrid = constructionTilemap.WorldToCell(transform.position);
            if (CanPlaceTile(playerPositionOnGrid))
            {
                isBuildTurnOn = !isBuildTurnOn;
                
                if (isBuildTurnOn)
                {
                    OnBuildStart?.Invoke();

                    StartCoroutine(Build(timeToBuild, playerPositionOnGrid, searchlightTile));
                }
            }
        }
    }

    private IEnumerator Build(float time, Vector3Int position, TileBase tile)
    {
        var elapsedTime = 0f;
        playerMovement.IsBlocked = true;
        
        while (elapsedTime <= time && isBuildTurnOn)
        {
            elapsedTime += Time.deltaTime;
            OnBuildProgress?.Invoke(elapsedTime / time);
            yield return new WaitForEndOfFrame();
        }
        
        if (elapsedTime >= time && isBuildTurnOn)
        {
            PlaceTile(position, tile);
            SearchlightCount--;
            var instance = Instantiate(Torch);
            instance.transform.position = transform.position;
            isBuildTurnOn = false;
        }
        
        playerMovement.IsBlocked = false;
        OnBuildEnd?.Invoke();
    }

    private bool CanPlaceTile(Vector3Int positionOnGrid)
    {
        return !constructionTilemap.GetTile<TileBase>(positionOnGrid);
    }
    

    private void PlaceTile(Vector3Int positionOnGrid, TileBase tile)
    {
        constructionTilemap.SetTile(positionOnGrid, tile);
    }
}
