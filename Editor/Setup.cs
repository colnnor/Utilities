using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

public static class Setup {
    [MenuItem("Tools/Setup/Import Essential Assets")]
    public static void ImportEssentials() {
        Assets.ImportAsset("Odin Inspector and Serializer", "Sirenix/Editor ExtensionsSystem");
        Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
        Assets.ImportAsset("Selection History", "Staggart Creations/Editor ExtensionsUtilities");
        Assets.ImportAsset("Audio Preview Tool", "Warped Imagination/Editor ExtensionsAudio");
        // and so on...
    }

    [MenuItem("Tools/Setup/Install Essential Packages")]
    public static void InstallPackages() {
        Packages.InstallPackages(new[] {
            "com.unity.2d.animation",
            "git+https://github.com/adammyhre/Unity-Improved-Timers.git",
            "com.unity.cinemachine",
            "com.unity.inputsystem"
        });
    }

    [MenuItem("Tools/Setup/Create Folders")]
    public static void CreateFolders() {
        Folders.Create("_Project", "Animation", "Art", "Input", "Materials", "Prefabs", "Scripts", "Audio", "Audio/Music", "Audio/SFX", "Models");
        Refresh();
        Folders.Move("_Project", "Scenes");
        Folders.Move("_Project", "Settings");
        Folders.Delete("TutorialInfo");
        Refresh();

        MoveAsset("Assets/InputSystem_Actions.inputactions", "Assets/_Project/Input/InputSystem_Actions.inputactions");
        DeleteAsset("Assets/Readme.asset");
        Refresh();
    }

    static class Assets {
        public static void ImportAsset(string asset, string folder) {
            var basePath = GetFolderPath(SpecialFolder.ApplicationData);
            var assetsFolder = Combine(basePath, "Unity/Asset Store-5.x");
            asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";
            ImportPackage(Combine(assetsFolder, folder, asset), false);
        }
    }

    static class Packages {
        static AddRequest request;
        static Queue<string> packagesToInstall = new Queue<string>();

        public static void InstallPackages(string[] packages) {
            foreach (var package in packages) {
                packagesToInstall.Enqueue(package);
            }

            if (packagesToInstall.Count > 0) {
                StartNextPackageInstallation();
            }
        }

        static async void StartNextPackageInstallation() {
            request = Client.Add(packagesToInstall.Dequeue());
            
            while (!request.IsCompleted) await Task.Delay(10);
            
            switch (request.Status)
            {
                case StatusCode.Success:
                    Debug.Log("Installed: " + request.Result.packageId);
                    break;
                case >= StatusCode.Failure:
                    Debug.LogError(request.Error.message);
                    break;
            }

            if (packagesToInstall.Count <= 0) return;
            
            await Task.Delay(1000);
            StartNextPackageInstallation();
        }
    }

    static class Folders {
        public static void Create(string root, params string[] folders) {
            var fullpath = Combine(Application.dataPath, root);
            if (!Directory.Exists(fullpath)) {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var folder in folders) {
                CreateSubFolders(fullpath, folder);
            }
        }
        
        static void CreateSubFolders(string rootPath, string folderHierarchy) {
            var folders = folderHierarchy.Split('/');
            var currentPath = rootPath;

            foreach (var folder in folders) {
                currentPath = Combine(currentPath, folder);
                if (!Directory.Exists(currentPath)) {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }
        
        public static void Move(string newParent, string folderName) {
            var sourcePath = $"Assets/{folderName}";
            if (IsValidFolder(sourcePath)) {
                var destinationPath = $"Assets/{newParent}/{folderName}";
                var error = MoveAsset(sourcePath, destinationPath);

                if (!string.IsNullOrEmpty(error)) {
                    Debug.LogError($"Failed to move {folderName}: {error}");
                }
            }
        }
        
        public static void Delete(string folderName) {
            var pathToDelete = $"Assets/{folderName}";

            if (IsValidFolder(pathToDelete)) {
                DeleteAsset(pathToDelete);
            }
        }
    }
}