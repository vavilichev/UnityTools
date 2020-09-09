using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.Tools {
    public class GameStateTester : MonoBehaviour {

        private const string PREF_KEY = "GAME_STATE";

        private GameState gameState;
        
        
        [ContextMenu("Load Game")]
        public void LoadGame() {
            if (!Storage.HasKey(PREF_KEY)) {
                this.gameState = new GameState();
                
                var testEntity = new TestEntity();
                this.gameState.AddEntity(testEntity);
                Debug.Log($"GameState created: {this.gameState}");
            }
            else {
                this.gameState = Storage.GetCustom(PREF_KEY, new GameState());
                this.gameState.Initialize();
                Debug.Log($"GameState loaded: {this.gameState}");
            }
            
            this.LogEntity();
        }

        private void LogEntity() {
            var entity = this.gameState.GetEntity<TestEntity>("test_entity");
            Debug.Log($"Entity integer changed: {entity.intOlolo}");
        }

        [ContextMenu("Change Entity")]
        public void ChangeTestEntity() {
            var entity = this.gameState.GetEntity<TestEntity>("test_entity");
            var rInteger = Random.Range(0, 1000);
            entity.intOlolo = rInteger;
            this.LogEntity();
        }

        [ContextMenu("Save Game")]
        public void SaveGame() {
            var jsonGameState = gameState.ToJson();
            Storage.SetString(PREF_KEY, jsonGameState);
            Debug.Log($"GameState saved: {this.gameState}");
            this.LogEntity();
        }

        [ContextMenu("Save Game Async")]
        public void SaveGameAsync() {
            this.gameState.ToJsonAsync(this.JsonDefinedAsync);
        }

        private void JsonDefinedAsync(string jsonGameState) {
            Storage.SetString(PREF_KEY, jsonGameState);
            Debug.Log($"GameState saved async: {this.gameState}");
            this.LogEntity();
        }
    }
}