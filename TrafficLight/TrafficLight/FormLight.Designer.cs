namespace TrafficLight
{
    partial class FormTrafficLight
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLightIP = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelRed = new System.Windows.Forms.Label();
            this.labelGreen = new System.Windows.Forms.Label();
            this.labelAmber = new System.Windows.Forms.Label();
            this.buttonCarArrived = new System.Windows.Forms.Button();
            this.connectServerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.idNumberLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.randomSendCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP number of server";
            // 
            // textBoxLightIP
            // 
            this.textBoxLightIP.Enabled = false;
            this.textBoxLightIP.Location = new System.Drawing.Point(171, 100);
            this.textBoxLightIP.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLightIP.Name = "textBoxLightIP";
            this.textBoxLightIP.Size = new System.Drawing.Size(131, 20);
            this.textBoxLightIP.TabIndex = 10;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(239, 32);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(50, 18);
            this.labelStatus.TabIndex = 9;
            this.labelStatus.Text = "Status";
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.Location = new System.Drawing.Point(31, 329);
            this.listBoxOutput.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(338, 160);
            this.listBoxOutput.TabIndex = 8;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(44, 25);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(176, 31);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Connect to sort of proxy";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelRed
            // 
            this.labelRed.AutoSize = true;
            this.labelRed.BackColor = System.Drawing.Color.Red;
            this.labelRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRed.Location = new System.Drawing.Point(65, 158);
            this.labelRed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRed.Name = "labelRed";
            this.labelRed.Size = new System.Drawing.Size(39, 37);
            this.labelRed.TabIndex = 12;
            this.labelRed.Text = "R";
            this.labelRed.Visible = false;
            // 
            // labelGreen
            // 
            this.labelGreen.AutoSize = true;
            this.labelGreen.BackColor = System.Drawing.Color.Lime;
            this.labelGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGreen.Location = new System.Drawing.Point(65, 248);
            this.labelGreen.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelGreen.Name = "labelGreen";
            this.labelGreen.Size = new System.Drawing.Size(42, 37);
            this.labelGreen.TabIndex = 13;
            this.labelGreen.Text = "G";
            this.labelGreen.Visible = false;
            // 
            // labelAmber
            // 
            this.labelAmber.AutoSize = true;
            this.labelAmber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelAmber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmber.Location = new System.Drawing.Point(65, 203);
            this.labelAmber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAmber.Name = "labelAmber";
            this.labelAmber.Size = new System.Drawing.Size(39, 37);
            this.labelAmber.TabIndex = 14;
            this.labelAmber.Text = "A";
            this.labelAmber.Visible = false;
            // 
            // buttonCarArrived
            // 
            this.buttonCarArrived.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCarArrived.Enabled = false;
            this.buttonCarArrived.Location = new System.Drawing.Point(171, 262);
            this.buttonCarArrived.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCarArrived.Name = "buttonCarArrived";
            this.buttonCarArrived.Size = new System.Drawing.Size(131, 21);
            this.buttonCarArrived.TabIndex = 15;
            this.buttonCarArrived.Text = "Send Car";
            this.buttonCarArrived.UseVisualStyleBackColor = true;
            this.buttonCarArrived.Click += new System.EventHandler(this.buttonCarArrived_Click);
            // 
            // connectServerButton
            // 
            this.connectServerButton.Enabled = false;
            this.connectServerButton.Location = new System.Drawing.Point(171, 134);
            this.connectServerButton.Name = "connectServerButton";
            this.connectServerButton.Size = new System.Drawing.Size(131, 23);
            this.connectServerButton.TabIndex = 16;
            this.connectServerButton.Text = "Connect to Server";
            this.connectServerButton.UseVisualStyleBackColor = true;
            this.connectServerButton.Click += new System.EventHandler(this.connectServerButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(166, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "ID Number: ";
            // 
            // idNumberLabel
            // 
            this.idNumberLabel.AutoSize = true;
            this.idNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.idNumberLabel.Location = new System.Drawing.Point(302, 196);
            this.idNumberLabel.Name = "idNumberLabel";
            this.idNumberLabel.Size = new System.Drawing.Size(0, 25);
            this.idNumberLabel.TabIndex = 19;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // randomSendCheckBox
            // 
            this.randomSendCheckBox.AutoSize = true;
            this.randomSendCheckBox.Enabled = false;
            this.randomSendCheckBox.Location = new System.Drawing.Point(171, 288);
            this.randomSendCheckBox.Name = "randomSendCheckBox";
            this.randomSendCheckBox.Size = new System.Drawing.Size(125, 17);
            this.randomSendCheckBox.TabIndex = 20;
            this.randomSendCheckBox.Text = "Send Cars Randomly";
            this.randomSendCheckBox.UseVisualStyleBackColor = true;
            this.randomSendCheckBox.CheckedChanged += new System.EventHandler(this.randomSendCheckBox_CheckedChanged);
            // 
            // FormTrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 514);
            this.Controls.Add(this.randomSendCheckBox);
            this.Controls.Add(this.idNumberLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.connectServerButton);
            this.Controls.Add(this.buttonCarArrived);
            this.Controls.Add(this.labelAmber);
            this.Controls.Add(this.labelGreen);
            this.Controls.Add(this.labelRed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLightIP);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.buttonConnect);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormTrafficLight";
            this.Text = "Traffic light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTrafficLight_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLightIP;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelRed;
        private System.Windows.Forms.Label labelGreen;
        private System.Windows.Forms.Label labelAmber;
        private System.Windows.Forms.Button buttonCarArrived;
        private System.Windows.Forms.Button connectServerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label idNumberLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.CheckBox randomSendCheckBox;
    }
}

