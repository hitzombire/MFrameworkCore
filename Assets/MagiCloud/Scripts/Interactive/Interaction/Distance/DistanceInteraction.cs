﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace MagiCloud.Interactive.Distance
{
    [System.Serializable]
    public class EventDistanceInteraction : UnityEvent<DistanceInteraction> { }

    /// <summary>
    /// 距离交互(挂在物体中)
    /// </summary>
    [ExecuteInEditMode]
    public class DistanceInteraction : MonoBehaviour
    {
        public DistanceData distanceData;

        //靠近、停留、离开、放下、中断(OnBreak)
        public EventDistanceInteraction OnEnter, OnStay, OnExit, OnRelease;

        public UnityEvent OnNotRelease;

        /// <summary>
        /// 功能对象
        /// </summary>
        public Features.FeaturesObjectController FeaturesObjectController { get; set; }

        /// <summary>
        /// 外部交互对象
        /// </summary>
        //public InteractionBase ExternalInteraction { get; set; }
        public IExternalInteraction ExternalInteraction { get; set; }

        /// <summary>
        /// 初始距离检测，在距离内则进行交互
        /// </summary>
        public bool AutoDetection = true;

        /// <summary>
        /// 是否被抓取
        /// </summary>
        public bool IsGrab { get; set; }

        private void Awake()
        {

            if (OnEnter == null)
                OnEnter = new EventDistanceInteraction();

            if (OnStay == null)
                OnStay = new EventDistanceInteraction();

            if (OnExit == null)
                OnExit = new EventDistanceInteraction();

            if (OnRelease == null)
                OnRelease = new EventDistanceInteraction();

            if (OnNotRelease == null)
                OnNotRelease = new UnityEvent();

            //加入时，检索一次
            if (distanceData == null)
            {
                distanceData = new DistanceData();
            }

            distanceData.Interaction = this;

            if (FeaturesObjectController == null)
            {
                FeaturesObjectController = gameObject.GetComponent<Features.FeaturesObjectController>();

                if (FeaturesObjectController == null)
                    FeaturesObjectController = gameObject.GetComponentInParent<Features.FeaturesObjectController>();
            }
        }

        protected virtual void OnEnable()
        {

            distanceData.IsEnabel = true;
            //统一调用，去匹配数据，还需要一个数据，每隔一段时间校验一次，用于匹配执行顺序等情况
            DistanceStorage.AddDistanceData(distanceData);

            //检索一次，是否有物体在距离内，在的话，则进行处理

        }

        private IEnumerator Start()
        {
            if (AutoDetection)
                yield return StartCoroutine(AutoInteraction(0.01f));
        }

        /// <summary>
        /// 自动交互
        /// </summary>
        /// <returns></returns>
        public IEnumerator AutoInteraction(float delay)
        {
            //关闭一些动作，只是单纯的数据初始化

            //在距离内的时候，进行一次检索
            if (InteractiveController.Instance != null && (distanceData.interactionType == InteractionType.Send || distanceData.interactionType == InteractionType.All))
            {
                InteractiveController.Instance.Search.OnStartInteraction(FeaturesObjectController.gameObject, false);
                yield return new WaitForSeconds(delay);
                InteractiveController.Instance.Search.OnStopInteraction(FeaturesObjectController.gameObject);
            }
        }

        protected virtual void OnDisable()
        {
            distanceData.IsEnabel = false;
            DistanceStorage.DeleteDistanceData(distanceData);
        }

        public virtual bool IsCanInteraction(DistanceInteraction distanceInteraction)
        {
            return true;
        }

        /// <summary>
        /// 移入
        /// </summary>
        public virtual void OnDistanceEnter(DistanceInteraction distanceInteraction)
        {
            if (OnEnter != null)
                OnEnter.Invoke(distanceInteraction);
        }

        /// <summary>
        /// 离开
        /// </summary>
        public virtual void OnDistanceExit(DistanceInteraction distanceInteraction)
        {
            if (OnExit != null)
            {
                OnExit.Invoke(distanceInteraction);
            }
        }

        /// <summary>
        /// 停留
        /// </summary>
        public virtual void OnDistanceStay(DistanceInteraction distanceInteraction)
        {
            if (OnStay != null)
            {
                OnStay.Invoke(distanceInteraction);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void OnDistanceRelesae(DistanceInteraction distanceInteraction)
        {

            if (OnRelease != null)
            {
                OnRelease.Invoke(distanceInteraction);
            }
        }

        /// <summary>
        /// 没有物体与它进行交互时，会进行释放
        /// </summary>
        public virtual void OnDistanceNotInteractionRelease()
        {
            if (OnNotRelease != null)
            {
                OnNotRelease.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (UnityEditor.Selection.activeObject == gameObject)
            {
                switch (distanceData.distanceShape)
                {
                    case DistanceShape.Sphere:

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(transform.position, distanceData.distanceValue);

                        break;
                    case DistanceShape.Cube:

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(transform.position, distanceData.Size);

                        break;
                }

                
            }
#endif

        }
    }
}

