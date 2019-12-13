using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjGynClientesNet.Classes
{
    class Uteis
    {
        /// <summary>
        /// Função Formata CNPJ ou CPF. 
        /// </summary>
        /// <param name="CnpjCpf">Cnpj ou Cpf</param>
        /// <returns>Se a string tiver mais que 11 caracteres formata como CNPJ senão formata como CPF</returns>
        public static string F_FormataCnpjCpf(string CnpjCpf)
        {
            if (CnpjCpf.Trim() != "")
            {
                CnpjCpf = F_RetiraPontos(CnpjCpf);
                if (CnpjCpf.Length > 11  )
                    return Convert.ToUInt64(CnpjCpf).ToString(@"00\.000\.000\/0000\-00"); //CNPJ
                else
                    return Convert.ToUInt64(CnpjCpf).ToString(@"000\.000\.000\-00"); //CPF
            }
                else
            {
                return "";
            }
        }


        /// <summary>
        /// Função Retira Pontos
        /// </summary>
        /// <param name="strAux">Recebe uma expressão alfanumerico (ex.: 99.999.999/0001-01)</param>
        /// <returns>Retorna Expressão sem pontos (Ex.: 99999999000101)</returns>
        public static string F_RetiraPontos(string strAux)
        {
            if (strAux.Trim() != "")
            {
                strAux = strAux.Replace(".", ""); //retira os pontos
                strAux = strAux.Replace("/", ""); //retira barras
                strAux = strAux.Replace("-", ""); //retira traço
                return strAux;
            }
            else
            {
                return "";
            }
        }

        ///// <summary>
        ///// Rotina GotFocus
        ///// </summary>
        ///// <param name="objTextBox">Recebe um Objeto TextBox e Seleciona seu Conteúdo</param>
        //public static void R_GotFocus(ref TextBox objTextBox )
        //{
        //    if (objTextBox.Text.Trim()!= "")
        //    { 
        //        objTextBox.SelectionStart = 0;
        //        objTextBox.SelectAll();
        //    }
        //}

        /// <summary>
        /// Função KeyPress Inteiro - Verifica se o caractere é um número
        /// </summary>
        /// <param name="e">KeyPressEventArgs</param>
        /// <returns>True or False</returns>
        public static bool F_KeyPressInteiro( KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Função Valida CNPJ
        /// Autor: Macoratti
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>True or False</returns>
        public static bool F_ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Função Valida CPF
        /// Autor: Macoratti
        /// </summary>
        /// <param name="cpf">CPF</param>
        /// <returns>True or False</returns>
        public static bool F_ValidaCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }

}

