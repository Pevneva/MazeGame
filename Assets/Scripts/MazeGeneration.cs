using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGeneration : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMoverTemplate;
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private DeathArea _deathAreaTemplate;
    [SerializeField] private Transform _tempLevelItemsContainer;
    [SerializeField] private Transform _floorContainer;
    [SerializeField] private Transform _playerStartPosition;
    [SerializeField] private Transform _finishArea;
    [SerializeField] private ScreenPanel _screenPanel;

    private readonly float _deathAreaOffset = 0.025f;
    private readonly float _fadeTime = 4;
    private readonly int _sizeArea = 15;
    private PlayerMover _playerMover;
    private GameObject _parentTempLevelItems;

    private void Awake()
    {
        CreateFloor(_sizeArea);
        GenerateLevel(_sizeArea);
        SetupPlayer();
    }

    private void CreateFloor(int size)
    {
        for (int i = -1; i <= size; i++)
        for (int j = -1; j <= size; j++)
        {
            CreateBorder(i, j, 2.4f, _floorContainer);

            if (i == -1 || i == size || j == -1 || j == size)
                CreateBorder(i, j, 3f, _floorContainer);
        }
    }

    private void GenerateLevel(int size)
    {
        _parentTempLevelItems = new GameObject();
        _parentTempLevelItems.transform.parent = _tempLevelItemsContainer;
        
        for (int i = 1; i < size; i += 2)
        {
            var hole = Random.Range(0, _sizeArea);
            for (int j = 0; j < size; j++)
            {
                if (j != hole)
                    CreateBorder(i, j, 3, _parentTempLevelItems.transform);
            }

            if ((i + 1) % 4 == 0)
            {
                var deathAreaY = Random.Range(0, _sizeArea);
                CreateDeathArea(_deathAreaTemplate, i - 1, deathAreaY, 2.88f, _parentTempLevelItems.transform);
            }
        }
    }

    private void SetupPlayer()
    {
        PlayerMover playerMover = Instantiate(_playerMoverTemplate, _playerStartPosition);
        _playerMover = playerMover;
        playerMover.Init(_finishArea);
        playerMover.gameObject.GetComponent<Player>().Died += OnDied;
        playerMover.gameObject.GetComponent<Player>().FinishReached += OnFinishReached;
        StartCoroutine(WaitMovePlayer(2));
    }

    private void OnFinishReached()
    {
        _screenPanel.ShowFinishPanel();
        Invoke(nameof(ResetLevel), _fadeTime);
        Destroy(_playerMover.gameObject, _fadeTime);
        _playerMover.gameObject.GetComponent<Player>().Died -= OnDied;
        _playerMover.gameObject.GetComponent<Player>().FinishReached -= OnFinishReached;
    }

    private void ResetLevel()
    {
        RemovedOldItems();
        GenerateLevel(_sizeArea);
        SetupPlayer();
    }

    private void RemovedOldItems()
    {
        Destroy(_parentTempLevelItems);
    }

    private void OnDied()
    {
        _playerMover.gameObject.GetComponent<Player>().Died -= OnDied;
        Invoke(nameof(SetupPlayer), 1);
    }

    private IEnumerator WaitMovePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerMover.Move();
    }

    private void CreateBorder(float xPosition, float yPosition, float height, Transform parent)
    {
        Instantiate(_wallTemplate, new Vector3(xPosition, height, yPosition), Quaternion.identity, parent);
    }

    private void CreateDeathArea(DeathArea deathArea, float xPosition, float yPosition, float height, Transform parent)
    {
        Instantiate(deathArea, new Vector3(xPosition, height, yPosition), Quaternion.identity, parent);
    }
}