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

using SharpDxf.Tables;

namespace SharpDxf.Blocks
{

    /// <summary>
    /// Represents the termination element of the block definiton.
    /// </summary>
    internal class BlockEnd :
        DxfObject
    {
        private Layer layer;

        /// <summary>
        /// Initializes a new instance of the <c>BlockEnd</c> class.
        /// </summary>
        public BlockEnd(Layer layer) : base(DxfObjectCode.BlockEnd)
        {
            this.layer = layer;
        }

        /// <summary>
        /// Gets or sets the block end <see cref="SharpDxf.Tables.Layer">layer</see>
        /// </summary>
        public Layer Layer
        {
            get { return this.layer; }
            set { if (value != null) this.layer = value; }
        }
    }
}