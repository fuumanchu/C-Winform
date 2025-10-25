namespace Do_an_tot_Nghiep
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.UpLoadButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSendOtp = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelStatusotp = new System.Windows.Forms.Label();
            this.textBoxotp = new System.Windows.Forms.TextBox();
            this.textBoxRece = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.buttonHandsk = new System.Windows.Forms.Button();
            this.groupBoxStm = new System.Windows.Forms.GroupBox();
            this.labelBootloader = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.labelTimeCountDown = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelOTPSTA = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timerOtpCountdown = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxStm.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // UpLoadButton
            // 
            this.UpLoadButton.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.UpLoadButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.UpLoadButton.Location = new System.Drawing.Point(28, 44);
            this.UpLoadButton.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.UpLoadButton.Name = "UpLoadButton";
            this.UpLoadButton.Size = new System.Drawing.Size(284, 49);
            this.UpLoadButton.TabIndex = 0;
            this.UpLoadButton.Text = "UpLoad";
            this.UpLoadButton.UseVisualStyleBackColor = false;
            this.UpLoadButton.Click += new System.EventHandler(this.UpLoadButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.buttonClose);
            this.groupBox1.Controls.Add(this.buttonOpen);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Location = new System.Drawing.Point(17, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox1.Size = new System.Drawing.Size(335, 185);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "status";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(28, 106);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(284, 38);
            this.progressBar1.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonClose.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonClose.Location = new System.Drawing.Point(183, 35);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(129, 48);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "CLOSE";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonOpen.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.buttonOpen.Location = new System.Drawing.Point(28, 35);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(145, 48);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "OPEN";
            this.buttonOpen.UseVisualStyleBackColor = false;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonSendOtp
            // 
            this.buttonSendOtp.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonSendOtp.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonSendOtp.Location = new System.Drawing.Point(214, 40);
            this.buttonSendOtp.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonSendOtp.Name = "buttonSendOtp";
            this.buttonSendOtp.Size = new System.Drawing.Size(98, 59);
            this.buttonSendOtp.TabIndex = 2;
            this.buttonSendOtp.Text = "Send OTP";
            this.buttonSendOtp.UseVisualStyleBackColor = false;
            this.buttonSendOtp.Click += new System.EventHandler(this.buttonSendOtp_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar2);
            this.groupBox2.Controls.Add(this.UpLoadButton);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Location = new System.Drawing.Point(17, 390);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox2.Size = new System.Drawing.Size(335, 185);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "firmware";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(28, 121);
            this.progressBar2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(284, 38);
            this.progressBar2.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.labelStatusotp);
            this.groupBox3.Controls.Add(this.textBoxotp);
            this.groupBox3.Controls.Add(this.buttonSendOtp);
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox3.Location = new System.Drawing.Point(17, 201);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox3.Size = new System.Drawing.Size(335, 185);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "OTP code";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(28, 118);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(284, 45);
            this.textBox1.TabIndex = 5;
            // 
            // labelStatusotp
            // 
            this.labelStatusotp.AutoSize = true;
            this.labelStatusotp.Location = new System.Drawing.Point(63, 121);
            this.labelStatusotp.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelStatusotp.Name = "labelStatusotp";
            this.labelStatusotp.Size = new System.Drawing.Size(0, 37);
            this.labelStatusotp.TabIndex = 4;
            // 
            // textBoxotp
            // 
            this.textBoxotp.Location = new System.Drawing.Point(28, 46);
            this.textBoxotp.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.textBoxotp.Name = "textBoxotp";
            this.textBoxotp.Size = new System.Drawing.Size(184, 45);
            this.textBoxotp.TabIndex = 3;
            this.textBoxotp.TextChanged += new System.EventHandler(this.textBoxotp_TextChanged);
            // 
            // textBoxRece
            // 
            this.textBoxRece.Location = new System.Drawing.Point(382, 358);
            this.textBoxRece.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.textBoxRece.Multiline = true;
            this.textBoxRece.Name = "textBoxRece";
            this.textBoxRece.Size = new System.Drawing.Size(743, 406);
            this.textBoxRece.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.progressBar3);
            this.groupBox4.Controls.Add(this.buttonHandsk);
            this.groupBox4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox4.Location = new System.Drawing.Point(17, 579);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox4.Size = new System.Drawing.Size(335, 185);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "EnterUpLoad";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(28, 121);
            this.progressBar3.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(284, 38);
            this.progressBar3.TabIndex = 3;
            // 
            // buttonHandsk
            // 
            this.buttonHandsk.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.buttonHandsk.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonHandsk.Location = new System.Drawing.Point(28, 44);
            this.buttonHandsk.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonHandsk.Name = "buttonHandsk";
            this.buttonHandsk.Size = new System.Drawing.Size(284, 49);
            this.buttonHandsk.TabIndex = 0;
            this.buttonHandsk.Text = "Enter";
            this.buttonHandsk.UseVisualStyleBackColor = false;
            this.buttonHandsk.Click += new System.EventHandler(this.buttonHandsk_Click);
            // 
            // groupBoxStm
            // 
            this.groupBoxStm.Controls.Add(this.labelBootloader);
            this.groupBoxStm.Controls.Add(this.label1);
            this.groupBoxStm.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxStm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBoxStm.Location = new System.Drawing.Point(382, 30);
            this.groupBoxStm.Name = "groupBoxStm";
            this.groupBoxStm.Size = new System.Drawing.Size(743, 126);
            this.groupBoxStm.TabIndex = 7;
            this.groupBoxStm.TabStop = false;
            this.groupBoxStm.Text = "Stm32Status";
            // 
            // labelBootloader
            // 
            this.labelBootloader.AutoSize = true;
            this.labelBootloader.Location = new System.Drawing.Point(310, 56);
            this.labelBootloader.Name = "labelBootloader";
            this.labelBootloader.Size = new System.Drawing.Size(341, 37);
            this.labelBootloader.TabIndex = 1;
            this.labelBootloader.Text = "Permery bootLoader";
            this.labelBootloader.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(82, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(376, 285);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 32);
            this.label3.TabIndex = 8;
            this.label3.Text = "Console:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.labelTimeCountDown);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.labelOTPSTA);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox5.Location = new System.Drawing.Point(382, 162);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(743, 120);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "OTPStatus";
            // 
            // labelTimeCountDown
            // 
            this.labelTimeCountDown.AutoSize = true;
            this.labelTimeCountDown.Location = new System.Drawing.Point(596, 56);
            this.labelTimeCountDown.Name = "labelTimeCountDown";
            this.labelTimeCountDown.Size = new System.Drawing.Size(71, 37);
            this.labelTimeCountDown.TabIndex = 3;
            this.labelTimeCountDown.Text = "0 s";
            this.labelTimeCountDown.Click += new System.EventHandler(this.labelTimeCountDown_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(375, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "CountDown :";
            // 
            // labelOTPSTA
            // 
            this.labelOTPSTA.AutoSize = true;
            this.labelOTPSTA.Location = new System.Drawing.Point(251, 56);
            this.labelOTPSTA.Name = "labelOTPSTA";
            this.labelOTPSTA.Size = new System.Drawing.Size(107, 37);
            this.labelOTPSTA.TabIndex = 1;
            this.labelOTPSTA.Text = "False";
            this.labelOTPSTA.Click += new System.EventHandler(this.labelOTPSTA_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(82, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 37);
            this.label4.TabIndex = 0;
            this.label4.Text = "Status : ";
            // 
            // timerOtpCountdown
            // 
            this.timerOtpCountdown.Interval = 10;
            this.timerOtpCountdown.Tick += new System.EventHandler(this.timerOtpCountdown_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 821);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBoxStm);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.textBoxRece);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Form1";
            this.Text = "FirmwareUpdate";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBoxStm.ResumeLayout(false);
            this.groupBoxStm.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button UpLoadButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSendOtp;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxRece;
        private System.Windows.Forms.TextBox textBoxotp;
        private System.Windows.Forms.Label labelStatusotp;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Button buttonHandsk;
        private System.Windows.Forms.GroupBox groupBoxStm;
        private System.Windows.Forms.Label labelBootloader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label labelOTPSTA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timerOtpCountdown;
        private System.Windows.Forms.Label labelTimeCountDown;
        private System.Windows.Forms.Label label2;
    }
}

