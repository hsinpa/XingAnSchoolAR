using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hsinpa.Model
{

    public abstract class ModelManager : MonoBehaviour
    {

        protected List<Model> models = new List<Model>();

        public virtual void SetUp()
        {

        }

        public T GetModel<T>() where T : Model
        {
            return models.First(x => typeof(T) == x.GetType()) as T;
        }
    }
}