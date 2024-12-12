using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplayCode : MonoBehaviour
{
    public TMP_Text coinCountText;

    void Update()
    {
        int coin = ResourceManagerCode.instance.GetResourceValue("coin");
        UpdateResourceCount(coin);
    }
    public void UpdateResourceCount(int coin)
    {
        coinCountText.text = "Coin: " + coin.ToString();
    }
}
