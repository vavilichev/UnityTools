using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Tools {
    public class PoolMono : Pool<MonoBehaviour> {

        public readonly bool autoScale;
        public Transform container;

        
        #region CONSTRUCTORS

        public PoolMono(Transform container, bool autoScale) {
            this.container = container;
            this.autoScale = autoScale;
        }

        public PoolMono(Transform container) {
            this.container = container;
            this.autoScale = false;
        }

        public PoolMono(bool autoScale) {
            this.container = null;
            this.autoScale = autoScale;
        }

        public PoolMono() {
            this.container = null;
            this.autoScale = false;
        }

        #endregion

        
        public void InitializePool(MonoBehaviour prefab, int count) {
            for (int i = 0; i < count; i++) {
                var createdObject = Object.Instantiate(prefab, this.container);
                createdObject.name = prefab.name;
                createdObject.gameObject.SetActive(false);
                this.pool.Add(createdObject);
            }
        }
        
        public override void InitializePool(int count) {
            throw new NotSupportedException("MonoBehaviour pool can be initialized only with InitializePool(prefab, count, container)");
        }

        public override MonoBehaviour GetFreeElement() {
            foreach (var element in this.pool) {
                if (!element.gameObject.activeInHierarchy) {
                    element.gameObject.SetActive(true);
                    return element;
                }
            }

            if (this.autoScale) {
                var createdElement = this.CreateAddictiveElement();
                createdElement.gameObject.SetActive(true);
                return createdElement;
            }

            throw new Exception($"There is no free MonoBehaviour elements in the pool. Total elements count: {this.length}.");
        }

        private MonoBehaviour CreateAddictiveElement() {
            var prefab = this.pool[0];
            var createdObject = Object.Instantiate(prefab, this.container);
            createdObject.name = prefab.name;
            createdObject.gameObject.SetActive(false);
            this.pool.Add(createdObject);
            return createdObject;
        }
    }
}