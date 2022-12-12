﻿using Pasteleria.Models.Modelos;
using Pasteleria.Models.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace Pasteleria.Controllers
{
    public class CotizacionesController : Controller
    {
        private CotizacionModel _cotizacionModel = new CotizacionModel();
        private ComentarioModel _comentarioModel = new ComentarioModel();

        // GET: Cotizaciones
        public ActionResult Index()
        {
            try {
                var lista = _cotizacionModel.ObtenerCotizaciones();
                return View(lista);
            }
            catch (Exception) {

                return View();
            }
        }

        public ActionResult Detalles(int id) {
            try {
                var cotizacion = _cotizacionModel.ObtenerCotizacion(id);
                var opciones = new List<SelectListItem>();
                var opcionesCotizacion = new List<OpcionObj>();
                foreach (var item in _cotizacionModel.ObtenerEstados())
                    opciones.Add(new SelectListItem { Text = item.Descripcion, Value = item.Id.ToString() });

                opcionesCotizacion = _cotizacionModel.ObtenerOpcionesCotizacion(id);

                var comentarios = _comentarioModel.ObtenerComentarios(id);

                ViewBag.Opciones = opcionesCotizacion;
                ViewBag.ComboEstados = opciones;
                ViewBag.Comentarios = comentarios;
                return View(cotizacion);
            }
            catch (Exception) {

                return View();
            }
        }

        [HttpPost]
        public ActionResult Editar(int COT_ID, decimal COT_ESTIMADO, int COT_EST_DESC) {
            try {
                var cotizacion = _cotizacionModel.ActualizarCotizacion(COT_ID, COT_ESTIMADO, COT_EST_DESC);
                TempData["Mensaje"] = "Modificación realizada con exito";

                return RedirectToAction("Detalles", new { id = COT_ID });
            }
            catch (Exception) {
                TempData["Error"] = "Error";
                return RedirectToAction("Detalles", new { id = COT_ID });
            }
        }


        [HttpPost]
        public ActionResult AgregarComentario(ComentarioObj com) {
            try {
                var email = Session["CorreoUsuario"].ToString();

                var cotizacion = _comentarioModel.AgregarComentarios(email, com);
                TempData["Mensaje"] = "Comentario añadido exitosamente";

                return RedirectToAction("Detalles", new { id = com.IdCotizacion });
            }
            catch (Exception) {
                TempData["Error"] = "Error al agregar comentario";
                return RedirectToAction("Detalles", new { id = com.IdCotizacion });
            }
        }
    }
}