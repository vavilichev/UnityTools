using System;

namespace VavilichevGD.Tools {
    public class TestEntity : GameEntityState {
        public string ololo;
        public int intOlolo;
        public bool boolololo;

        public TestEntity() {
            this.id = "test_entity";
            this.ololo = "ololo";
            this.intOlolo = 999;
            this.boolololo = true;
        }

        public override Type GetEntityType() {
            return typeof(TestEntity);
        }
    }
}