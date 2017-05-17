/*
 * AtomBTCblock - tool created to rebuild and verify BTC block in the way it is used 
 * by Stratum pool and other miner software/hardware, powered by AtomMiner
 *
 * Copyright 2015-2017 AtomMiner <info@atomminer.com>
 *
 * BTC donation: 3LwsJAzPd8weD1FypVWmkDFMwA7rgjPSif
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option)
 * any later version.  See COPYING for more details.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AtomBTCblock
{
	public class Form1 : Form
	{
		private IContainer components = null;

		private TabControl tabControl1;

		private TabPage tabBlockSha;

		private TabPage tabShaVectors;

		private ListView lstTest;

		private ColumnHeader Data;

		private ColumnHeader Expected;

		private Label label1;

		private Button btnRunTest;

		private ColumnHeader sha256;

		private ColumnHeader columnHeader1;

		private Label label6;

		private TextBox txtShad;

		private Label label5;

		private TextBox txtSha;

		private Label label4;

		private TextBox txtReverse;

		private Button button1;

		private Label label3;

		private TextBox txtData;

		private Label label2;

		private Button button2;

		private CheckBox checkBox2;

		private CheckBox checkBox1;

		private TabPage tabBlockBuilder;

		private TextBox txtMrkl;

		private Label label8;

		private TextBox txtPrev;

		private Label label7;

		private Label label9;

		private TextBox txtBits;

		private Label label10;

		private TextBox txtTime;

		private GroupBox groupBox1;

		private Label lblDiff;

		private Label label11;

		private NumericUpDown txtNonce;

		private TextBox txtHdr;

		private TextBox txtVersion;

		private Label label12;

		private Label label13;

		private TextBox txtTarget;

		private TextBox txtDiff;

		private TextBox txtHdrShad;

		private Label label14;

		public Form1()
		{
			this.InitializeComponent();
		}

		private void btnRunTest_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem listViewItem in this.lstTest.Items)
			{
				listViewItem.BackColor = SystemColors.Window;
			}
			foreach (ListViewItem listViewItem in this.lstTest.Items)
			{
				string text = listViewItem.SubItems[0].Text;
				string text2;
				if (text.IndexOf("0x") == 0)
				{
					text2 = ss.hexHash(text.Substring(2));
				}
				else
				{
					text2 = ss.strHash(text);
				}
				listViewItem.SubItems[2].Text = text2;
				bool flag = text2.ToLower().Equals(listViewItem.SubItems[1].Text.ToLower());
				listViewItem.SubItems[3].Text = (flag ? "OK" : "FAIL");
				listViewItem.BackColor = (flag ? Color.LightGreen : Color.LightCoral);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.lstTest.Items.Add(new ListViewItem(new string[]
			{
				"abc",
				"ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
				"",
				""
			}));
			this.lstTest.Items.Add(new ListViewItem(new string[]
			{
				"abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq",
				"248D6A61D20638B8E5C026930C3E6039A33CE45964FF2167F6ECEDD419DB06C1",
				"",
				""
			}));
			this.lstTest.Items.Add(new ListViewItem(new string[]
			{
				"0xd3",
				"28969cdfa74a12c82f3bad960b0b000aca2ac329deea5c2328ebc6f2ba9802c1",
				"",
				""
			}));
			this.txtData.Text = "02000000502a989242bdfa912da58a972836c9cdfedd4a0278a467e00000000000000000077d1b77483270a9987cee2548e848878b812b13439e2284df7d3318c605232b5c8c0553535f0119ccad1657";
			this.txtNonce.Value = 1461104076m;
			this.button1_Click(sender, e);

			this.txtHdrShad.ForeColor = Color.Crimson;
			this.txtHdrShad.BackColor = SystemColors.Control;

			this.validateBlockHash();
			this.txtHdrShad.ReadOnly = false;
			this.txtHdrShad.KeyPress += new KeyPressEventHandler(this.txt_KeyPress);

		}

		private void txt_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void txtData_TextChanged(object sender, EventArgs e)
		{
			this.label3.Text = string.Format("Data Length: {0} ({1} byte(s))", this.txtData.Text.Length, this.txtData.Text.Length / 2);
			if (this.txtData.Text.Length % 2 == 0)
			{
				byte[] array = ss.StringToByteArray(this.txtData.Text);
				byte[] bytes = Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(array));
				this.txtReverse.Text = ss.arrToHex(bytes);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.checkBox1.Checked = false;
			this.checkBox2.Checked = false;
			this.txtSha.Text = ss.hexHashRev(this.txtData.Text);
			this.txtShad.Text = ss.hexHashDRev(this.txtData.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.txtData.Text = this.txtReverse.Text;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			byte[] array = ss.StringToByteArray(this.txtSha.Text);
			byte[] bytes = Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(array));
			this.txtSha.Text = ss.arrToHex(bytes);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			byte[] array = ss.StringToByteArray(this.txtShad.Text);
			byte[] bytes = Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(array));
			this.txtShad.Text = ss.arrToHex(bytes);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.button3_Click(sender, e);
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			this.button4_Click(sender, e);
		}

		private void validateBlockHash()
		{
			bool flag = false;
			try
			{
				ss.StringToByteArray(this.txtPrev.Text);
				if (this.txtPrev.Text.Length != 64)
				{
					throw new Exception();
				}
				this.txtPrev.BackColor = SystemColors.Window;
			}
			catch (Exception)
			{
				flag = true;
				this.txtPrev.BackColor = Color.Crimson;
			}
			try
			{
				ss.StringToByteArray(this.txtMrkl.Text);
				if (this.txtMrkl.Text.Length != 64)
				{
					throw new Exception();
				}
				this.txtMrkl.BackColor = SystemColors.Window;
			}
			catch (Exception)
			{
				flag = true;
				this.txtMrkl.BackColor = Color.Crimson;
			}
			uint num = 0u;
			try
			{
				num = Convert.ToUInt32(this.txtTime.Text, 16);
			}
			catch (Exception)
			{
				num = 0u;
			}
			if (num == 0u)
			{
				flag = true;
				this.txtTime.BackColor = Color.Crimson;
			}
			else
			{
				this.txtTime.BackColor = SystemColors.Window;
			}
			try
			{
				num = Convert.ToUInt32(this.txtBits.Text, 16);
			}
			catch (Exception)
			{
				num = 0u;
			}
			if (num == 0u)
			{
				flag = true;
				this.txtBits.BackColor = Color.Crimson;
				this.txtDiff.Text = "N/A";
				this.txtTarget.Text = "N/A";
			}
			else
			{
				this.txtBits.BackColor = SystemColors.Window;
				this.txtDiff.Text = string.Format("{0}", ss.getDiff(num));
				this.txtTarget.Text = ss.getTargetStr(num);
			}
			if (flag)
			{
				this.txtHdr.Text = "N/A";
				this.txtHdrShad.Text = "N/A";
			}
			else
			{
				List<byte> list = new List<byte>();
				string text = "";
				text += Convert.ToUInt32(this.txtNonce.Value).ToString("x8");
				text += Convert.ToUInt32(this.txtBits.Text, 16).ToString("x8");
				text += Convert.ToUInt32(this.txtTime.Text, 16).ToString("x8");
				text += this.txtMrkl.Text;
				text += this.txtPrev.Text;
				text += Convert.ToUInt32(this.txtVersion.Text).ToString("x8");
				this.txtHdr.Text = text;
				byte[] bytes = Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ss.StringToByteArray(text)));
				this.txtHdrShad.Text = ss.hexHashDRev(bytes);
				if (this.txtHdrShad.Text.ToLower().CompareTo(this.txtTarget.Text.ToLower()) > 0)
				{
					this.txtHdrShad.ForeColor = Color.Crimson;
				}
				else
				{
					this.txtHdrShad.ForeColor = Color.Green;
				}
			}
		}

		private void textPrev_TextChanged(object sender, EventArgs e)
		{
			this.validateBlockHash();
		}

		private void txtNonce_ValueChanged(object sender, EventArgs e)
		{
			this.validateBlockHash();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.tabControl1 = new TabControl();
			this.tabBlockSha = new TabPage();
			this.tabShaVectors = new TabPage();
			this.label1 = new Label();
			this.lstTest = new ListView();
			this.Data = new ColumnHeader();
			this.Expected = new ColumnHeader();
			this.btnRunTest = new Button();
			this.sha256 = new ColumnHeader();
			this.columnHeader1 = new ColumnHeader();
			this.label2 = new Label();
			this.txtData = new TextBox();
			this.label3 = new Label();
			this.button1 = new Button();
			this.txtReverse = new TextBox();
			this.label4 = new Label();
			this.label5 = new Label();
			this.txtSha = new TextBox();
			this.label6 = new Label();
			this.txtShad = new TextBox();
			this.button2 = new Button();
			this.checkBox1 = new CheckBox();
			this.checkBox2 = new CheckBox();
			this.tabBlockBuilder = new TabPage();
			this.label7 = new Label();
			this.txtPrev = new TextBox();
			this.txtMrkl = new TextBox();
			this.label8 = new Label();
			this.label9 = new Label();
			this.txtTime = new TextBox();
			this.txtBits = new TextBox();
			this.label10 = new Label();
			this.lblDiff = new Label();
			this.groupBox1 = new GroupBox();
			this.txtNonce = new NumericUpDown();
			this.label11 = new Label();
			this.txtHdr = new TextBox();
			this.txtVersion = new TextBox();
			this.label12 = new Label();
			this.label13 = new Label();
			this.txtDiff = new TextBox();
			this.txtTarget = new TextBox();
			this.label14 = new Label();
			this.txtHdrShad = new TextBox();
			this.tabControl1.SuspendLayout();
			this.tabBlockSha.SuspendLayout();
			this.tabShaVectors.SuspendLayout();
			this.tabBlockBuilder.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.txtNonce).BeginInit();
			base.SuspendLayout();
			this.tabControl1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tabControl1.Controls.Add(this.tabBlockBuilder);
			this.tabControl1.Controls.Add(this.tabBlockSha);
			this.tabControl1.Controls.Add(this.tabShaVectors);
			this.tabControl1.Location = new Point(-3, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(622, 323);
			this.tabControl1.TabIndex = 0;
			this.tabBlockSha.Controls.Add(this.checkBox2);
			this.tabBlockSha.Controls.Add(this.checkBox1);
			this.tabBlockSha.Controls.Add(this.button2);
			this.tabBlockSha.Controls.Add(this.label6);
			this.tabBlockSha.Controls.Add(this.txtShad);
			this.tabBlockSha.Controls.Add(this.label5);
			this.tabBlockSha.Controls.Add(this.txtSha);
			this.tabBlockSha.Controls.Add(this.label4);
			this.tabBlockSha.Controls.Add(this.txtReverse);
			this.tabBlockSha.Controls.Add(this.button1);
			this.tabBlockSha.Controls.Add(this.label3);
			this.tabBlockSha.Controls.Add(this.txtData);
			this.tabBlockSha.Controls.Add(this.label2);
			this.tabBlockSha.Location = new Point(4, 22);
			this.tabBlockSha.Name = "tabBlockSha";
			this.tabBlockSha.Padding = new Padding(3);
			this.tabBlockSha.Size = new Size(614, 297);
			this.tabBlockSha.TabIndex = 0;
			this.tabBlockSha.Text = "Block SHA256";
			this.tabBlockSha.UseVisualStyleBackColor = true;
			this.tabShaVectors.Controls.Add(this.btnRunTest);
			this.tabShaVectors.Controls.Add(this.lstTest);
			this.tabShaVectors.Controls.Add(this.label1);
			this.tabShaVectors.Location = new Point(4, 22);
			this.tabShaVectors.Name = "tabShaVectors";
			this.tabShaVectors.Padding = new Padding(3);
			this.tabShaVectors.Size = new Size(614, 297);
			this.tabShaVectors.TabIndex = 1;
			this.tabShaVectors.Text = "SHA256 Test Vectors";
			this.tabShaVectors.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(6, 14);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0, 13);
			this.label1.TabIndex = 0;
			this.lstTest.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lstTest.Columns.AddRange(new ColumnHeader[]
			{
				this.Data,
				this.Expected,
				this.sha256,
				this.columnHeader1
			});
			this.lstTest.FullRowSelect = true;
			this.lstTest.GridLines = true;
			this.lstTest.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.lstTest.Location = new Point(9, 43);
			this.lstTest.Name = "lstTest";
			this.lstTest.Size = new Size(596, 242);
			this.lstTest.TabIndex = 2;
			this.lstTest.UseCompatibleStateImageBehavior = false;
			this.lstTest.View = View.Details;
			this.Data.Text = "Data";
			this.Data.Width = 99;
			this.Expected.Text = "Expected";
			this.Expected.Width = 227;
			this.btnRunTest.Location = new Point(11, 10);
			this.btnRunTest.Name = "btnRunTest";
			this.btnRunTest.Size = new Size(130, 23);
			this.btnRunTest.TabIndex = 3;
			this.btnRunTest.Text = "Test NIST vectors";
			this.btnRunTest.UseVisualStyleBackColor = true;
			this.btnRunTest.Click += new EventHandler(this.btnRunTest_Click);
			this.sha256.Text = "SHA256";
			this.sha256.Width = 221;
			this.columnHeader1.Text = "Pass";
			this.columnHeader1.Width = 35;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(8, 7);
			this.label2.Name = "label2";
			this.label2.Size = new Size(85, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "HEX Block Data";
			this.txtData.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtData.Location = new Point(11, 23);
			this.txtData.Name = "txtData";
			this.txtData.Size = new Size(557, 20);
			this.txtData.TabIndex = 1;
			this.txtData.TextChanged += new EventHandler(this.txtData_TextChanged);
			this.label3.Location = new Point(11, 46);
			this.label3.Name = "label3";
			this.label3.Size = new Size(591, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "Data length: 0 (0 bytes)";
			this.button1.Anchor = AnchorStyles.Top;
			this.button1.Location = new Point(267, 67);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Calc";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.txtReverse.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtReverse.Location = new Point(11, 107);
			this.txtReverse.Name = "txtReverse";
			this.txtReverse.ReadOnly = true;
			this.txtReverse.Size = new Size(591, 20);
			this.txtReverse.TabIndex = 4;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(11, 91);
			this.label4.Name = "label4";
			this.label4.Size = new Size(71, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Byte Reverse";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(11, 130);
			this.label5.Name = "label5";
			this.label5.Size = new Size(82, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "SHA-256 (Data)";
			this.txtSha.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtSha.Location = new Point(11, 146);
			this.txtSha.Name = "txtSha";
			this.txtSha.ReadOnly = true;
			this.txtSha.Size = new Size(557, 20);
			this.txtSha.TabIndex = 6;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(11, 169);
			this.label6.Name = "label6";
			this.label6.Size = new Size(88, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "SHA-256d (Data)";
			this.txtShad.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtShad.Location = new Point(11, 185);
			this.txtShad.Name = "txtShad";
			this.txtShad.ReadOnly = true;
			this.txtShad.Size = new Size(557, 20);
			this.txtShad.TabIndex = 8;
			this.button2.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.button2.Location = new Point(572, 23);
			this.button2.Name = "button2";
			this.button2.Size = new Size(36, 23);
			this.button2.TabIndex = 10;
			this.button2.Text = "Rev";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.checkBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.checkBox1.Appearance = Appearance.Button;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(571, 146);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(37, 23);
			this.checkBox1.TabIndex = 13;
			this.checkBox1.Text = "Rev";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
			this.checkBox2.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.checkBox2.Appearance = Appearance.Button;
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new Point(571, 183);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new Size(37, 23);
			this.checkBox2.TabIndex = 14;
			this.checkBox2.Text = "Rev";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
			this.tabBlockBuilder.Controls.Add(this.txtDiff);
			this.tabBlockBuilder.Controls.Add(this.txtVersion);
			this.tabBlockBuilder.Controls.Add(this.label12);
			this.tabBlockBuilder.Controls.Add(this.label11);
			this.tabBlockBuilder.Controls.Add(this.txtNonce);
			this.tabBlockBuilder.Controls.Add(this.groupBox1);
			this.tabBlockBuilder.Controls.Add(this.lblDiff);
			this.tabBlockBuilder.Controls.Add(this.txtBits);
			this.tabBlockBuilder.Controls.Add(this.label10);
			this.tabBlockBuilder.Controls.Add(this.txtTime);
			this.tabBlockBuilder.Controls.Add(this.label9);
			this.tabBlockBuilder.Controls.Add(this.txtMrkl);
			this.tabBlockBuilder.Controls.Add(this.label8);
			this.tabBlockBuilder.Controls.Add(this.txtPrev);
			this.tabBlockBuilder.Controls.Add(this.label7);
			this.tabBlockBuilder.Location = new Point(4, 22);
			this.tabBlockBuilder.Name = "tabBlockBuilder";
			this.tabBlockBuilder.Size = new Size(614, 297);
			this.tabBlockBuilder.TabIndex = 2;
			this.tabBlockBuilder.Text = "Block Builder";
			this.tabBlockBuilder.UseVisualStyleBackColor = true;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(8, 8);
			this.label7.Name = "label7";
			this.label7.Size = new Size(107, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "Previous Hash (HEX)";
			this.txtPrev.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtPrev.Location = new Point(11, 24);
			this.txtPrev.Name = "txtPrev";
			this.txtPrev.Size = new Size(591, 20);
			this.txtPrev.TabIndex = 1;
			this.txtPrev.Text = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a50";
			this.txtPrev.TextChanged += new EventHandler(this.textPrev_TextChanged);
			this.txtMrkl.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtMrkl.Location = new Point(11, 63);
			this.txtMrkl.Name = "txtMrkl";
			this.txtMrkl.Size = new Size(591, 20);
			this.txtMrkl.TabIndex = 3;
			this.txtMrkl.Text = "2b2305c618337ddf84229e43132b818b8748e84825ee7c98a9703248771b7d07";
			this.txtMrkl.TextChanged += new EventHandler(this.textPrev_TextChanged);
			this.label8.AutoSize = true;
			this.label8.Location = new Point(8, 47);
			this.label8.Name = "label8";
			this.label8.Size = new Size(96, 13);
			this.label8.TabIndex = 2;
			this.label8.Text = "Merkel Root (HEX)";
			this.label9.AutoSize = true;
			this.label9.Location = new Point(8, 86);
			this.label9.Name = "label9";
			this.label9.Size = new Size(61, 13);
			this.label9.TabIndex = 4;
			this.label9.Text = "Time (HEX)";
			this.txtTime.Location = new Point(11, 102);
			this.txtTime.Name = "txtTime";
			this.txtTime.Size = new Size(300, 20);
			this.txtTime.TabIndex = 5;
			this.txtTime.Text = "53058C5C";
			this.txtTime.TextChanged += new EventHandler(this.textPrev_TextChanged);
			this.txtBits.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtBits.Location = new Point(317, 102);
			this.txtBits.Name = "txtBits";
			this.txtBits.Size = new Size(285, 20);
			this.txtBits.TabIndex = 7;
			this.txtBits.Text = "19015f53";
			this.txtBits.TextChanged += new EventHandler(this.textPrev_TextChanged);
			this.label10.AutoSize = true;
			this.label10.Location = new Point(314, 86);
			this.label10.Name = "label10";
			this.label10.Size = new Size(55, 13);
			this.label10.TabIndex = 6;
			this.label10.Text = "Bits (HEX)";
			this.lblDiff.Location = new Point(11, 172);
			this.lblDiff.Name = "lblDiff";
			this.lblDiff.Size = new Size(93, 13);
			this.lblDiff.TabIndex = 8;
			this.lblDiff.Text = "Block Difficulty: ";
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.txtTarget);
			this.groupBox1.Controls.Add(this.txtHdrShad);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.txtHdr);
			this.groupBox1.Location = new Point(11, 194);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(591, 100);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Header";
			this.txtNonce.Location = new Point(11, 141);
			NumericUpDown arg_1466_0 = this.txtNonce;
			int[] array = new int[4];
			array[1] = 1;
			arg_1466_0.Maximum = new decimal(array);
			this.txtNonce.Name = "txtNonce";
			this.txtNonce.Size = new Size(300, 20);
			this.txtNonce.TabIndex = 11;
			this.txtNonce.ValueChanged += new EventHandler(this.txtNonce_ValueChanged);
			this.label11.AutoSize = true;
			this.label11.Location = new Point(8, 125);
			this.label11.Name = "label11";
			this.label11.Size = new Size(39, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Nonce";
			this.txtHdr.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.txtHdr.BackColor = SystemColors.Control;
			this.txtHdr.BorderStyle = BorderStyle.None;
			this.txtHdr.HideSelection = false;
			this.txtHdr.Location = new Point(11, 19);
			this.txtHdr.Multiline = true;
			this.txtHdr.Name = "txtHdr";
			this.txtHdr.ReadOnly = true;
			this.txtHdr.Size = new Size(574, 36);
			this.txtHdr.TabIndex = 0;
			this.txtHdr.Text = "02000000502a989242bdfa912da58a972836c9cdfedd4a0278a467e00000000000000000077d1b77483270a9987cee2548e848878b812b13439e2284df7d3318c605232b5c8c0553535f0119cbad1657";
			this.txtVersion.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtVersion.Enabled = false;
			this.txtVersion.Location = new Point(317, 141);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.ReadOnly = true;
			this.txtVersion.Size = new Size(285, 20);
			this.txtVersion.TabIndex = 14;
			this.txtVersion.Text = "2";
			this.label12.AutoSize = true;
			this.label12.Location = new Point(314, 125);
			this.label12.Name = "label12";
			this.label12.Size = new Size(42, 13);
			this.label12.TabIndex = 13;
			this.label12.Text = "Version";
			this.label13.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.label13.Cursor = Cursors.IBeam;
			this.label13.Location = new Point(9, 78);
			this.label13.Name = "label13";
			this.label13.Size = new Size(82, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "Block Target: ";
			this.txtDiff.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtDiff.BorderStyle = BorderStyle.None;
			this.txtDiff.Location = new Point(110, 172);
			this.txtDiff.Name = "txtDiff";
			this.txtDiff.ReadOnly = true;
			this.txtDiff.Size = new Size(492, 13);
			this.txtDiff.TabIndex = 16;
			this.txtDiff.Text = "32";
			this.txtTarget.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.txtTarget.BorderStyle = BorderStyle.None;
			this.txtTarget.Font = new Font("Courier New", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txtTarget.Location = new Point(99, 78);
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.ReadOnly = true;
			this.txtTarget.Size = new Size(486, 14);
			this.txtTarget.TabIndex = 17;
			this.txtTarget.Text = "00000000000000015f5300000000000000000000000000000000000000000000";
			this.label14.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.label14.Cursor = Cursors.IBeam;
			this.label14.Location = new Point(8, 58);
			this.label14.Name = "label14";
			this.label14.Size = new Size(93, 13);
			this.label14.TabIndex = 18;
			this.label14.Text = "SHA-256d";
			this.txtHdrShad.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.txtHdrShad.BorderStyle = BorderStyle.None;
			this.txtHdrShad.Font = new Font("Courier New", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txtHdrShad.Location = new Point(99, 58);
			this.txtHdrShad.Name = "txtHdrShad";
			this.txtHdrShad.ReadOnly = true;
			this.txtHdrShad.Size = new Size(486, 14);
			this.txtHdrShad.TabIndex = 18;
			this.txtHdrShad.Text = "00000000000000015f5300000000000000000000000000000000000000000000";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(615, 319);
			base.Controls.Add(this.tabControl1);
			this.MinimumSize = new Size(623, 346);
			base.Name = "Form1";
			this.Text = "Bitcoin Block Debug";
			base.Load += new EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabBlockSha.ResumeLayout(false);
			this.tabBlockSha.PerformLayout();
			this.tabShaVectors.ResumeLayout(false);
			this.tabShaVectors.PerformLayout();
			this.tabBlockBuilder.ResumeLayout(false);
			this.tabBlockBuilder.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.txtNonce).EndInit();
			base.ResumeLayout(false);
		}
	}
}
