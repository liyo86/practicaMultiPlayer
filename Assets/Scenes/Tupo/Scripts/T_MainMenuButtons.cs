using UnityEngine;
using Unity.Netcode;

public class T_MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _buttonCG;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        HideButtons();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        HideButtons();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        HideButtons();
    }

    private void HideButtons()
    {
        _buttonCG.alpha = 0;
        _buttonCG.interactable = false;
    }


}
