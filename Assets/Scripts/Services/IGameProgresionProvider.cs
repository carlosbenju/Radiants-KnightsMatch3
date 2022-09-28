using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Services
{
    public interface IGameProgresionProvider
    {
        Task<bool> Initialize();
        public string Get(string key, string defaultValue);
        public int Get(int key, int defaultValue);
        public float Get(float key, float defaultValue);
        public bool Get(bool key, bool defaultValue);
        public T Get<T>(T key, T defaultValue);

        void Set(string key, string value);
        void Set(int key, int value);
        void Set(float key, float value);
        void Set(bool key, bool value);
        void Set(T key, T value);
    }
}

