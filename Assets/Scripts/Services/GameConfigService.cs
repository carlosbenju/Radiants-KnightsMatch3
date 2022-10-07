using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class GameConfigService : IService
    {
        public RemoteConfigGameService GameConfig;
        public List<InGameResourceConfig> ResourcesConfig;

        public int InitialGold { get; private set; }
        public int InitialDiamonds { get; private set; }
        public string InitialProfileImage { get; private set; }
        public int InitialHeroId { get; private set; }
        public int InitialBombBoosters { get; private set; }
        public int InitialColorBombBoosters { get; private set; }

        public void Initialize(RemoteConfigGameService config)
        {
            InitialGold = config.GetInt("InitialGold", 500);
            InitialDiamonds = config.GetInt("InitialDiamonds", 30);
            InitialProfileImage = config.GetString("InitialProfileImage", "galaxy-icon");
            InitialHeroId = config.GetInt("InitialHeroId", 1);
            InitialBombBoosters = config.GetInt("InitialBombBoosters", 1);
            InitialColorBombBoosters = config.GetInt("InitialColorBombBoosters", 1);
        }


        public void Clear()
        {
        }
    }
}

