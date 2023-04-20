using _5112_Cumulative_1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _5112_Cumulative_1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext school = new SchoolDbContext();

        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(String SearchKey =null ) 
        {
            MySqlConnection conn = school.AccessDatabase();

            conn.Open();

            //Establish a new command (query) for the database
            MySqlCommand cmd = conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" +  SearchKey + "%");
            cmd.Prepare();

            //Gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>{};

            //Loop through each row the result set
            while (ResultSet.Read())
            {
                //Access column information by the database column as an index
                int Teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                string Teacherfname = ResultSet["teacherfname"].ToString();
                string Teacherlname = ResultSet["teacherlname"].ToString();
                string Employeenumber = ResultSet["Employeenumber"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.Teacherid = Teacherid;
                NewTeacher.Teacherfname = Teacherfname;
                NewTeacher.Teacherlname = Teacherlname;
                NewTeacher.Employeenumber = Employeenumber;

                //Add the teacher name to the list
                Teachers.Add(NewTeacher);
            }

            //Close the connection betweeen MySQL and Webserver
            conn.Close();

            //Return the final list of teacher name
            return Teachers;
        }


        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //create an instance connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between webserver and database
            Conn.Open();

            //Establish a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int Teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                string Teacherfname = ResultSet["teacherfname"].ToString();
                string Teacherlname = ResultSet["teacherlname"].ToString();
                string Employeenumber = ResultSet["employeenumber"].ToString();

                NewTeacher.Teacherid = Teacherid;
                NewTeacher.Teacherfname = Teacherfname;
                NewTeacher.Teacherlname = Teacherlname;
                NewTeacher.Employeenumber = Employeenumber;

            }

            return NewTeacher;

        }
    }
}
