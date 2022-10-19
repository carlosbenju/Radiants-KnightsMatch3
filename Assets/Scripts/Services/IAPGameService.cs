using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using Game.Services;

public class IAPGameService : IService, IStoreListener
{
    bool _isInitialized = false;
    IStoreController _unityStoreController = null;
    TaskStatus _purchaseTaskStatus = TaskStatus.Created;
    TaskStatus _initializeTaskStatus = TaskStatus.Created;

    public async Task Initialize(Dictionary<string, string> products)
    {
        _isInitialized = false;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (KeyValuePair<string, string> productEntry in products)
        {
            string[] stores = new[] { GooglePlay.Name };
            var ids = new IDs();
            ids.Add(productEntry.Value, stores);
            builder.AddProduct(productEntry.Key, ProductType.Consumable, ids);
        }

        _initializeTaskStatus = TaskStatus.Running;
        UnityPurchasing.Initialize(this, builder);
        while (_initializeTaskStatus == TaskStatus.Running)
        {
            await Task.Delay(100);
        }
    }

    public bool IsReady() => _isInitialized;

    public string GetLocalizedPrice(string product)
    {
        if (!_isInitialized)
            return string.Empty;

        Product unityProduct = _unityStoreController.products.WithID(product);
        return unityProduct?.metadata?.localizedPriceString;
    }

    public async Task<bool> StartPurchase(string product)
    {
        _purchaseTaskStatus = TaskStatus.Running;
        _unityStoreController.InitiatePurchase(product);

        while (_purchaseTaskStatus == TaskStatus.Running)
        {
            await Task.Delay(500);
        }

        return _purchaseTaskStatus == TaskStatus.RanToCompletion;
    }

    public void Clear()
    {

    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _isInitialized = true;
        _unityStoreController = controller;
        _initializeTaskStatus = TaskStatus.RanToCompletion;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        _isInitialized = false;
        Debug.Log("Initialization failed reason: " + error);
        _initializeTaskStatus = TaskStatus.RanToCompletion;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        _purchaseTaskStatus = TaskStatus.RanToCompletion;
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase failed with error: " + failureReason);
        _purchaseTaskStatus = TaskStatus.Faulted;
    }
}
