using Dapper;
using ManejoPresupuestos.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace ManejoPresupuestos.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task Editar(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObteneroPorId(int id, int usuarioId);
    }


    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionStirng;

        public RepositorioCategorias(IConfiguration configuration)
        {
            this.connectionStirng = configuration.GetConnectionString("DefaultConnectionSQL");
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionStirng);
            int id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                                            VALUES  (@Nombre, @TipoOperacionId, @UsuarioId)
                                                            SELECT SCOPE_IDENTITY();", categoria);

            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionStirng);
            return await connection.QueryAsync<Categoria>(@"SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionStirng);
            return await connection.QueryAsync<Categoria>(@"SELECT * 
                                                            FROM Categorias 
                                                            WHERE UsuarioId = @UsuarioId AND TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });
        }

        public async Task<Categoria> ObteneroPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionStirng);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"
                    SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId} );
        }

        public async Task Editar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionStirng);
            await connection.ExecuteAsync(@"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                                                    WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionStirng);
            await connection.ExecuteAsync(@"DELETE Categorias WHERE Id = @Id", new { id });
        }

    }
}
