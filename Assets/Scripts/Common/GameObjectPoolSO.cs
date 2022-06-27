using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class GameObjectPoolSO<T> : ScriptableObject
    where T : Component
    {
        protected Transform _parentTrans;

        public abstract IFactory<T> Factory { get; set; }

        protected List<T> golist = new List<T>();


        private void OnDisable()
        {
            golist.Clear();
        }


        #region Public Methods
        public void InitPool(Transform poolRoot, int num, string memberName = "PoolMember")
        {
            
            SetPoolRoot(poolRoot);
            InitFillPool(num, memberName);
        }

        public void SetPoolRoot(Transform theParent)
        {
            _parentTrans = theParent;
        }

        public virtual void InitFillPool(int num, string memberName = "PoolMember")
        {
            golist.Clear();
            for (int i = 0; i < num; i++)
            {
                T member = Create();
                if (_parentTrans != null)
                {
                    member.transform.SetParent(_parentTrans);
                }
                member.name = memberName;
                golist.Add(member);
                ReturnToPool(member);
            }
        }

        public virtual T Request(bool isSetActive = true)
        {
            T member = FindUsable();
            member.gameObject.SetActive(isSetActive);
            return member;
        }

        public virtual void ReturnToPool(T member)
        {
            member.gameObject.SetActive(false);
        }

        public virtual List<T> GetAllMembers()
        {
            return golist;
        }

        public virtual void ClearPool()
        {
            foreach (T member in golist)
            {
                ReturnToPool(member);
            }
        }
        #endregion


        #region Private Methods


        protected virtual T Create()
        {
            return Factory.Create();
        }


        protected virtual T FindUsable()
        {
            T t = golist.Find(p => !p.gameObject.activeSelf);
            if (t != null)
            {
                return t;
            }
            else
            {
                ExtendPool();
                //Debug.Log("没有可用的");
                t = golist.Find(p => !p.gameObject.activeSelf);
                return t;
            }
        }

        protected virtual void ExtendPool()
        {
            InitFillPool(10);
        }


        #endregion
    }
}
