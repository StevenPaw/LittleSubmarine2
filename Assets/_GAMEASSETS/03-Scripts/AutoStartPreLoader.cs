#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LittleSubmarine2
{
    [InitializeOnLoadAttribute]
    public class AutoStartPreLoader
    {
        static AutoStartPreLoader(){
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }
   
        static void LoadDefaultScene(PlayModeStateChange state){
            if (state == PlayModeStateChange.ExitingEditMode) {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
            }
 
            if (state == PlayModeStateChange.EnteredPlayMode) {
                EditorSceneManager.LoadScene (0);
            }
        }  
    }
}
#endif