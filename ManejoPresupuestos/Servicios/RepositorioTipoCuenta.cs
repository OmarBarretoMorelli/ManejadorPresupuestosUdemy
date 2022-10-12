using Dapper;
using ManejoPresupuestos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuestos.Servicios
{
    public interface IRepositorioTipoCuenta
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task BorrarTipoCuenta(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tiposCuenta);
    }

    public class RepositorioTipoCuenta : IRepositorioTipoCuenta
    {
        public readonly string connectionString;

        public RepositorioTipoCuenta(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnectionSQL");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("TiposCuentas_Insertar", new { usuarioId = tipoCuenta.UsuarioId,
                                                                                           Nombre = tipoCuenta.Nombre },
                                                                                           commandType: System.Data.CommandType.StoredProcedure); //En esta linea estoy indicando que quiero ejecutar un procedimiento almacenado.
            tipoCuenta.Id = id; //Aqui luego de que la base de datos cree el registro y le asigne un id, yo le asigno el id que me retorno la base de datos y se lo asigno a la instancia del objeto.

        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                                                                      @" SELECT 1 
                                                                      FROM TiposCuentas
                                                                      WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new { nombre, usuarioId });
            return existe != 0;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                            FROM TiposCuentas
                                                            WHERE UsuarioId = @UsuarioId
                                                            ORDER BY Orden;"
                                                            , new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas
                                            SET Nombre = @Nombre
                                            WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                                        FROM TiposCuentas
                                                                        WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id , usuarioId });
        }

        public async Task BorrarTipoCuenta(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tiposCuenta)
        {
            var query = "UPDATE TiposCuentas\tSET Orden = @Orden WHERE Id = @Id";

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tiposCuenta); //Dapper ejecuta el query para cada obj en tiposCuenta
        }
    }
}
