// =====================================================================
// Copyright 2013-2022 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System;
using FluffyUnderware.DevTools;
using UnityEngine;

namespace FluffyUnderware.Curvy
{
    /// <summary>
    /// Base class for components that process a spline to produce something else, a line renderer or an edge collider for example
    /// </summary>
    [ExecuteInEditMode]
    public abstract class SplineProcessor : DTVersionedMonoBehaviour
    {
        public CurvySpline Spline
        {
            get { return m_Spline; }
            set
            {
                if (m_Spline != value)
                {
                    UnbindEvents();
                    m_Spline = value;
                    BindEvents();
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Method that processes the associated <see cref="CurvySpline"/>
        /// </summary>
        abstract public void Refresh();

        #region private

        /*! \cond PRIVATE */

        [SerializeField] protected CurvySpline m_Spline;

        /*! \endcond */

        private void OnSplineRefresh(CurvySplineEventArgs e)
        {
            Refresh();
        }

        private void OnSplineCoordinatesChanged(CurvySpline spline)
        {
            Refresh();
        }

        #endregion

        #region protected

        #region Unity callbacks

        protected virtual void Awake()
        {
            if (m_Spline == null)
            {
                m_Spline = GetComponent<CurvySpline>();
                if (ReferenceEquals(m_Spline, null) == false)
                    DTLog.Log(String.Format("[Curvy] Spline '{0}' was assigned to the {1} by default.", this.name, this.GetType().Name));
            }
        }

        protected virtual void OnEnable()
        {
            BindEvents();
        }

        protected virtual void OnDisable()
        {
            UnbindEvents();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            UnbindEvents();
            BindEvents();

            Refresh();
        }
#endif
        protected virtual void Start()
        {
            Refresh();
        }

        #endregion

        protected void BindEvents()
        {
            if (Spline)
            {
                Spline.OnRefresh.AddListenerOnce(OnSplineRefresh);
                Spline.OnGlobalCoordinatesChanged += OnSplineCoordinatesChanged;
            }
        }

        protected void UnbindEvents()
        {
            if (Spline)
            {
                Spline.OnRefresh.RemoveListener(OnSplineRefresh);
                Spline.OnGlobalCoordinatesChanged -= OnSplineCoordinatesChanged;
            }
        }

        #endregion
    }
}