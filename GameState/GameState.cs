using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

namespace VavilichevGD.Tools {
    [Serializable]
    public sealed class GameState {

        public List<string> jsonEntities;

        public bool isConverting { get; private set; }
        
        private Dictionary<string, object> entitiesMap;
        private Coroutine convertingRoutine;


        public GameState() {
            this.jsonEntities = new List<string>();
            this.entitiesMap = new Dictionary<string, object>();
        }
        
        
        public void Initialize() {
            this.entitiesMap = new Dictionary<string, object>();
            
            foreach (var jsonEntity in this.jsonEntities) {
                var entity = JsonUtility.FromJson<GameEntityState>(jsonEntity);
                var id = entity.id;
                var type = entity.GetEntityType();
                var entityObject = JsonUtility.FromJson(jsonEntity, type);

                this.entitiesMap[id] = entityObject;
            }
        }
        
        public T GetEntity<T>(string id) where T : GameEntityState {
            var foundEntity = this.entitiesMap[id];
            return (T) foundEntity;
        }

        public void AddEntity(string id, GameEntityState entity) {
            if (this.entitiesMap.ContainsKey(id))
                throw new Exception($"Game State already contains entity with ID = {id}");

            this.entitiesMap[id] = entity;
            this.jsonEntities.Add(entity.ToJson());
        }

        public void AddEntity(GameEntityState entity) {
            this.AddEntity(entity.id, entity);
        }

        public void RemoveEntity(string id) {
            if (this.entitiesMap.ContainsKey(id)) {
                this.entitiesMap.Remove(id);
                this.SyncJsonsWithEntities();
                Logging.Log($"Entity \"{id}\" has been removed from Game state");
            }
        }

        private void SyncJsonsWithEntities() {
            this.jsonEntities.Clear();
            var entityObjects = this.entitiesMap.Values.ToArray();
            foreach (var entityObject in entityObjects) {
                var entity = (GameEntityState) entityObject;
                this.jsonEntities.Add(entity.ToJson());
            }
        }
        
        
        public string ToJson() {
            this.SyncJsonsWithEntities();
            return JsonUtility.ToJson(this);
        }

        public Coroutine ToJsonAsync(UnityAction<string> callback) {
            if (this.isConverting)
                throw new Exception("Game State is converting now.");

            this.convertingRoutine = Coroutines.StartRoutine(this.ToJsonAsyncRoutine(callback));
            return this.convertingRoutine;
        }

        private IEnumerator ToJsonAsyncRoutine(UnityAction<string> callback) {
            this.isConverting = true;

            yield return Coroutines.StartRoutine(this.SyncJsonsWithEntitiesAsyncRoutine());
            var jsonGameState = JsonUtility.ToJson(this);

            callback?.Invoke(jsonGameState);
            this.isConverting = false;
        }
        
        private IEnumerator SyncJsonsWithEntitiesAsyncRoutine() {
            this.jsonEntities.Clear();
            var entityObjects = this.entitiesMap.Values.ToArray();
            foreach (var entityObject in entityObjects) {
                var entity = (GameEntityState) entityObject;
                this.jsonEntities.Add(entity.ToJson());
                yield return null;
            }
        }

        public override string ToString() {
            var line = $"GameState: Jsons count = {this.jsonEntities.Count}, entities count: {this.entitiesMap.Count}";
            return line;
        }
    }
}