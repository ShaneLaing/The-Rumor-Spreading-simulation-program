using System;

namespace RumerSpreading.Ver1
{
    partial class ChartView3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.formsPlot2 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // formsPlot2
            // 
            this.formsPlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot2.Location = new System.Drawing.Point(0, 0);
            this.formsPlot2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(908, 504);
            this.formsPlot2.TabIndex = 0;
            this.formsPlot2.Load += new System.EventHandler(this.formsPlot1_Load);
            // 
            // ChartView3
            // 
            this.ClientSize = new System.Drawing.Size(908, 504);
            this.Controls.Add(this.formsPlot2);
            this.Name = "ChartView3";
            this.Load += new System.EventHandler(this.ChartView3_Load_1);
            this.ResumeLayout(false);

        }

        private ScottPlot.FormsPlot formsPlot2;
    }
    #endregion
}