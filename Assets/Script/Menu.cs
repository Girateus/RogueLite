using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void OnclickEnter()
   {
      Debug.Log("Play");
      SceneManager.LoadScene("Hub");
   }

   public void OnClickExit()
   {
      //#if Unity_EDITOR
      Debug.Log("ByeBye");
      Application.Quit();
   }
   
   public void OnClickMenu()
   {
      SceneManager.LoadScene("Menu");
   }
}
