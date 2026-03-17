using Microsoft.AspNetCore.Components.Authorization;
using ScreenSound.Web.Response;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory httpClientFactory) : AuthenticationStateProvider
{
    private bool autenticado = false;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        autenticado = false;
        var pessoa = new ClaimsPrincipal();
        var response = await _httpClient.GetAsync("auth/manage/info");

        if (response.IsSuccessStatusCode)
        {
            var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();

            Claim[] dados = 
                [
                    new Claim(ClaimTypes.Name, info.Email ?? string.Empty),
                    new Claim(ClaimTypes.Email, info.Email ?? string.Empty)
                ];
            var identity = new ClaimsIdentity(dados, "Cookies");
            pessoa = new ClaimsPrincipal(identity);
            autenticado = true;
        }
        return new AuthenticationState(pessoa);
    }

    public async Task<AuthResponse> LoginAsync(string email, string senha)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login?useCookies=true", new
        { email, password = senha });

        if (response.IsSuccessStatusCode)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new AuthResponse { Sucesso = true };
        }
        else
        {
            return new AuthResponse {Sucesso = false, Erros = new[] { "Falha ao autenticar. Verifique suas credenciais." } };
        }
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("auth/logout", null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var state = await GetAuthenticationStateAsync();
        //return state.User.Identity?.IsAuthenticated ?? false;
        return autenticado;
    }
}
