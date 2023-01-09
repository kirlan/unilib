﻿using LandscapeGeneration.PathFind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    /// <summary>
    /// Территория - образование на карте, имеющее замкнутую границу и связанное с 1 или несколькими информационными слоями, известное как <typeparamref name="LAYER"/>
    /// </summary>
    public abstract class Territory<LAYER> : TransportationNode, IInfoLayer
        where LAYER : Territory<LAYER>, new()
    {
        /// <summary>
        /// Границы с другими <typeparamref name="LAYER"/>
        /// </summary>
        public virtual Dictionary<LAYER, List<VoronoiEdge>> BorderWith { get; } = new Dictionary<LAYER, List<VoronoiEdge>>();

        protected readonly static LAYER m_pForbidden = new LAYER().SetForbidden();

        public virtual bool Forbidden { get; private set; } = false;

        /// <summary>
        /// Суммарная длина всех линий в <see cref="BorderWith"/>
        /// </summary>
        public virtual float PerimeterLength { get; protected set; } = 0;

        protected LAYER SetForbidden()
        {
            Forbidden = true;
            return (LAYER)this;
        }

        private readonly Dictionary<Type, dynamic> m_cInfoLayers = new Dictionary<Type, dynamic>();

        public void AddLayer<T>(T value) where T : class, IInfoLayer
        {
            m_cInfoLayers[typeof(T)] = value;
        }

        public T As<T>() where T : class, IInfoLayer
        {
            if (GetType() == typeof(T))
                return this as T;

            if (m_cInfoLayers.TryGetValue(typeof(T), out dynamic pResult))
                return pResult as T;
            else
                return null;
        }

        public bool Is<T>() where T : class, IInfoLayer
        {
            if (GetType() == typeof(T))
                return true;

            return m_cInfoLayers.ContainsKey(typeof(T));
        }

        public void ClearLayers()
        {
            m_cInfoLayers.Clear();
        }
    }

    /// <summary>
    /// Территория, являющаяся частью <typeparamref name="OWNER"/>.
    /// </summary>
    /// <typeparam name="OWNER"></typeparam>
    public abstract class TerritoryOf<LAYER, OWNER> : Territory<LAYER>, IInfoLayer<OWNER>
        where LAYER : TerritoryOf<LAYER, OWNER>, new()
        where OWNER : class, IInfoLayer
    {
        private OWNER m_cOwnerInfoLayer = null;

        /// <summary>
        /// Маркирует эту территорию как часть указанного <typeparamref name="OWNER"/>
        /// </summary>
        /// <param name="value"></param>
        public void SetOwner(OWNER value)
        {
            if (m_cOwnerInfoLayer != null)
                throw new InvalidOperationException("Owner InfoLayer already set! Clear it first!");

            m_cOwnerInfoLayer = value;
        }

        /// <summary>
        /// Возвращает <typeparamref name="OWNER"/>, частью которого является эта территория
        /// </summary>
        /// <returns></returns>
        public OWNER GetOwner()
        {
            return m_cOwnerInfoLayer;
        }

        /// <summary>
        /// Является ли эта территория частью какого-нибудь <typeparamref name="OWNER"/>?
        /// </summary>
        /// <returns></returns>
        public bool HasOwner()
        {
            return m_cOwnerInfoLayer != null;
        }

        /// <summary>
        /// Маркирует эту территорию как не принадлежащую ни одному <typeparamref name="OWNER"/>
        /// </summary>
        public void ClearOwner()
        {
            m_cOwnerInfoLayer = null;
        }
    }

    //TODO: подумать, как не писать один и тот же код в 2 классах?

    /// <summary>
    /// Дополнительный информационный слой для <typeparamref name="BASE"/>, сам являющаяся частью <typeparamref name="OWNER"/>
    /// </summary>
    /// <typeparam name="OWNER"></typeparam>
    /// <typeparam name="BASE"></typeparam>
    public abstract class TerritoryExtended<LAYER, OWNER, BASE> : TerritoryOf<LAYER, OWNER>
        where LAYER : TerritoryExtended<LAYER, OWNER, BASE>, new()
        where OWNER : class, IInfoLayer
        where BASE : Territory<BASE>, IInfoLayer, new()
    {
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Forbidden</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override bool Forbidden => Origin == null || Origin.Forbidden;

        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>PerimeterLength</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float PerimeterLength => Origin.PerimeterLength;

        #region Поля TransportationNode - чтобы корректно работал DistanceTo и вычисление пути

        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>X</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float X => Origin.X;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Y</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float Y => Origin.Y;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>H</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float H => Origin.H;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Links</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override Dictionary<TransportationNode, TransportationLinkBase> Links => Origin.Links;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>IsHarbor</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsHarbor => Origin.IsHarbor;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>GetMovementCost()</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float GetMovementCost()
        {
            return Origin.GetMovementCost();
        }

        #endregion

        /// <summary>
        /// Базовый объект <typeparamref name="BASE"/>, расширением которого является <typeparamref name="LAYER"/>
        /// <inheritdoc/>
        /// </summary>
        public BASE Origin => this.As<BASE>();

        protected TerritoryExtended(BASE pBase)
        {
            AddLayer(pBase);
        }

        protected TerritoryExtended()
        {
        }

        public void FillBorderWithKeys()
        {
            foreach (var pLink in Origin.BorderWith)
            {
                LAYER key = pLink.Key.As<LAYER>() ?? m_pForbidden;
                BorderWith[key] = pLink.Value;
            }
        }
    }

    /// <summary>
    /// Дополнительный информационный слой для <typeparamref name="BASE"/>, без информации о владельце.
    /// </summary>
    /// <typeparam name="BASE"></typeparam>
    public abstract class TerritoryExtended<LAYER, BASE> : Territory<LAYER>
        where LAYER : TerritoryExtended<LAYER, BASE>, new()
        where BASE : Territory<BASE>, IInfoLayer, new()
    {
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Forbidden</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override bool Forbidden => Origin.Forbidden;

        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>PerimeterLength</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float PerimeterLength => Origin.PerimeterLength;

        #region Поля TransportationNode - чтобы корректно работал DistanceTo и вычисление пути

        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>X</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float X => Origin.X;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Y</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float Y => Origin.Y;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>H</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float H => Origin.H;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>Links</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override Dictionary<TransportationNode, TransportationLinkBase> Links => Origin.Links;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>IsHarbor</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsHarbor => Origin.IsHarbor;
        /// <summary>
        /// Переадресует обращение к базовому <typeparamref name="BASE"/>::<c>GetMovementCost()</c><br/>
        /// <inheritdoc/>
        /// </summary>
        public override float GetMovementCost()
        {
            return Origin.GetMovementCost();
        }

        #endregion

        /// <summary>
        /// Базовый объект <typeparamref name="BASE"/>, расширением которого является <typeparamref name="LAYER"/>
        /// <inheritdoc/>
        /// </summary>
        public BASE Origin => this.As<BASE>();

        protected TerritoryExtended(BASE pBase)
        {
            AddLayer(pBase);
        }

        protected TerritoryExtended()
        { }

        public void FillBorderWithKeys()
        {
            foreach (var pLink in Origin.BorderWith)
            {
                LAYER key = pLink.Key.As<LAYER>() ?? m_pForbidden;
                BorderWith[key] = pLink.Value;
            }
        }
    }
}
