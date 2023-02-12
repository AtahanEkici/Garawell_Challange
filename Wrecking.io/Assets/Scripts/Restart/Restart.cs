using UnityEngine;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
    private void Update()
    {
        ListenToMouse();
    }
    private void ListenToMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Restarter();
        }
    }
    private void Restarter()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
