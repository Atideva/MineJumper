using System.Collections;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Setup")]
        [SerializeField] private GameConfig config;
        [SerializeField] private Transform startPos;
        [SerializeField] private Cube spawnPrefab;
        [SerializeField] private Player playerOnScene;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Spawner spawner;
        [SerializeField] private Pool pool;
        [SerializeField] private Curve curves;
        [SerializeField] private SpawnerLocator spawnLocator;
        [SerializeField] private CubeList cubeList;
        [SerializeField] private JumpMessage jumpMessage;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private JumpQuality jumpQuality;

        [Header("Debug")]
        [SerializeField] private int cubeId;

        [Header("Test")]
        public bool testSpawn;
        bool _test;

        private Player _player;

        void Start()
        {
            if (!Application.isPlaying)
                return;

            InitComponents();
            InitPlayer();
            Subscribe();
            AutoSpawn();
        }

        void Subscribe()
        {
            spawner.OnCubeSpawned += OnCubeSpawn;
            _player.Jump.OnJump += OnJump;
            _player.Jump.OnLandAtCube += OnLandAtCube;
        }

        void OnCubeSpawn(Cube cube)
        {
            cube.SetID(++cubeId);
            cubeList.Add(cube);
        }

         void OnJump(float lastJumpProgress)
        {
            var quality = jumpQuality.GetQuality(lastJumpProgress);
            SpawnJumpMessage(quality);
            RefreshSpeedSlider();
        }

        void OnLandAtCube()
        {
            cubeList.Dequeue();

            if (spawnLocator.IsNewDataRequire)
            {
                spawnLocator.SetData(GetLocatorData());
            }

            spawnLocator.NextStep();
            spawner.Spawn(spawnLocator.GetPos());
        }

        void InitPlayer()
        {
            _player = playerOnScene ? playerOnScene : Instantiate(playerPrefab);
            _player.Init(cubeList, startPos.position, config.Jump.LandThreshold);
        }


        void InitComponents()
        {
            cubeId = -1;
            speedSlider.value = 0;

            pool.Init(spawnPrefab);
            spawner.Init(pool);
            jumpQuality.Init(config.Jump);

            var locatorData = GetLocatorData();
            locatorData.startPos = startPos.position;
            spawnLocator.SetData(locatorData);
        }


#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying)
            {
                if (playerOnScene)
                    playerOnScene.transform.position = startPos.position + new Vector3(0, 1, 0);
            }

            if (testSpawn)
            {
                testSpawn = false;

                if (spawnLocator.IsNewDataRequire)
                {
                    spawnLocator.SetData(GetLocatorData());
                }

                if (!_test)
                {
                    _test = true;
                }
                else
                {
                    spawnLocator.NextStep();
                }

                var pos = spawnLocator.GetPos();
                spawner.Spawn(pos);
            }
        }
#endif
        
        void AutoSpawn() => StartCoroutine(AutoSpawnRoutine(autoSpawnCount));

        IEnumerator AutoSpawnRoutine(int spawnCount)
        {
            var pos0 = spawnLocator.GetPos();
            spawner.Spawn(pos0);
            yield return null;

            for (var i = 1; i < spawnCount; i++)
            {
                spawnLocator.NextStep();
                var pos = spawnLocator.GetPos();
                spawner.Spawn(pos);
                yield return null;
            }
        }



        void RefreshSpeedSlider()
        {
            var spdBoost = _player.SpeedBoostFactor;
            speedSlider.value = spdBoost;
            Debug.Log("Speed boost factor: " + spdBoost);
        }

        void SpawnJumpMessage(JumpQualityType quality)
        {
            var jumpData = config.Messages.GetJumpData(quality);
            var r = Random.Range(0, jumpData.content.Count);
            var text = jumpData.content[r];
            var color = jumpData.color;

            jumpMessage.Spawn(text, color);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(startPos.position, Vector3.one);
        }

        SpawnerLocatorData GetLocatorData()
        {
            var locatorData = new SpawnerLocatorData
            {
                height = curves.GetHeightCurve(),
                direction = curves.GetDirectionCurve(),
                maxHeight = curveHeight,
                maxWidth = curveWidth,
                totalSteps = cubesPerCurve,
                startPos = cubeList.GetLastCube() ? cubeList.GetLastCube().transform.position : Vector3.zero
            };
            return locatorData;
        }
    }
}