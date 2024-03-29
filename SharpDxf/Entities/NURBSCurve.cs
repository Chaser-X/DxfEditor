﻿#region SharpDxf, Copyright(C) 2012 Lomatus, Licensed under LGPL.

//                        SharpDxf library( Base on netDxf by Daniel Carvajal )
// Copyright (C) 2012 Lomatus (tourszhou@gmail.com)
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 

#endregion

using System;
using System.Collections.Generic;
using SharpDxf.Tables;

namespace SharpDxf.Entities
{
    /// <summary>
    /// Represents a nurbs curve <see cref="SharpDxf.Entities.IEntityObject">entity</see>.
    /// </summary>
    /// <remarks>The nurbs curve uses a default open uniform knot vector.</remarks>
    public class NurbsCurve :
        IEntityObject
    {
        #region private fields

        private const EntityType TYPE = EntityType.NurbsCurve;
        private const string DXF_NAME = DxfObjectCode.Polyline;
        private AciColor color;
        private Layer layer;
        private LineType lineType;
        private Dictionary<ApplicationRegistry, XData> xData;
        private List<NurbsVertex> controlPoints;
        private float[] knotVector;
        private int order;
        private float elevation;
        private float thickness;
        private Vector3f normal;
        private int curvePoints;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <c>NurbsCurve</c> class.
        /// </summary>
        public NurbsCurve()
        {
            this.controlPoints = new List<NurbsVertex>();
            this.normal = Vector3f.UnitZ;
            this.layer = Layer.Default;
            this.color = AciColor.ByLayer;
            this.lineType = LineType.ByLayer;
            this.order = 0;
            this.curvePoints = 30;
            this.elevation = 0.0f;
            this.thickness = 0.0f;
            this.normal = Vector3f.UnitZ;
        }

        /// <summary>
        /// Initializes a new instance of the <c>NurbsCurve</c> class.
        /// </summary>
        /// <param name="controlPoints">The nurbs curve <see cref="SharpDxf.Entities.NurbsVertex">control point</see> list.</param>
        /// <param name="order">The nurbs curve order.</param>
        public NurbsCurve(List<NurbsVertex> controlPoints, int order)
        {
            if (controlPoints.Count<order)
                throw new ArgumentOutOfRangeException("order",order,"The order of the curve must be less or equal the number of control points.");
            this.controlPoints = controlPoints;
            this.normal = Vector3f.UnitZ;
            this.layer = Layer.Default;
            this.color = AciColor.ByLayer;
            this.lineType = LineType.ByLayer;
            this.order = order;
            this.curvePoints = 30;
            this.elevation = 0.0f;
            this.thickness = 0.0f;
            this.normal = Vector3f.UnitZ;
        }


        #endregion

        #region public properties

        /// <summary>
        /// Gets the nurbs curve <see cref="NurbsVertex">control point</see> list.
        /// </summary>
        public List<NurbsVertex> ControlPoints
        {
            get { return this.controlPoints; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value"); 
                this.controlPoints = value;
            }
        }

        /// <summary>
        /// Gets or sets the nurbs curve order.
        /// </summary>
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }

