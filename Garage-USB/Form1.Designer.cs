
namespace Garage_USB
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_tip_powerup = new System.Windows.Forms.Button();
            this.btn_tip_download = new System.Windows.Forms.Button();
            this.btn_tip_restart = new System.Windows.Forms.Button();
            this.btn_tip_calibrate = new System.Windows.Forms.Button();
            this.btn_tip_press = new System.Windows.Forms.Button();
            this.btn_tip_result = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pb_process = new System.Windows.Forms.ProgressBar();
            this.img_preview = new System.Windows.Forms.PictureBox();
            this.btn_live = new System.Windows.Forms.Button();
            this.btn_result = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_com = new System.Windows.Forms.Button();
            this.tb_com = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_ns = new System.Windows.Forms.CheckBox();
            this.cb_btn = new System.Windows.Forms.CheckBox();
            this.cb_cv = new System.Windows.Forms.CheckBox();
            this.cb_mp = new System.Windows.Forms.CheckBox();
            this.btn_get_cv = new System.Windows.Forms.Button();
            this.btn_led_0 = new System.Windows.Forms.Button();
            this.btn_led_1 = new System.Windows.Forms.Button();
            this.btn_s0_0 = new System.Windows.Forms.Button();
            this.btn_s0_1 = new System.Windows.Forms.Button();
            this.btn_usb0 = new System.Windows.Forms.Button();
            this.btn_usb1 = new System.Windows.Forms.Button();
            this.btn_poweroff = new System.Windows.Forms.Button();
            this.btn_poweron = new System.Windows.Forms.Button();
            this.cb_calibrate = new System.Windows.Forms.CheckBox();
            this.tb_gain = new System.Windows.Forms.TextBox();
            this.btn_stop = new System.Windows.Forms.Button();
            this.cb_enhance = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_project = new System.Windows.Forms.Label();
            this.lb_product = new System.Windows.Forms.Label();
            this.lb_sn = new System.Windows.Forms.Label();
            this.lb_parameter = new System.Windows.Forms.Label();
            this.lb_graylevel = new System.Windows.Forms.Label();
            this.lb_rv = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lb_current = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lb_voltage = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_noise = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lb_version = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lb_bin = new System.Windows.Forms.Label();
            this.btn_tip_blink = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.lb_station = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lb_pf = new System.Windows.Forms.Label();
            this.tb_read = new System.Windows.Forms.TextBox();
            this.btn_read = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmb_project = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btn_reload = new System.Windows.Forms.Button();
            this.cmb_channel = new System.Windows.Forms.ComboBox();
            this.lb_err_message = new System.Windows.Forms.Label();
            this.lb_station_type = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_preview)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Font = new System.Drawing.Font("Arial Unicode MS", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_start.Location = new System.Drawing.Point(12, 583);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(984, 66);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "點擊開始 Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_tip_powerup
            // 
            this.btn_tip_powerup.BackColor = System.Drawing.Color.White;
            this.btn_tip_powerup.Enabled = false;
            this.btn_tip_powerup.FlatAppearance.BorderSize = 0;
            this.btn_tip_powerup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_powerup.Location = new System.Drawing.Point(10, 30);
            this.btn_tip_powerup.Name = "btn_tip_powerup";
            this.btn_tip_powerup.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_powerup.TabIndex = 1;
            this.btn_tip_powerup.Text = "POWER_UP >>";
            this.btn_tip_powerup.UseVisualStyleBackColor = false;
            // 
            // btn_tip_download
            // 
            this.btn_tip_download.BackColor = System.Drawing.Color.White;
            this.btn_tip_download.Enabled = false;
            this.btn_tip_download.FlatAppearance.BorderSize = 0;
            this.btn_tip_download.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_download.Location = new System.Drawing.Point(99, 30);
            this.btn_tip_download.Name = "btn_tip_download";
            this.btn_tip_download.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_download.TabIndex = 2;
            this.btn_tip_download.Text = "DOWNLOAD >>";
            this.btn_tip_download.UseVisualStyleBackColor = false;
            // 
            // btn_tip_restart
            // 
            this.btn_tip_restart.BackColor = System.Drawing.Color.White;
            this.btn_tip_restart.Enabled = false;
            this.btn_tip_restart.FlatAppearance.BorderSize = 0;
            this.btn_tip_restart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_restart.Location = new System.Drawing.Point(188, 30);
            this.btn_tip_restart.Name = "btn_tip_restart";
            this.btn_tip_restart.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_restart.TabIndex = 3;
            this.btn_tip_restart.Text = "RESTART >>";
            this.btn_tip_restart.UseVisualStyleBackColor = false;
            // 
            // btn_tip_calibrate
            // 
            this.btn_tip_calibrate.BackColor = System.Drawing.Color.White;
            this.btn_tip_calibrate.Enabled = false;
            this.btn_tip_calibrate.FlatAppearance.BorderSize = 0;
            this.btn_tip_calibrate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_calibrate.Location = new System.Drawing.Point(276, 30);
            this.btn_tip_calibrate.Name = "btn_tip_calibrate";
            this.btn_tip_calibrate.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_calibrate.TabIndex = 4;
            this.btn_tip_calibrate.Text = "CALIBRATE >>";
            this.btn_tip_calibrate.UseVisualStyleBackColor = false;
            // 
            // btn_tip_press
            // 
            this.btn_tip_press.BackColor = System.Drawing.Color.White;
            this.btn_tip_press.Enabled = false;
            this.btn_tip_press.FlatAppearance.BorderSize = 0;
            this.btn_tip_press.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_press.Location = new System.Drawing.Point(365, 30);
            this.btn_tip_press.Name = "btn_tip_press";
            this.btn_tip_press.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_press.TabIndex = 5;
            this.btn_tip_press.Text = "PRESS >>";
            this.btn_tip_press.UseVisualStyleBackColor = false;
            // 
            // btn_tip_result
            // 
            this.btn_tip_result.BackColor = System.Drawing.Color.White;
            this.btn_tip_result.Enabled = false;
            this.btn_tip_result.FlatAppearance.BorderSize = 0;
            this.btn_tip_result.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_tip_result.Location = new System.Drawing.Point(451, 30);
            this.btn_tip_result.Name = "btn_tip_result";
            this.btn_tip_result.Size = new System.Drawing.Size(90, 22);
            this.btn_tip_result.TabIndex = 6;
            this.btn_tip_result.Text = "RESULT";
            this.btn_tip_result.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pb_process);
            this.groupBox1.Controls.Add(this.btn_tip_result);
            this.groupBox1.Controls.Add(this.btn_tip_powerup);
            this.groupBox1.Controls.Add(this.btn_tip_download);
            this.groupBox1.Controls.Add(this.btn_tip_restart);
            this.groupBox1.Controls.Add(this.btn_tip_calibrate);
            this.groupBox1.Controls.Add(this.btn_tip_press);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 89);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process";
            // 
            // pb_process
            // 
            this.pb_process.BackColor = System.Drawing.SystemColors.Control;
            this.pb_process.ForeColor = System.Drawing.Color.PaleGreen;
            this.pb_process.Location = new System.Drawing.Point(10, 52);
            this.pb_process.Name = "pb_process";
            this.pb_process.Size = new System.Drawing.Size(531, 12);
            this.pb_process.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_process.TabIndex = 56;
            // 
            // img_preview
            // 
            this.img_preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.img_preview.Image = ((System.Drawing.Image)(resources.GetObject("img_preview.Image")));
            this.img_preview.Location = new System.Drawing.Point(100, 100);
            this.img_preview.Name = "img_preview";
            this.img_preview.Size = new System.Drawing.Size(288, 132);
            this.img_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_preview.TabIndex = 8;
            this.img_preview.TabStop = false;
            // 
            // btn_live
            // 
            this.btn_live.Location = new System.Drawing.Point(205, 359);
            this.btn_live.Name = "btn_live";
            this.btn_live.Size = new System.Drawing.Size(41, 23);
            this.btn_live.TabIndex = 9;
            this.btn_live.Text = "Live";
            this.btn_live.UseVisualStyleBackColor = true;
            this.btn_live.Click += new System.EventHandler(this.btn_live_Click);
            // 
            // btn_result
            // 
            this.btn_result.BackColor = System.Drawing.Color.White;
            this.btn_result.Enabled = false;
            this.btn_result.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_result.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_result.Location = new System.Drawing.Point(12, 278);
            this.btn_result.Name = "btn_result";
            this.btn_result.Size = new System.Drawing.Size(453, 56);
            this.btn_result.TabIndex = 18;
            this.btn_result.Text = "等待開始";
            this.btn_result.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(16, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 18);
            this.label7.TabIndex = 22;
            this.label7.Text = "PORT";
            // 
            // btn_com
            // 
            this.btn_com.Location = new System.Drawing.Point(121, 34);
            this.btn_com.Name = "btn_com";
            this.btn_com.Size = new System.Drawing.Size(75, 23);
            this.btn_com.TabIndex = 23;
            this.btn_com.Text = "SET";
            this.btn_com.UseVisualStyleBackColor = true;
            // 
            // tb_com
            // 
            this.tb_com.Location = new System.Drawing.Point(63, 35);
            this.tb_com.Name = "tb_com";
            this.tb_com.Size = new System.Drawing.Size(52, 21);
            this.tb_com.TabIndex = 24;
            this.tb_com.Text = "COM21";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_ns);
            this.groupBox3.Controls.Add(this.cb_btn);
            this.groupBox3.Controls.Add(this.cb_cv);
            this.groupBox3.Controls.Add(this.cb_mp);
            this.groupBox3.Controls.Add(this.btn_get_cv);
            this.groupBox3.Controls.Add(this.btn_led_0);
            this.groupBox3.Controls.Add(this.btn_led_1);
            this.groupBox3.Controls.Add(this.btn_s0_0);
            this.groupBox3.Controls.Add(this.btn_s0_1);
            this.groupBox3.Controls.Add(this.btn_usb0);
            this.groupBox3.Controls.Add(this.btn_usb1);
            this.groupBox3.Controls.Add(this.btn_poweroff);
            this.groupBox3.Controls.Add(this.btn_poweron);
            this.groupBox3.Controls.Add(this.cb_calibrate);
            this.groupBox3.Controls.Add(this.tb_com);
            this.groupBox3.Controls.Add(this.btn_com);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(13, 499);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(983, 78);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "setting";
            // 
            // cb_ns
            // 
            this.cb_ns.AutoSize = true;
            this.cb_ns.Checked = true;
            this.cb_ns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_ns.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_ns.Location = new System.Drawing.Point(759, 48);
            this.cb_ns.Name = "cb_ns";
            this.cb_ns.Size = new System.Drawing.Size(42, 19);
            this.cb_ns.TabIndex = 54;
            this.cb_ns.Text = "NS";
            this.cb_ns.UseVisualStyleBackColor = true;
            // 
            // cb_btn
            // 
            this.cb_btn.AutoSize = true;
            this.cb_btn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_btn.Location = new System.Drawing.Point(759, 23);
            this.cb_btn.Name = "cb_btn";
            this.cb_btn.Size = new System.Drawing.Size(42, 19);
            this.cb_btn.TabIndex = 53;
            this.cb_btn.Text = "BN";
            this.cb_btn.UseVisualStyleBackColor = true;
            // 
            // cb_cv
            // 
            this.cb_cv.AutoSize = true;
            this.cb_cv.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_cv.Location = new System.Drawing.Point(709, 48);
            this.cb_cv.Name = "cb_cv";
            this.cb_cv.Size = new System.Drawing.Size(42, 19);
            this.cb_cv.TabIndex = 52;
            this.cb_cv.Text = "CV";
            this.cb_cv.UseVisualStyleBackColor = true;
            // 
            // cb_mp
            // 
            this.cb_mp.AutoSize = true;
            this.cb_mp.Checked = true;
            this.cb_mp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_mp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_mp.Location = new System.Drawing.Point(709, 23);
            this.cb_mp.Name = "cb_mp";
            this.cb_mp.Size = new System.Drawing.Size(44, 19);
            this.cb_mp.TabIndex = 51;
            this.cb_mp.Text = "MP";
            this.cb_mp.UseVisualStyleBackColor = true;
            this.cb_mp.CheckedChanged += new System.EventHandler(this.cb_mp_CheckedChanged);
            // 
            // btn_get_cv
            // 
            this.btn_get_cv.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_get_cv.Location = new System.Drawing.Point(628, 20);
            this.btn_get_cv.Name = "btn_get_cv";
            this.btn_get_cv.Size = new System.Drawing.Size(75, 52);
            this.btn_get_cv.TabIndex = 49;
            this.btn_get_cv.Text = "get_CV";
            this.btn_get_cv.UseVisualStyleBackColor = true;
            this.btn_get_cv.Click += new System.EventHandler(this.btn_get_cv_Click);
            // 
            // btn_led_0
            // 
            this.btn_led_0.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_led_0.Location = new System.Drawing.Point(539, 49);
            this.btn_led_0.Name = "btn_led_0";
            this.btn_led_0.Size = new System.Drawing.Size(75, 23);
            this.btn_led_0.TabIndex = 48;
            this.btn_led_0.Text = "LED_0";
            this.btn_led_0.UseVisualStyleBackColor = true;
            this.btn_led_0.Click += new System.EventHandler(this.btn_led_0_Click);
            // 
            // btn_led_1
            // 
            this.btn_led_1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_led_1.Location = new System.Drawing.Point(539, 20);
            this.btn_led_1.Name = "btn_led_1";
            this.btn_led_1.Size = new System.Drawing.Size(75, 23);
            this.btn_led_1.TabIndex = 47;
            this.btn_led_1.Text = "LED_1";
            this.btn_led_1.UseVisualStyleBackColor = true;
            this.btn_led_1.Click += new System.EventHandler(this.btn_led_1_Click);
            // 
            // btn_s0_0
            // 
            this.btn_s0_0.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_s0_0.Location = new System.Drawing.Point(458, 49);
            this.btn_s0_0.Name = "btn_s0_0";
            this.btn_s0_0.Size = new System.Drawing.Size(75, 23);
            this.btn_s0_0.TabIndex = 46;
            this.btn_s0_0.Text = "S0_0";
            this.btn_s0_0.UseVisualStyleBackColor = true;
            this.btn_s0_0.Click += new System.EventHandler(this.btn_s0_0_Click);
            // 
            // btn_s0_1
            // 
            this.btn_s0_1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_s0_1.Location = new System.Drawing.Point(458, 20);
            this.btn_s0_1.Name = "btn_s0_1";
            this.btn_s0_1.Size = new System.Drawing.Size(75, 23);
            this.btn_s0_1.TabIndex = 45;
            this.btn_s0_1.Text = "S0_1";
            this.btn_s0_1.UseVisualStyleBackColor = true;
            this.btn_s0_1.Click += new System.EventHandler(this.btn_s0_1_Click);
            // 
            // btn_usb0
            // 
            this.btn_usb0.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_usb0.Location = new System.Drawing.Point(377, 49);
            this.btn_usb0.Name = "btn_usb0";
            this.btn_usb0.Size = new System.Drawing.Size(75, 23);
            this.btn_usb0.TabIndex = 44;
            this.btn_usb0.Text = "USB0";
            this.btn_usb0.UseVisualStyleBackColor = true;
            this.btn_usb0.Click += new System.EventHandler(this.btn_usb0_Click);
            // 
            // btn_usb1
            // 
            this.btn_usb1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_usb1.Location = new System.Drawing.Point(377, 20);
            this.btn_usb1.Name = "btn_usb1";
            this.btn_usb1.Size = new System.Drawing.Size(75, 23);
            this.btn_usb1.TabIndex = 43;
            this.btn_usb1.Text = "USB1";
            this.btn_usb1.UseVisualStyleBackColor = true;
            this.btn_usb1.Click += new System.EventHandler(this.btn_usb1_Click);
            // 
            // btn_poweroff
            // 
            this.btn_poweroff.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_poweroff.Location = new System.Drawing.Point(295, 49);
            this.btn_poweroff.Name = "btn_poweroff";
            this.btn_poweroff.Size = new System.Drawing.Size(75, 23);
            this.btn_poweroff.TabIndex = 41;
            this.btn_poweroff.Text = "PowerOff";
            this.btn_poweroff.UseVisualStyleBackColor = true;
            this.btn_poweroff.Click += new System.EventHandler(this.btn_poweroff_Click);
            // 
            // btn_poweron
            // 
            this.btn_poweron.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_poweron.Location = new System.Drawing.Point(295, 20);
            this.btn_poweron.Name = "btn_poweron";
            this.btn_poweron.Size = new System.Drawing.Size(75, 23);
            this.btn_poweron.TabIndex = 40;
            this.btn_poweron.Text = "PowerOn";
            this.btn_poweron.UseVisualStyleBackColor = true;
            this.btn_poweron.Click += new System.EventHandler(this.btn_poweron_Click);
            // 
            // cb_calibrate
            // 
            this.cb_calibrate.AutoSize = true;
            this.cb_calibrate.Checked = true;
            this.cb_calibrate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_calibrate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cb_calibrate.Location = new System.Drawing.Point(202, 35);
            this.cb_calibrate.Name = "cb_calibrate";
            this.cb_calibrate.Size = new System.Drawing.Size(87, 22);
            this.cb_calibrate.TabIndex = 39;
            this.cb_calibrate.Text = "calibrate";
            this.cb_calibrate.UseVisualStyleBackColor = true;
            // 
            // tb_gain
            // 
            this.tb_gain.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tb_gain.Location = new System.Drawing.Point(143, 360);
            this.tb_gain.Name = "tb_gain";
            this.tb_gain.Size = new System.Drawing.Size(41, 21);
            this.tb_gain.TabIndex = 49;
            this.tb_gain.Text = "24";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(262, 359);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(40, 23);
            this.btn_stop.TabIndex = 29;
            this.btn_stop.Text = "stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // cb_enhance
            // 
            this.cb_enhance.AutoSize = true;
            this.cb_enhance.Location = new System.Drawing.Point(76, 361);
            this.cb_enhance.Name = "cb_enhance";
            this.cb_enhance.Size = new System.Drawing.Size(61, 20);
            this.cb_enhance.TabIndex = 31;
            this.cb_enhance.Text = "enhance";
            this.cb_enhance.UseVisualStyleBackColor = true;
            this.cb_enhance.CheckedChanged += new System.EventHandler(this.cb_enhance_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(6, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Parameter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "SN";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(5, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "Gray Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "Product";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(6, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 18);
            this.label6.TabIndex = 15;
            this.label6.Text = "RV";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 10;
            this.label1.Text = "Project";
            // 
            // lb_project
            // 
            this.lb_project.AutoSize = true;
            this.lb_project.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_project.Location = new System.Drawing.Point(90, 25);
            this.lb_project.Name = "lb_project";
            this.lb_project.Size = new System.Drawing.Size(13, 18);
            this.lb_project.TabIndex = 16;
            this.lb_project.Text = "-";
            // 
            // lb_product
            // 
            this.lb_product.AutoSize = true;
            this.lb_product.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_product.Location = new System.Drawing.Point(90, 49);
            this.lb_product.Name = "lb_product";
            this.lb_product.Size = new System.Drawing.Size(13, 18);
            this.lb_product.TabIndex = 17;
            this.lb_product.Text = "-";
            // 
            // lb_sn
            // 
            this.lb_sn.AutoSize = true;
            this.lb_sn.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_sn.Location = new System.Drawing.Point(90, 71);
            this.lb_sn.Name = "lb_sn";
            this.lb_sn.Size = new System.Drawing.Size(12, 16);
            this.lb_sn.TabIndex = 18;
            this.lb_sn.Text = "-";
            // 
            // lb_parameter
            // 
            this.lb_parameter.AutoSize = true;
            this.lb_parameter.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_parameter.Location = new System.Drawing.Point(90, 96);
            this.lb_parameter.Name = "lb_parameter";
            this.lb_parameter.Size = new System.Drawing.Size(12, 16);
            this.lb_parameter.TabIndex = 19;
            this.lb_parameter.Text = "-";
            // 
            // lb_graylevel
            // 
            this.lb_graylevel.AutoSize = true;
            this.lb_graylevel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_graylevel.Location = new System.Drawing.Point(90, 117);
            this.lb_graylevel.Name = "lb_graylevel";
            this.lb_graylevel.Size = new System.Drawing.Size(22, 18);
            this.lb_graylevel.TabIndex = 20;
            this.lb_graylevel.Text = "-1";
            // 
            // lb_rv
            // 
            this.lb_rv.AutoSize = true;
            this.lb_rv.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_rv.Location = new System.Drawing.Point(90, 140);
            this.lb_rv.Name = "lb_rv";
            this.lb_rv.Size = new System.Drawing.Size(22, 18);
            this.lb_rv.TabIndex = 21;
            this.lb_rv.Text = "-1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(321, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 18);
            this.label9.TabIndex = 22;
            this.label9.Text = "Current";
            // 
            // lb_current
            // 
            this.lb_current.AutoSize = true;
            this.lb_current.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_current.Location = new System.Drawing.Point(384, 119);
            this.lb_current.Name = "lb_current";
            this.lb_current.Size = new System.Drawing.Size(22, 18);
            this.lb_current.TabIndex = 23;
            this.lb_current.Text = "-1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(321, 142);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 18);
            this.label10.TabIndex = 24;
            this.label10.Text = "Voltage";
            // 
            // lb_voltage
            // 
            this.lb_voltage.AutoSize = true;
            this.lb_voltage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_voltage.Location = new System.Drawing.Point(384, 142);
            this.lb_voltage.Name = "lb_voltage";
            this.lb_voltage.Size = new System.Drawing.Size(22, 18);
            this.lb_voltage.TabIndex = 25;
            this.lb_voltage.Text = "-1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_noise);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lb_version);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.lb_voltage);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.lb_current);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lb_rv);
            this.groupBox2.Controls.Add(this.lb_graylevel);
            this.groupBox2.Controls.Add(this.lb_parameter);
            this.groupBox2.Controls.Add(this.lb_sn);
            this.groupBox2.Controls.Add(this.lb_product);
            this.groupBox2.Controls.Add(this.lb_project);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 171);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Info";
            // 
            // lb_noise
            // 
            this.lb_noise.AutoSize = true;
            this.lb_noise.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_noise.Location = new System.Drawing.Point(384, 96);
            this.lb_noise.Name = "lb_noise";
            this.lb_noise.Size = new System.Drawing.Size(22, 18);
            this.lb_noise.TabIndex = 29;
            this.lb_noise.Text = "-1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(320, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 18);
            this.label8.TabIndex = 28;
            this.label8.Text = "Noise";
            // 
            // lb_version
            // 
            this.lb_version.AutoSize = true;
            this.lb_version.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_version.Location = new System.Drawing.Point(309, 25);
            this.lb_version.Name = "lb_version";
            this.lb_version.Size = new System.Drawing.Size(12, 16);
            this.lb_version.TabIndex = 27;
            this.lb_version.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(246, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 18);
            this.label12.TabIndex = 26;
            this.label12.Text = "Version";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(350, 355);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 36);
            this.label11.TabIndex = 26;
            this.label11.Text = "BIN";
            // 
            // lb_bin
            // 
            this.lb_bin.AutoSize = true;
            this.lb_bin.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_bin.Location = new System.Drawing.Point(424, 355);
            this.lb_bin.Name = "lb_bin";
            this.lb_bin.Size = new System.Drawing.Size(33, 36);
            this.lb_bin.TabIndex = 28;
            this.lb_bin.Text = "0";
            // 
            // btn_tip_blink
            // 
            this.btn_tip_blink.BackColor = System.Drawing.Color.White;
            this.btn_tip_blink.Enabled = false;
            this.btn_tip_blink.FlatAppearance.BorderSize = 0;
            this.btn_tip_blink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tip_blink.Location = new System.Drawing.Point(74, 14);
            this.btn_tip_blink.Name = "btn_tip_blink";
            this.btn_tip_blink.Size = new System.Drawing.Size(341, 335);
            this.btn_tip_blink.TabIndex = 32;
            this.btn_tip_blink.Text = "button1";
            this.btn_tip_blink.UseVisualStyleBackColor = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(876, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 18);
            this.label13.TabIndex = 33;
            this.label13.Text = "Station";
            // 
            // lb_station
            // 
            this.lb_station.AutoSize = true;
            this.lb_station.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_station.Location = new System.Drawing.Point(939, 9);
            this.lb_station.Name = "lb_station";
            this.lb_station.Size = new System.Drawing.Size(13, 18);
            this.lb_station.TabIndex = 34;
            this.lb_station.Text = "-";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(876, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 18);
            this.label15.TabIndex = 37;
            this.label15.Text = "OK/NG";
            // 
            // lb_pf
            // 
            this.lb_pf.AutoSize = true;
            this.lb_pf.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_pf.Location = new System.Drawing.Point(938, 29);
            this.lb_pf.Name = "lb_pf";
            this.lb_pf.Size = new System.Drawing.Size(13, 18);
            this.lb_pf.TabIndex = 38;
            this.lb_pf.Text = "-";
            // 
            // tb_read
            // 
            this.tb_read.Location = new System.Drawing.Point(17, 430);
            this.tb_read.Multiline = true;
            this.tb_read.Name = "tb_read";
            this.tb_read.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_read.Size = new System.Drawing.Size(316, 63);
            this.tb_read.TabIndex = 43;
            // 
            // btn_read
            // 
            this.btn_read.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_read.Location = new System.Drawing.Point(350, 470);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 48;
            this.btn_read.Text = "Read";
            this.btn_read.UseVisualStyleBackColor = true;
            // 
            // btn_clear
            // 
            this.btn_clear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_clear.Location = new System.Drawing.Point(350, 434);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 50;
            this.btn_clear.Text = "Clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.img_preview);
            this.groupBox4.Controls.Add(this.tb_gain);
            this.groupBox4.Controls.Add(this.cb_enhance);
            this.groupBox4.Controls.Add(this.btn_live);
            this.groupBox4.Controls.Add(this.btn_stop);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.btn_tip_blink);
            this.groupBox4.Location = new System.Drawing.Point(506, 99);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(478, 394);
            this.groupBox4.TabIndex = 51;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Image";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(90, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(309, 156);
            this.button1.TabIndex = 50;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Silver;
            this.button2.Enabled = false;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(85, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(320, 320);
            this.button2.TabIndex = 34;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // cmb_project
            // 
            this.cmb_project.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_project.FormattingEnabled = true;
            this.cmb_project.Location = new System.Drawing.Point(606, 62);
            this.cmb_project.Name = "cmb_project";
            this.cmb_project.Size = new System.Drawing.Size(127, 22);
            this.cmb_project.TabIndex = 52;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(602, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 18);
            this.label16.TabIndex = 53;
            this.label16.Text = "GN Version";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label18.Location = new System.Drawing.Point(703, 29);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(13, 18);
            this.label18.TabIndex = 54;
            this.label18.Text = "-";
            // 
            // btn_reload
            // 
            this.btn_reload.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_reload.Location = new System.Drawing.Point(877, 60);
            this.btn_reload.Name = "btn_reload";
            this.btn_reload.Size = new System.Drawing.Size(75, 23);
            this.btn_reload.TabIndex = 55;
            this.btn_reload.Text = "RELOAD";
            this.btn_reload.UseVisualStyleBackColor = true;
            this.btn_reload.Click += new System.EventHandler(this.btn_reload_Click);
            // 
            // cmb_channel
            // 
            this.cmb_channel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmb_channel.FormattingEnabled = true;
            this.cmb_channel.Location = new System.Drawing.Point(757, 62);
            this.cmb_channel.Name = "cmb_channel";
            this.cmb_channel.Size = new System.Drawing.Size(75, 22);
            this.cmb_channel.TabIndex = 56;
            this.cmb_channel.SelectedIndexChanged += new System.EventHandler(this.cmb_channel_SelectedIndexChanged);
            // 
            // lb_err_message
            // 
            this.lb_err_message.AutoSize = true;
            this.lb_err_message.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_err_message.Location = new System.Drawing.Point(76, 355);
            this.lb_err_message.Name = "lb_err_message";
            this.lb_err_message.Size = new System.Drawing.Size(0, 36);
            this.lb_err_message.TabIndex = 57;
            // 
            // lb_station_type
            // 
            this.lb_station_type.AutoSize = true;
            this.lb_station_type.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_station_type.Location = new System.Drawing.Point(795, 29);
            this.lb_station_type.Name = "lb_station_type";
            this.lb_station_type.Size = new System.Drawing.Size(13, 18);
            this.lb_station_type.TabIndex = 59;
            this.lb_station_type.Text = "-";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label17.Location = new System.Drawing.Point(746, 29);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 18);
            this.label17.TabIndex = 58;
            this.label17.Text = "TYPE";
            // 
            // Form1
            // 
            this.AcceptButton = this.btn_start;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.lb_station_type);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.lb_err_message);
            this.Controls.Add(this.cmb_channel);
            this.Controls.Add(this.btn_reload);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.cmb_project);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_read);
            this.Controls.Add(this.tb_read);
            this.Controls.Add(this.lb_pf);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lb_station);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lb_bin);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_result);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "Form1";
            this.Text = " Garage-USB PixelAuth 1.0.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Enter += new System.EventHandler(this.btn_start_Click);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.img_preview)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_tip_powerup;
        private System.Windows.Forms.Button btn_tip_download;
        private System.Windows.Forms.Button btn_tip_restart;
        private System.Windows.Forms.Button btn_tip_calibrate;
        private System.Windows.Forms.Button btn_tip_press;
        private System.Windows.Forms.Button btn_tip_result;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_live;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_com;
        private System.Windows.Forms.TextBox tb_com;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lb_current;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.CheckBox cb_enhance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_tip_blink;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lb_station;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lb_pf;
        private System.Windows.Forms.Button btn_poweroff;
        private System.Windows.Forms.Button btn_poweron;
        private System.Windows.Forms.CheckBox cb_calibrate;
        private System.Windows.Forms.Button btn_s0_0;
        private System.Windows.Forms.Button btn_s0_1;
        private System.Windows.Forms.Button btn_usb0;
        private System.Windows.Forms.Button btn_usb1;
        private System.Windows.Forms.Button btn_led_0;
        private System.Windows.Forms.Button btn_led_1;
        private System.Windows.Forms.TextBox tb_read;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.TextBox tb_gain;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btn_get_cv;
        private System.Windows.Forms.CheckBox cb_mp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cmb_project;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox cb_ns;
        private System.Windows.Forms.CheckBox cb_btn;
        private System.Windows.Forms.CheckBox cb_cv;
        private System.Windows.Forms.Button btn_reload;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.PictureBox img_preview;
        public System.Windows.Forms.Label lb_project;
        public System.Windows.Forms.Label lb_version;
        public System.Windows.Forms.Label lb_product;
        public System.Windows.Forms.Label lb_sn;
        public System.Windows.Forms.Label lb_parameter;
        public System.Windows.Forms.Label lb_graylevel;
        public System.Windows.Forms.Label lb_rv;
        public System.Windows.Forms.Label lb_noise;
        public System.Windows.Forms.Label lb_voltage;
        public System.Windows.Forms.Button btn_result;
        public System.Windows.Forms.Label lb_bin;
        public System.Windows.Forms.ProgressBar pb_process;
        private System.Windows.Forms.ComboBox cmb_channel;
        public System.Windows.Forms.Label lb_err_message;
        private System.Windows.Forms.Label lb_station_type;
        private System.Windows.Forms.Label label17;
    }
}

