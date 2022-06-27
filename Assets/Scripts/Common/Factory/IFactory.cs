using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IFactory<T>
    {
        public T Create();
    }
}
