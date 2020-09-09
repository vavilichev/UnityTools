using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace VavilichevGD.Tools {
    public class EditorDownloader {

        private static UnityWebRequest request;
        private static UnityAction<UnityWebRequest> m_callback;
        public static bool isLoading { get; private set; }
        
        public static void Download(string url, UnityAction<UnityWebRequest> callback) {
            if (isLoading)
                throw new Exception("EDITOR DOWNLOADER: Downloading now. Wait until completed.");

            m_callback = callback;
            isLoading = true;
            request = UnityWebRequest.Get(url);
            request.SendWebRequest();

            EditorApplication.update += EditorUpdate;
        }

        
        private static void EditorUpdate() {
            if (!request.isDone)
                return;


            isLoading = false;
            EditorApplication.update -= EditorUpdate;
            var textLoaded = request.downloadHandler != null ? request.downloadHandler.text : "EMPTY";
            Debug.Log($"EDITOR DOWNLOADER: Load something. Text: {textLoaded}.");

            m_callback.Invoke(request);
            request = null;
        }


        [MenuItem("Tools/Reset Editor Downloader")]
        public static void ResetEditorDownloader() {
            if (request != null) {
                request.Abort();
                
                if (m_callback != null)
                    m_callback.Invoke(request);
                
                request = null;
                isLoading = false;
            }
            EditorApplication.update -= EditorUpdate;
            Debug.Log("EDITOR DOWNLOADER: Resetted.");
        }
    }
}