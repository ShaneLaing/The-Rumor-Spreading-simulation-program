namespace RumerSpreading.Ver1
{
    partial class ChartView
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.formsPlot2 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formsPlot1.Location = new System.Drawing.Point(0, 0);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(800, 225);
            this.formsPlot1.TabIndex = 0;
            // 
            // formsPlot2
            // 
            this.formsPlot2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.formsPlot2.Location = new System.Drawing.Point(0, 211);
            this.formsPlot2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(800, 239);
            this.formsPlot2.TabIndex = 1;
            // 
            // ChartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.formsPlot2);
            this.Controls.Add(this.formsPlot1);
            this.Font = new System.Drawing.Font("標楷體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "ChartView";
            this.Text = "Chart View";
            this.Load += new System.EventHandler(this.Scottplot_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private ScottPlot.FormsPlot formsPlot2;
    }
}