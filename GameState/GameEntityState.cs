using System;
using UnityEngine;

namespace VavilichevGD.Tools {
    [Serializable]
    public class GameEntityState {
        public string id;
        
        public string ToJson() {
            return JsonUtility.ToJson(this);
        }

        public virtual Type GetEntityType() {
            return this.GetType();
        }
    }
}