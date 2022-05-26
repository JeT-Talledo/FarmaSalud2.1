﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;
using appPROYECTO_DSW_I.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace appPROYECTO_DSW_I.Controllers
{
    public class SesionController : Controller
    {
        const string sesionusuario = "_User";

        private readonly IConfiguration _IConfig;
        public SesionController(IConfiguration IConfig)
        {
            _IConfig = IConfig;
        }

        ///----LOGIN SESION
        public IActionResult LoginSesion()
        {
            return View(new UsuarioModel());
        }
        [HttpPost]
        public async Task<IActionResult> LoginSesion(UsuarioModel reg)
        {
            string cnn = _IConfig["ConnectionStrings:cn"];
            SqlConnection cn = new SqlConnection(cnn);
            cn.Open();
            SqlCommand CMD = new SqlCommand("usp_seg_UserSesion", cn);
            CMD.CommandType = CommandType.StoredProcedure;
            CMD.Parameters.AddWithValue("@correo_Usuario", reg.correoUser);
            CMD.Parameters.AddWithValue("@contra", reg.claveUser);
            SqlDataReader dr = CMD.ExecuteReader();

            if (dr.Read())
            {
                //var _CLAIMS = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, dr.GetString(0)),  //Para tomar el correo
                //    new Claim("correoUser", dr.GetString(1)),
                //    new Claim("claveUser", dr.GetString(2)),  //Para tomar la clave
                //    new Claim(ClaimTypes.Role, dr.GetString(3)), //Para capturar el nivel se seguridad 1(Admin) o 2(Cliente)
                //};

                //var _claimsIdentity = new ClaimsIdentity(_CLAIMS, CookieAuthenticationDefaults.AuthenticationScheme);
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(_claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Los datos ingresados no son validos ");
            }
            cn.Close();
            return View(reg);
        }
        public async Task<IActionResult> Salir()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginSesion", "Sesion");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}