        /// <summary>
        /// Gets or sets the nurbs curve <see cref="SharpDxf.Vector3f">normal</see>.
        /// </summary>
        public Vector3f Normal
        {
            get { return this.normal; }
            set
            {
                value.Normalize();
                this.normal = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of points generated along the nurbs curve during the conversion to a polyline.
        /// </summary>
        public int CurvePoints
        {
            get { return this.curvePoints; }
            set { this.curvePoints = value; }
        }

        /// <summary>
        /// Gets or sets the nurbs curve thickness.
        /// </summary>
        public float Thickness
        {
            get { return this.thickness; }
            set { this.thickness = value; }
        }

        /// <summary>
        /// Gets or sets the nurbs curve elevation.
        /// </summary>
        public float Elevation
        {
            get { return this.elevation; }
            set { this.elevation = value; }
        }
        #endregion

        #region IEntityObject Members

        /// <summary>
        /// Gets the dxf code that represents the entity.
        /// </summary>
        public string DxfName
        {
            get { return DXF_NAME; }
        }

        /// <summary>
        /// Gets the entity <see cref="SharpDxf.Entities.EntityType">type</see>.
        /// </summary>
        public EntityType Type
        {
            get { return TYPE; }
        }

        /// <summary>
        /// Gets or sets the entity <see cref="SharpDxf.AciColor">color</see>.
        /// </summary>
        public AciColor Color
        {
            get { return this.color; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.color = value;
            }
        }

        /// <summary>
        /// Gets or sets the entity <see cref="SharpDxf.Tables.Layer">layer</see>.
        /// </summary>
        public Layer Layer
        {
            get { return this.layer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.layer = value;
            }
        }

        /// <summary>
        /// Gets or sets the entity <see cref="SharpDxf.Tables.LineType">line type</see>.
        /// </summary>
        public LineType LineType
        {
            get { return this.lineType; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.lineType = value;
            }
        }

        /// <summary>
        /// Gets or sets the entity <see cref="SharpDxf.XData">extende data</see>.
        /// </summary>
        public Dictionary<ApplicationRegistry, XData> XData
        {
            get { return this.xData; }
            set { this.xData = value; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Obtains a list of vertexes that represent the nurbs curve.
        /// </summary>
        /// <param name="precision">Number of point to approximate the curve to a polyline.</param>
        /// <returns>The vertexes are expresed in object coordinate system.</returns>
        public List<Vector2f> PolygonalVertexes(int precision)
        {
            if (this.controlPoints.Count < this.order)
                throw new ArithmeticException("The order of the curve must be less or equal the number of control points.");

            this.knotVector = this.SetKnotVector();
            float[][][] nurbsBasisFunctions = this.DefineBasisFunctions(precision);

            List<Vector2f> vertexes = new List<Vector2f>();

            for (int i = 0; i < precision; i++)
            {
                float x = 0.0f;
                float y = 0.0f;
                for (int ctrlPointIndex = 0; ctrlPointIndex < this.controlPoints.Count; ctrlPointIndex++)
                {
                    x += this.controlPoints[ctrlPointIndex].Location.X*nurbsBasisFunctions[i][ctrlPointIndex][this.order - 1];
                    y += this.controlPoints[ctrlPointIndex].Location.Y*nurbsBasisFunctions[i][ctrlPointIndex][this.order - 1];
                }

                vertexes.Add(new Vector2f(x, y));
            }

            return vertexes;
        }

        /// <summary>
        /// Sets a constant weight for all the nurbs curve <see cref="NurbsVertex">vertex</see> list.
        /// </summary>
        /// <param name="weight">Nurbs vertex weight.</param>
        public void SetUniformWeights(float weight)
        {
            foreach (NurbsVertex v in this.controlPoints)
            {
                v.Weight = weight;
            }
        }

        #endregion

        #region private methods

        private float[][][] DefineBasisFunctions(int precision)
        {
            float[][][] nurbsBasisFunctions;
            float[][][] basisFunctions;

            basisFunctions = new float[precision][][];

            nurbsBasisFunctions = new float[precision][][];

            for (int vertexIndex = 0; vertexIndex < precision; vertexIndex++)
            {
                basisFunctions[vertexIndex] = new float[this.controlPoints.Count + 1][];
                nurbsBasisFunctions[vertexIndex] = new float[this.controlPoints.Count + 1][];

                float t = vertexIndex/(float) (precision - 1);

                if (t == 1.0f) t = 1.0f - MathHelper.EpsilonF;

                for (int ctrlPointIndex = 0; ctrlPointIndex < this.controlPoints.Count + 1; ctrlPointIndex++)
                {
                    basisFunctions[vertexIndex][ctrlPointIndex] = new float[this.order];
                    nurbsBasisFunctions[vertexIndex][ctrlPointIndex] = new float[this.order];

                    if (t >= this.knotVector[ctrlPointIndex] && t < this.knotVector[ctrlPointIndex + 1])
                        basisFunctions[vertexIndex][ctrlPointIndex][0] = 1.0f;
                    else
                        basisFunctions[vertexIndex][ctrlPointIndex][0] = 0.0f;
                }
            }

            for (int orderIndex = 1; orderIndex < this.order; orderIndex++)
            {
                for (int ctrlPointIndex = 0; ctrlPointIndex < this.controlPoints.Count; ctrlPointIndex++)
                {
                    for (int vertexIndex = 0; vertexIndex < precision; vertexIndex++)
                    {
                        float t = vertexIndex/(float) (precision - 1);

                        float Nikm1 = basisFunctions[vertexIndex][ctrlPointIndex][orderIndex - 1];
                        float Nip1km1 = basisFunctions[vertexIndex][ctrlPointIndex + 1][orderIndex - 1];

                        float xi = this.knotVector[ctrlPointIndex];
                        float xikm1 = this.knotVector[ctrlPointIndex + orderIndex - 1 + 1];
                        float xik = this.knotVector[ctrlPointIndex + orderIndex + 1];
                        float xip1 = this.knotVector[ctrlPointIndex + 1];

                        float FirstTermBasis;
                        if (Math.Abs(xikm1 - xi) < MathHelper.EpsilonF)
                            FirstTermBasis = 0.0f;
                        else
                            FirstTermBasis = ((t - xi)*Nikm1)/(xikm1 - xi);

                        float SecondTermBasis;
                        if (Math.Abs(xik - xip1) < MathHelper.EpsilonF)
                            SecondTermBasis = 0.0f;
                        else
                            SecondTermBasis = ((xik - t)*Nip1km1)/(xik - xip1);

                        basisFunctions[vertexIndex][ctrlPointIndex][orderIndex] = FirstTermBasis + SecondTermBasis;
                    }
                }
            }

            for (int orderIndex = 1; orderIndex < this.order; orderIndex++)
            {
                for (int ctrlPointIndex = 0; ctrlPointIndex < this.controlPoints.Count; ctrlPointIndex++)
                {
                    for (int vertexIndex = 0; vertexIndex < precision; vertexIndex++)
                    {
                        float denominator = 0.0f;
                        for (int controlWeight = 0; controlWeight < this.controlPoints.Count; controlWeight++)
                        {
                            denominator += this.controlPoints[controlWeight].Weight*basisFunctions[vertexIndex][controlWeight][orderIndex];
                        }

                        nurbsBasisFunctions[vertexIndex][ctrlPointIndex][orderIndex] = this.controlPoints[ctrlPointIndex].Weight*
                                                                           basisFunctions[vertexIndex][ctrlPointIndex][orderIndex]/
                                                                           denominator;
                    }
                }
            }

            return nurbsBasisFunctions;
        }

        private float[] SetKnotVector()
        {
            //This code creates an open uniform knot vector
            float[] knots = new float[this.controlPoints.Count + this.order];
            int knotValue = 0;
            for (int i = 0; i < this.order + this.controlPoints.Count; i++)
            {
                if (i <= this.controlPoints.Count && i >= this.order)
                    knotValue++;

                knots[i] = knotValue/(float) (this.controlPoints.Count - this.order + 1);
            }
            return knots;
        }

        #endregion
    }
}