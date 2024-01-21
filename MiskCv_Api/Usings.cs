global using System.Data;
global using System.Text;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Text.Json.Serialization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using MapsterMapper;
global using Mapster;
global using MiskCv_Api.Mapping;

global using MiskCv_Api.Data;
global using MiskCv_Api.Services.DistributedCacheService;
global using MiskCv_Api.Services.JwtService;
global using MiskCv_Api.Models;

global using MiskCv_Api.Data.Repositories.AddressesRepository;
global using MiskCv_Api.Data.Repositories.CompaniesRepository;
global using MiskCv_Api.Data.Repositories.SkillsRepository;
global using MiskCv_Api.Data.Repositories.UsersRepository;
global using MiskCv_Api.Dtos.AddressDtos;
global using MiskCv_Api.Dtos.CompanyDtos;
global using MiskCv_Api.Dtos.SkillDtos;
global using MiskCv_Api.Dtos.UserDtos;