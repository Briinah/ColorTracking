namespace ColorTracking
{
    partial class CameraCapture
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
            this.components = new System.ComponentModel.Container();
            this.CamImgBox = new Emgu.CV.UI.ImageBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.outImgBox = new Emgu.CV.UI.ImageBox();
            this.btnBlue = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.Button();
            this.btnYellow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CamImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outImgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CamImgBox
            // 
            this.CamImgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CamImgBox.Location = new System.Drawing.Point(12, 12);
            this.CamImgBox.Name = "CamImgBox";
            this.CamImgBox.Size = new System.Drawing.Size(547, 423);
            this.CamImgBox.TabIndex = 2;
            this.CamImgBox.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(189, 453);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(136, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // outImgBox
            // 
            this.outImgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outImgBox.Location = new System.Drawing.Point(583, 12);
            this.outImgBox.Name = "outImgBox";
            this.outImgBox.Size = new System.Drawing.Size(547, 423);
            this.outImgBox.TabIndex = 4;
            this.outImgBox.TabStop = false;
            // 
            // btnBlue
            // 
            this.btnBlue.Location = new System.Drawing.Point(692, 453);
            this.btnBlue.Name = "btnBlue";
            this.btnBlue.Size = new System.Drawing.Size(75, 23);
            this.btnBlue.TabIndex = 5;
            this.btnBlue.Text = "Blue filter";
            this.btnBlue.UseVisualStyleBackColor = true;
            this.btnBlue.Click += new System.EventHandler(this.btnBlue_Click);
            // 
            // btnGreen
            // 
            this.btnGreen.Location = new System.Drawing.Point(823, 452);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(75, 23);
            this.btnGreen.TabIndex = 6;
            this.btnGreen.Text = "Green filter";
            this.btnGreen.UseVisualStyleBackColor = true;
            this.btnGreen.Click += new System.EventHandler(this.btnGreen_Click);
            // 
            // btnYellow
            // 
            this.btnYellow.Location = new System.Drawing.Point(951, 451);
            this.btnYellow.Name = "btnYellow";
            this.btnYellow.Size = new System.Drawing.Size(75, 23);
            this.btnYellow.TabIndex = 7;
            this.btnYellow.Text = "Yellow filter";
            this.btnYellow.UseVisualStyleBackColor = true;
            this.btnYellow.Click += new System.EventHandler(this.btnYellow_Click);
            // 
            // CameraCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 488);
            this.Controls.Add(this.btnYellow);
            this.Controls.Add(this.btnGreen);
            this.Controls.Add(this.btnBlue);
            this.Controls.Add(this.outImgBox);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.CamImgBox);
            this.Name = "CameraCapture";
            this.Text = "Camera Output";
            ((System.ComponentModel.ISupportInitialize)(this.CamImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outImgBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox CamImgBox;
        private System.Windows.Forms.Button btnStart;
        private Emgu.CV.UI.ImageBox outImgBox;
        private System.Windows.Forms.Button btnBlue;
        private System.Windows.Forms.Button btnGreen;
        private System.Windows.Forms.Button btnYellow;
    }
}

