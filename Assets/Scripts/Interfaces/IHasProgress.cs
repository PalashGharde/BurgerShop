using System;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgressUpdateArgs> OnProgressUpdate;

    public class OnProgressUpdateArgs : EventArgs
    {
        public float progressAmount;
        public bool playAnimation;

        // Can also add color fied for burning the meat
    }
}
