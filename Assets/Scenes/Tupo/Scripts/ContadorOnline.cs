using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ContadorOnline : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _contadorText;

    public NetworkVariable<int> ContadorNumero;


    private void Update()
    {
        _contadorText.text = $"Número:\n{ContadorNumero.Value}";
    }

    #region INCREASE

    public void IncreaseContador()
    {
        if (IsServer)
            ContadorNumero.Value++;
        else
            IncreaseContadorServerRpc();

    }

    [ServerRpc(RequireOwnership = false)]
    private void IncreaseContadorServerRpc()
    {
        ContadorNumero.Value++;
    }

    #endregion

    #region DECREASE

    public void DecreaseContador()
    {
        if (IsServer)
            ContadorNumero.Value--;
        else
            DecreaseContadorServerRpc();

    }

    [ServerRpc(RequireOwnership = false)]
    private void DecreaseContadorServerRpc()
    {
        ContadorNumero.Value--;
    }

    #endregion

}
