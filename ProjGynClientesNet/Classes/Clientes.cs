using System;
using System.Data;

namespace ProjGynClientesNet.Classes
{
    public class Clientes
    {
        private int Id_Cli;
        private string RazaoSocial_Cli;
        private string Fantasia_Cli;
        private int FisJur_Cli;
        private string CNPJ_CPF_Cli;
        private string IE_Cli;
        private string IM_Cli;
        private int Tp_Cli;
        private int Indexador_Cli;
        private int QtdIndex_Cli;
        private int DiaVenc_Cli;
        private int TpCobranca_Cli;
        private int EmiteNF_Cli;
        private string Obs_Cli;

        public int Id { get => Id_Cli; set => Id_Cli = value; }
        public string RazaoSocial { get => RazaoSocial_Cli; set => RazaoSocial_Cli = value; }
        public string Fantasia { get => Fantasia_Cli; set => Fantasia_Cli = value; }
        public int FisJur { get => FisJur_Cli; set => FisJur_Cli = value; }
        public string CNPJ_CPF { get => CNPJ_CPF_Cli; set => CNPJ_CPF_Cli = value; }
        public string IE { get => IE_Cli; set => IE_Cli = value; }
        public string IM { get => IM_Cli; set => IM_Cli = value; }
        public int Tipo { get => Tp_Cli; set => Tp_Cli = value; }
        public int Indexador { get => Indexador_Cli; set => Indexador_Cli = value; }
        public int QtdIndex { get => QtdIndex_Cli; set => QtdIndex_Cli = value; }
        public int DiaVenc { get => DiaVenc_Cli; set => DiaVenc_Cli = value; }
        public int TipoCobranca { get => TpCobranca_Cli; set => TpCobranca_Cli = value; }
        public int EmiteNF { get => EmiteNF_Cli; set => EmiteNF_Cli = value; }
        public string Obs { get => Obs_Cli; set => Obs_Cli = value; }

        ConexaoMySQL objConexao = new ConexaoMySQL(); //Declara o Objeto Conexão (classe que abre o DB e executa as instruções SQL)        

        /// <summary>
        /// Função Preenche Objeto Cliente
        /// </summary>
        /// <param name="Id">Id do cliente</param>
        /// <returns>Retorna um Objeto Cliente preenchido</returns>
        public Clientes F_PreencheObjCliente(long Id)
        {
            Clientes objCli = new Clientes(); //Declara o Objeto Cliente
            string msgErro = "";
            string sqlQuery = "SELECT * FROM cliente WHERE Id_Cli = " + Id; //Monta a Query SQL
            DataTable tAux = objConexao.F_ExecutaSQL_Retorna_DataTable(sqlQuery, "cliente", ref msgErro); //Faz a busca no banco de dados. 
            foreach (DataRow dr in tAux.Rows)
            {



                // Deixei esse comentário aqui pq é uma dica. Se o nome das propriedades da classe forem identicas às colunas da tabela do banco de dados, dá pra fazer essa atribuição genérica.
                //foreach(var prop in objCli.GetType().GetProperties())
                //{
                //    if (prop.PropertyType == typeof( int) )
                //        prop.SetValue(typeof (int), Convert.ToInt16( dr[prop.Name]));
                //    if (prop.PropertyType == typeof(string))
                //        prop.SetValue(typeof(string), dr[prop.Name].ToString());
                //}

                if (objCli.DiaVenc == 0) objCli.DiaVenc = 1; //Se o dia do Vencimento vier zero, grava 1º


                          objCli.Id = (int) dr["Id_Cli"];
                 objCli.RazaoSocial = dr["RazaoSocial_Cli"].ToString();
                    objCli.Fantasia = dr["Fantasia_Cli"].ToString();
                      objCli.FisJur = Convert.ToInt16(dr["FisJur_Cli"]);
                    objCli.CNPJ_CPF = dr["CNPJ_CPF_Cli"].ToString();
                          objCli.IE = dr["IE_Cli"].ToString();
                          objCli.IM = dr["IM_Cli"].ToString();
                        objCli.Tipo = Convert.ToInt16(dr["TP_Cli"]);
                   objCli.Indexador = Convert.ToInt16(dr["Indexador_Cli"]);
                    objCli.QtdIndex = Convert.ToInt16(dr["QtdIndex_Cli"]);
                     objCli.DiaVenc = Convert.ToInt16(dr["DiaVenc_Cli"]);
                objCli.TipoCobranca = Convert.ToInt16(dr["TpCobranca_Cli"]);
                     objCli.EmiteNF = Convert.ToInt16(dr["EmiteNF_Cli"]);
                         objCli.Obs = dr["Obs_Cli"].ToString();
                break; //Coloquei esse break aqui pq no select só vem 1 registro
            }
            
            return objCli;
        }

