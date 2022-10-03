import React, { useState } from 'react';
import debug from 'sabio-debug';
import { LoadScript, Autocomplete } from '@react-google-maps/api';
import './autocomplete.css';
import PropTypes from 'prop-types';

const _logger = debug.extend('Locations').extend('LocationForm');
//Needs to be declared outside of the component to avoid getting warnings on browser.
const libraries = ['places'];
const API_KEY = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

function LocationAutoComplete({ handleChange }) {
    const [autoLoad, setAutoLoad] = useState({});

    const onLoad = (autocomplete) => {
        setAutoLoad(autocomplete);
    };

    const onPlaceChange = () => {
        if (autoLoad?.getPlace) {
            const obj = autoLoad.getPlace();
            let formData = {};
            if (obj?.address_components) {
                let location = buildLocationObj(obj.address_components);
                formData.lineOne = `${location.street_number ? location.street_number : ''}${
                    location.route ? ` ${location.route}` : ''
                }`;
                formData.lineTwo = '';
                if (location?.sublocality_level_1) {
                    formData.city = location.sublocality_level_1;
                } else {
                    formData.city = location.locality;
                }
                formData.state = `${location.administrative_area_level_1 ? location.administrative_area_level_1 : ''}`;
                formData.latitude = obj.geometry.location.lat();
                formData.longitude = obj.geometry.location.lng();
                formData.zip = `${location.postal_code ? location.postal_code : ''}`;
                formData.locationTypeId = 0;
            } else {
                formData.incomplete = obj.name;
            }
            handleChange(formData);
        } else {
            _logger('Autocomplete is not loaded yet!');
        }
    };

    const buildLocationObj = (components) => {
        let location = {};
        Object.keys(components).forEach((key) => {
            location[components[key].types[0]] = components[key].long_name;
        });
        return location;
    };
    return (
        <React.Fragment>
            <LoadScript googleMapsApiKey={API_KEY} libraries={libraries}>
                <div className="form-group mt-2">
                    <Autocomplete
                        onLoad={onLoad}
                        onPlaceChanged={onPlaceChange}
                        fields={['address_components', 'geometry.location']}
                        types={['street_address', 'locality', 'administrative_area_level_1', 'postal_code']}
                        restrictions={{ country: 'us' }}>
                        <input type="text" placeholder="Search...." className="form-control" />
                    </Autocomplete>
                </div>
            </LoadScript>
        </React.Fragment>
    );
}

LocationAutoComplete.propTypes = {
    handleChange: PropTypes.func,
};

export default LocationAutoComplete;
