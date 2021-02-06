using Horizon.Visualizer.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Allegro.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));


            DataTable dt2 = new DataTable();
            ds.Tables.Add(dt2);

            dt2.Columns.Add("Name", typeof(string));
            dt2.Columns.Add("Salary", typeof(decimal));
            dt2.Columns.Add("Active", typeof(bool));



            DataTable dt3 = new DataTable();
            dt3.Columns.Add("Name", typeof(string));
            dt3.Columns.Add("Salary", typeof(decimal));

            DataRow row1 = dt3.NewRow();
            row1["Name"] = "John";
            row1["Salary"] = 200;
            dt3.Rows.Add(row1);

            DataSetDebuggerSide.TestShowVisualizer(dt3);

            //List<Employee> list = new List<Employee>();
            //list.Add(new Employee() { Name = "John Doe", DOB = new DateTime(1981, 5, 10), Salary = 20000M });
            //list.Add(new Employee() { Name = "Jane Doe", DOB = new DateTime(1990, 6, 20), Salary = 25000M });

            //ListDebuggerSide.ShowVisualizerForm(list, "test");


        }

    }

    [Serializable]
    class Employee
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public decimal Salary { get; set; }
    }

}
