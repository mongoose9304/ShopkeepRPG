using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shopkeeper {
    /// <summary>
    /// Responsible for saving and loading games.
    /// </summary>
    public class UniversalSaveManager : MonoBehaviour {
        private void Start() {
            Save(SaveSlot.Autosave);
        }

        /// <summary>
        /// Starts new game.
        /// </summary>
        public void New() {
            Load(new JObject());
        }
        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit() {
            SceneManager.LoadSceneAsync(m_MainmenuScene);
        }

        /// <summary>
        /// Saves the game into the specified file path.
        /// </summary>
        /// <param name="path">The file path.</param>
        public void Save(string path) {
            var jobj = new JObject();
            Save(jobj);
            using var fstream = File.OpenWrite(path);
            using var fwriter = new StreamWriter(fstream);
            using var jwriter = new JsonTextWriter(fwriter);
            jwriter.Formatting = Formatting.Indented;
            jobj.WriteTo(jwriter);
        }
        /// <summary>
        /// Saves the game into the specified json object.
        /// </summary>
        /// <param name="json">The json object.</param>
        public void Save(JObject json) {
            var managers = ManagerUniversal.GetManagers<ISaveListener>()
                .Concat(ManagerGlobal.GetManagers<ISaveListener>())
                .Concat(ManagerActivity.GetManagers<ISaveListener>());
            foreach (var manager in managers)
                manager.SavePre(json);
            foreach (var manager in managers)
                manager.Save(json);
            foreach (var manager in managers)
                manager.SavePost(json);
        }
        /// <summary>
        /// Saves the game into the specified save slot.
        /// </summary>
        /// <param name="slot">The save slot.</param>
        public void Save(SaveSlot slot) {
            Save(Locate(slot));
        }

        /// <summary>
        /// Loads the game from the specified file path.
        /// </summary>
        /// <param name="path">The file path.</param>
        public void Load(string path) {
            using var fstream = File.OpenRead(path);
            using var freader = new StreamReader(fstream);
            using var jreader = new JsonTextReader(freader);
            Load(JObject.Load(jreader));
        }
        /// <summary>
        /// Loads the game from the specified json object.
        /// </summary>
        /// <param name="json">The json object.</param>
        public void Load(JObject json) {
            SceneManager.LoadSceneAsync(m_GameplayScene).completed += (AsyncOperation op) => {
                var managers = ManagerUniversal.GetManagers<ISaveListener>()
                    .Concat(ManagerGlobal.GetManagers<ISaveListener>())
                    .Concat(ManagerActivity.GetManagers<ISaveListener>());
                foreach (var manager in managers)
                    manager.LoadPre(json);
                foreach (var manager in managers)
                    manager.Load(json);
                foreach (var manager in managers)
                    manager.LoadPost(json);
            };
        }
        /// <summary>
        /// Loads the game from the specified save slot.
        /// </summary>
        /// <param name="slot">The save slot.</param>
        public void Load(SaveSlot slot) {
            Load(Locate(slot));
        }

        /// <summary>
        /// Returns the file path for the specified save slot.
        /// </summary>
        /// <param name="slot">The save slot.</param>
        /// <returns>The file path for the specified save slot.</returns>
        public string Locate(SaveSlot slot) {
            switch (slot) {
                case SaveSlot.Autosave:
                    return string.Format("{0}/Saves/Autosave.json", Application.persistentDataPath);
                case SaveSlot.Slot1:
                    return string.Format("{0}/Saves/Slot1.json", Application.persistentDataPath);
                case SaveSlot.Slot2:
                    return string.Format("{0}/Saves/Slot2.json", Application.persistentDataPath);
                case SaveSlot.Slot3:
                    return string.Format("{0}/Saves/Slot3.json", Application.persistentDataPath);
                case SaveSlot.Slot4:
                    return string.Format("{0}/Saves/Slot4.json", Application.persistentDataPath);
                case SaveSlot.Slot5:
                    return string.Format("{0}/Saves/Slot5.json", Application.persistentDataPath);
            }
            throw new ArgumentException("The specified save slot is invalid.");
        }

        /// <summary>
        /// Defines the save slots.
        /// </summary>
        public enum SaveSlot {
            Autosave,
            Slot1,
            Slot2,
            Slot3,
            Slot4,
            Slot5
        }
        /// <summary>
        /// Universal, Global, and Active managers can implement this interface to receive events when saving and loading the game.
        /// </summary>
        public interface ISaveListener {
            /// <summary>
            /// Called when the game is saved.
            /// </summary>
            /// <param name="json">The json.</param>
            public void Save(JObject json);
            /// <summary>
            /// Called before the game is saved.
            /// </summary>
            /// <param name="json">The json.</param>
            public void SavePre(JObject json);
            /// <summary>
            /// Called after the game is saved.
            /// </summary>
            /// <param name="json">The json.</param>
            public void SavePost(JObject json);
            /// <summary>
            /// Called when the game is loaded.
            /// </summary>
            /// <param name="json">The json.</param>
            public void Load(JObject json);
            /// <summary>
            /// Called before the game is loaded.
            /// </summary>
            /// <param name="json">The json.</param>
            public void LoadPre(JObject json);
            /// <summary>
            /// Called after the game is loader.
            /// </summary>
            /// <param name="json">The json.</param>
            public void LoadPost(JObject json);
        }
        
        [SerializeField] private string m_MainmenuScene;
        [SerializeField] private string m_GameplayScene;
    }
}
