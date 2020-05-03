using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Apocalypse {
    public class EnemySpawn : MonoBehaviour {
        [SerializeField] private int _poolSize = 30;
        private GameObject[] _pool;
        private bool _isSingle;

        private void Awake() {
            _isSingle = gameObject.CompareTag("enemySpawnSingle");
            _poolSize = _isSingle ? 1 : _poolSize;
            
            FillEnemyPool();
        }

        private void Start() {
            if (!_isSingle)
                SpawnAllOutside();
            else
                SpawnEnemy();
        }

        private void FillEnemyPool() {
            _pool = new GameObject[_poolSize];
                
            for (int i=0; i<_poolSize; ++i) {
                int randEnemy = Random.Range(0, (int) EnemyType.EOF);
                string prefabName = Enum.GetName(typeof(EnemyType), randEnemy);
                EnemyType et = (EnemyType) randEnemy;
                
                GameObject prefab = Resources.Load("Enemies/" + prefabName) as GameObject;
                GameObject enemy = Instantiate(prefab, transform);
                
                enemy.GetComponent<EnemyAI>().Enemy = new Enemy(et);
                
                _pool[i] = enemy;
            }
        }

        private void SpawnEnemy() { // singolo, interno shop
            int idx = Random.Range(0, _pool.Length);
            GameObject enemy = _pool[idx];

            enemy.transform.position = transform.position;
            //enemy.GetComponent<EnemyAI>().SetStatic();
            enemy.SetActive(true);
        }

        private void SpawnAllOutside() { // la fila
            Vector3 pos = transform.position;
            float y = pos.y;
            
            foreach (GameObject e in _pool) {
                e.transform.position = pos;
                e.SetActive(true);

                y -= e.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

                pos.Set(pos.x, y, 0f);
            }
        }
    }
}
