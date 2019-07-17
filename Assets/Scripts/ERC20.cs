using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Signer;

public class ERC20 : MonoBehaviour
{
    void Start()
    {
        var url = "http://localhost:8545";
        var ecKey = EthECKey.GenerateKey();
        var privateKey = ecKey.GetPrivateKey();
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        Debug.Log(string.Format("account {0}", account.Address));
    }
}
