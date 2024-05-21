namespace Mapbox.Unity.MeshGeneration.Factories
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Data;
	using Mapbox.Directions;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using Modifiers;
	using UnityEngine;

	public class DirectionsFactory : MonoBehaviour
	{
		//Clase que contiene las listas de puntos de interes elegidos
		public CargarDatosLugar lugaresElegidos;

		public SpawnOnMap direccionesLugares;

		[SerializeField]
		AbstractMap _map; // Referencia al mapa abstracto de Mapbox

		[SerializeField]
		MeshModifier[] MeshModifiers; // Modificadores de malla para aplicar a la geometría generada

		[SerializeField]
		Material _material; // Material para renderizar la malla

		[SerializeField]
		Transform[] _waypoints; // Array de transformaciones de puntos de referencia
		private List<Vector3> _cachedWaypoints; // Lista para almacenar posiciones de puntos de referencia en la última actualización

		[SerializeField]
		[Range(1, 10)]
		private float UpdateFrequency = 2; // Frecuencia de actualización en segundos

		private Directions _directions; // Instancia del servicio de direcciones de Mapbox
		private int _counter; // Contador para los datos de malla

		GameObject _directionsGO; // GameObject para la ruta generada
		private bool _recalculateNext; // Bandera para indicar si se necesita recalcular la ruta

		// Lista para almacenar los puntos convertidos a Vector2d
		public List<Vector2d> locations = new List<Vector2d>();

		protected virtual void Awake()
		{
			// Obtener las ubicaciones de la otra clase lugaresElegidos
			foreach (string locationString in direccionesLugares._locationStrings)
			{
				string[] parts = locationString.Split(','); // Dividir la cadena en partes separadas por coma

				if (parts.Length == 2) // Verificar que hay dos partes (latitud y longitud)
				{
					double latitude, longitude;

					// Intentar convertir las partes a valores numéricos
					if (double.TryParse(parts[0], out latitude) && double.TryParse(parts[1], out longitude))
					{
						// Crear un nuevo objeto Vector2d y agregarlo a la lista
						Vector2d location = new Vector2d(latitude, longitude);
						locations.Add(location);
					}
					else
					{
						Debug.LogError("Error al convertir la cadena de ubicación a valores numéricos: " + locationString);
					}
				}
				else
				{
					Debug.LogError("Formato de ubicación incorrecto: " + locationString);
				}
			}

			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
			_directions = MapboxAccess.Instance.Directions;
			_map.OnInitialized += Query;
			_map.OnUpdated += Query;
		}

		public void Start()
		{
			_cachedWaypoints = new List<Vector3>(); // Corrección aquí
			foreach (var item in _waypoints)
			{
				_cachedWaypoints.Add(item.position);
			}
			_recalculateNext = false;

			foreach (var modifier in MeshModifiers)
			{
				modifier.Initialize();
			}

			// Selecciona un punto deseado (puedes cambiar esta lógica según sea necesario)
			Vector2d selectedPoint = locations[0]; // Por ejemplo, el primer punto de la lista

			// Actualiza la posición del eventSpawner
			SetEventSpawnerPosition(selectedPoint);

			// Iniciar el temporizador de consulta
			StartCoroutine(QueryTimer());
		}

		protected virtual void OnDestroy()
		{
			_map.OnInitialized -= Query;
			_map.OnUpdated -= Query;
		}

		void Query()
		{
			// Asegúrate de que tenemos al menos dos waypoints: uno para el jugador y uno para el evento
			if (_waypoints.Count() < 2)
			{
				Debug.LogError("No hay suficientes waypoints asignados.");
				return;
			}

			var count = _waypoints.Count();
			var wp = new Vector2d[count];
			for (int i = 0; i < count; i++)
			{
				wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale); // Convertir cada punto de referencia a coordenadas geográficas
			}
			var _directionResource = new DirectionResource(wp, RoutingProfile.Driving); // Crear recurso de dirección con los puntos de referencia y perfil de conducción
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse); // Consultar la ruta
		}

		public IEnumerator QueryTimer()
		{
			while (true)
			{
				yield return new WaitForSeconds(UpdateFrequency);
				for (int i = 0; i < _waypoints.Count(); i++)
				{
					if (_waypoints[i].position != _cachedWaypoints[i])
					{
						_recalculateNext = true;
						_cachedWaypoints[i] = _waypoints[i].position;
					}
				}

				if (_recalculateNext)
				{
					Query();
					_recalculateNext = false;
				}
			}
		}

		void HandleDirectionsResponse(DirectionsResponse response)
		{
			if (response == null || null == response.Routes || response.Routes.Count < 1)
			{
				return;
			}

			var meshData = new MeshData();
			var dat = new List<Vector3>();
			foreach (var point in response.Routes[0].Geometry)
			{
				dat.Add(
					Conversions
						.GeoToWorldPosition(
							point.x,
							point.y,
							_map.CenterMercator,
							_map.WorldRelativeScale
						)
						.ToVector3xz()
				);
			}

			var feat = new VectorFeatureUnity();
			feat.Points.Add(dat);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				mod.Run(feat, meshData, _map.WorldRelativeScale);
			}

			CreateGameObject(meshData);
		}

		GameObject CreateGameObject(MeshData data)
		{
			if (_directionsGO != null)
			{
				_directionsGO.Destroy();
			}
			_directionsGO = new GameObject("direction waypoint " + " entity");
			var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
			mesh.subMeshCount = data.Triangles.Count;

			mesh.SetVertices(data.Vertices);
			_counter = data.Triangles.Count;
			for (int i = 0; i < _counter; i++)
			{
				var triangle = data.Triangles[i];
				mesh.SetTriangles(triangle, i);
			}

			_counter = data.UV.Count;
			for (int i = 0; i < _counter; i++)
			{
				var uv = data.UV[i];
				mesh.SetUVs(i, uv);
			}

			mesh.RecalculateNormals();
			_directionsGO.AddComponent<MeshRenderer>().material = _material;
			return _directionsGO;
		}

		void SetEventSpawnerPosition(Vector2d location)
		{
			// Asegúrate de que tenemos al menos dos waypoints: uno para el jugador y uno para el evento
			if (_waypoints.Count() < 2)
			{
				Debug.LogError("No hay suficientes waypoints asignados.");
				return;
			}

			// Convertir la ubicación seleccionada a una posición en el mundo
			Vector3 worldPosition = _map.GeoToWorldPosition(location, false);
			_waypoints[1].position = worldPosition; // Actualiza la posición del segundo waypoint (eventSpawner)

			Debug.Log("Nueva posición del eventSpawner establecida en: " + location);
		}
	}
}
