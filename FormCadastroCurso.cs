using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCadastroBD
{
    public partial class FormCadastroCurso : MaterialForm
    {
        bool isAlteracao = false;
        string conexao = @"server=127.0.0.1;" +
                "port=3307;" +
                "uid=root;" +
                "pwd=;" + // Substitua pela senha real
                "database=academico";
        public FormCadastroCurso()
        {
            InitializeComponent();
        }
        private void Salvar()
        {
            var con = new MySqlConnection(conexao);
            con.Open();
            var sql = "";
            if (isAlteracao)
            {
                sql = "UPDATE curso SET " +
                    "codigo = @codigo, " +
                    "duracao = @duracao, " +
                    "nome = @nome, " +
                    "area = @area, " +
                    "nivel = @nivel, " +
                    "periodo = @periodo " +
                    " WHERE id = @id";
            }
            else
            {
                sql = "INSERT INTO curso " +
               "(codigo, duracao, nome, area, " +
               "nivel, periodo)" +
               "VALUES" +
               "(@codigo, @duracao, @nome, @area, " +
               "@nivel, @periodo)";
            }
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
            cmd.Parameters.AddWithValue("@duracao", txtDuracao.Text);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@area", cboArea.Text);
            cmd.Parameters.AddWithValue("@nivel", cboNivel.Text);
            cmd.Parameters.AddWithValue("@periodo", cboPeriodo.Text);

            if (isAlteracao)
                cmd.Parameters.AddWithValue("@id", txtId.Text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
            LimpaCampos();
        }
        private void LimpaCampos()
        {
            foreach (var control in tabPageCadastro.Controls)
            {
                if (control is MaterialTextBoxEdit)
                {
                    ((MaterialTextBoxEdit)control).Clear();
                }
                if (control is MaterialMaskedTextBox)
                {
                    ((MaterialMaskedTextBox)control).Clear();
                }

            }
        }

        private bool ValidaFormulario()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Código é Obrigatório!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodigo.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Nome Obrigatório!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNome.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtDuracao.Text))
            {
                MessageBox.Show("Duração Obrigatório!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDuracao.Focus();
                return false;
            }
            return true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Atenção: Informações não salvas serão perdidas. \r\n"
                + "Deseja Cancelar?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpaCampos();
                TabControlCadastro.SelectedIndex = 1;
            }
        }
        private void Editar()
        {
            if (dataGridViewCursos.SelectedRows.Count > 0)
            {
                isAlteracao = true;
                var linha = dataGridViewCursos.SelectedRows[0];
                txtId.Text = linha.Cells["id"].Value.ToString();
                txtCodigo.Text = linha.Cells["codigo"].Value.ToString();
                txtDuracao.Text = linha.Cells["duracao"].Value.ToString();
                txtNome.Text = linha.Cells["nome"].Value.ToString();
                cboArea.Text = linha.Cells["area"].Value.ToString();
                cboNivel.Text = linha.Cells["nivel"].Value.ToString();
                cboPeriodo.Text = linha.Cells["periodo"].Value.ToString();
                TabControlCadastro.SelectedIndex = 0;
                txtCodigo.Focus();
            }
            else
            {
                MessageBox.Show("Selecione algum curso!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void CarregaGrid()
        {
            var con = new MySqlConnection(conexao);
            con.Open();
            var sql = "SELECT * FROM curso;";
            var sqlAd = new MySqlDataAdapter(sql, con);
            var dt = new DataTable();
            sqlAd.Fill(dt);
            dataGridViewCursos.DataSource = dt;
            con.Close();
        }
        private void Excluir(int id)
        {
            var con = new MySqlConnection(conexao);
            con.Open();
            var sql = "DELETE FROM  curso WHERE id = @id";
            var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void FormCadastroAluno_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
            }
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void tabPageConsulta_Enter(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridViewCursos.SelectedRows.Count > 0)
            {
                if ((MessageBox.Show("Deseja realmente excluir?", "IFSP", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
                {
                    int id = (int)dataGridViewCursos.SelectedRows[0].Cells["id"].Value;
                    Excluir(id);
                    CarregaGrid();
                }
            }
            else
            {
                MessageBox.Show("Selecione algum curso", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            TabControlCadastro.SelectedIndex = 0;
            txtCodigo.Focus();
        }

        private void dataGridViewCursos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Editar();
        }
    }
}
