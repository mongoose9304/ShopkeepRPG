using UnityEngine;

namespace Shopkeeper.Menus {
    public class MenuMain : MonoBehaviour {
        public void BtnStart() {
            ManagerUniversal.GetManager<UniversalSaveManager>().New();
        }
        public void BtnLoad() {
            Debug.Log("Implement me!");
        }
        public void BtnGuide() {
            Debug.Log("Implement me!");
        }
        public void BtnSettings() {
            Debug.Log("Implement me!");
        }
        public void BtnLanguage() {
            Debug.Log("Implement me!");
        }
        public void BtnQuit() {
            Application.Quit();
        }
    }
}
