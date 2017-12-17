namespace Miner.Forms
{
    partial class OptionsForm
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
            this.nudFieldWidth = new System.Windows.Forms.NumericUpDown();
            this.lblFieldWidth = new System.Windows.Forms.Label();
            this.lblFieldHeight = new System.Windows.Forms.Label();
            this.nudFieldHeight = new System.Windows.Forms.NumericUpDown();
            this.lblNumMines = new System.Windows.Forms.Label();
            this.nudNumMines = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumMines)).BeginInit();
            this.SuspendLayout();
            // 
            // nudFieldWidth
            // 
            this.nudFieldWidth.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudFieldWidth.Location = new System.Drawing.Point(12, 25);
            this.nudFieldWidth.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudFieldWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFieldWidth.Name = "nudFieldWidth";
            this.nudFieldWidth.Size = new System.Drawing.Size(73, 20);
            this.nudFieldWidth.TabIndex = 0;
            this.nudFieldWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFieldWidth.ValueChanged += new System.EventHandler(this.nudSize_ValueChanged);
            // 
            // lblFieldWidth
            // 
            this.lblFieldWidth.AutoSize = true;
            this.lblFieldWidth.Location = new System.Drawing.Point(9, 9);
            this.lblFieldWidth.Name = "lblFieldWidth";
            this.lblFieldWidth.Size = new System.Drawing.Size(76, 13);
            this.lblFieldWidth.TabIndex = 1;
            this.lblFieldWidth.Text = "Ширина поля:";
            // 
            // lblFieldHeight
            // 
            this.lblFieldHeight.AutoSize = true;
            this.lblFieldHeight.Location = new System.Drawing.Point(88, 9);
            this.lblFieldHeight.Name = "lblFieldHeight";
            this.lblFieldHeight.Size = new System.Drawing.Size(75, 13);
            this.lblFieldHeight.TabIndex = 2;
            this.lblFieldHeight.Text = "Высота поля:";
            // 
            // nudFieldHeight
            // 
            this.nudFieldHeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudFieldHeight.Location = new System.Drawing.Point(91, 25);
            this.nudFieldHeight.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudFieldHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFieldHeight.Name = "nudFieldHeight";
            this.nudFieldHeight.Size = new System.Drawing.Size(72, 20);
            this.nudFieldHeight.TabIndex = 3;
            this.nudFieldHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFieldHeight.ValueChanged += new System.EventHandler(this.nudSize_ValueChanged);
            // 
            // lblNumMines
            // 
            this.lblNumMines.AutoSize = true;
            this.lblNumMines.Location = new System.Drawing.Point(166, 9);
            this.lblNumMines.Name = "lblNumMines";
            this.lblNumMines.Size = new System.Drawing.Size(92, 13);
            this.lblNumMines.TabIndex = 4;
            this.lblNumMines.Text = "Количество мин:";
            // 
            // nudNumMines
            // 
            this.nudNumMines.Location = new System.Drawing.Point(169, 25);
            this.nudNumMines.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumMines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumMines.Name = "nudNumMines";
            this.nudNumMines.Size = new System.Drawing.Size(72, 20);
            this.nudNumMines.TabIndex = 5;
            this.nudNumMines.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(91, 63);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 102);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.nudNumMines);
            this.Controls.Add(this.lblNumMines);
            this.Controls.Add(this.nudFieldHeight);
            this.Controls.Add(this.lblFieldHeight);
            this.Controls.Add(this.lblFieldWidth);
            this.Controls.Add(this.nudFieldWidth);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFieldHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumMines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudFieldWidth;
        private System.Windows.Forms.Label lblFieldWidth;
        private System.Windows.Forms.Label lblFieldHeight;
        private System.Windows.Forms.NumericUpDown nudFieldHeight;
        private System.Windows.Forms.Label lblNumMines;
        private System.Windows.Forms.NumericUpDown nudNumMines;
        private System.Windows.Forms.Button btnOK;
    }
}