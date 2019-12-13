using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProjGynClientesNet.Classes
{

    public class ConexaoMySQL
    {

        MySqlConnection mConn = new MySqlConnection(); //DB
        
        /// <summary>
        /// Função Abre Conexão
        /// </summary>
        /// <param name="mConn">Recebe por referência um Objeto do tipo MySqlConnection e o retorna setado</param>
        /// <param name="MensagemErro">Se houver erro na abertura do DB, o erro será retornado nesta variável passada por referencia</param>
        /// <returns>Retorna o "ConnectionState" da conexão. Ex.: 0-Closed; 1-Open; 2-Connecting; 4-Executing; 8-Fetching; 16-Broken </returns>
        public System.Data.ConnectionState F_AbreConexao(ref string msgErro)
        {
            string strConexao = "Persist Security Info=False;server=gynsoft.com.br;database=softhost_gs_clientes;uid=cliente;server=gynsoft.com.br;database=softhost_gs_clientes;uid=cliente;pwd=gynsoft8299";
            msgErro = "";
            try
            {
                if (mConn.State == System.Data.ConnectionState.Closed )
                { 
                    mConn = new MySqlConnection(strConexao);
                    mConn.Open();
                }
            }
            catch (System.Exception e)
            {
                msgErro = e.Message.ToString();
            }
            return mConn.State;
        }

        /// <summary>
        /// Recebe o Objeto do tipo MySqlConnection e o fecha. 
        /// </summary>
        /// <param name="mConn"></param>
        public void R_FechaConexao()
        {
            if (mConn.State != System.Data.ConnectionState.Closed)
                mConn.Close();
        }

        /// <summary>
        /// Função que executa uma query e retorna um DataTable
        /// </summary>
        /// <param name="strQuery">SELECT SQL</param>
        /// <param name="strNomeTabela">Nome da tabela a ser retornada</param>
        /// <param name="msgErro">Mensagem de erro, caso ocorra algum</param>
        /// <returns>Retorna um DataTable</returns>
        public DataTable F_ExecutaSQL_Retorna_DataTable(string strQuery, string strNomeTabela, ref string msgErro )
        {
            DataTable tAux = new DataTable(strNomeTabela); //Cria tabela com os dados vindos do DB
            System.Data.ConnectionState EstadoConexao = new System.Data.ConnectionState();
            EstadoConexao = F_AbreConexao(ref msgErro);//Chama função que abre a conexão
            if (EstadoConexao == System.Data.ConnectionState.Open )//Se o estado da conexão for diferente de "Open", não faz o select
            {
                try
                {
                    try
                    {
                        MySqlCommand comando = new MySqlCommand(strQuery, mConn);
                        MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                        adaptador.Fill(tAux);
                    }
                    catch (Exception e)
                    {
                        msgErro = e.Message;
                        throw;
                    }
                }
                catch (Exception e)
                {
                    msgErro = e.Message;
                    throw;
                }
            }
            return tAux;
        }

        /// <summary>
        /// Função que Executa comando SQL que não retorna nada. Ex.: INSERT, UPDATE, DELETE ...
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns>Se tudo der certo, retorna uma string vazia, senão, retorna o erro</returns>
        public string F_ExecutaSQL(string strQuery)
        {
            string ret = ""; //Variável de retorno
            MySqlCommand comando = new MySqlCommand();
            try
            {
                if (F_AbreConexao(ref ret) == System.Data.ConnectionState.Open  )
                {
                    comando = new MySqlCommand(strQuery, mConn);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                ret = e.Message; //se houver algum erro, retorna uma msg na função
                throw;
            }
            finally
            {
                R_FechaConexao(); //Fecha conexão com o banco de dados.
            }
            return ret;
        }


        /// <summary>
        /// Função Prepara campo String para ser incorporado no comando SQL do MySql
        /// Se houver aspas simples na string, ela será substituída por crase. 
        /// </summary>
        /// <param name="strConteudo">string a ser verficiada</param>
        /// <returns>retorna a palavra "null" ou retorna o próprio conteúdo entre aspas simpes. ex.: 'teste'</returns>
        public string F_PreparaCampoMySQLString(string strConteudo)
        {
            if (strConteudo.Trim() == "")
            { 
                return "null";
            }
            else
            {
                strConteudo = strConteudo.Replace("'", "`"); //substitui a aspas simples por crase
                return "'" + strConteudo.Trim() + "'"; //adiciona aspas simples
            }
        }

        /// <summary>
        /// Função Executa SQL Scalar Double (retorna apenas 1 valor)
        /// </summary>
        /// <param name="strQuery">String comando SQL</param>
        /// <returns>Retorna num valor Double com o resultado</returns>
        public double F_ExecutaScalarSQL_Double(string strQuery)
        {
            double ret = 0;
            string msgErro = "";
            try
            {
                if (F_AbreConexao(ref msgErro) == System.Data.ConnectionState.Open)
                {
                    using (var comando = new MySqlCommand(strQuery, mConn))
                    {
                        ret = (double)comando.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                ret = 0;
                throw;
            }
            R_FechaConexao();
            return ret;
        }

        /// <summary>
        /// Função Executa SQL Scalar String (retorna apenas 1 valor)
        /// </summary>
        /// <param name="strQuery">String comando SQL</param>
        /// <returns>Retorna num valor Double com o resultado</returns>
        public string F_ExecutaScalarSQL_String(string strQuery)
        {
            string ret = "";
            string msgErro = "";
            try
            {
                if (F_AbreConexao(ref msgErro) == System.Data.ConnectionState.Open)
                {
                    using (var comando = new MySqlCommand(strQuery, mConn))
                    {
                        ret = comando.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception)
            {
                ret = "";
                throw;
            }
            R_FechaConexao();
            return ret;
        }
    }
}
