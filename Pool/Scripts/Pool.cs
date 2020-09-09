using System.Collections.Generic;

namespace VavilichevGD.Tools {
    public abstract class Pool<T> {

        public readonly List<T> pool = new List<T>();

        public int length => this.pool.Count;

        public abstract void InitializePool(int count);

        public abstract T GetFreeElement();

    }
}