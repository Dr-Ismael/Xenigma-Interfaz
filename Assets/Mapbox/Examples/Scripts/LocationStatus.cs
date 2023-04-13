namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Text;

	public class LocationStatus : MonoBehaviour
	{

		[SerializeField]
		Text _statusText;

		private AbstractLocationProvider _locationProvider = null;
		Location currLoc; 
		void Start()
		{
			if (null == _locationProvider)
			{
				_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
			}
		}


		void Update()
		{
			currLoc = _locationProvider.CurrentLocation;

			if (currLoc.IsLocationServiceInitializing)
			{
				_statusText.text = "iniciando servicio de localizacion";
			}
			else
			{
				if (!currLoc.IsLocationServiceEnabled)
				{
					_statusText.text = "servicio de localicacion no iniciado";
				}
				else
				{
					if (currLoc.LatitudeLongitude.Equals(Vector2d.zero))
					{
						_statusText.text = "esperando localizacion ....";
					}
					else
					{
						_statusText.text = string.Format("{0}", currLoc.LatitudeLongitude);
					}
				}
			}

		}

		public double GetLocationLat()
		{
			return currLoc.LatitudeLongitude.x;
		}

		public double GetLocationLon()
		{
			return currLoc.LatitudeLongitude.y;
		}
	}
}
