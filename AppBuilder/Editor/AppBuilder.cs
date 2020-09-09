using System;
using System.Collections.Generic;
using System.Globalization;
using GooglePlayServices;
using UnityEditor;
using UnityEngine;

namespace EcoClicker.Extensions {
    public static class AppBuilder {

        #region CONSTANTS

        private const string CONFIG_PATH = "Assets/VavilichevGD/Tools/AppBuilder/Config/AppBuilderConfig.asset";

        #endregion

        private static AppBuilderConfig config {
            get {
                if (m_config == null)
                    m_config = AssetDatabase.LoadAssetAtPath<AppBuilderConfig>(CONFIG_PATH);
                return m_config;
            }
        }
        private static AppBuilderConfig m_config;
        
#if UNITY_ANDROID
        
        public static string GetAndroidBuildPath(string version, bool aabBundle = false) {
            var endWord = aabBundle ? "aab" : "apk";
            var projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            var path = $"{projectPath}/Builds/{config.appTitle} v.{version}.{endWord}";
            return path;
        }

        #region Buttons

        [MenuItem("Builds/Build and Run Development")]
        public static void RunDevelopmentBuildAndroid() {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            PlayServicesResolver.Resolve(() => {
                UpdateVersion();
                const BuildOptions options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
                var version = $"{Application.version}d";
                var filePath = GetAndroidBuildPath(version);
                Build(options, filePath);
            });
        }


        [MenuItem("Builds/Build and Run Release APK")]
        public static void BuildAndRunAPK() {
            BuildAPK(true);
        }

        [MenuItem("Builds/Build Release APK")]
        public static void BuildAPK() {
            BuildAPK(false);
        }

        public static void BuildAPK(bool alsoRunIt) {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            PlayServicesResolver.Resolve(() => {
                UpdateVersion();
                BuildOptions options = alsoRunIt ? BuildOptions.None | BuildOptions.AutoRunPlayer : BuildOptions.None;
                var version = $"{Application.version}";
                var filePath = GetAndroidBuildPath(version);
                Build(options, filePath);
            });
        }


        [MenuItem("Builds/Build Release AAB")]
        public static void BuildAAB() {
            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.bundleVersionCode++;


            PlayServicesResolver.Resolve(() => {
                UpdateVersion();
                BuildOptions options = BuildOptions.None;
                var version = $"{Application.version}";
                var filePath = GetAndroidBuildPath(version, true);
                Build(options, filePath);
            });
        }

        #endregion
        

        private static void UpdateVersion() {
            double version = GetCurrentVersion();
            double newVersion = Math.Round(version + config.versionStep, config.versionStepRoundValue);
            SetNewVersion(newVersion);
        }

        private static double GetCurrentVersion() {
            CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(PlayerSettings.bundleVersion, NumberStyles.Any, ci);
        }

        private static void SetNewVersion(double newVersion) {
            string format = "0.";
            for (int i = 0; i < config.versionStepRoundValue; i++)
                format += "0";
            PlayerSettings.bundleVersion =
                newVersion.ToString(format, CultureInfo.InvariantCulture);
        }


        private static void Build(BuildOptions buildOptions, string path) {
            PreparePasswords();

            var scenesPaths = new List<string>();
            foreach (var scene in config.buildScenes) {
                var scenePath = AssetDatabase.GetAssetPath(scene);
                scenesPaths.Add(scenePath);
            }
            
            var message = BuildPipeline.BuildPlayer(
                scenesPaths.ToArray(),
                path,
                BuildTarget.Android,
                buildOptions
            );
            
            Debug.Log($"Android build complete: {message}");
        }

        private static void PreparePasswords() {
            PlayerSettings.runInBackground = false;

            PlayerSettings.keystorePass = config.keyAliasPassword;
            PlayerSettings.keyaliasPass = config.keyAliasPassword;
        }

#endif

    }
}
