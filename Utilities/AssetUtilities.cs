/*
 * ii's Stupid Menu  Utilities/AssetUtilities.cs
 * A mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Goldentrophy Software
 * https://github.com/iiDk-the-actual/iis.Stupid.Menu
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using iiMenu.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static iiMenu.Utilities.FileUtilities;
using Object = UnityEngine.Object;

namespace iiMenu.Utilities
{
    public class AssetUtilities
    {
        private const int MaxAudioPoolSize = 16;
        private const int MaxTextureCacheSize = 32;
        private static readonly Queue<string> _audioPoolOrder = new Queue<string>();
        private static AssetBundle assetBundle;
        private static void LoadAssetBundle()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{PluginInfo.ClientResourcePath}.iimenu");
            if (stream != null)
                assetBundle = AssetBundle.LoadFromStream(stream);
            else
                LogManager.LogError("Failed to load assetbundle");
        }

        public static T LoadObject<T>(string assetName) where T : Object
        {
            if (assetBundle == null)
                LoadAssetBundle();

            return Object.Instantiate(assetBundle.LoadAsset<T>(assetName));
        }

        public static T LoadAsset<T>(string assetName) where T : Object
        {
            if (assetBundle == null)
                LoadAssetBundle();

            return assetBundle.LoadAsset(assetName) as T;
        }

        public static readonly Dictionary<string, AudioClip> audioFilePool = new Dictionary<string, AudioClip>();
        public static AudioClip LoadSoundFromFile(string fileName)
        {
            if (audioFilePool.TryGetValue(fileName, out var value))
                return value;

            string filePath = $"{GetGamePath()}/{PluginInfo.BaseDirectory}/{fileName}";
            AudioClip sound;
            using (UnityWebRequest actualrequest = UnityWebRequestMultimedia.GetAudioClip($"file://{filePath}", GetAudioType(GetFileExtension(fileName))))
            {
                UnityWebRequestAsyncOperation newvar = actualrequest.SendWebRequest();
                while (!newvar.isDone) { }
                sound = DownloadHandlerAudioClip.GetContent(actualrequest);
            }

            bool shouldCache = sound != null && !fileName.StartsWith("TTS", StringComparison.OrdinalIgnoreCase);
            if (shouldCache)
            {
                if (audioFilePool.Count >= MaxAudioPoolSize && _audioPoolOrder.Count > 0)
                {
                    string oldestKey = _audioPoolOrder.Dequeue();
                    if (audioFilePool.TryGetValue(oldestKey, out AudioClip oldClip))
                    {
                        if (oldClip != null) Object.Destroy(oldClip);
                        audioFilePool.Remove(oldestKey);
                    }
                }

                audioFilePool[fileName] = sound;
                _audioPoolOrder.Enqueue(fileName);
            }

            return sound;
        }

        public static AudioClip LoadSoundFromURL(string resourcePath, string fileName)
        {
            string filePath = $"{PluginInfo.BaseDirectory}/{fileName}";
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(filePath)) return LoadSoundFromFile(fileName);
            LogManager.Log("Downloading " + fileName);
            using WebClient stream = new WebClient();
            stream.DownloadFile(resourcePath, filePath);

            return LoadSoundFromFile(fileName);
        }

        public static readonly Dictionary<string, Texture2D> textureResourceDictionary = new Dictionary<string, Texture2D>();
        public static Texture2D LoadTextureFromResource(string resourcePath)
        {
            if (textureResourceDictionary.TryGetValue(resourcePath, out Texture2D existingTexture))
                return existingTexture;

            Texture2D texture = new Texture2D(2, 2);

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            if (stream != null)
            {
                byte[] fileData = new byte[stream.Length];
                stream.Read(fileData, 0, (int)stream.Length);
                texture.LoadImage(fileData);
            }
            else
                LogManager.LogError("Failed to load texture from resource: " + resourcePath);

            textureResourceDictionary[resourcePath] = texture;
            return texture;
        }

        public static readonly Dictionary<string, Texture2D> textureUrlDictionary = new Dictionary<string, Texture2D>();
        private static readonly Queue<string> _texUrlOrder = new Queue<string>();
        public static Texture2D LoadTextureFromURL(string resourcePath, string fileName)
        {
            if (textureUrlDictionary.TryGetValue(resourcePath, out Texture2D existingTexture))
                return existingTexture;

            string filePath = $"{PluginInfo.BaseDirectory}/{fileName}";
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(filePath))
            {
                LogManager.Log("Downloading " + fileName);
                using WebClient stream = new WebClient();
                stream.DownloadFile(resourcePath, filePath);
            }

            Texture2D texture = LoadTextureFromFile(fileName);

            if (textureUrlDictionary.Count >= MaxTextureCacheSize && _texUrlOrder.Count > 0)
            {
                string oldest = _texUrlOrder.Dequeue();
                if (textureUrlDictionary.TryGetValue(oldest, out Texture2D oldTex))
                {
                    if (oldTex != null) Object.Destroy(oldTex);
                    textureUrlDictionary.Remove(oldest);
                }
            }
            textureUrlDictionary[resourcePath] = texture;
            _texUrlOrder.Enqueue(resourcePath);
            return texture;
        }

        public static readonly Dictionary<string, Texture2D> textureFileDirectory = new Dictionary<string, Texture2D>();
        private static readonly Queue<string> _texFileOrder = new Queue<string>();
        public static Texture2D LoadTextureFromFile(string fileName)
        {
            if (textureFileDirectory.TryGetValue(fileName, out Texture2D existingTexture))
                return existingTexture;

            string filePath = $"{PluginInfo.BaseDirectory}/{fileName}";
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Texture2D texture = new Texture2D(2, 2);
            byte[] bytes = File.ReadAllBytes(filePath);
            texture.LoadImage(bytes);

            if (textureFileDirectory.Count >= MaxTextureCacheSize && _texFileOrder.Count > 0)
            {
                string oldest = _texFileOrder.Dequeue();
                if (textureFileDirectory.TryGetValue(oldest, out Texture2D oldTex))
                {
                    if (oldTex != null) Object.Destroy(oldTex);
                    textureFileDirectory.Remove(oldest);
                }
            }
            textureFileDirectory[fileName] = texture;
            _texFileOrder.Enqueue(fileName);
            return texture;
        }

        public static void ClearAllCaches()
        {
            foreach (var clip in audioFilePool.Values)
                if (clip != null) Object.Destroy(clip);
            audioFilePool.Clear();
            _audioPoolOrder.Clear();

            foreach (var tex in textureUrlDictionary.Values)
                if (tex != null) Object.Destroy(tex);
            textureUrlDictionary.Clear();
            _texUrlOrder.Clear();

            foreach (var tex in textureFileDirectory.Values)
                if (tex != null) Object.Destroy(tex);
            textureFileDirectory.Clear();
            _texFileOrder.Clear();
        }

        public static void PurgeStaleTTSFiles()
        {
            try
            {
                string basePath = $"{GetGamePath()}/{PluginInfo.BaseDirectory}";
                string[] ttsDirs = Directory.GetDirectories(basePath, "TTS*");
                foreach (string dir in ttsDirs)
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(file);
                            if (fi.Length <= 0 || fi.Length == 121700)
                                fi.Delete();
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        public static void UnloadEmbeddedBundle()
        {
            if (assetBundle != null)
            {
                assetBundle.Unload(false);
                assetBundle = null;
            }
        }
    }
}