using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace CakeСonfigurator
{
    public partial class Form_Configurator : Form
    {
        public Form_Configurator()
        {
            InitializeComponent();
        }

        static class GlobalVars
        {
            public static string result_marking_file = "";//Обозначение для названия файла
            public static int NumberMin_Коржи = 1;//Минимальное количество коржей
            public static int NumberMax_Коржи = 5;//Максимальное количество коржей
            public static int NumberMultiplierCream = 100;//Количство крема на 1 корж, г (единиц измерения)
            public static int МаксимальноДопустимоеКоличествоВидовКрема = 1;//Количство крема на 1 корж, г (единиц измерения)
            public static int МинимальноДопустимоеКоличествоВидовУкрашений = 1;//Количство крема на 1 корж, г (единиц измерения)
            public static int ВесКоржа = 50;//Вес коржа, г (единиц измерения)
            public static int ДлинаЛентыНаКоробку = 75;//??? из метров перевёл в сантиметры чтобы не ломать логику
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cakeСonfiguratorDataSet.Упаковка". При необходимости она может быть перемещена или удалена.
            this.упаковкаTableAdapter.Fill(this.cakeСonfiguratorDataSet.Упаковка);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cakeСonfiguratorDataSet.Улучшения". При необходимости она может быть перемещена или удалена.
            this.улучшенияTableAdapter.Fill(this.cakeСonfiguratorDataSet.Улучшения);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cakeСonfiguratorDataSet.Дополнительные_начинки". При необходимости она может быть перемещена или удалена.
            this.дополнительные_начинкиTableAdapter.Fill(this.cakeСonfiguratorDataSet.Дополнительные_начинки);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cakeСonfiguratorDataSet.Крема". При необходимости она может быть перемещена или удалена.
            this.кремаTableAdapter.Fill(this.cakeСonfiguratorDataSet.Крема);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cakeСonfiguratorDataSet.Коржи". При необходимости она может быть перемещена или удалена.
            this.коржиTableAdapter.Fill(this.cakeСonfiguratorDataSet.Коржи);

        }

        //Коржи
        private void dataGridView_Коржи_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell txtxCell = (DataGridViewTextBoxCell)dataGridView_Коржи.Rows[e.RowIndex].Cells["Коржи_number"];

            if (txtxCell.Value == null)
            {
                txtxCell.Value = 0;
            }



            txtxCell_number_calc(txtxCell, e.ColumnIndex);

        }

        //Крема
        private void dataGridView_Крема_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell txtxCell = (DataGridViewTextBoxCell)dataGridView_Крема.Rows[e.RowIndex].Cells["Крема_number"];

            if (txtxCell.Value == null)
            {
                txtxCell.Value = 0;
            }

            txtxCell_number_calc(txtxCell, e.ColumnIndex , GlobalVars.NumberMultiplierCream);

        }
        //ДопНачинки
        private void dataGridView_ДопНачинки_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell txtxCell = (DataGridViewTextBoxCell)dataGridView_ДопНачинки.Rows[e.RowIndex].Cells["ДопНачинки_number"];

            if (txtxCell.Value == null)
            {
                txtxCell.Value = 0;
            }

            txtxCell_number_calc(txtxCell, e.ColumnIndex);

        }

        //Украшения
        private void dataGridView_Украшения_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell txtxCell = (DataGridViewTextBoxCell)dataGridView_Украшения.Rows[e.RowIndex].Cells["Украшения_number"];

            if (txtxCell.Value == null)
            {
                txtxCell.Value = 0;
            }

            txtxCell_number_calc(txtxCell, e.ColumnIndex);

        }

        //Упаковка
        private void dataGridView_Упаковка_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell txtxCell = (DataGridViewTextBoxCell)dataGridView_Упаковка.Rows[e.RowIndex].Cells["Упаковка_number"];

            if (txtxCell.Value == null)
            {
                txtxCell.Value = 0;
            }

            txtxCell_number_calc(txtxCell, e.ColumnIndex);

        }

        //Расчёт количества компонентов
        private void txtxCell_number_calc(DataGridViewTextBoxCell txtxCell, int ColumnIndex, int NumberMultiplier = 1/*коэффициент изменения количества*/) {

            switch (ColumnIndex)
            {
                case 6:
                    if ((int)txtxCell.Value > 0)
                    {
                        txtxCell.Value = (int)txtxCell.Value - 1* NumberMultiplier;
                    }
                    break;
                case 8:
                    txtxCell.Value = (int)txtxCell.Value + 1* NumberMultiplier;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }


            //Проверки на соблюдение правил

            //Число коржей не более 5
            int КоличествоКоржей = 0;
            lbl_warning_Коржи.Text = "";

            for (int i = 0; i < dataGridView_Коржи.RowCount; i++)
                if (dataGridView_Коржи["Коржи_number", i].Value != null && (int)dataGridView_Коржи["Коржи_number", i].Value != 0)
                {
                    if ((int)dataGridView_Коржи["Коржи_number", i].Value > GlobalVars.NumberMax_Коржи)//защита
                    {
                        dataGridView_Коржи["Коржи_number", i].Value = GlobalVars.NumberMax_Коржи;//восстанавливаем справедливость
                    }

                    КоличествоКоржей += (int)dataGridView_Коржи["Коржи_number", i].Value;
                }

            if (КоличествоКоржей < GlobalVars.NumberMin_Коржи)
            {
                lbl_warning_Коржи.Text = "Общее число коржей не должно быть меньше " + GlobalVars.NumberMin_Коржи;
                return;
            }
            if (КоличествоКоржей  > GlobalVars.NumberMax_Коржи)
            {
                lbl_warning_Коржи.Text = "Общее число коржей не должно превышать " + GlobalVars.NumberMax_Коржи;
                return;
            }

            //КоличествоВидовКрема не более 1
            int КоличествоВидовКрема = 0;
            int idRow = -1;//номер строки с заполненными данными
            lbl_warning_Крем.Text = "";

            for (int i = 0; i < dataGridView_Крема.RowCount; i++)
                if (dataGridView_Крема["Крема_number", i].Value != null && (int)dataGridView_Крема["Крема_number", i].Value != 0)
                {
                    КоличествоВидовКрема += 1;
                    idRow = i;
                }
            if (КоличествоВидовКрема > GlobalVars.МаксимальноДопустимоеКоличествоВидовКрема)
            {
                lbl_warning_Крем.Text = "Превышено допустимое количество видов крема равное " + GlobalVars.МаксимальноДопустимоеКоличествоВидовКрема;
                return;
            }

            //Приравниваем количество крема к числу коржей если допустимое число видов крема = 1
            if (GlobalVars.МаксимальноДопустимоеКоличествоВидовКрема == 1)
            {
                if (КоличествоВидовКрема == 1) {
                    dataGridView_Крема["Крема_number", idRow].Value = КоличествоКоржей * GlobalVars.NumberMultiplierCream;
                }
            }
            else {
                //Число слоёв крема по 100 г = числу коржей
                int КоличествоСлоёвКрема = 0;

                for (int i = 0; i < dataGridView_Крема.RowCount; i++)
                    if (dataGridView_Крема["Крема_number", i].Value != null && (int)dataGridView_Крема["Крема_number", i].Value != 0)
                    {
                        КоличествоСлоёвКрема += (int)dataGridView_Крема["Крема_number", i].Value / GlobalVars.NumberMultiplierCream;
                    }

                if (КоличествоСлоёвКрема != КоличествоКоржей )
                {
                    lbl_warning_Крем.Text = "На каждый корж должно приходиться 100 г крема: на " + КоличествоКоржей + " коржей - " + КоличествоКоржей * GlobalVars.NumberMultiplierCream + " г. крема";
                    return;
                }
            }

            //Торт должен быть украшен хотя-бы одним украшением, при этом можно украсить торт несколькими украшениями. ??? Хотя было непонятно - речь о разных видах украшений или несколько грамм одного вида
            int КоличествоУкрашений = 0;
            Boolean ФактУкрашенияВишенкой = false;
            Boolean ФактУкрашенияМарципановойФигуркой = false;
            lbl_warning_Украшения.Text = "";

            for (int i = 0; i < dataGridView_Украшения.RowCount; i++)
                if (dataGridView_Украшения["Украшения_number", i].Value != null && (int)dataGridView_Украшения["Украшения_number", i].Value != 0)
                {
                    КоличествоУкрашений += 1;
                    if ((int)dataGridView_Украшения["Украшения_id", i].Value == 1) { ФактУкрашенияВишенкой = true; }
                    if ((int)dataGridView_Украшения["Украшения_id", i].Value == 3) { ФактУкрашенияМарципановойФигуркой = true; }
                }

            if (КоличествоУкрашений < GlobalVars.МинимальноДопустимоеКоличествоВидовУкрашений)
            {
                lbl_warning_Украшения.Text = "Минимальное количество украшений: " + GlobalVars.МинимальноДопустимоеКоличествоВидовУкрашений;
                return;
            }

            //Нельзя украсить торт вишенкой и марципановой фигуркой одновременно.
            if (ФактУкрашенияВишенкой && ФактУкрашенияМарципановойФигуркой) {
                lbl_warning_Украшения.Text = "Нельзя украсить торт вишенкой и марципановой фигуркой одновременно.";
                return;
            }

            //Торт можно упаковать (число упаковой всегда = 1), при этом должна быть использована лента для перевязки - на одну упаковку 0,75 метра ленты
            int idRow_Коробка = -1;//номер строки с заполненными данными
            int idRow_Лента = -1;//номер строки с заполненными данными

            for (int i = 0; i < dataGridView_Упаковка.RowCount; i++)
                switch ((int)dataGridView_Упаковка["упаковка_id", i].Value)
                {
                    case 1://Коробка
                        idRow_Коробка = i;
                        break;
                    case 2://Лента
                        idRow_Лента = i;
                        break;
                }

            if (dataGridView_Упаковка["Упаковка_number", idRow_Коробка].Value != null && (int)dataGridView_Упаковка["Упаковка_number", idRow_Коробка].Value != 0)
            {
                if ((int)dataGridView_Упаковка["Упаковка_number", idRow_Коробка].Value > 1)
                {
                    dataGridView_Упаковка["Упаковка_number", idRow_Коробка].Value = 1;
                }
                if ((int)dataGridView_Упаковка["Упаковка_number", idRow_Коробка].Value == 1)//Задана коробка
                {
                    dataGridView_Упаковка["Упаковка_number", idRow_Лента].Value = GlobalVars.ДлинаЛентыНаКоробку;
                }
            }






            //Расчёт стоимости
            Calculate();
        }





        private void textBox_Comment_Click(object sender, EventArgs e)
        {
            if(textBox_Comment.Text == "Дополнительный комментарий к рецепту"){
                textBox_Comment.Text = "";
            }
                
        }

        private void textBox_Comment_Leave(object sender, EventArgs e)
        {
            if (textBox_Comment.Text == "")
            {
                textBox_Comment.Text = "Дополнительный комментарий к рецепту";
            }
        }

        //Расчёт итогов
        private void Calculate()
        {
            decimal result_sum = 0;//Стоимость
            decimal result_weight = 0;//Вес 
            string result_marking = "" ;//Обозначение 
            //string Коржи_number = "";

            //Для каждого из гридов

            for (int i = 0; i < dataGridView_Коржи.RowCount; i++)
                if (dataGridView_Коржи["Коржи_number", i].Value != null && (int)dataGridView_Коржи["Коржи_number", i].Value != 0)
                {

                        result_sum += (int)dataGridView_Коржи["Коржи_number", i].Value * (decimal)dataGridView_Коржи["Коржи_price", i].Value;
                        //Предположим что коржи весят по 50г
                        result_weight += (int)dataGridView_Коржи["Коржи_number", i].Value * GlobalVars.ВесКоржа;

                        if ((int)dataGridView_Коржи["Коржи_number", i].Value > 1)
                        {
                            //Коржи_number = (string)dataGridView_Коржи["Коржи_number", i].FormattedValue;
                            //result_marking += (string)dataGridView_Коржи["Коржи_number", i].Value + dataGridView_Коржи["Коржи_marking", i].Value;
                            result_marking += (string)dataGridView_Коржи["Коржи_number", i].FormattedValue + ((string)dataGridView_Коржи["Коржи_marking", i].Value).Trim();
                        }
                        else if ((int)dataGridView_Коржи["Коржи_number", i].Value == 1)
                        {
                            result_marking += ((string)dataGridView_Коржи["Коржи_marking", i].FormattedValue).Trim();
                        }
                        //break;
                }
            result_marking += "|";

            for (int i = 0; i < dataGridView_Крема.RowCount; i++)
                if (dataGridView_Крема["Крема_number", i].Value != null && (int)dataGridView_Крема["Крема_number", i].Value != 0)
                {
                    result_sum += (int)dataGridView_Крема["Крема_number", i].Value * (decimal)dataGridView_Крема["Крема_price", i].Value;
                    result_weight += (int)dataGridView_Крема["Крема_number", i].Value ;

                    if ((int)dataGridView_Крема["Крема_number", i].Value > 1)
                    {
                        result_marking += (string)dataGridView_Крема["Крема_number", i].FormattedValue + ((string)dataGridView_Крема["Крема_marking", i].Value).Trim();
                    }
                    else if ((int)dataGridView_Крема["Крема_number", i].Value == 1)
                    {
                        result_marking += ((string)dataGridView_Крема["Крема_marking", i].FormattedValue).Trim();
                    }
                    //break;
                }
            result_marking += "|";

            for (int i = 0; i < dataGridView_ДопНачинки.RowCount; i++)
                if (dataGridView_ДопНачинки["ДопНачинки_number", i].Value != null && (int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value != 0)
                {
                    result_sum += (int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value * (decimal)dataGridView_ДопНачинки["ДопНачинки_price", i].Value;
                    result_weight += (int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value ;

                    if ((int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value > 1)
                    {
                        result_marking += (string)dataGridView_ДопНачинки["ДопНачинки_number", i].FormattedValue + ((string)dataGridView_ДопНачинки["ДопНачинки_marking", i].Value).Trim();
                    }
                    else if ((int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value == 1)
                    {
                        result_marking += ((string)dataGridView_ДопНачинки["ДопНачинки_marking", i].FormattedValue).Trim();
                    }
                    //break;
                }
            result_marking += "|";

            for (int i = 0; i < dataGridView_Украшения.RowCount; i++)
                if (dataGridView_Украшения["Украшения_number", i].Value != null && (int)dataGridView_Украшения["Украшения_number", i].Value != 0)
                {
                    result_sum += (int)dataGridView_Украшения["Украшения_number", i].Value * (decimal)dataGridView_Украшения["Украшения_price", i].Value;

                    if ((int)dataGridView_Украшения["Украшения_number", i].Value > 1)
                    {
                        result_marking += (string)dataGridView_Украшения["Украшения_number", i].FormattedValue + ((string)dataGridView_Украшения["Украшения_marking", i].Value).Trim();
                    }
                    else if ((int)dataGridView_Украшения["Украшения_number", i].Value == 1)
                    {
                        result_marking += ((string)dataGridView_Украшения["Украшения_marking", i].FormattedValue).Trim();
                    }
                    //break;
                }
            result_marking += "|";

            for (int i = 0; i < dataGridView_Упаковка.RowCount; i++)
                if (dataGridView_Упаковка["Упаковка_number", i].Value != null && (int)dataGridView_Упаковка["Упаковка_number", i].Value != 0)
                {
                    result_sum += (int)dataGridView_Упаковка["Упаковка_number", i].Value * (decimal)dataGridView_Упаковка["Упаковка_price", i].Value;

                    if ((int)dataGridView_Упаковка["Упаковка_number", i].Value > 1)
                    {
                        result_marking += (string)dataGridView_Упаковка["Упаковка_number", i].FormattedValue + ((string)dataGridView_Упаковка["Упаковка_marking", i].Value).Trim();
                    }
                    else if ((int)dataGridView_Упаковка["Упаковка_number", i].Value == 1)
                    {
                        result_marking += ((string)dataGridView_Упаковка["Упаковка_marking", i].FormattedValue).Trim();
                    }
                    //break;
                }



            textBox_Itog.Text = "Вес: " + result_weight + " г   " + "Стоимость: " + Math.Round(result_sum,2) + " руб.";

            //замена дублирующихся разделителей
            while (result_marking.Contains("||")){
                result_marking = result_marking.Replace("||", "|");
            }
            if (result_marking.Substring(result_marking.Length - 1) == "|") {
                result_marking = result_marking.Substring(0,result_marking.Length - 1);
            }
            textBox_Обозначение.Text = "Торт - " + result_marking ;

            //замена разделителей | на допустимые _
            GlobalVars.result_marking_file = result_marking.Replace("|", "_");
        }



        private void btn_Сохранить_в_XML_Click(object sender, EventArgs e)
        {

                /*XDocument doc =
                      new XDocument(
                        new XElement("Cake",
                          new XElement("date", new XAttribute("modified", DateTime.Now)),
                          new XElement("Коржи",
                            list.Select(x => new XElement("data", new XAttribute("value", x)))
                          )
                        )
                      );

                doc.Save(Dir.SelectedPath + "/" + "Торт-"+GlobalVars.result_marking_file + ".xml");
                */

                XmlDocument XD = new XmlDocument();
                XmlDeclaration xmlDeclaration = XD.CreateXmlDeclaration("1.0", "UTF-8", null);
                XD.AppendChild(xmlDeclaration);

                XmlElement root = XD.CreateElement("Торт");
                XmlElement date = XD.CreateElement("Дата");
                date.SetAttribute("Изменён", DateTime.Now.ToString());
                root.AppendChild(date);

                Boolean FactAppendChild = false;

                XmlElement Коржи = XD.CreateElement("Коржи");
                for (int i = 0; i < dataGridView_Коржи.RowCount; i++)
                    if (dataGridView_Коржи["Коржи_number", i].Value != null && (int)dataGridView_Коржи["Коржи_number", i].Value != 0)
                    {
                        XmlElement da = XD.CreateElement((string)dataGridView_Коржи["Коржи_id", i].FormattedValue);
                        da.SetAttribute("number", (string)dataGridView_Коржи["Коржи_number", i].FormattedValue);
                        Коржи.AppendChild(da);
                        FactAppendChild = true;
                    }
                if (FactAppendChild == true)
                {
                    root.AppendChild(Коржи);
                }

                FactAppendChild = false;
                XmlElement Крема = XD.CreateElement("Крема");
                for (int i = 0; i < dataGridView_Крема.RowCount; i++)
                    if (dataGridView_Крема["Крема_number", i].Value != null && (int)dataGridView_Крема["Крема_number", i].Value != 0)
                    {
                        XmlElement da = XD.CreateElement((string)dataGridView_Крема["Крема_id", i].FormattedValue);
                        da.SetAttribute("number", (string)dataGridView_Крема["Крема_number", i].FormattedValue);
                        Крема.AppendChild(da);
                        FactAppendChild = true;
                    }
                if (FactAppendChild == true)
                {
                    root.AppendChild(Крема);
                }

                FactAppendChild = false;
                XmlElement ДопНачинки = XD.CreateElement("ДопНачинки");
                for (int i = 0; i < dataGridView_ДопНачинки.RowCount; i++)
                    if (dataGridView_ДопНачинки["ДопНачинки_number", i].Value != null && (int)dataGridView_ДопНачинки["ДопНачинки_number", i].Value != 0)
                    {
                        XmlElement da = XD.CreateElement((string)dataGridView_ДопНачинки["ДопНачинки_id", i].FormattedValue);
                        da.SetAttribute("number", (string)dataGridView_ДопНачинки["ДопНачинки_number", i].FormattedValue);
                        ДопНачинки.AppendChild(da);
                        FactAppendChild = true;
                    }
                if (FactAppendChild == true)
                {
                    root.AppendChild(ДопНачинки);
                }

                FactAppendChild = false;
                XmlElement Украшения = XD.CreateElement("Украшения");
                for (int i = 0; i < dataGridView_Украшения.RowCount; i++)
                    if (dataGridView_Украшения["Украшения_number", i].Value != null && (int)dataGridView_Украшения["Украшения_number", i].Value != 0)
                    {
                        XmlElement da = XD.CreateElement((string)dataGridView_Украшения["Украшения_id", i].FormattedValue);
                        da.SetAttribute("number", (string)dataGridView_Украшения["Украшения_number", i].FormattedValue);
                        Украшения.AppendChild(da);
                        FactAppendChild = true;
                    }
                if (FactAppendChild == true)
                {
                    root.AppendChild(Украшения);
                }

                FactAppendChild = false;
                XmlElement Упаковка = XD.CreateElement("Упаковка");
                for (int i = 0; i < dataGridView_Упаковка.RowCount; i++)
                    if (dataGridView_Упаковка["Упаковка_number", i].Value != null && (int)dataGridView_Упаковка["Упаковка_number", i].Value != 0)
                    {
                        XmlElement da = XD.CreateElement((string)dataGridView_Упаковка["Упаковка_id", i].FormattedValue);
                        da.SetAttribute("number", (string)dataGridView_Упаковка["Упаковка_number", i].FormattedValue);
                        Упаковка.AppendChild(da);
                        FactAppendChild = true;
                    }
                if (FactAppendChild == true)
                {
                    root.AppendChild(Упаковка);
                }

                XD.AppendChild(root);
                XD.Save(Properties.Settings.Default.SelectedPathXML + "/" + "Торт-" + GlobalVars.result_marking_file + ".xml");



        }

        private void btn_XMLSavePafhSet_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dir = new FolderBrowserDialog();
            Dir.SelectedPath = Properties.Settings.Default.SelectedPathXML;
            if (Dir.ShowDialog() == DialogResult.OK)
            {
                System.IO.Directory.GetFiles(Dir.SelectedPath);
                Properties.Settings.Default.SelectedPathXML = Dir.SelectedPath;
                Properties.Settings.Default.Save();
            }

        }

        private void btn_СоздатьШоколадныйТорт_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex=0;//костыль - иначе в гриде не отображаются изменения, причина не определена
            for (int i = 0; i < dataGridView_Коржи.RowCount; i++)
                if ((int)dataGridView_Коржи["Коржи_id", i].Value == 3)//Бисквитный
                {
                    dataGridView_Коржи["Коржи_number", i].Value = 3;
                    break;
                }
            tabControl1.SelectedIndex=1;
            for (int i = 0; i < dataGridView_Крема.RowCount; i++)
                if ((int)dataGridView_Крема["Крема_id", i].Value == 1)//Масляный
                {
                    dataGridView_Крема["Крема_number", i].Value = 3*100;
                    break;
                }
            tabControl1.SelectedIndex=2;
            for (int i = 0; i < dataGridView_ДопНачинки.RowCount; i++)
                if ((int)dataGridView_ДопНачинки["ДопНачинки_id", i].Value == 1)//Орехи
                {
                    dataGridView_ДопНачинки["ДопНачинки_number", i].Value = 20;//г
                   break;
                }
            tabControl1.SelectedIndex=3;
            for (int i = 0; i < dataGridView_Украшения.RowCount; i++)
                if ((int)dataGridView_Украшения["Украшения_id", i].Value == 2)//Шоколадная глазурь
                {
                    dataGridView_Украшения["Украшения_number", i].Value = 1;//шт
                    break;
                }
            tabControl1.SelectedIndex=0;
            //Расчёт стоимости
            Calculate();
        }
    }
}
