﻿/*
 * Created by SharpDevelop.
 * User: YZHOU
 * Date: 2012/8/17
 * Time: 13:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SharpDxf.Viewer
{
	partial class Canvas
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// Canvas
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "Canvas";
			this.Size = new System.Drawing.Size(149, 141);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasMouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CanvasMouseUp);
			this.MouseWheel +=new System.Windows.Forms.MouseEventHandler(this.CanvasMouseWheel);
			this.ResumeLayout(false);
		}
	}
}
