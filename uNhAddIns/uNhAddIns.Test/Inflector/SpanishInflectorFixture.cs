using NUnit.Framework;
using uNhAddIns.Inflector;

namespace uNhAddIns.Test.Inflector
{
	[TestFixture]
	public class SpanishInflectorFixture : BaseInflectorFixture
	{
		public class CustomInflector: SpanishInflector
		{
			public CustomInflector()
			{
				AddDataDictionary("OrdenCliente", "OrdenesClientes");
				AddDataDictionary("OrdenProveedor", "OrdenesProveedores");
				AddDataDictionary("ArticuloProveedor", "ArticulosProveedores");
				AddDataDictionary("ClienteGrupoCliente", "ClienteGruposClientes");
				AddDataDictionary("ConceptoAplicacion", "ConceptosAplicaciones");
				AddDataDictionary("ConceptoComponente", "ConceptosComponentes");
				AddDataDictionary("ConceptoGrupoConcepto", "ConceptoGruposConceptos");
				AddDataDictionary("CuentaGrupoCuenta", "CuentaGruposCuentas");
				AddDataDictionary("DireccionTipo", "DireccionesTipos");
				AddDataDictionary("DocumentoDocumento", "DocumentosDocumentos");
				AddDataDictionary("DocumentoExtensionDocumento", "DocumentoExtensionesDocumentos");
				AddDataDictionary("DocumentoTipoAplicacion", "DocumentoTiposAplicaciones");
				AddDataDictionary("DocumentoTipoConcepto", "DocumentoTiposConceptos");
				AddDataDictionary("DocumentoTipoGrupoDocumentoTipo", "DocumentoTipoGruposDocumentoTipos");
				AddDataDictionary("DocumentoTipoProducto", "DocumentoTiposProductos");
				AddDataDictionary("EntidadGrupoEntidad", "EntidadGruposEntidades");
				AddDataDictionary("NexoTipo", "NexosTipos");
				AddDataDictionary("OrganizacionActividad", "OrganizacionesActividades");
				AddDataDictionary("PagoForma", "PagosFormas");
				AddDataDictionary("PersonaActividad", "PersonasActividades");
				AddDataDictionary("PersonaOcupacion", "PersonasOcupaciones");
				AddDataDictionary("PersonaTitulo", "PersonasTitulos");
				AddDataDictionary("ProductoFabricante", "ProductosFabricantes");
				AddDataDictionary("ProductoGrupoProducto", "ProductoGruposProductos");
				AddDataDictionary("ProveedorGrupoProveedor", "ProveedorGruposProveedores");
				AddDataDictionary("ProveedorProducto", "ProveedoresProductos");
				AddDataDictionary("UsuarioRol", "UsuariosRoles");
			}
		}

