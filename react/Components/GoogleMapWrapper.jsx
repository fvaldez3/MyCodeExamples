import React, { useEffect, useState } from 'react';
import { GoogleMap, LoadScript, Marker } from '@react-google-maps/api';
import debug from 'sabio-debug';
import PropTypes from 'prop-types';
import './autocomplete.css';

const _logger = debug.extend('Locations').extend('GoogleMapWrapper');

function GoogleMapWrapper(props) {
    const defaultMapPosition = {
        zoom: 4,
        center: { lat: 39.8283, lng: -98.5795 },
    };
    const [mapMarker, setMapMarker] = useState({});
    const [mapPosition, setMapPosition] = useState(defaultMapPosition);
    const API_KEY = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

    useEffect(() => {
        _logger(props.marker);
        setMapMarker(() => {
            const mm = markerMapper(props.marker);
            return mm;
        });
        setMapPosition((prevState) => {
            const mp = { ...prevState };
            if (props.marker.id > 0) {
                mp.zoom = 12;
                mp.center = {
                    lat: props.marker.lat,
                    lng: props.marker.lng,
                };
            } else {
                mp.zoom = defaultMapPosition.zoom;
                mp.center = defaultMapPosition.center;
            }
            return mp;
        });
    }, [props.marker]);

    const markerMapper = (marker) => {
        return <Marker position={{ lat: marker.lat, lng: marker.lng }} key={`marker_${marker.id}`} />;
    };

    return (
        <React.Fragment>
            <h1>Map</h1>
            <div>
                <LoadScript googleMapsApiKey={API_KEY}>
                    <GoogleMap
                        mapContainerStyle={{ height: '600px', width: '600px' }}
                        zoom={mapPosition.zoom}
                        center={mapPosition.center}>
                        {mapMarker}
                    </GoogleMap>
                </LoadScript>
            </div>
        </React.Fragment>
    );
}

GoogleMapWrapper.propTypes = {
    marker: PropTypes.shape({
        id: PropTypes.number.isRequired,
        lat: PropTypes.number.isRequired,
        lng: PropTypes.number.isRequired,
    }),
};

export default React.memo(GoogleMapWrapper);
