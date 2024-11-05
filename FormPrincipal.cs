using ReaLTaiizor.Forms;

namespace ProjetoCadastroBD
{
    public partial class FormPrincipal : MaterialForm
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void alunoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCadastroAluno formCurso = new FormCadastroAluno();
            formCurso.MdiParent = this;
            formCurso.Show();
        }

        private void cursosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCadastroCurso formCurso = new FormCadastroCurso();
            formCurso.MdiParent = this;
            formCurso.Show();
        }
    }
}
