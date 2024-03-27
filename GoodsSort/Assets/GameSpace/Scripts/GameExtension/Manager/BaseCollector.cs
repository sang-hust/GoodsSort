using System.Collections;
using UnityEngine;

public enum ItemTypeEnum 
{
    None,
    item_01,
    item_02,
    item_03,
    item_04,
    item_05,
    item_06
}

public class BaseCollector
{
    public static void Claim(IEnumerable items, bool isSendMess = true)
    {
        if (isSendMess)
        {
            foreach (var obj in items)
            {
                if (obj is IReward ir)
                {
                    ir.OnClaimed();
                }
            }
        }
        else
        {
            foreach (var obj in items)
            {
                if (obj is IReward ir)
                {
                    ir.OnClaimedNotSend();
                }
            }
        }
    }
    public static void Claim(IReward item, bool isSendMess = true)
    {
        if (isSendMess)
        {
            item.OnClaimed();
        }
        else
        {
            item.OnClaimedNotSend();
        }
    }
}

public interface IReward
{
    void OnClaimed();
    void OnClaimedNotSend();
}
public struct RewardInfo : IReward
{
    public string rewardKey;
    public int rewardAmount;
    public string rewardData;
    
    public RewardInfo(ItemTypeEnum rewardKey, int rewardAmount)
    {
        this.rewardKey = rewardKey.ToString();
        this.rewardAmount = rewardAmount;
        this.rewardData = null;
    }

    public RewardInfo(string rewardKey, int rewardAmount)
    {
        this.rewardKey = rewardKey;
        this.rewardAmount = rewardAmount;
        this.rewardData = null;
    }

    public RewardInfo(string rewardKey, int rewardAmount, string data)
    {
        this.rewardKey = rewardKey;
        this.rewardAmount = rewardAmount;
        this.rewardData = data;
    }

    public RewardInfo(ItemTypeEnum rewardKey, int rewardAmount, string data)
    {
        this.rewardKey = rewardKey.ToString();
        this.rewardAmount = rewardAmount;
        this.rewardData = data;
    }
    public static implicit operator ItemTypeEnum(RewardInfo rw) => rw.rewardKey.Parse(ItemTypeEnum.None);
    
    public void OnClaimed()
    {
        if (rewardAmount <= 0) return;
        //GameDataCenter.Instance.Get(out WrapperPlayer wrapperPlayer);
       // wrapperPlayer.ModifyResourceAmount(this, rewardAmount);
    }

    public void OnClaimedNotSend()
    {
        if (rewardAmount <= 0) return;
        //GameDataCenter.Instance.Get(out WrapperPlayer wrapperPlayer);
       // wrapperPlayer.ModifyResourceAmountNotSend(this, rewardAmount);
    }
}

public class ResourceInHome
{
    public static readonly ResourceInHome Instance = new ResourceInHome();
    public DataReward gold, nut;

    //private ResourceInHome() => GameDataCenter.Instance.GetOrCreate(out player);
    //private readonly WrapperPlayer player;
    public struct DataReward
    {
        public int bonusValue;
    }
    public void Do()
    {
        //CoroutineChain.Start.Play(DoGold());
        //CoroutineChain.Start.Play(DoNut());
    }
    // private IEnumerator DoGold()
    // {
    //     if (gold.bonusValue <= 0)
    //     {
    //         HomeUIManager.Instance.gold.ReplaceValue(player.Client.golds).DoAnim(0.5f);
    //         yield break;
    //     }
    //     HomeUIManager.Instance.gold.ReplaceValue(player.Client.golds - gold.bonusValue).DoImmediately();
    //     yield return new WaitForSeconds(1);
    //     SoundManager.Instance.PlaySfx("CoinPreCollect");
    //     HomeUIManager.Instance.gold.MoreValue(gold.bonusValue).DoCollectAnim(new Vector3(0.5f, 0, 1));
    //     gold.bonusValue = 0;
    // }
}