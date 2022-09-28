using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IIAPService : IService
{
    bool IsReady();

    Task Initialize(Dictionary<string, string> products);

    Task<bool> StartPurchase(string product);

    string GetLocalizedPrice(string product);
}
