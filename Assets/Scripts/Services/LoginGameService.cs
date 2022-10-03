using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Game.Services
{
    public class LoginGameService : IService
    {
        public async Task Initialize()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Logged in with ID: " + AuthenticationService.Instance.PlayerId);
            }
        }

        public string GetUserId()
        {
            return AuthenticationService.Instance.PlayerId;
        }

        public void Clear()
        {
        }
    }
}

