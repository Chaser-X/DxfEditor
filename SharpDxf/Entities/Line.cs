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
    /// Represents a line <see cref="SharpDxf.Entities.IEntityObject">entity</see>.
    /// </summary>
    public class Line :
        DxfObject,
        IEntityObject
    {
        #region private fields

        private const EntityType TYPE = EntityType.Line;
        private Vector3f startPoint;
        private Vector3f endPoint;
        private float thickness;
        private AciColor color;
        private Layer layer;
        private LineType lineType;
        private Vector3f normal;
        private Dictionary<ApplicationRegistry, XData> xData;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <c>Line</c> class.
        /// </summary>
        /// <param name="startPoint">Line <see cref="Vector3f">start point.</see></param>
        /// <param name="endPoint">Line <see cref="Vector3f">end point.</see></param>
        public Line(Vector3f startPoint, Vector3f endPoint) 
            : base(DxfObjectCode.Line)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.thickness = 0.0f;
            this.layer = Layer.Default;
            this.color = AciColor.ByLayer;
            this.lineType = LineType.ByLayer;
            this.normal = Vector3f.UnitZ;
        }

        /// <summary>
        /// Initializes a new instance of the <c>Line</c> class.
        /// </summary>
        public Line()
            : base(DxfObjectCode.Line)
        {
            this.startPoint = Vector3f.Zero;
            this.endPoint = Vector3f.Zero;
            this.thickness = 0.0f;
            this.layer = Layer.Default;
            this.color = AciColor.ByLayer;
            this.lineType = LineType.ByLayer;
            this.normal = Vector3f.UnitZ;
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets or sets the line <see cref="SharpDxf.Vector3f">start point</see>.
        /// </summary>
        public Vector3f StartPoint
        {
            get { return this.startPoint; }
            set { this.startPoint = value; }
        }

        /// <summary>
        /// Gets or sets the line <see cref="SharpDxf.Vector3f">end point</see>.
        /// </summary>
        public Vector3f EndPoint
        {
            get { return this.endPoint; }
            set { this.endPoint = value; }
        }

        /// <summary>
        /// Gets or sets the line thickness.
        /// </summary>
        public float Thickness
        {
            get { return this.thickness ; }
            set { this.thickness = value; }
        }

        /// <summary>
        /// Gets or sets the line <see cref="SharpDxf.Vector3f">normal</see>.
        /// </summary>
        public Vector3f Normal
        {
            get { return this.normal; }
            set
            {
                if (Vector3f.Zero == value)
                    throw new ArgumentNullException("value","The normal can not be the zero vector");
                value.Normalize();
                this.normal = value;
            }
        }

        #endregion

        #region IEntityObject Members

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

        #region overrides

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return TYPE.ToString();
        }

        #endregion

    }
}