        /// <summary>
        /// Função Salvar - Se houver Id, atualiza, senão, inclui.
        /// </summary>
        /// <param name="objCli">Recebe o Objeto Cliente preenchido</param>
        /// <returns>Retorna vazio se deu tudo certo ou retorna a msg de erro. </returns>
        public string F_Salvar( Clientes objCli)
        {
            string ret = F_VerificaCampos(objCli); //Chama função que verifica se campos obrigatórios foram preenchidos. 
            if (ret == "" ) //Se vier vazio é pq não ocorreram campos obrigatórios vazios.
            { 
                string sqlQuery = "";
                int idAux = objCli.Id;
                if (idAux == 0)
                {
                    sqlQuery = "INSERT INTO cliente(RazaoSocial_Cli, Fantasia_Cli, FisJur_Cli, CNPJ_CPF_Cli, IE_Cli, IM_Cli, Tp_Cli, Indexador_Cli, QtdIndex_Cli, DiaVenc_Cli, TpCobranca_Cli, EmiteNF_Cli, Obs_Cli) " +
                               "VALUES (" + objConexao.F_PreparaCampoMySQLString(objCli.RazaoSocial) +
                                      "," + objConexao.F_PreparaCampoMySQLString(objCli.Fantasia) +
                                      "," + objCli.FisJur +
                                      "," + objConexao.F_PreparaCampoMySQLString(objCli.CNPJ_CPF) +
                                      "," + objConexao.F_PreparaCampoMySQLString(objCli.IE) +
                                      "," + objConexao.F_PreparaCampoMySQLString(objCli.IM) +
                                      "," + objCli.Tipo +
                                      "," + objCli.Indexador +
                                      "," + objCli.QtdIndex +
                                      "," + objCli.DiaVenc +
                                      "," + objCli.TipoCobranca +
                                      "," + objCli.EmiteNF +
                                      "," + objConexao.F_PreparaCampoMySQLString(objCli.Obs) + ");";
                                  
                }
                else
                {
                    sqlQuery = "UPDATE cliente SET " +
                                    "  RazaoSocial_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.RazaoSocial) +
                                    ", Fantasia_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.Fantasia) +
                                    ", FisJur_Cli = " + objCli.FisJur +
                                    ", CNPJ_CPF_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.CNPJ_CPF) +
                                    ", IE_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.IE) +
                                    ", IM_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.IM )+
                                    ", Tp_Cli = " + objCli.Tipo +
                                    ", Indexador_Cli = " + objCli.Indexador +
                                    ", QtdIndex_Cli = " + objCli.QtdIndex +
                                    ", DiaVenc_Cli = " + objCli.DiaVenc +
                                    ", TpCobranca_Cli = " + objCli.TipoCobranca +
                                    ", EmiteNF_Cli = " + objCli.EmiteNF +
                                    ", Obs_Cli = " + objConexao.F_PreparaCampoMySQLString(objCli.Obs )+ ");";
                           
                }

                ret = objConexao.F_ExecutaSQL( sqlQuery );
            }
            return ret;
        }

        /// <summary>
        /// Função que verifica se os campos obrigatórios foram preenchidos. 
        /// </summary>
        /// <param name="objCli"></param>
        /// <returns>Se algum campo não estiver preenchido, retorna uma mensagem com </returns>
        private string F_VerificaCampos(Clientes objCli)
        {
            string ret = "";
            if (objCli.RazaoSocial.Trim() == "") ret += "Razão Social ";
            if (objCli.Fantasia.Trim() == "") ret += "Fantasia ";
            if (objCli.CNPJ_CPF.Trim() == "") ret += "CNPJ/CPF ";
            
            if (ret.Trim()!="")
            {
                ret = "Campo(s) não preenchido(s): " + ret;
            }
            return ret;
        }

        /// <summary>
        /// Função que recebe Id e CnpjCpf, pesquisa na tabela pelo CnpjCpf, verifica se o Id informado é gual ao do encontrado
        /// </summary>
        /// <param name="Id">Id do Cliente</param>
        /// <param name="CnpjCpf">CnpjCpf do cliente</param>
        /// <returns>Se encontrar o CnpjCpf e o Id foi igual ao informado, retorna 0(zero), senão retorna o Id do registro encontrado. </returns>
        public int F_VerificaCnpjCpfExistente(long Id, string CnpjCpf)
        {
            int idAux = (int) objConexao.F_ExecutaScalarSQL_Double("SELECT Id_Cli FROM cliente WHERE CNPJ_CPF_Cli ='" + CnpjCpf + "'");
            if (idAux != Id)
                return idAux;
            else
                return 0;
        }
    }
}
