using System.Collections;
using Configs;
using Curves;
using Jump;
using Misc;
using PlayerScripts;
using Spawn;
using UI;
using UnityEngine;



namespace Gameplay
{
    [ExecuteInEditMode]
    public class Manager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int autoSpawnCount;
        [SerializeField] private int cubesPerCurve;
        [SerializeField] private float curveHeight;
        [SerializeField] private float curveWidth;

        [Header("Spawn")]
        [SerializeField] private Cube spawnPrefab;
        [SerializeField] private Vector3 spawnPrefabScale = Vector3.one;
        [SerializeField] private float spawnPadding;
        [SerializeField] private Vector3 spawnDirection;
        [SerializeField] private Transform spawnPos;

        [Header("Player")]
        [SerializeField] private Player playerOnScene;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Controller playerController;

        [Header("UI")]
        [SerializeField] private JumpQualityUI jumpQualityUI;
        [SerializeField] private SpeedBoostUI speedBoostUI;
        [SerializeField] private LivesUI livesUI;
        [SerializeField] private ScreenVfxUI screenVfxUI;

        [Header("Setup")]
        [SerializeField] private GameConfig config;
        [SerializeField] private Spawner spawner;
        [SerializeField] private SpawnPool spawnPool;
        [SerializeField] private Curve curves;
        [SerializeField] private SpawnLocator spawnLocator;
        [SerializeField] private SpawnList spawnList;
        [SerializeField] private SpawnID spawnIDs;

        Cube _currentSpawnPrefab;


        [Header("DEBUG")]
        [SerializeField] private int cubeId;
        public bool testSpawn;
        bool _test;

        Player _player;

        void Start()
        {
            if (!Application.isPlaying)
                return;

            // _currentSpawnPrefab = spawnPrefab;
            Init();
            Subscribe();
            AutoSpawn();
        }

        void Subscribe()
        {
            _player.Jump.OnLandAtCube += OnLandAtCube;
        }

        void OnLandAtCube()
        {
            SpawnNextCube();
            if (_player.CurrentCubeId < 2) return;
            spawnList.DestroyLastCube();
        }



        void Init()
        {
            _player = playerOnScene ? playerOnScene : Instantiate(playerPrefab);
            _player.Init(spawnPos.position, spawnPrefab.GetTopY(spawnPrefabScale), spawnList);
            playerController.Init(_player);
            
            jumpQualityUI.Init(config,_player);
            speedBoostUI.Init(_player.DangerSpeedFactor, _player);
            livesUI.Init(_player);
            screenVfxUI.Init(_player);
            
            spawnPool.Init(spawnPrefab);
            spawner.Init(spawnPool, spawnPrefabScale);
            spawnIDs.Init(spawner);
            spawnList.Init(spawner);
            spawnLocator.Init(spawnPadding, spawnDirection, GetNextCurve());
        }


#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying)
            {
                if (playerOnScene)
                    playerOnScene.transform.position = spawnPos.position + new Vector3(0, 1, 0);
            }

            if (testSpawn)
            {
                testSpawn = false;

                if (spawnLocator.IsEndOfCurve)
                {
                    spawnLocator.SetCurve(GetNextCurve());
                }

                if (!_test)
                {
                    _test = true;
                }
                else
                {
                    spawnLocator.NextStep();
                }

                var pos = spawnLocator.Position;
                spawner.Spawn(pos);
            }
        }
#endif

        void AutoSpawn() => StartCoroutine(AutoSpawnRoutine(autoSpawnCount));

        IEnumerator AutoSpawnRoutine(int spawnCount)
        {
            spawner.Spawn(spawnLocator.Position); // first position
            yield return null;

            for (var i = 1; i < spawnCount; i++)
            {
                spawnLocator.NextStep();
                spawner.Spawn(spawnLocator.Position);
                yield return null;
            }
        }



        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(spawnPos.position, Vector3.one);
        }

        void SpawnNextCube()
        {
            if (spawnLocator.IsEndOfCurve)
            {
                spawnLocator.SetCurve(GetNextCurve());
            }

            spawnLocator.NextStep();
            spawner.Spawn(spawnLocator.Position);
        }

        CurveData GetNextCurve() => new CurveData()
            .With(c => c.height = curves.GetHeightCurve())
            .With(c => c.direction = curves.GetDirectionCurve())
            .With(c => c.maxHeight = curveHeight)
            .With(c => c.maxWidth = curveWidth)
            .With(c => c.totalSteps = cubesPerCurve)
            .With(c => c.startPos =
                spawnList.GetLastCube() ? spawnList.GetLastCube().transform.position : spawnPos.position);


        // CurveData GetNextCurve()
        // {
        //     var locatorData = new CurveData
        //     {
        //         height = curves.GetHeightCurve(),
        //         direction = curves.GetDirectionCurve(),
        //         maxHeight = curveHeight,
        //         maxWidth = curveWidth,
        //         totalSteps = cubesPerCurve,
        //         startPos = cubeList.GetLastCube() ? cubeList.GetLastCube().transform.position : Vector3.zero
        //     };
        //     return locatorData;
        // }
    }
}