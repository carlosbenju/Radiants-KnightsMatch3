using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Services
{
    public class GameConfigService : IService
    {
        public int InitialGold { get; private set; }
        public int InitialDiamonds { get; private set; }
        public string InitialProfileImage { get; private set; }
        public int InitialHeroId { get; private set; }
        public int InitialBombBoosters { get; private set; }
        public int InitialColorBombBoosters { get; private set; }

        public void Initialize(RemoteConfigGameService dataProvider)
        {
            InitialGold = dataProvider.GetInt("InitialGold", 500);
            InitialDiamonds = dataProvider.GetInt("InitialDiamonds", 30);
            InitialProfileImage = dataProvider.GetString("InitialProfileImage", "galaxy-icon");
            InitialHeroId = dataProvider.GetInt("InitialHeroId", 1);
            InitialBombBoosters = dataProvider.GetInt("InitialBombBoosters", 1);
            InitialColorBombBoosters = dataProvider.GetInt("InitialColorBombBoosters", 1);
        }


        public void Clear()
        {
        }
    }
}

