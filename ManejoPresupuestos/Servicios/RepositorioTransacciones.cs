using Dapper;
using ManejoPresupuestos.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejoPresupuestos.Servicios
{

    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Crear(TransaccionCreacionViewModel transaccion);
    }


    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string connectionString;

        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnectionSQL"); 
        }

        public async Task Crear(TransaccionCreacionViewModel transaccion)
        {
            using var connection = new SqlConnection(connectionString);
            int id = await connection.QuerySingleAsync<int>("Transacciones_Insertar", new 
                                                            { transaccion.UsuarioId, transaccion.FechaTransaccion,
                                                              transaccion.Monto, transaccion.CategoriaId,
                                                              transaccion.CuentaId, transaccion.Nota, transaccion.TipoOperacionId },
                                                              commandType: System.Data.CommandType.StoredProcedure);
            transaccion.Id = id;
        }

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Actualizar", new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnterior
            }, commandType: System.Data.CommandType.StoredProcedure);            
        }


    }
}
