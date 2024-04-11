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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SMA_Radious = new System.Windows.Forms.TextBox();
            this.Statistic = new System.Windows.Forms.Button();
            this.CurveFitting = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MV_PrimeValues = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formsPlot2
            // 
            this.formsPlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot2.Location = new System.Drawing.Point(0, 0);
            this.formsPlot2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot2.Name = "formsPlot2";
            this.formsPlot2.Size = new System.Drawing.Size(894, 626);
            this.formsPlot2.TabIndex = 0;
            this.formsPlot2.Load += new System.EventHandler(this.formsPlot1_Load);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.panel1.Controls.Add(this.MV_PrimeValues);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.SMA_Radious);
            this.panel1.Controls.Add(this.Statistic);
            this.panel1.Controls.Add(this.CurveFitting);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 626);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Century", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(79, 65);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 23);
            this.label15.TabIndex = 37;
            this.label15.Text = "2~100";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Cursor = System.Windows.Forms.Cursors.Default;
            this.label16.Font = new System.Drawing.Font("Century", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(17, 36);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(136, 23);
            this.label16.TabIndex = 36;
            this.label16.Text = "SMV-Radious";
            // 
            // SMA_Radious
            // 
            this.SMA_Radious.Location = new System.Drawing.Point(17, 63);
            this.SMA_Radious.Margin = new System.Windows.Forms.Padding(4);
            this.SMA_Radious.Name = "SMA_Radious";
            this.SMA_Radious.Size = new System.Drawing.Size(54, 29);
            this.SMA_Radious.TabIndex = 35;
            this.SMA_Radious.TextChanged += new System.EventHandler(this.SMA_Radious_TextChanged);
            // 
            // Statistic
            // 
            this.Statistic.BackColor = System.Drawing.SystemColors.Info;
            this.Statistic.Location = new System.Drawing.Point(17, 223);
            this.Statistic.Name = "Statistic";
            this.Statistic.Size = new System.Drawing.Size(146, 32);
            this.Statistic.TabIndex = 32;
            this.Statistic.Text = "Statistic";
            this.Statistic.UseVisualStyleBackColor = false;
            this.Statistic.Click += new System.EventHandler(this.Statistic_Click);
            // 
            // CurveFitting
            // 
            this.CurveFitting.AutoSize = true;
            this.CurveFitting.Location = new System.Drawing.Point(17, 149);
            this.CurveFitting.Name = "CurveFitting";
            this.CurveFitting.Size = new System.Drawing.Size(172, 22);
            this.CurveFitting.TabIndex = 0;
            this.CurveFitting.Text = "Allow Curve Fitting";
            this.CurveFitting.UseVisualStyleBackColor = true;
            this.CurveFitting.CheckedChanged += new System.EventHandler(this.CurveFitting_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.formsPlot2);
            this.splitContainer1.Size = new System.Drawing.Size(1153, 626);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 2;
            // 
            // MV_PrimeValues
            // 
            this.MV_PrimeValues.AutoSize = true;
            this.MV_PrimeValues.Location = new System.Drawing.Point(17, 186);
            this.MV_PrimeValues.Name = "MV_PrimeValues";
            this.MV_PrimeValues.Size = new System.Drawing.Size(210, 22);
            this.MV_PrimeValues.TabIndex = 38;
            this.MV_PrimeValues.Text = "Allow MV_Prime Values";
            this.MV_PrimeValues.UseVisualStyleBackColor = true;
            this.MV_PrimeValues.CheckedChanged += new System.EventHandler(this.MV_PrimeValues_CheckedChanged);
            // 
            // ChartView3
            // 
            this.ClientSize = new System.Drawing.Size(1153, 626);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChartView3";
            this.Load += new System.EventHandler(this.ChartView3_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private ScottPlot.FormsPlot formsPlot2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox CurveFitting;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button Statistic;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox SMA_Radious;
        private System.Windows.Forms.CheckBox MV_PrimeValues;
    }
    #endregion
}