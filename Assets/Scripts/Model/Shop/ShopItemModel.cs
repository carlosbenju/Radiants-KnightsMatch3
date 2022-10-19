[System.Serializable]
public class ShopItemModel
{
    public string Id;
    public string Image;

    public string CostType;
    public int CostAmount;

    public string RewardType;
    public string RewardName;
    public int RewardAmount;

    public bool IsObtainedWithAd;
    public bool IsObtainedWithIAP;

    public string RemoteId;
}
