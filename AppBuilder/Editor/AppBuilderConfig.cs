using UnityEditor;
using UnityEngine;

namespace EcoClicker.Extensions {
    [CreateAssetMenu(fileName = "AppBuilderConfig", menuName = "Tools/AppBuilder/New AppBuilderConfig")]
    public class AppBuilderConfig : ScriptableObject {
        [SerializeField] private string m_appTitle = "YOUR APP TITLE";
        [SerializeField] private string m_keyStorePassword = "KEYSTORE PASSWORD";
        [SerializeField] private string m_keyAliasPassword = "KEYALIAS PASSWORD";
        [SerializeField] private SceneAsset[] m_buildScenes;
        [Header("Version properties")]
        [SerializeField] private float m_versionStep = 0.01f;
        [Tooltip("Uses when round new version. For example: 0.01f + 0.01f can be 0.0199999 value. And we can round it to 0.02f")]
        [SerializeField]
        private int m_versionStepRoundValue = 2;

        public string appTitle => this.m_appTitle;
        public string keyStorePassword => this.m_keyStorePassword;
        public string keyAliasPassword => this.m_keyAliasPassword;
        public SceneAsset[] buildScenes => this.m_buildScenes;
        public float versionStep => this.m_versionStep;
        public int versionStepRoundValue => this.m_versionStepRoundValue;

    }
}