﻿using ET.Base.Response;
using ET.Base.Token;
using ET.Business.Cqrs;
using ET.Data;
using ET.Data.Entities;
using ET.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ET.Business.Command
{
    public class TokenCommandHandler :
    IRequestHandler<CreateTokenCommand, ApiResponse<TokenResponse>>
    {
        private readonly ETDbContext dbContext;
        private readonly JwtConfig jwtConfig;

        public TokenCommandHandler(ETDbContext dbContext, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this.dbContext = dbContext;
            this.jwtConfig = jwtConfig.CurrentValue;
        }

        public async Task<ApiResponse<TokenResponse>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Set<ApplicationUser>().Where(x => x.UserName == request.Model.UserName)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                return new ApiResponse<TokenResponse>("Invalid user information");
            }

            
            if (request.Model.Password != user.Password)
            {
                user.LastActivityDate = DateTime.UtcNow;
                user.PasswordRetryCount++;
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ApiResponse<TokenResponse>("Invalid user information");
            }

            if (user.Status != 1)
            {
                return new ApiResponse<TokenResponse>("Invalid user status");
            }
            if (user.PasswordRetryCount > 3)
            {
                return new ApiResponse<TokenResponse>("Invalid user status");
            }

            user.LastActivityDate = DateTime.UtcNow;
            user.PasswordRetryCount = 0;
            await dbContext.SaveChangesAsync(cancellationToken);

            string token = Token(user);

            return new ApiResponse<TokenResponse>(new TokenResponse()
            {
                Email = user.Email,
                Token = token,
                ExpireDate = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration)
            });
        }

        private string Token(ApplicationUser user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        private Claim[] GetClaims(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("UserName", user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

            return claims;
        }
    }
}
