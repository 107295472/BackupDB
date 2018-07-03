using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BackupDB
{
    public partial class MainDB : Form
    {
        public MainDB()
        {
            InitializeComponent();
        }
        private void btnHY_Click(object sender, EventArgs e)
        {
            string text = this.txtBakPath.Text;
            string str2 = this.txtMdfPath.Text;
            string str3 = this.txtLdfPath.Text;
            string str4 = this.txtBakLogicName.Text;
            string str5 = this.txtBakPath.Text;
            string str6 = this.txtHyLogicName.Text;
            if (string.IsNullOrEmpty(str5))
            {
                MessageBox.Show("还原名字不能为空!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("bak路径不能为空!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (string.IsNullOrEmpty(str2))
            {
                MessageBox.Show("mdf路径不能为空!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (string.IsNullOrEmpty(str3))
            {
                MessageBox.Show("ldf路径不能为空!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (string.IsNullOrEmpty(str4))
            {
                MessageBox.Show("逻辑名不能为空!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.btnHY.Text = "还原中…";
                this.btnHY.Enabled = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(Config.ConnString))
                    {
                        SqlCommand command = new SqlCommand
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        SqlParameter parameter = new SqlParameter("@dbNewName", SqlDbType.VarChar, 200)
                        {
                            Value = str6
                        };
                        SqlParameter parameter2 = new SqlParameter("@dbName", SqlDbType.VarChar, 200)
                        {
                            Value = str4
                        };
                        SqlParameter parameter3 = new SqlParameter("@bakUrl", SqlDbType.VarChar, 200)
                        {
                            Value = text
                        };
                        SqlParameter parameter4 = new SqlParameter("@dbMdfUrl", SqlDbType.VarChar, 200)
                        {
                            Value = str2
                        };
                        SqlParameter parameter5 = new SqlParameter("@dbLdfUrl", SqlDbType.VarChar, 200)
                        {
                            Value = str3
                        };
                        command.CommandText = "Usp_restore";
                        command.Connection = connection;
                        command.Parameters.Add(parameter3);
                        command.Parameters.Add(parameter4);
                        command.Parameters.Add(parameter5);
                        command.Parameters.Add(parameter2);
                        command.Parameters.Add(parameter);
                        connection.Open();
                        int num = Convert.ToInt32(command.ExecuteScalar());
                        if (str4==str6)
                        {
                            if (num > 0)
                            {
                                MessageBox.Show("还原成功!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }else
                        {
                            StringBuilder builder = new StringBuilder();
                            command.CommandType = CommandType.Text;
                            builder.Append("ALTER DATABASE " + str6 + " MODIFY FILE (NAME = " + str4 + ", NEWNAME = " + str6 + ") ");
                            builder.Append("ALTER DATABASE " + str6 + " MODIFY FILE (NAME = " + str4 + "_log, NEWNAME = " + str6 + "_log) ");
                            command.CommandText = builder.ToString();
                            command.ExecuteNonQuery();
                            if (num > 0)
                            {
                                MessageBox.Show("还原成功!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                       
                       
                        this.btnHY.Enabled = true;
                        this.btnHY.Text = "还原数据库";
                        //base.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    MessageBox.Show(exception.StackTrace);
                    Application.Exit();
                }
            }
        }

        private void btnSelectBak_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtBakPath.Text))
            {
                MessageBox.Show("请选择bak文件路径!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string selectCommandText = string.Format("restore filelistonly from disk='{0}'", this.txtBakPath.Text);
                using (SqlConnection connection = new SqlConnection(Config.ConnString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    this.dataGridView1.DataSource = dataSet.Tables[0];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ofdBak.ShowDialog();
            string fileName = this.ofdBak.FileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                this.txtBakPath.Text = fileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ofdMdf.ShowDialog();
            string fileName = this.ofdMdf.FileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                this.txtMdfPath.Text = fileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ofdLdf.ShowDialog();
            string fileName = this.ofdLdf.FileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                this.txtLdfPath.Text = fileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.fbdS.ShowDialog();
            this.txtBackPath.Text = this.fbdS.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.txtBackDbName.Text == "")
            {
                MessageBox.Show("请填写数据库名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.txtBackPath.Text == "")
            {
                MessageBox.Show("请填写数据库路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = this.txtBackDbName.Text;
                item.SubItems.Add(this.txtBackPath.Text);
                this.lvSview.Items.Add(item);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.lvSview.Items.Count <= 0)
            {
                MessageBox.Show("请填写备份数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                SqlConnection connection = new SqlConnection(Config.ConnString);
                SqlCommand command = new SqlCommand();
                new StringBuilder();
                new StringBuilder();
                command.Connection = connection;
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = new SqlParameter("@dbUrl", SqlDbType.VarChar, 200)
                    {
                        Value = this.txtBackPath.Text + @"\"
                    };
                    SqlParameter parameter2 = new SqlParameter("@dbName", SqlDbType.VarChar, 200)
                    {
                        Value = this.txtBackDbName.Text
                    };
                    command.CommandText = "usp_backup";
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(parameter2);
                    command.ExecuteScalar().ToString();
                    connection.Close();
                    MessageBox.Show("备份完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //base.Dispose();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    MessageBox.Show(exception.StackTrace);
                    Application.Exit();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtBakPath.Text))
            {
                MessageBox.Show("请选择bak文件路径!", "还原提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string selectCommandText = string.Format("restore filelistonly from disk='{0}'", this.txtBakPath.Text);
                using (SqlConnection connection = new SqlConnection(Config.ConnString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    this.dataGridView1.DataSource = dataSet.Tables[0];
                }
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.lvSview.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的地址！", "出错备份提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.lvSview.Items.Remove(this.lvSview.SelectedItems[0]);
            }
        }
    }
}
