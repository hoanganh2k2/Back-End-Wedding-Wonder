using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class StringeeTokenService
{
    private readonly string _apiKeySid = "SK.0.CveZd7K1J3gb179oSG0Ol9PnARcMyS6J";  
    private readonly string _apiSecret = "N2pyY0xsOU5UMTl3VkJtMTFZUjFwa3pMakxnd0pv";  
    public string GenerateAccessToken(int userId)
    { 
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, _apiKeySid + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds()),   
            new Claim(JwtRegisteredClaimNames.Iss, _apiKeySid),   
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString()),   
            new Claim("userId", userId.ToString()),   
            new Claim("call_api", "true")
        };
         
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
         
        var header = new JwtHeader(creds);
        header["cty"] = "stringee-api;v=1";   
         
        var token = new JwtSecurityToken(
            issuer: _apiKeySid,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
         
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
