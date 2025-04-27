using Newtonsoft.Json;
using NugetBeeyondScreen.Models;
using System.Net.Http.Headers;
using System.Text;

namespace MvcBeeyondScreenClient.Services
{
    public class ServiceCine
    {
        private string ApiUrl;
        private MediaTypeWithQualityHeaderValue header;
        private string _authToken;

        public string AuthToken
        {
            get => _authToken;
            set => _authToken = value;
        }

        public ServiceCine(IConfiguration configuration)
        {
            this.ApiUrl =
                configuration.GetValue<string>("ApiUrls:ApiBeeyondScreen");
            this.header = new
                MediaTypeWithQualityHeaderValue("application/json");
        }

        #region Métodos de Películas

        // Obtener detalles de una película
        public async Task<ModelDetailsPelicula> GetDetailsPeliculaAsync(int idPelicula)
        {
            string request = $"api/peliculas/{idPelicula}";
            return await this.CallApiAsync<ModelDetailsPelicula>(request);
        }

        // Obtener todas las películas
        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            string request = "api/peliculas";
            return await this.CallApiAsync<List<Pelicula>>(request);
        }

        // Insertar una película
        public async Task<bool> InsertPeliculaAsync(Pelicula pelicula)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                string json = JsonConvert.SerializeObject(pelicula);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/peliculas", content);
                return response.IsSuccessStatusCode;
            }
        }

        // Actualizar una película
        public async Task<bool> UpdatePeliculaAsync(Pelicula pelicula)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                string json = JsonConvert.SerializeObject(pelicula);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"api/peliculas", content);
                return response.IsSuccessStatusCode;
            }
        }

        // Eliminar una película
        public async Task<bool> DeletePeliculaAsync(int idPelicula)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                HttpResponseMessage response = await client.DeleteAsync($"api/peliculas/{idPelicula}");
                return response.IsSuccessStatusCode;
            }
        }

        #endregion

        #region Métodos de Usuarios

        // Obtener facturas de boletos de un usuario
        public async Task<List<ViewFacturaBoleto>> GetFacturasBoletoUserAsync(int idUsuario)
        {

            string request = $"api/usuarios/facturas/{idUsuario}";
            return await this.CallApiAsync<List<ViewFacturaBoleto>>(request,true);
        }

        // Obtener una factura específica de un usuario
        public async Task<ViewFacturaBoleto> GetFacturaBoletoUserAsync(int idBoletoUser)
        {
            string request = $"api/usuarios/factura/{idBoletoUser}";
            return await this.CallApiAsync<ViewFacturaBoleto>(request);
        }

        // Obtener el último ID de usuario
        public async Task<int> GetLastIdUserAsync()
        {
            string request = "api/usuarios/lastid";
            return await this.CallApiAsync<int>(request);
        }

        // Obtener todos los usuarios
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            string request = "api/usuarios";
            return await this.CallApiAsync<List<Usuario>>(request);
        }

        // Encontrar un usuario por ID
        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            string request = $"api/usuarios/{idUsuario}";
            return await this.CallApiAsync<Usuario>(request);
        }

        // Actualizar perfil de usuario sin cambiar contraseña
        public async Task<bool> UpdateUsuarioProfileAsync(int idUsuario, string nombre, string email, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                var usuarioData = new
                {
                    IdUsuario = idUsuario,
                    Nombre = nombre,
                    Email = email,
                    Imagen = imagen
                };

                string json = JsonConvert.SerializeObject(usuarioData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("api/usuarios/profile", content);
                return response.IsSuccessStatusCode;
            }
        }

        // Actualizar perfil de usuario con cambio de contraseña
        public async Task<bool> UpdateUsuarioAsync(int idUsuario, string nombre, string email, string imagen, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                var usuarioData = new
                {
                    IdUsuario = idUsuario,
                    Nombre = nombre,
                    Email = email,
                    Imagen = imagen,
                    Password = password
                };

                string json = JsonConvert.SerializeObject(usuarioData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("api/usuarios", content);
                return response.IsSuccessStatusCode;
            }
        }

        // Registrar nuevo usuario
        public async Task<bool> RegisterUserAsync(string nombre, string email, string password, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                var usuarioData = new
                {
                    Nombre = nombre,
                    Email = email,
                    Password = password,
                    Imagen = imagen
                };

                string json = JsonConvert.SerializeObject(usuarioData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/usuarios/register", content);
                return response.IsSuccessStatusCode;
            }
        }

        // Login de usuario
        public async Task<Usuario> LoginUserAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                var loginData = new
                {
                    Email = email,
                    Password = password
                };

                string json = JsonConvert.SerializeObject(loginData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/usuarios/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(data);
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion


        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response = await
                    client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>
        (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        private async Task<T> CallApiAsync<T>(string request, bool requiresAuth = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                // Si se requiere autenticación y tenemos un token, lo añadimos
                if (requiresAuth && !string.IsNullOrEmpty(this.AuthToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + this.AuthToken);
                }

                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

    }
}
