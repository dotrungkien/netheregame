using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Contracts;

public class ERC20 : MonoBehaviour
{
    private Account account;
    private Web3 web3;

    void Start()
    {
        var url = "http://localhost:8545";
        var ecKey = EthECKey.GenerateKey();
        // var privateKey = ecKey.GetPrivateKey();
        var privateKey = "0xb3f177835a10ccc6a7b132d8e690c073738719bf73f93fcec16ce884cab4d569";
        account = new Account(privateKey);
        web3 = new Web3(account, url);
        Debug.Log(string.Format("account {0}", account.Address));
        DeployContract();
    }

    async void DeployContract()
    {
        var deploymentMessage = new StandardTokenDeployment
        {
            TotalSupply = 100000
        };
        var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();
        var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
        var contractAddress = transactionReceipt.ContractAddress;
        Debug.Log(string.Format("deploy complete, address {0}", contractAddress));
    }
    // void BalanceOf(string address)
    // {
    //     var balanceOfFunctionMessage = new BalanceOfFunction()
    //     {
    //         Owner = account.Address,
    //     };

    //     var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
    //     var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);
    // }
}

public class StandardTokenDeployment : ContractDeploymentMessage
{

    public static string BYTECODE = "";

    public StandardTokenDeployment() : base(BYTECODE) { }

    [Parameter("uint256", "totalSupply")]
    public BigInteger TotalSupply { get; set; }
}

[Function("balanceOf", "uint256")]
public class BalanceOfFunction : FunctionMessage
{
    [Parameter("address", "_owner", 1)]
    public string Owner { get; set; }
}

[Function("transfer", "bool")]
public class TransferFunction : FunctionMessage
{
    [Parameter("address", "_to", 1)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 2)]
    public BigInteger TokenAmount { get; set; }
}

[Event("Transfer")]
public class TransferEventDTO : IEventDTO
{
    [Parameter("address", "_from", 1, true)]
    public string From { get; set; }

    [Parameter("address", "_to", 2, true)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 3, false)]
    public BigInteger Value { get; set; }
}
