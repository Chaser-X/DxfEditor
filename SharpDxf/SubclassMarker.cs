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

namespace SharpDxf
{
    /// <summary>
    /// Dxf object subclass string markers.
    /// </summary>
    internal static class SubclassMarker
    {
        public const string ApplicationId = "AcDbRegAppTableRecord";
        public const string Table = "AcDbSymbolTable";
        public const string TableRecord = "AcDbSymbolTableRecord";
        public const string Layer = "AcDbLayerTableRecord";
        public const string ViewPort = "AcDbViewportTableRecord";
        public const string View = "AcDbViewTableRecord";
        public const string LineType = "AcDbLinetypeTableRecord";
        public const string TextStyle = "AcDbTextStyleTableRecord";
        public const string DimensionStyleTable = "AcDbDimStyleTable";
        public const string DimensionStyle = "AcDbDimStyleTableRecord";
        public const string BlockRecord = "AcDbBlockTableRecord";
        public const string BlockBegin = "AcDbBlockBegin";
        public const string BlockEnd = "AcDbBlockEnd";
        public const string Entity = "AcDbEntity";
        public const string Arc = "AcDbArc";
        public const string Circle = "AcDbCircle";
        public const string Ellipse = "AcDbEllipse";
        public const string Face3d = "AcDbFace";
        public const string Insert = "AcDbBlockReference";
        public const string Line = "AcDbLine";
        public const string Point = "AcDbPoint";
        public const string Vertex = "AcDbVertex";
        public const string Polyline = "AcDb2dPolyline";
        public const string LightWeightPolyline = "AcDbPolyline";
        public const string PolylineVertex = "AcDb2dVertex ";
        public const string Polyline3d = "AcDb3dPolyline";
        public const string Polyline3dVertex = "AcDb3dPolylineVertex";
        public const string PolyfaceMesh = "AcDbPolyFaceMesh";
        public const string PolyfaceMeshVertex = "AcDbPolyFaceMeshVertex";
        public const string PolyfaceMeshFace = "AcDbFaceRecord";
        public const string Solid = "AcDbTrace";
        public const string Text = "AcDbText";
        public const string Attribute = "AcDbAttribute";
        public const string AttributeDefinition = "AcDbAttributeDefinition";
        public const string Dictionary = "AcDbDictionary";
    }
}