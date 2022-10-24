using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class GameConfigService : IService
    {
        public RemoteConfigGameService GameConfig;
        public List<InGameResourceConfig> ResourcesConfig;
        public string InitialProfileImage { get; private set; }
        public int InitialHeroId { get; private set; }

        public void Initialize(RemoteConfigGameService config)
        {
            InitialProfileImage = config.GetString("InitialProfileImage", "galaxy-icon");
            InitialHeroId = config.GetInt("InitialHeroId", 1);
        }


        public void Clear()
        {
        }
    }
}

