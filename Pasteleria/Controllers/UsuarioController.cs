﻿using Pasteleria.Models.Modelos;
using Pasteleria.Models.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasteleria.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioModel modelUsuario = new UsuarioModel();


        public ActionResult Validar(UsuarioObj usuario) {
            var resultado = modelUsuario.ValidarUsuario(usuario);

            if (resultado != null && resultado.token != null) {
                Session["CodigoSeguridad"] = resultado.token;
                Session["CorreoUsuario"] = resultado.correo;
                Session["RolUsuario"] = resultado.tipoUsuario.descripcion;
                return RedirectToAction("Principal", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [FiltroSesiones]
        public ActionResult UsuariosConsulta() {
            var resultado = modelUsuario.UsuariosLista();
            if (resultado != null)
                return View(resultado);
            else
                return View("Error");
        }

        [FiltroSesiones]
        public ActionResult UsuariosRegistrar() {
            var opciones = new List<SelectListItem>();
            opciones.Add(new SelectListItem { Text = "Administrador", Value = "1" });
            opciones.Add(new SelectListItem { Text = "Usuario", Value = "2" });

            ViewBag.ComboTiposUsuario = opciones;

            return View();
        }

        public ActionResult NuevoRegistro(UsuarioObj usuario) {
            try {
                var resultado = modelUsuario.NuevoRegistro(usuario);
                if (resultado) {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception) {

            }
            return RedirectToAction("Registrar", "Home");
        }

        [FiltroSesiones]
        public ActionResult UsuariosActualizar(int id) {
            return View();
        }

    }
}