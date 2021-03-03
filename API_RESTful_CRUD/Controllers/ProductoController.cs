using API_RESTful_CRUD.Context;
using API_RESTful_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_RESTful_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContetx contetx;
        private readonly string _connectionString;
        public ProductoController(AppDbContetx contetx, IConfiguration configuration)
        {
            this.contetx = contetx;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        //GET: api/<ProductoController>
        [HttpGet]
        public IEnumerable<Producto> Get()
        {

            return contetx.Producto.ToList();
        }



        [HttpGet("storeProcedure")]
        public async Task<List<Producto>> GetActionResult()
        {

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("pruebaSelect", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<Producto>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }

            }

        }

        private Producto MapToValue(SqlDataReader reader)
        {
            return new Producto()
            {
                pro_codigo = reader["pro_codigo"].ToString(),
                pro_nombre = reader["pro_nombre"].ToString(),
                pro_descripcion = reader["pro_descripcion"].ToString(),
                pro_precio = (decimal)reader["pro_precio"]
            };
        }


        //[HttpGet]
        //public DataSet Get()
        //{
        //    SqlConnection conn = (SqlConnection)contetx.Database.GetDbConnection();

        //    SqlCommand cmd = conn.CreateCommand();

        //    conn.Open();
        //    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"EXEC [pruebaSelect]", conn);

        //   DataSet dataSet = new DataSet();
        //    sqlDataAdapter.Fill(dataSet);

        //    return dataSet;

        //    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    //cmd.CommandText = "pruebaSelect";

        //    //parametros
        //    //cmd.Parameters.Add("@nombre", system.Data.SqlDbType.VarChar).Value = "Valor del parametro";

        //    //cmd.ExecuteNonQuery();

        //}

        // GET api/<ProductoController>/5
        [HttpGet("{id}")]
        public Producto Get(string id)
        {
            var producto = contetx.Producto.FirstOrDefault(p => p.pro_codigo == id);

            return producto;
        }

        // POST api/<ProductoController>
        [HttpPost]
        public IActionResult Post([FromBody] Producto producto)
        {
            try
            {
                contetx.Producto.Add(producto);
                contetx.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        // PUT api/<ProductoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Producto producto)
        {
            if (producto.pro_codigo == id)
            {
                contetx.Entry(producto).State = EntityState.Modified;
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        // DELETE api/<ProductoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {

            var producto = contetx.Producto.FirstOrDefault(p => p.pro_codigo == id);

            if (producto != null)
            {
                contetx.Producto.Remove(producto);
                contetx.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