		public SpanishInflectorFixture()
		{
			SingularToPlural.Add("inglés", "ingleses");
			SingularToPlural.Add("hijo", "hijos");
			SingularToPlural.Add("paz", "paces");
			SingularToPlural.Add("crisis", "crisis");
			SingularToPlural.Add("praxis", "praxis");
			SingularToPlural.Add("apendicitis", "apendicitis");
			SingularToPlural.Add("llave", "llaves");
			SingularToPlural.Add("auto", "autos");
			SingularToPlural.Add("ordén", "ordenes");
			SingularToPlural.Add("item", "items");
			SingularToPlural.Add("linea", "lineas");
			SingularToPlural.Add("proveedor", "proveedores");
			SingularToPlural.Add("Terminal", "Terminales");
			SingularToPlural.Add("ParteFichaTecnica", "ParteFichaTecnicas");
			SingularToPlural.Add("pago", "pagos");
			SingularToPlural.Add("Ubicación", "Ubicaciones");
			SingularToPlural.Add("Origén", "Origenes");
			SingularToPlural.Add("rol", "roles");
			SingularToPlural.Add("ciudad", "ciudades");
			SingularToPlural.Add("documento", "documentos");
			SingularToPlural.Add("Historial", "Historiales");
			SingularToPlural.Add("Promoción", "Promociones");

			ClassNameToTableName.Add("Origenes", "Origén" );
			ClassNameToTableName.Add("Ordenes", "Ordén");
			ClassNameToTableName.Add("OrdenesClientes", "OrdenCliente");
			ClassNameToTableName.Add("OrdenesProveedores", "OrdenProveedor");
			ClassNameToTableName.Add("Facturas", "Factura");

			AddClassNameToTableName("Actividad", "Actividades");
			AddClassNameToTableName("Actor", "Actores");
			AddClassNameToTableName("ActorDireccion", "ActorDirecciones");
			AddClassNameToTableName("ActorRol", "ActorRoles");
			AddClassNameToTableName("Agente", "Agentes");
			AddClassNameToTableName("AgenteTipo", "AgenteTipos");
			AddClassNameToTableName("Almacen", "Almacenes");
			AddClassNameToTableName("AlmacenTipo", "AlmacenTipos");
			AddClassNameToTableName("Aplicacion", "Aplicaciones");
			AddClassNameToTableName("AplicacionArbol", "AplicacionArboles");
			AddClassNameToTableName("AplicacionCampo", "AplicacionCampos");
			AddClassNameToTableName("AplicacionComponente", "AplicacionComponentes");
			AddClassNameToTableName("AplicacionLista", "AplicacionListas");
			AddClassNameToTableName("Arbol", "Arboles");
			AddClassNameToTableName("ArbolCampo", "ArbolCampos");
			AddClassNameToTableName("Arqueo", "Arqueos");
			AddClassNameToTableName("ArqueoDetalle", "ArqueoDetalles");
			AddClassNameToTableName("Articulo", "Articulos");
			AddClassNameToTableName("ArticuloProveedor", "ArticulosProveedores");
			AddClassNameToTableName("ArticuloTipo", "ArticuloTipos");
			AddClassNameToTableName("Caja", "Cajas");
			AddClassNameToTableName("Cajero", "Cajeros");
			AddClassNameToTableName("Campo", "Campos");
			AddClassNameToTableName("Ciudad", "Ciudades");
			AddClassNameToTableName("CivilEstado", "CivilEstados");
			AddClassNameToTableName("Clasificacion", "Clasificaciones");
			AddClassNameToTableName("Cliente", "Clientes");
			AddClassNameToTableName("ClienteGrupo", "ClienteGrupos");
			AddClassNameToTableName("ClienteGrupoCliente", "ClienteGruposClientes");
			AddClassNameToTableName("ClienteTipo", "ClienteTipos");
			AddClassNameToTableName("Componente", "Componentes");
			AddClassNameToTableName("Concepto", "Conceptos");
			AddClassNameToTableName("ConceptoAplicacion", "ConceptosAplicaciones");
			AddClassNameToTableName("ConceptoComponente", "ConceptosComponentes");
			AddClassNameToTableName("ConceptoConfiguracion", "ConceptoConfiguraciones");
			AddClassNameToTableName("ConceptoGrupo", "ConceptoGrupos");
			AddClassNameToTableName("ConceptoGrupoConcepto", "ConceptoGruposConceptos");
			AddClassNameToTableName("ConceptoProducto", "ConceptoProductos");
			AddClassNameToTableName("ConceptoTipo", "ConceptoTipos");
			AddClassNameToTableName("Contacto", "Contactos");
			AddClassNameToTableName("ContactoMetodo", "ContactoMetodos");
			AddClassNameToTableName("Cuenta", "Cuentas");
			AddClassNameToTableName("CuentaGrupo", "CuentaGrupos");
			AddClassNameToTableName("CuentaGrupoCuenta", "CuentaGruposCuentas");
			AddClassNameToTableName("CuentaTipo", "CuentaTipos");
			AddClassNameToTableName("DatoTipo", "DatoTipos");
			AddClassNameToTableName("DefectoValor", "DefectoValores");
			AddClassNameToTableName("Denominacion", "Denominaciones");
			AddClassNameToTableName("Direccion", "Direcciones");
			AddClassNameToTableName("DireccionTipo", "DireccionesTipos");
			AddClassNameToTableName("Documento", "Documentos");
			AddClassNameToTableName("DocumentoComplemento", "DocumentoComplementos");
			AddClassNameToTableName("DocumentoConfiguracion", "DocumentoConfiguraciones");
			AddClassNameToTableName("DocumentoDocumento", "DocumentosDocumentos");
			AddClassNameToTableName("DocumentoExtension", "DocumentoExtensiones");
			AddClassNameToTableName("DocumentoExtensionDocumento", "DocumentoExtensionesDocumentos");
			AddClassNameToTableName("DocumentoTipo", "DocumentoTipos");
			AddClassNameToTableName("DocumentoTipoAplicacion", "DocumentoTiposAplicaciones");
			AddClassNameToTableName("DocumentoTipoComplemento", "DocumentoTipoComplementos");
			AddClassNameToTableName("DocumentoTipoConcepto", "DocumentoTiposConceptos");
			AddClassNameToTableName("DocumentoTipoGrupo", "DocumentoTipoGrupos");
			AddClassNameToTableName("DocumentoTipoGrupoDocumentoTipo", "DocumentoTipoGruposDocumentoTipos");
			AddClassNameToTableName("DocumentoTipoProducto", "DocumentoTiposProductos");
			AddClassNameToTableName("DocumentoTipoClase", "DocumentoTipoClases");
			AddClassNameToTableName("Empleado", "Empleados");
			AddClassNameToTableName("EmpleadoGrupo", "EmpleadoGrupos");
			AddClassNameToTableName("EmpleadoTipo", "EmpleadoTipos");
			AddClassNameToTableName("Entidad", "Entidades");
			AddClassNameToTableName("EntidadGrupo", "EntidadGrupos");
			AddClassNameToTableName("EntidadGrupoEntidad", "EntidadGruposEntidades");
			AddClassNameToTableName("EntidadTipo", "EntidadTipos");
			AddClassNameToTableName("Equivalencia", "Equivalencias");
			AddClassNameToTableName("Establecimiento", "Establecimientos");
			AddClassNameToTableName("EstablecimientoTipo", "EstablecimientoTipos");
			AddClassNameToTableName("EstudioNivel", "EstudioNiveles");
			AddClassNameToTableName("Fabricante", "Fabricantes");
			AddClassNameToTableName("Fuente", "Fuentes");
			AddClassNameToTableName("IdentificacionTipo", "IdentificacionTipos");
			AddClassNameToTableName("Limite", "Limites");
			AddClassNameToTableName("Lista", "Listas");
			AddClassNameToTableName("ListaCampo", "ListaCampos");
			AddClassNameToTableName("ListaItem", "ListaItems");
			AddClassNameToTableName("Localidad", "Localidades");
			AddClassNameToTableName("LocalidadTipo", "LocalidadTipos");
			AddClassNameToTableName("Localizacion", "Localizaciones");
			AddClassNameToTableName("MedidaUnidad", "MedidaUnidades");
			AddClassNameToTableName("Movimiento", "Movimientos");
			AddClassNameToTableName("MovimientoPago", "MovimientoPagos");
			AddClassNameToTableName("Municipio", "Municipios");
			AddClassNameToTableName("Nexo", "Nexos");
			AddClassNameToTableName("NexoTipo", "NexosTipos");
			AddClassNameToTableName("Ocupacion", "Ocupaciones");
			AddClassNameToTableName("Organizacion", "Organizaciones");
			AddClassNameToTableName("OrganizacionActividad", "OrganizacionesActividades");
			AddClassNameToTableName("PagoForma", "PagosFormas");
			AddClassNameToTableName("PagoFormaTipo", "PagoFormaTipos");
			AddClassNameToTableName("Pais", "Paises");
			AddClassNameToTableName("Parentesco", "Parentescos");
			AddClassNameToTableName("Persona", "Personas");
			AddClassNameToTableName("PersonaActividad", "PersonasActividades");
			AddClassNameToTableName("PersonaOcupacion", "PersonasOcupaciones");
			AddClassNameToTableName("PersonaTitulo", "PersonasTitulos");
			AddClassNameToTableName("Precio", "Precios");
			AddClassNameToTableName("Producto", "Productos");
			AddClassNameToTableName("ProductoClasificacion", "ProductoClasificaciones");
			AddClassNameToTableName("ProductoFabricante", "ProductosFabricantes");
			AddClassNameToTableName("ProductoGrupo", "ProductoGrupos");
			AddClassNameToTableName("ProductoGrupoProducto", "ProductoGruposProductos");
			AddClassNameToTableName("ProductoTipo", "ProductoTipos");
			AddClassNameToTableName("Proveedor", "Proveedores");
			AddClassNameToTableName("ProveedorGrupo", "ProveedorGrupos");
			AddClassNameToTableName("ProveedorGrupoProveedor", "ProveedorGruposProveedores");
			AddClassNameToTableName("ProveedorProducto", "ProveedoresProductos");
			AddClassNameToTableName("ProveedorTipo", "ProveedorTipos");
			AddClassNameToTableName("Region", "Regiones");
			AddClassNameToTableName("Rol", "Roles");
			AddClassNameToTableName("Sucursal", "Sucursales");
			AddClassNameToTableName("SucursalTipo", "SucursalTipos");
			AddClassNameToTableName("Tarifa", "Tarifas");
			AddClassNameToTableName("Telefono", "Telefonos");
			AddClassNameToTableName("TelefonoTipo", "TelefonoTipos");
			AddClassNameToTableName("Territorio", "Territorios");
			AddClassNameToTableName("Titulo", "Titulos");
			AddClassNameToTableName("Tratamiento", "Tratamientos");
			AddClassNameToTableName("Ubicacion", "Ubicaciones");
			AddClassNameToTableName("Usuario", "Usuarios");
			AddClassNameToTableName("UsuarioRol", "UsuariosRoles");
			AddClassNameToTableName("VendedorGrupo", "VendedorGrupos");

			TestInflector = new CustomInflector();
		}

		private void AddClassNameToTableName(string singular, string plural)
		{
			ClassNameToTableName.Add(plural, singular);
		}
	}
}