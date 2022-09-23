using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public static class TileAnimations
{
    private static float _tweenDuration = 0.25f;

    public static async Task DissapearAnimation(TileView tileToDissapear)
    {
        Sequence disappearIconSeq = DOTween.Sequence();

        disappearIconSeq.Join(tileToDissapear.icon.transform.DOScale(Vector3.zero, _tweenDuration));

        await disappearIconSeq.Play()
            .AsyncWaitForCompletion();
    }

    public static async Task DissapearAnimation(GOTileView tileToDissapear)
    {
        Sequence disappearIconSeq = DOTween.Sequence();
        disappearIconSeq.Join(tileToDissapear.transform.DOScale(Vector3.zero, _tweenDuration));

        await disappearIconSeq.Play()
            .AsyncWaitForCompletion();
    }

    public static async Task AppearAnimation(TileView tileToCreate)
    {
        Sequence appearIconSeq = DOTween.Sequence();

       appearIconSeq.Join(tileToCreate.icon.transform.DOScale(Vector3.one, _tweenDuration));

        await appearIconSeq.Play()
            .AsyncWaitForCompletion();
    }
}
