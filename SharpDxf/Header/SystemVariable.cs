#region SharpDxf, Copyright(C) 2012 Lomatus, Licensed under LGPL.

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

namespace SharpDxf.Header
{
    /// <summary>
    /// Strings system variables
    /// </summary>
    public static class SystemVariable
    {
        /// <summary>
        /// The AutoCAD drawing database version number.
        /// </summary>
        public const string DabaseVersion = "$ACADVER";

        /// <summary>
        /// Next available handle (this variable must be present in the header section)
        /// </summary>
        public const string HandSeed = "$HANDSEED";
    }
}